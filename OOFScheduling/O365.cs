using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace OOFScheduling
{
    internal class O365
    {

        private static string ClientId = "c0eceb27-8cd3-4bb8-9271-c90596069f74";
        private static string logonUrl = "https://login.microsoftonline.com/organizations/";
        internal static IPublicClientApplication PublicClientApp;
        internal static string AutomatedReplySettingsURL = "/mailboxSettings/automaticRepliesSetting";
        internal static string MailboxSettingsURL = "/mailboxSettings";
        public static object pcaInitLock = new object();

        //Set the API Endpoint to Graph 'me' endpoint
        static string _graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";

        //Set the scope for API call to user.read
        static string[] _scopes = new string[] { "user.read", "MailboxSettings.ReadWrite" };

        //create something we can lock against
        internal static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        internal static AuthenticationResult authResult = null;

        internal enum AADAction
        {
            SignIn, SignOut, ForceSignIn
        }

        private static string _defaultUserUPN = null;
        public static string DefaultUserUPN
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultUserUPN))
                {
                    _defaultUserUPN = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToLower();

                    //when on a domain-joined machine, it will be in the domain\user structure
                    //in that case, just return the user for pattern matching
                    if (_defaultUserUPN.Contains("\\"))
                    {
                        _defaultUserUPN = _defaultUserUPN.Split('\\').Last();
                    }
                }
                return _defaultUserUPN;
            }
            set
            {
                _defaultUserUPN = value;
            }
        }

        internal static bool isLoggedIn
        {
            get
            {
                //return authResult.ExpiresOn >= DateTime.UtcNow;

                //MSAL 1.0
                //return PublicClientApp.Users.Any();

                //MSAL 3.0
                Task<IEnumerable<IAccount>> accountTask = PublicClientApp.GetAccountsAsync();
                accountTask.Wait(10000);

                //for checking if logged in, want to do an exact match
                IAccount account = null;
                try { account = accountTask.Result.FirstOrDefault(p => p.Username.ToLower() == DefaultUserUPN.ToLower()); } catch (Exception) { }

                return (account != null && account.Username.ToLower() == DefaultUserUPN.ToLower());
            }
        }

        /// <summary>
        /// Call AcquireTokenAsync - to acquire a token requiring user to sign-in
        /// </summary>
        internal async static Task<bool> MSALWork(AADAction action)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            bool _result = false;

            //lock this so we don't get multiple auth prompts
            OOFSponder.Logger.Info("Attempting to enter critical section for auth code");
            await semaphoreSlim.WaitAsync();
            OOFSponder.Logger.Info("Inside critical section for auth code");

            OOFSponder.Logger.Info("Attempting to build PublicClientApp with multitenant endpoint");
            lock (pcaInitLock)
            {
                PublicClientApp = PublicClientApplicationBuilder.Create(ClientId)
                    .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
                    .WithAuthority(logonUrl)
                    .Build();

                MSALTokenCacheHelper.EnableSerialization(PublicClientApp.UserTokenCache);
            }

            //grab the logged in user UPN so we can decide whether or not to force the prompt

            try
            {
                Task<IEnumerable<IAccount>> accountTask = PublicClientApp.GetAccountsAsync();
                accountTask.Wait(10000);


                //  give UserTokenCache a go
                //in this case, we are OK doing some pattern matching since we jsut want to find the right user in the cache
                OOFSponder.Logger.Info("give UserTokenCache a go");
                IAccount account = null;
                try { account = accountTask.Result.FirstOrDefault(p => p.Username.ToLower().Contains(DefaultUserUPN.ToLower())); } catch (Exception) { }

                if (account != null && account.Username.ToLower().Contains(DefaultUserUPN.ToLower()))
                {
                    OOFSponder.Logger.Info("Found user in UserTokenCache that matches DefaultUserUPN");
                    try
                    {
                        Task<AuthenticationResult> authUITask = PublicClientApp.AcquireTokenSilent(_scopes, account)
                            .ExecuteAsync();
                        authUITask.Wait(10000);
                        authResult = authUITask.Result;
                        if (authResult != null)
                        {
                            OOFSponder.Logger.Info("AcquireTokenSilent -> OK");
                        }
                        _result = true;
                    }
                    catch (Exception x)
                    {
                        OOFSponder.Logger.Error("AcquireTokenSilent -> " + x.GetType().ToString());
                    }
                }

                //couldn't get token silently - need to use the UI
                if (!_result)
                {
                    OOFSponder.Logger.Info("couldn't get token silently - need to use the UI");
                    try
                    {
                        Task<AuthenticationResult> authUITask = PublicClientApp.AcquireTokenInteractive(_scopes)
                            .WithLoginHint(DefaultUserUPN)
                            .WithPrompt(Prompt.NoPrompt)
                            .ExecuteAsync();
                        authUITask.Wait(10000);
                        authResult = authUITask.Result;
                        _result = true;
                    }
                    catch (Exception ex)
                    {
                        OOFSponder.Logger.Info("Got an exception when using the UI");
                        OOFSponder.Logger.Info(ex.Message);
                        if (ex is MsalUiRequiredException || ex.InnerException is MsalUiRequiredException ||
                            ex is MsalClientException || ex.InnerException is MsalClientException ||
                            ex is MsalServiceException || ex.InnerException is MsalServiceException)
                        // MSAL service or client exception here is most likely down to need for UI
                        // even if it is not MsalUiRequiredException
                        {
                            try
                            {
                                Task<AuthenticationResult> authUITask = PublicClientApp.AcquireTokenInteractive(_scopes)
                                    .WithPrompt(Prompt.NoPrompt)
                                    .WithLoginHint(DefaultUserUPN).ExecuteAsync();
                                authUITask.Wait(10000);
                                authResult = authUITask.Result;
                                _result = true;
                            }
                            catch (Exception ex2)
                            {
                                string _error2 = "GetTokenFromAAD: Failed to get interactive auth token: " + ExceptionChain(ex2);
                                OOFSponder.Logger.Error(new Exception(_error2, ex2));
                            }
                        }
                        else
                        {
                            string _error = "GetTokenFromAAD: UI might be required for MSAL logon: " + ExceptionChain(ex);
                            OOFSponder.Logger.Error(new Exception(_error));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                OOFSponder.Logger.Error(new Exception("Generalized auth failure: ** ", ex));
            }
            finally
            {

                //store the UPN for future use
                if (authResult != null)
                {
                    DefaultUserUPN = authResult.Account.Username;
                }

                //release the critical section we are using to prevent multiple auth prompts
                OOFSponder.Logger.Info("Leaving critical section for auth code");
                semaphoreSlim.Release();
                OOFSponder.Logger.Info("Left critical section for auth code");
            }

            OOFSponderInsights.UserGUID = authResult.UniqueId;
            OOFSponder.Logger.Info("UserGUID: " + OOFSponderInsights.UserGUID);

            return _result;
        }

        /// <summary>
        /// Perform an HTTP GET request to a URL using an HTTP Authorization header
        /// </summary>
        /// <param name="url">The URL</param>
        /// <returns>String containing the results of the GET operation</returns>
        public static async Task<string> GetHttpContentWithToken(string url)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            //check and refresh token if necessary
            await O365.MSALWork(O365.AADAction.SignIn);

            if (authResult != null)
            {
                var httpClient = new System.Net.Http.HttpClient();
                System.Net.Http.HttpResponseMessage response;
                try
                {
                    var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, UrlCombine(_graphAPIEndpoint, url));
                    //Add the token in Authorization header
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                    response = await httpClient.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                catch (Exception ex)
                {
                    OOFSponder.Logger.Error(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// Perform an HTTP GET request to a URL using an HTTP Authorization header
        /// </summary>
        /// <param name="url">The URL</param>
        /// <param name="token">The token</param>
        /// <returns>String containing the results of the GET operation</returns>
        public static async Task<System.Net.Http.HttpResponseMessage> PatchHttpContentWithToken(string url, Microsoft.Graph.AutomaticRepliesSetting OOF)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            //check and refresh token if necessary
            await O365.MSALWork(O365.AADAction.SignIn);

            var httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpMethod method = new System.Net.Http.HttpMethod("PATCH");
            System.Net.Http.HttpResponseMessage response = null;

            //         var response = client.PostAsync("api/AgentCollection", new StringContent(
            //new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")).Result;


            try
            {

#if !NOOOF
                Microsoft.Graph.MailboxSettings mbox = new Microsoft.Graph.MailboxSettings();
                mbox.AutomaticRepliesSetting = OOF;

                var request = new System.Net.Http.HttpRequestMessage(method, UrlCombine(_graphAPIEndpoint, url));
                var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(mbox);
                System.Net.Http.StringContent iContent = new System.Net.Http.StringContent(jsonBody, Encoding.UTF8, "application/json");
                request.Content = iContent;
                //Add the token in Authorization header
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult.AccessToken);

                OOFSponder.Logger.Info("Sending OOF request to O365");
                response = await httpClient.SendAsync(request);
                OOFSponder.Logger.Info("Got response back from O365");
#endif
                return response;
            }
            catch (Exception ex)
            {
                OOFSponder.Logger.Error(new Exception("Unable to set OOF", ex));
                throw new Exception("Unable to set OOF: " + ex.Message, ex);
            }
        }

        // Combines urls like System.IO.Path.Combine
        // Usage: this.Literal1.Text = CommonCode.UrlCombine("http://stackoverflow.com/", "/questions ", " 372865", "path-combine-for-urls");
        public static string UrlCombine(params string[] urls)
        {
            string retVal = string.Empty;
            foreach (string url in urls)
            {
                var path = url.Trim().TrimEnd('/').TrimStart('/').Trim();
                retVal = string.IsNullOrWhiteSpace(retVal) ? path : new System.Uri(new System.Uri(retVal + "/"), path).ToString();
            }
            return retVal;

        }

        private static string ExceptionChain(Exception ex)
        {
            string _error = "** " + ex.GetType().ToString() + " ** " + ex.Message;

            string verboseError = _error;
            Exception inner = ex.InnerException;
            while (inner != null)
            {
                verboseError += "\r\n** " + inner.GetType().ToString() + " ** " + inner.Message;
                inner = inner.InnerException;
            }

            OOFSponder.Logger.Error(new Exception(_error, ex));
            return _error;
        }
    }

}

public static class MSALTokenCacheHelper
{
    public static void EnableSerialization(ITokenCache tokenCache)
    {
        tokenCache.SetBeforeAccess(BeforeAccessNotification);
        tokenCache.SetAfterAccess(AfterAccessNotification);
    }

    /// <summary>
    /// Path to the token cache
    /// </summary>
    private static readonly string CacheFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
        "\\msalcache.bin3.";

    private static readonly object FileLock = new object();

    private static List<string> bypassCacheForClient = null;

    public static List<string> BypassCacheForClient
    {
        set
        {
            lock (FileLock)
            {
                if (bypassCacheForClient == null)
                    bypassCacheForClient = new List<string>(value);
                else
                {
                    foreach (string c in value)
                        if (!bypassCacheForClient.Contains(c))
                            bypassCacheForClient.Add(c);
                }
            }
        }
    }

    private static void BeforeAccessNotification(TokenCacheNotificationArgs args)
    {
        bool bBypassCache = false;
        lock (FileLock)
        {
            string binPath = CacheFilePath + args.ClientId;
            if (bypassCacheForClient != null && bypassCacheForClient.Contains(args.ClientId))
            {
                bBypassCache = true;
                bypassCacheForClient.Remove(args.ClientId);
                try
                {
                    File.Delete(binPath);
                    OOFSponder.Logger.Info($"Token cache file deleted for {args.ClientId}");
                }
                catch (Exception x)
                {
                    OOFSponder.Logger.Info($"Failed to delete token cache file: {x.Message}");
                }
            }
            try
            {
                args.TokenCache.DeserializeMsalV3(File.Exists(binPath) && !bBypassCache
                        ? ProtectedData.Unprotect(File.ReadAllBytes(binPath),
                                                  null,
                                                  DataProtectionScope.CurrentUser)
                        : null, true);
            }
            catch (Exception x)
            {
                OOFSponder.Logger.Error(new Exception("TokenCache deserialize failure: ", x));
            }

        }
    }

    private static void AfterAccessNotification(TokenCacheNotificationArgs args)
    {
        // if the access operation resulted in a cache update
        if (args.HasStateChanged)
        {
            lock (FileLock)
            {
                try
                {
                    string binPath = CacheFilePath + args.ClientId;
                    // reflect changesgs in the persistent store
                    File.WriteAllBytes(binPath,
                                        ProtectedData.Protect(args.TokenCache.SerializeMsalV3(),
                                                                null,
                                                                DataProtectionScope.CurrentUser)
                                        );
                }
                catch (Exception x)
                {
                    OOFSponder.Logger.Error(new Exception("TokenCache serialize failure: ", x));
                }
            }
        }
    }
}
