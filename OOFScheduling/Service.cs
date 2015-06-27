﻿using System; 
using System.Net; 
using Microsoft.Exchange.WebServices.Data; 
 
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
                prompt.Title = "Save credentials for Recurring OOF";

                prompt.ExcludeCertificates = true;
                prompt.GenericCredentials = true;

                if (System.Windows.Forms.DialogResult.OK == prompt.ShowDialog(/* owner */))
                {

                    if (prompt.ExpectConfirmation && prompt.SaveChecked)
                    {
                        prompt.ConfirmCredentials();
                    }

                    Instance.Credentials = new System.Net.NetworkCredential(prompt.UserName, prompt.Password);

                    if (userData.AutodiscoverUrl == null)
                    {
                        Console.Write(string.Format("Using Autodiscover to find EWS URL for {0}. Please wait... ", prompt.UserName));
                        Instance.TraceEnabled = true;
                        Instance.AutodiscoverUrl(prompt.UserName, Service.RedirectionUrlValidationCallback);
                        Console.WriteLine("Autodiscover Complete");
                    }
                    else
                    {
                        Instance.Url = userData.AutodiscoverUrl;
                    }

                    //to make it easier to grap, let's populate the User object
                    Exchange101.UserData.user.AutodiscoverUrl = Instance.Url;
                    Exchange101.UserData.user.EmailAddress = prompt.UserName;
                    Exchange101.UserData.user.Password = prompt.Password;

                }
            }

 
            return Instance; 
        }


    }

        

    }

