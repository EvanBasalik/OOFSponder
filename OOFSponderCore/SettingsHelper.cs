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

        //https://stackoverflow.com/questions/60832072/how-to-write-data-to-appsettings-json-in-a-console-application-net-core
        public static void AddOrUpdateAppSetting<T>(string sectionPathKey, T value, bool isUserSetting = true)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            try
            {
                //but if setting a user setting, then switch to AppData/LocalRoaming and per user settings file
                string _targetFile = BaseSettingsFile();
                if (isUserSetting)
                {
                    _targetFile = Path.Combine(PerUserDataFolder(), PerUserSettingsFile());
                }

                //make sure the file exists
                //if not, copy appsettings.json over
                //as usersettings.json
#if NET8_0_OR_GREATER
                if (!Path.Exists(_targetFile))
#endif
#if NETFRAMEWORK
                if (!System.IO.File.Exists(_targetFile))
#endif
                {
                    System.IO.File.Copy(BaseSettingsFile(), _targetFile);
                }

                string json = System.IO.File.ReadAllText(_targetFile);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                Logger.Info("Successfully deserialized json");

                SetValueRecursively(sectionPathKey, jsonObj, value);
                Logger.Info("Successfully SetValueRecursively");

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                Logger.Info("Successfully serialized json");

                System.IO.File.WriteAllText(_targetFile, output);
                Logger.Info("Successfully updated settings file");

            }
            catch (Exception ex)
            {
                Logger.Error("Error persisting setting for " + sectionPathKey + ":" + ex.Message);
            }
        }

        private static void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObj, T value)
        {

            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            // split the string at the first ':' character
#if NET8_0_OR_GREATER
            var remainingSections = sectionPathKey.Split(":", 2);
#endif
#if NETFRAMEWORK
            char[] delimiter = {':'};
            var remainingSections = sectionPathKey.Split(delimiter, 2);
#endif
            Logger.Info("remainingSections.Length: " + remainingSections.Length.ToString());

            var currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                // continue with the procress, moving down the tree
                var nextSection = remainingSections[1];
                SetValueRecursively(nextSection, jsonObj[currentSection], value);
            }
            else
            {
                // we've got to the end of the tree, set the value
                jsonObj[currentSection] = value;
            }
        }
    }
}
