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
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OOFSponder\\");
        }

        //user preferences settings file
        internal static string PerUserSettingsFile()
        {
            return "usersettings.json";
        }

        //https://stackoverflow.com/questions/60832072/how-to-write-data-to-appsettings-json-in-a-console-application-net-core
        public static void AddOrUpdateAppSetting<T>(string sectionPathKey, T value, bool isUserSetting=true)
        {
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
                if (!Path.Exists(_targetFile))
                {
                    System.IO.File.Copy(BaseSettingsFile(), _targetFile);
                }

                string json = File.ReadAllText(filePath);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                SetValueRecursively(sectionPathKey, jsonObj, value);

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, output);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing app settings | {0}", ex.Message);
            }
        }

        private static void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObj, T value)
        {
            // split the string at the first ':' character
            char[] delimiterChars = { ':'};
            var remainingSections = sectionPathKey.Split(delimiterChars);

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
