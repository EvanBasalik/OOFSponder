using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace OOFScheduling
{
    internal class O365
    {

        private static string ClientId = "c0eceb27-8cd3-4bb8-9271-c90596069f74";
        internal static PublicClientApplication PublicClientApp = new PublicClientApplication(ClientId);
        internal static string AutomatedReplySettingsURL = "/mailboxSettings/automaticRepliesSetting";

        //Set the API Endpoint to Graph 'me' endpoint
        static string _graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";

        //Set the scope for API call to user.read
        static string[] _scopes = new string[] { "user.read" };

        internal static AuthenticationResult authResult = null;

        internal enum AADAction
        {
            SignIn, SignOut, ForceSignIn
        }

        /// <summary>
        /// Call AcquireTokenAsync - to acquire a token requiring user to sign-in
        /// </summary>
        internal async static Task<bool> MSALWork(AADAction action)
        {
            bool _result = false;

            if (action == AADAction.SignIn | action == AADAction.ForceSignIn)
            {
                try
                {
                    authResult = await PublicClientApp.AcquireTokenAsync(_scopes);
                }
                catch (MsalUiRequiredException ex)
                {
                    // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                    OOFSponder.Logger.Info($"MsalUiRequiredException: {ex.Message}");

                    if (action == AADAction.ForceSignIn)
                    {
                        try
                        {
                            authResult = await PublicClientApp.AcquireTokenAsync(_scopes);
                        }
                        catch (MsalException msalex)
                        {
                            OOFSponder.Logger.Error(new Exception($"Error Acquiring Token:{System.Environment.NewLine}", msalex));
                        }
                    }

                }
                catch (Exception ex)
                {
                    OOFSponderInsights.TrackException("Error Acquiring Token Silently", ex);
                    return false;
                }

                if (PublicClientApp.Users.Count() > 0)
                {
                    //BuddyOptions.authResult = BuddyOptions.authResult;
                    _result = true;

                    //also, update the Application Insights info with the authenticated user
                   OOFSponderInsights.AIClient.Context.User.Id = authResult.User.DisplayableId.Split('@')[0];
                }
                else
                {
                    _result = false;
                }
            }
            else
            {
                if (PublicClientApp.Users.Any())
                {
                    try
                    {
                        PublicClientApp.Remove(PublicClientApp.Users.FirstOrDefault());
                        _result = true;
                    }
                    catch (MsalException ex)
                    {
                        OOFSponder.Logger.Error($"Error signing-out user: {ex.Message}");
                        OOFSponderInsights.TrackException($"Error signing-out user: {ex.Message}", ex);
                    }
                }
            }

            return _result;
        }

        /// <summary>
        /// Perform an HTTP GET request to a URL using an HTTP Authorization header
        /// </summary>
        /// <param name="url">The URL</param>
        /// <param name="token">The token</param>
        /// <returns>String containing the results of the GET operation</returns>
        public static async Task<string> GetHttpContentWithToken(string url)
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
                return ex.ToString();
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
