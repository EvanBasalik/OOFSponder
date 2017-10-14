using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace OOFScheduling
{
    class O365
    {

        public static string ClientId = "0b3ba50e-9082-4dd9-b8f1-e4b24280c4e1"; // "1affca8b-a48a-45a5-acdd-4ca36abb3d5b"; // "f5ddaa5e-d521-44d5-95f0-5a6327d1ae9a";//"ec49c239-177a-4b48-b4d7-fa380ef17c5b"; //"e6ee43eb-0fbc-4546-bc52-4c161fcdf4c4";// 
        public static PublicClientApplication PublicClientApp = new PublicClientApplication(ClientId);

        //Set the API Endpoint to Graph 'me' endpoint
        string _graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";

        //Set the scope for API call to user.read
        string[] _scopes = new string[] { "user.read" };

        private enum AADAction
        {
            SignIn, SignOut, ForceSignIn
        }

        /// <summary>
        /// Call AcquireTokenAsync - to acquire a token requiring user to sign-in
        /// </summary>
        private async Task<bool> MSALWork(AADAction action)
        {
            bool _result = false;
            AuthenticationResult authResult = null;

            if (action == AADAction.SignIn | action == AADAction.ForceSignIn)
            {
                try
                {
                    authResult = await PublicClientApp.AcquireTokenAsync(_scopes);
                }
                catch (MsalUiRequiredException ex)
                {
                    // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                    DbgLog($"MsalUiRequiredException: {ex.Message}");

                    if (action == AADAction.ForceSignIn)
                    {
                        try
                        {
                            authResult = await PublicClientApp.AcquireTokenAsync(_scopes);
                        }
                        catch (MsalException msalex)
                        {
                            DbgLog($"Error Acquiring Token:{System.Environment.NewLine}{msalex}");
                        }
                    }

                }
                catch (Exception ex)
                {
                    BuddyInsights.AIClient.TrackException(new Exception("Error Acquiring Token Silently", ex));
                    return false;
                }

                if (PublicClientApp.Users.Count() > 0)
                {
                    //BuddyOptions.authResult = BuddyOptions.authResult;
                    _result = true;

                    //also, update the Application Insights info with the authenticated user
                    BuddyInsights.AIClient.Context.User.Id = authResult.User.DisplayableId.Split('@')[0];
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
                        DbgLog($"Error signing-out user: {ex.Message}");
                        BuddyInsights.AIClient.TrackException(new Exception($"Error signing-out user: {ex.Message}", ex));
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
        public async Task<string> GetHttpContentWithToken(string url, string token)
        {
            var httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response;
            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                //Add the token in Authorization header
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }

}
