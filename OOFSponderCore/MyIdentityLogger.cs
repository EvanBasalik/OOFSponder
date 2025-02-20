using Microsoft.IdentityModel.Abstractions;
using System;

namespace OOFScheduling
{
    internal class MyIdentityLogger : IIdentityLogger
    {
        public EventLogLevel MinLogLevel { get; }

        public MyIdentityLogger()
        {
            //Retrieve the log level from an environment variable
            //https://learn.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.abstractions.eventloglevel?view=msal-web-dotnet-latest
            //setx MSAL_LOG_LEVEL "VERBOSE"  --> enable enhanced logging
            //reg delete "HKEY_CURRENT_USER\Environment" /f /v MSAL_LOG_LEVEL  --> remove environment variable
            //Get-ChildItem Env:* | Where-Object {$_.Name -eq "MSAL_LOG_LEVEL"}   --> list in PowerShell
            //echo %MSAL_LOG_LEVEL%  --> list in command prompt

            var msalEnvLogLevel = Environment.GetEnvironmentVariable("MSAL_LOG_LEVEL");

            //if not set, then default to Informational
            if (msalEnvLogLevel == null)
            {
                msalEnvLogLevel = EventLogLevel.Error.ToString();
            }

            //parse the environment variable
            if (Enum.TryParse(msalEnvLogLevel, out EventLogLevel msalLogLevel))
            {
                MinLogLevel = msalLogLevel;
            }

            OOFSponder.Logger.Info("MSAL_LOG_LEVEL: " + msalLogLevel);
        }

        public bool IsEnabled(EventLogLevel eventLogLevel)
        {
            return eventLogLevel <= MinLogLevel;
        }

        public void Log(LogEntry entry)
        {
            //call into OOFSponder logger
            //safe to do since we don't ever log PII
            //however, we only want to log to the log file
            //and not AppInsights to be sure
            OOFSponder.Logger.Info(entry.Message, false);
        }
    }
}
