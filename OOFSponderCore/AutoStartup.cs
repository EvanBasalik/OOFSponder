using ClickOnceHelper;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;

namespace OOFScheduling
{
    internal static class AutoStartup
    {

        //TODO: really shouldn't be hard-coded, but being lazy here
        private static readonly string PublisherName = "Evan Basalik";
        private static readonly string ProductName = "OOFSponder";
        private static readonly string ApprefExtension = ".appref-ms";
        private static readonly string LauncherExecutableName = "OOFSponderLauncher.exe";
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

        /// <summary>
        /// Gets the path to the OOFSponderLauncher.exe in the application directory
        /// </summary>
        /// <returns>Fully-qualified path of OOFSponderLauncher.exe</returns>
        private static string GetLauncherPath()
        {
            var appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(appDirectory, LauncherExecutableName);
        }

        public static void AddToStartup(bool UseRegistry = false)
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
                //this time through, just update the registry key
                //Launcher will download the necessary bits and then be prepped
                //for the next startup to switch over to the standalone deployment. This is because we need a transition period where we support both, and the easiest way to do that is to have the launcher handle the transition

                OOFSponder.Logger.Info("Adding launcher to startup to switch to standalone deployment");
                string launcherPath = GetLauncherPath();

                if (!File.Exists(launcherPath))
                {
                    OOFSponder.Logger.Error($"Launcher not found at: {launcherPath}");
                    return;
                }

                // Attempt to open the registry key
                // The path to the key where Windows looks for startup applications
                string targetKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(targetKey, true))
                {
                    var currentValue = key.GetValue(keyName)?.ToString();
                    
                    //if the value equals what we want, don't touch it
                    //otherwise, set it
                    if (currentValue != launcherPath)
                    {
                        OOFSponder.Logger.Info("Not set in registry. Setting...");
                        key.SetValue(keyName, $"\"{launcherPath}\"");
                        OOFSponder.Logger.Info($"Set registry key {keyName}: {launcherPath}");
                    }
                    else
                    {
                        OOFSponder.Logger.Info("Registry startup entry already correct");
                    }
                } // The 'using' statement ensures the key is properly closed
            }

            //we are moving away from ClickOnce. All the logic for this is encapsulated in
            //OOFSponderLauncher, so all we have to do is launch that EXE on startup, and it will handle the rest (including uninstalling the ClickOnce version if it exists)


        }

    }
}
