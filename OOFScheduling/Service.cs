﻿using System; 
using System.Net; 
using Microsoft.Exchange.WebServices.Data;
using System.Windows.Forms;
 
namespace Exchange101 
{ 
    // This sample is for demonstration purposes only. Before you run this sample, make sure that the code meets the coding requirements of your organization. 
    public static class Service 
    {
        public static ExchangeService Instance = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
        public static string Target = "OOFSponder";

        static Service() 
        { 
            CertificateCallback.Initialize(); 
        } 
 
        // The following is a basic redirection validation callback method. It  
        // inspects the redirection URL and only allows the Service object to  
        // follow the redirection link if the URL is using HTTPS.  
        // 
        // This redirection URL validation callback provides sufficient security 
        // for development and testing of your application. However, it may not 
        // provide sufficient security for your deployed application. You should 
        // always make sure that the URL validation callback method that you use 
        // meets the security requirements of your organization. 
        internal static bool RedirectionUrlValidationCallback(string redirectionUrl) 
        { 
            // The default for the validation callback is to reject the URL. 
            bool result = false; 
 
            Uri redirectionUri = new Uri(redirectionUrl); 
 
            // Validate the contents of the redirection URL. In this simple validation 
            // callback, the redirection URL is considered valid if it is using HTTPS 
            // to encrypt the authentication credentials.  
            if (redirectionUri.Scheme == "https") 
            { 
                result = true; 
            } 
 
            return result; 
        } 

        //clear the stored credentials
        public static bool ClearCredentaials()
        {
            bool _result = false;

            Kerr.CredentialSet creds = new Kerr.CredentialSet(Target);
            foreach (Kerr.Credential cred in creds)
            {
                if (cred.TargetName==Target)
                {
                    cred.Delete();
                    _result = true;
                }
            }

            return _result;
        }

        public static ExchangeService ConnectToService(bool traceToFile) 
        { 
            // We use this to get the target Exchange version.  
            UserData userData = new UserData(); 
 
            ExchangeService service = new ExchangeService(userData.Version); 
            //service.PreAuthenticate = true; 
 
            if (traceToFile) 
                service.TraceListener = new TraceListener(); 
            else 
            { 
                service.TraceEnabled = true; 
                service.TraceFlags = TraceFlags.All; 
                service.TraceEnablePrettyPrinting = true; 
            }

            ConnectToService(userData, null);
 
            return service; 
        } 


        public static ExchangeService ConnectToService(UserData userData) 
        { 
            return ConnectToService(userData, null); 
        } 
 
        public static ExchangeService ConnectToService(UserData userData, ITraceListener listener) 
        {
             
            if (listener != null) 
            { 
                Instance.TraceListener = listener; 
                Instance.TraceFlags = TraceFlags.All; 
                Instance.TraceEnabled = true; 
            }

            using (Kerr.PromptForCredential prompt = new Kerr.PromptForCredential())
            {
                prompt.TargetName = Target;
                prompt.Title = "Please enter your email address and password";

                prompt.ExcludeCertificates = true;
                prompt.GenericCredentials = true;
                prompt.ExpectConfirmation = true;

                if (System.Windows.Forms.DialogResult.OK == prompt.ShowDialog(/* owner */))
                {

                    Instance.Credentials = new System.Net.NetworkCredential(prompt.UserName, prompt.Password);

                    if (userData.AutodiscoverUrl == null)
                    {
                        Console.Write(string.Format("Using Autodiscover to find EWS URL for {0}. Please wait... ", prompt.UserName));
                        Instance.TraceEnabled = true;
                        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                        {
                            try
                            {
                                Instance.AutodiscoverUrl(prompt.UserName, Service.RedirectionUrlValidationCallback);
                            }
                            catch (AutodiscoverLocalException ex)
                            {
                                throw;
                            }
                            catch (System.FormatException ex)
                            {
                                //if we catch a FormatException, that means that the
                                //username wasn't entered as a UPN, so we cannot figure out the email format
                                DialogResult result = MessageBox.Show("Please enter your username in name@domain.com format", "OOFSponder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                //clear the credentials
                                Exchange101.Service.ClearCredentaials();
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                        else
                        {
                            throw new AutodiscoverLocalException("No network detected");
                        }
                        Console.WriteLine("Autodiscover Complete");
                    }
                    else
                    {
                        Instance.Url = userData.AutodiscoverUrl;
                    }

                    //if we have the URL, attempt to authenticate
                    Authenticate(prompt);

                }
            }

 
            return Instance; 
        }

        public static void Authenticate(Kerr.PromptForCredential prompt=null)
        {
            // Once we have the URL, try a ConvertId operation to check if we can access the service. We expect that 
            // the user will be authenticated and that we will get an error code due to the invalid format. Expect a
            // ServiceResponseException. 
            try
            {
                Console.WriteLine("Attempting to connect to EWS...");
                AlternateIdBase response = Service.Instance.ConvertId(new AlternateId(IdFormat.EwsId, "Placeholder", prompt.UserName), IdFormat.EwsId);
            }
            catch (ServiceResponseException)
            {
                //if we get a ServiceResponseException, that means we authenticated
                //since we were able to authenticate, store the credentials
                if (prompt.SaveChecked)
                {
                    prompt.ConfirmCredentials();
                }

                //to make it easier to grap, let's populate the User object
                Exchange101.UserData.user.AutodiscoverUrl = Instance.Url;
                Exchange101.UserData.user.EmailAddress = prompt.UserName;
                Exchange101.UserData.user.Password = prompt.Password;
            }
            catch (Exception ex)
            {
                //if we get an authentication exception, that means the URL was correct
                //but the credentials were not
                DialogResult result = MessageBox.Show("Credentials incorrect. Do you want OOFSponder to delete the credentials from Credential Manager?", "OOFSponder", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.Yes)
                {
                    Exchange101.Service.ClearCredentaials();
                }
                throw new System.Security.Authentication.AuthenticationException("Unable to login", ex);
            }

        }


    }

        

    }

