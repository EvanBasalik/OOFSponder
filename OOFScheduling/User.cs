﻿using System;
using System.Security;
using Microsoft.Exchange.WebServices.Data;

namespace Exchange101
{
    // This sample is for demonstration purposes only. Before you run this sample, make sure that the code meets the coding requirements of your organization. 
    public interface IUserData
    {
        ExchangeVersion Version { get; }
        string EmailAddress { get; }
        SecureString Password { get; }
        Uri AutodiscoverUrl { get; set; }
    }

    public class UserData : IUserData
    {
        public static UserData user = new UserData();
        public static string Target = "OOFSponder";

        public static IUserData GetUserData(ref ExchangeService service)
        {
            //if (user == null)
            //{
            //    user = new UserData();
            //}

            return user;
        }

        //internal static void GetUserfromCredMan(string EWSURL, ref ExchangeService service)
        //{
        //    //leverage the Kerr library to do the CredMan stuff
        //    using (Kerr.CredentialSet creds = new Kerr.CredentialSet(Target))
        //    {
        //        foreach (Kerr.Credential cred in creds)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Found the cred: " + cred.UserName);
        //            user.Password = cred.Password;
        //            user.EmailAddress = cred.UserName;
        //        }

        //    }

        //    if (user.EmailAddress==null)
        //    {
        //        //leverage the Kerr library to do the CredMan stuff
        //        using (Kerr.PromptForCredential prompt = new Kerr.PromptForCredential())
        //        {
        //            prompt.TargetName = Target;
        //            prompt.Title = "Save credentials for Recurring OOF";

        //            prompt.ExcludeCertificates = true;
        //            prompt.GenericCredentials = true;

        //            if (System.Windows.Forms.DialogResult.OK == prompt.ShowDialog(/* owner */))
        //            {

        //                if (prompt.ExpectConfirmation && prompt.SaveChecked)
        //                {
        //                    prompt.ConfirmCredentials();
        //                }

        //                user.Password = prompt.Password;
        //                user.EmailAddress = prompt.UserName;

        //                if (user.AutodiscoverUrl == null)
        //                {
        //                    Console.Write(string.Format("Using Autodiscover to find EWS URL for {0}. Please wait... ", prompt.UserName));

        //                    service.Credentials = new System.Net.NetworkCredential(user.EmailAddress, user.Password);
        //                    service.TraceEnabled = true;
        //                    service.AutodiscoverUrl(user.EmailAddress, Service.RedirectionUrlValidationCallback);
        //                    user.AutodiscoverUrl = service.Url;
        //                    Console.WriteLine("Autodiscover Complete");
        //                }

        //            }
        //        }
        //    }
        //}

        internal UserData GetUserfromCredMan(UserData user, ref ExchangeService service)
        {
            UserData _user = new UserData();
            _user.AutodiscoverUrl = user.AutodiscoverUrl;
            //leverage the Kerr library to do the CredMan stuff
            using (Kerr.CredentialSet creds = new Kerr.CredentialSet(Target))
            {
                foreach (Kerr.Credential cred in creds)
                {
                    System.Diagnostics.Debug.WriteLine("Found the cred: " + cred.UserName);
                    _user.Password = cred.Password;
                    _user.EmailAddress = cred.UserName;
                }

            }

            if (_user.EmailAddress == null)
            {
                //leverage the Kerr library to do the CredMan stuff
                using (Kerr.PromptForCredential prompt = new Kerr.PromptForCredential())
                {
                    prompt.TargetName = Target;
                    prompt.Title = "Please enter your email address and password for OOFSPonder";

                    prompt.ExcludeCertificates = true;
                    prompt.GenericCredentials = true;

                    if (System.Windows.Forms.DialogResult.OK == prompt.ShowDialog(/* owner */))
                    {

                        if (prompt.ExpectConfirmation && prompt.SaveChecked)
                        {
                            prompt.ConfirmCredentials();
                        }

                        _user.Password = prompt.Password;
                        _user.EmailAddress = prompt.UserName;

                        if (service.Url == null)
                        {
                            Console.Write(string.Format("Using Autodiscover to find EWS URL for {0}. Please wait... ", prompt.UserName));

                            service.Credentials = new System.Net.NetworkCredential(user.EmailAddress, user.Password);
                            service.TraceEnabled = true;
                            service.AutodiscoverUrl(user.EmailAddress, Service.RedirectionUrlValidationCallback);
                            _user.AutodiscoverUrl = service.Url;
                            Console.WriteLine("Autodiscover Complete");
                        }
                    }
                }
            }

            return _user;

        }

        public ExchangeVersion Version { get { return ExchangeVersion.Exchange2013; } }

        public string EmailAddress
        {
            get;
            set;
        }

        public SecureString Password
        {
            get;
            set;
        }

        public Uri AutodiscoverUrl
        {
            get;
            set;
        }
    }
}
