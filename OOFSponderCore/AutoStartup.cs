using ClickOnceHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOFScheduling
{
    internal static class AutoStartup
    {

        //TODO: really shouldn't be hard-coded, but being lazy here
        private static readonly string PublisherName = "Evan Basalik";
        private static readonly string ProductName = "OOFSponder";
        private static readonly string ApprefExtension = ".appref-ms";
        private static readonly string keyName = ProductName;

        /// <summary>
        /// Finds the .appref-ms in Programs
        /// </summary>
        /// <returns>Fully-qualified path of .appref-ms</returns>
        private static string GetShortcutPath()
        {
            var allProgramsPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            var shortcutPath = Path.Combine(allProgramsPath, PublisherName);
            return Path.Combine(shortcutPath, ProductName) + ApprefExtension;
        }

        /// <summary>
        /// Finds the location of the Startup folder and appends the .appref-ms
        /// </summary>
        /// <returns>Fully-qualified path of Startup folder and .appref-ms</returns>
        private static string GetStartupShortcutPath()
        {
            var startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            return Path.Combine(startupPath, ProductName) + ApprefExtension;
        }

        public static void AddToStartup(bool UseRegistry=false)
        {
            OOFSponder.Logger.Info("UseRegistry: " + UseRegistry);

            if (!UseRegistry)
            {
                if (!ApplicationDeployment.IsNetworkDeployed)
                {
                    OOFSponder.Logger.Info("IsNetworkDeployed: False, so not adding to Startup");
                    return;
                }

                var startupPath = GetStartupShortcutPath();
                if (File.Exists(startupPath))
                {
                    OOFSponder.Logger.Info("Already exists in Startup");
                    return;
                }

                OOFSponder.Logger.Info("Doesn't exist in Startup. Copying over...");
                File.Copy(GetShortcutPath(), startupPath);
                OOFSponder.Logger.Info("Copied into Startup");
            }
            else
            {
                string startPath = GetShortcutPath();

                // Attempt to open the registry key
                // The path to the key where Windows looks for startup applications
                string targetKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(targetKey, true))
                {
                    //if the value equals what we want, don't touch it
                    //otherwise, set it
                    if (key.GetValue(keyName).ToString() != startPath)
                    {
                        OOFSponder.Logger.Info("Not set in registry. Setting...");
                        key.SetValue(keyName, startPath);
                        OOFSponder.Logger.Info("Set registry key " + keyName + ":" + startPath);
                    }
                } // The 'using' statement ensures the key is properly closed
            }
        }

    }
}
