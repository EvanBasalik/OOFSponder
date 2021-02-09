using Microsoft.Identity.Client;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOFScheduling
{
    internal class O365
    {

        private static string ClientId = "c0eceb27-8cd3-4bb8-9271-c90596069f74";
        private static string Tenant = "72f988bf-86f1-41af-91ab-2d7cd011db47";
        internal static IPublicClientApplication PublicClientApp = PublicClientApplicationBuilder.Create(ClientId)
                .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
                .WithAuthority(AzureCloudInstance.AzurePublic, Tenant)
                .Build();
        internal static string AutomatedReplySettingsURL = "/mailboxSettings/automaticRepliesSetting";
        internal static string MailboxSettingsURL = "/mailboxSettings";

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

        internal static bool isLoggedIn
        {
            get
            {
                //return authResult.ExpiresOn >= DateTime.UtcNow;

                //MSAL 1.0
                //return PublicClientApp.Users.Any();

                //MSAL 3.0
                var accounts = Task.Run(() => PublicClientApp.GetAccountsAsync()).GetAwaiter().GetResult();
                var firstAccount = accounts.FirstOrDefault();
                return (firstAccount != null);
            }
        }

        /// <summary>
        /// Call AcquireTokenAsync - to acquire a token requiring user to sign-in
        /// </summary>
        internal async static Task<bool> MSALWork(AADAction action)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            //lock this so we don't get multiple auth prompts
            OOFSponder.Logger.Info("Attempting to enter critical section for auth code");
            await semaphoreSlim.WaitAsync();
            OOFSponder.Logger.Info("Inside critical section for auth code");

            bool _result = false;
            var accounts = await PublicClientApp.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();

            try
            {
                if (action == AADAction.SignIn | action == AADAction.ForceSignIn)
                {
                    try
                    {
                        //MSAL 1.0 style - deprecated
                        //authResult = await PublicClientApp.AcquireTokenSilentAsync(_scopes, PublicClientApp.Users.FirstOrDefault());

                        //MSAL 3.0 style
                        authResult = await PublicClientApp.AcquireTokenSilent(_scopes, firstAccount).ExecuteAsync();
                    }
                    catch (MsalUiRequiredException ex)
                    {
                        // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                        //Don't track this one since it can basically be considered expected.
                        OOFSponder.Logger.Warning(new Exception($"Unable to acquire token silently: ", ex));

                        try
                        {
                            //MSAL 1.0 style
                            //authResult = await PublicClientApp.AcquireTokenAsync(_scopes);

                            //MSAL 3.0 style
                            authResult = await PublicClientApp.AcquireTokenInteractive(_scopes).ExecuteAsync();
                        }
                        catch (MsalException msalex)
                        {
                            OOFSponder.Logger.Error(new Exception($"Error acquiring token interactively: ", msalex));
                        }
                    }
                    catch (Exception ex)
                    {
                        OOFSponder.Logger.Error(new Exception($"Error acquiring token: ", ex));
                        return false;
                    }

                    //MSAL 1.0 style
                    //if (PublicClientApp.Users.Count() > 0)

                    //MSAL 3.0 style
                    if (authResult != null)
                    {
                        _result = true;

                        //also, update the Application Insights info with the authenticated user
                        //MSAL 1.0 style
                        //OOFSponderInsights.AIClient.Context.User.Id = authResult.User.DisplayableId.Split('@')[0];

                        //MSAL 3.0 style
                        OOFSponderInsights.AIClient.Context.User.Id = authResult.Account.Username;

                    }
                    else
                    {
                        _result = false;
                    }
                }
                else
                {
                    //MSAL 1.0
                    //if (PublicClientApp.Users.Any())

                    //MSAL 3.0
                    if (firstAccount != null)
                    {
                        try
                        {
                            //MSAL 1.0
                            //PublicClientApp.Remove(PublicClientApp.Users.FirstOrDefault());

                            //MSAL 3.0
                            await PublicClientApp.RemoveAsync(firstAccount);
                            _result = true;
                        }
                        catch (MsalException ex)
                        {
                            OOFSponder.Logger.Error(new Exception("Error signing out user: ", ex));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OOFSponder.Logger.Error(new Exception("MSAL code failed miserably for user: ", ex));
            }
            finally
            {
                //release the critical section we are using to prevent multiple auth prompts
                OOFSponder.Logger.Info("Leaving critical section for auth code");
                semaphoreSlim.Release();
                OOFSponder.Logger.Info("Left critical section for auth code");
            }

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
  
        public static async Task<int> PatchWithPowershell(Microsoft.Graph.AutomaticRepliesSetting OOF, String Tenant)
        {

            string[] scopes = new string[] { "https://outlook.office365.com/AdminApi.AccessAsUser.All", "https://outlook.office365.com/FfoPowerShell.AccessAsUser.All", "https://outlook.office365.com/RemotePowerShell.AccessAsUser.All" };
            IPublicClientApplication app;

            app = PublicClientApplicationBuilder.Create("a0c73c16-a7e3-4564-9a95-2bdf47383716")
                   .WithAuthority(AzureCloudInstance.AzurePublic, Tenant)
                  .WithRedirectUri("urn:ietf:wg:oauth:2.0:oob")
                  .Build();

            var accounts = await app.GetAccountsAsync();

            AuthenticationResult res = null;
            if (accounts.Any())
            {
                res = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                                  .ExecuteAsync();
            }
            else
            {
                try
                {

                    res = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
                    string tokenheader = "Bearer " + res.AccessToken;
                    string connectionUri = "https://outlook.office365.com/PowerShell-LiveId?BasicAuthToOAuthConversion=true";

                    SecureString secpassword = new SecureString();
                    foreach (char c in tokenheader)
                    {
                        secpassword.AppendChar(c);
                    }


                    PSCredential credential = new PSCredential(res.Account.Username, secpassword);

                    //CONNECT HTTPS POWERSHELL AUTHENTICATION POWERSHELL SESSION
                    Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();
                    PowerShell powershell = PowerShell.Create();
                    PSCommand command = new PSCommand();
                    command.AddCommand("New-PSSession");
                    command.AddParameter("ConfigurationName", "Microsoft.Exchange");
                    command.AddParameter("ConnectionUri", new Uri(connectionUri));
                    command.AddParameter("Credential", credential);
                    command.AddParameter("Authentication", "Basic");
                    command.AddParameter("AllowRedirection");
                    powershell.Commands = command;
                    runspace.Open();
                    powershell.Runspace = runspace;
                    Collection<System.Management.Automation.PSObject> result = powershell.Invoke();
                    if (powershell.Streams.Error.Count > 0 || result.Count != 1)
                    {

                        return 400;
                        //ERROR

                    }

                    // RUN COMMAND FOR SETTING THE OOF.
                    powershell = PowerShell.Create();
                    command = new PSCommand();
                    command.AddCommand("Invoke-Command");
                    string cmdlet = "";
                    string[] col = res.Account.Username.Split('@');
                    cmdlet += "Set-MailboxAutoReplyConfiguration -identity " + col[0];
                    cmdlet += " -AutoReplyState Scheduled -StartTime \"2/9/2021 00:00:00\"";
                    cmdlet += " -EndTime \"2/09/2021 15:00:00\"";
                    cmdlet += " -InternalMessage \"" + OOF.InternalReplyMessage + "\"";
                    cmdlet += " -ExternalMessage \"" + OOF.ExternalReplyMessage + "\"";


                    command.AddParameter("ScriptBlock", System.Management.Automation.ScriptBlock.Create(cmdlet));
                    command.AddParameter("Session", result[0]);
                    powershell.Commands = command;
                    powershell.Runspace = runspace;
                    var mailBoxes = powershell.Invoke();
                    if (powershell.Streams.Error.Count > 0)
                    {
                        return 400;
                        //ERROR 
                    }
                    return 200;
                   

                }
                catch (MsalException e)
                {
                    // Need to have logging with the already implemented logger class
                    return 400;
                }
                

            }
            return 400; ;
        }


            /// <summary>
            /// Perform an HTTP GET request to a URL using an HTTP Authorization header
            /// </summary>
            /// <param name="url">The URL</param>
            /// <param name="token">The token</param>
            /// <returns>String containing the results of the GET operation</returns>
            /// 
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
                response = await httpClient.SendAsync(request);
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
    }

}
