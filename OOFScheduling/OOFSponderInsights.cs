using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace OOFScheduling
{
    internal static class OOFSponderInsights
    {

        private const string InfoEvent = "INFO";
        private const string AppName = "OOFSponder";

        //prep for ApplicationInsights right away so we can even instrument startup
        private static TelemetryClient appInsightsClient;

        public static void Track(string eventName)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            Track(eventName, properties);
        }

        public static void TrackInfo(string Details)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("Details", Details);
            Track(InfoEvent, properties);
        }

        public static void Track(string eventName, Dictionary<string, string> properties)
        {
            properties.Add("Application", AppName);
            AIClient.TrackEvent(eventName, properties);
        }

        public static void TrackException(String message, Exception exception)
        {
            Dictionary<string, string> _properties = new Dictionary<string, string>();

            Exception _exception = new Exception(message + ": " + exception.Message, exception);
            _properties.Add("Message", _exception.Message);
            _properties.Add("CallStack", _exception.StackTrace);

            Track("Exception", _properties);
        }

        internal static string CurrentMethod([CallerMemberName] string name = "")
        {
            return name;
        }

        public static TelemetryClient AIClient
        {
            get
            {
                if (appInsightsClient == null)
                {
                    appInsightsClient = new TelemetryClient();

                    //go set the application key, etc.
                    ConfigureApplicationInsights();
                }
                return appInsightsClient;
            }
        }

        private static string _userGUID;
        public static string UserGUID {
            get
            { 
                return _userGUID;
            }
          
            
            set
            {
                _userGUID = value;
                appInsightsClient.Context.User.Id = value;
            } 
        }


        public static bool ConfigureApplicationInsights()
        {
            bool isConfigured = false;

            try
            {

                AIClient.InstrumentationKey = "e4249f05-e5b1-4723-b425-1ffc634f5623";

#if DEBUG
                Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = true;
#endif

                AIClient.Context.Properties["MachineName"] = Environment.MachineName;
                AIClient.Context.Properties["Version"] = OOFData.version;

                isConfigured = true;
                OOFSponder.Logger.Info("Successfully configured ApplicationInsights");
                OOFSponder.Logger.Info("MachineName: " + Environment.MachineName);
                OOFSponder.Logger.Info("Version: " + OOFData.version);
            }
            catch (Exception ex)
            {
                OOFSponder.Logger.Error("Unable to configure ApplicationInsights: " + ex.Message);
            }

            return isConfigured;

        }
    }
}
