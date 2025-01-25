using Microsoft.Graph;
using OOFSponder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOFScheduling
{
    public static class SettingsHelpers
    {
        //user preferences and OOF messages folder
        internal static string PerUserDataFolder()
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            string _AppDataRoamingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OOFSponder");

            //check to make sure it exists. If not, create AppData\Roaming\OOFSponder
            try
            {
#if NET8_0_OR_GREATER
                if (!Path.Exists(_AppDataRoamingFolder))
#endif
#if NETFRAMEWORK
                if (!System.IO.Directory.Exists(_AppDataRoamingFolder))
#endif
                {
                    System.IO.Directory.CreateDirectory(_AppDataRoamingFolder);
                }
            }
            catch (Exception ex)
            {
                string _errorMessage = "Unable to create " + _AppDataRoamingFolder;
                Logger.Error(_errorMessage, ex);
                throw new Exception(_errorMessage, ex);
            }

            return _AppDataRoamingFolder;
        }

        internal static string BaseSettingsFile()
        {
            return Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        }

        //user preferences settings file
        internal static string PerUserSettingsFile()
        {
            return "usersettings.json";
        }

    }
}
