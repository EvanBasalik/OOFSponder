using OOFSponder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace OOFScheduling
{
    /// <summary>
    /// Provides helpers for detecting and managing cloud storage paths for OOFSponder settings.
    /// The user's choice of cloud storage folder is persisted locally in cloudconfig.json so that
    /// the settings file itself can live in a synced cloud folder (OneDrive, Dropbox, Google Drive, etc.).
    /// </summary>
    public static class CloudStorageHelper
    {
        private const string CloudConfigFileName = "cloudconfig.json";
        private const string OOFSponderFolderName = "OOFSponder";

        /// <summary>
        /// Path to the local file that records the user's chosen cloud storage path.
        /// This file always lives in the local AppData folder (not in the cloud).
        /// </summary>
        private static string CloudConfigFilePath =>
            Path.Combine(Program.AppDataRoamingFolder, CloudConfigFileName);

        /// <summary>
        /// Represents a detected cloud storage provider.
        /// </summary>
        public class CloudProvider
        {
            public string Name { get; set; }
            public string BasePath { get; set; }

            /// <summary>
            /// Suggested OOFSponder subfolder within this cloud provider's path.
            /// </summary>
            public string OOFSponderPath => Path.Combine(BasePath, OOFSponderFolderName);
        }

        /// <summary>
        /// Detects common cloud storage providers that are available on this machine.
        /// </summary>
        public static List<CloudProvider> DetectCloudProviders()
        {
            var providers = new List<CloudProvider>();
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // OneDrive Personal - environment variable is the most reliable source
            string oneDrivePath = Environment.GetEnvironmentVariable("OneDrive")
                               ?? Environment.GetEnvironmentVariable("OneDriveConsumer");
            if (string.IsNullOrEmpty(oneDrivePath))
            {
                oneDrivePath = Path.Combine(userProfile, "OneDrive");
            }
            if (Directory.Exists(oneDrivePath))
            {
                providers.Add(new CloudProvider { Name = "OneDrive", BasePath = oneDrivePath });
            }

            // OneDrive for Business
            string oneDriveBusinessPath = Environment.GetEnvironmentVariable("OneDriveCommercial");
            if (!string.IsNullOrEmpty(oneDriveBusinessPath) && Directory.Exists(oneDriveBusinessPath)
                && oneDriveBusinessPath != oneDrivePath)
            {
                providers.Add(new CloudProvider { Name = "OneDrive for Business", BasePath = oneDriveBusinessPath });
            }

            // Dropbox
            string dropboxPath = GetDropboxPath();
            if (!string.IsNullOrEmpty(dropboxPath) && Directory.Exists(dropboxPath))
            {
                providers.Add(new CloudProvider { Name = "Dropbox", BasePath = dropboxPath });
            }

            // Google Drive
            string googleDrivePath = GetGoogleDrivePath();
            if (!string.IsNullOrEmpty(googleDrivePath) && Directory.Exists(googleDrivePath))
            {
                providers.Add(new CloudProvider { Name = "Google Drive", BasePath = googleDrivePath });
            }

            return providers;
        }

        private static string GetDropboxPath()
        {
            // Check the Dropbox info file first (most reliable)
            string[] infoFileCandidates =
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Dropbox", "info.json"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Dropbox", "info.json"),
            };

            foreach (var infoFile in infoFileCandidates)
            {
                if (File.Exists(infoFile))
                {
                    try
                    {
                        string json = File.ReadAllText(infoFile);
                        using var doc = JsonDocument.Parse(json);
                        if (doc.RootElement.TryGetProperty("personal", out var personal) &&
                            personal.TryGetProperty("path", out var pathProp))
                        {
                            string path = pathProp.GetString();
                            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                                return path;
                        }
                    }
                    catch
                    {
                        // Fall through to path-based detection
                    }
                }
            }

            // Fall back to common path locations
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] commonPaths =
            {
                Path.Combine(userProfile, "Dropbox"),
            };

            foreach (var path in commonPaths)
            {
                if (Directory.Exists(path))
                    return path;
            }

            return null;
        }

        private static string GetGoogleDrivePath()
        {
            // Google Drive for Desktop creates virtual drives; check common locations
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] commonPaths =
            {
                Path.Combine(userProfile, "Google Drive"),
                Path.Combine(userProfile, "GoogleDrive"),
            };

            foreach (var path in commonPaths)
            {
                if (Directory.Exists(path))
                    return path;
            }

            // Google Drive for Desktop mounts a virtual drive (commonly G:\My Drive)
            bool hasGoogleDriveApp = Directory.Exists(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "DriveFS"));

            if (hasGoogleDriveApp)
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        string myDrivePath = Path.Combine(drive.Name, "My Drive");
                        if (Directory.Exists(myDrivePath))
                            return myDrivePath;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the currently configured cloud storage path.
        /// Returns <c>null</c> if no cloud storage has been configured.
        /// </summary>
        public static string GetCloudStoragePath()
        {
            try
            {
                if (!File.Exists(CloudConfigFilePath))
                    return null;

                string json = File.ReadAllText(CloudConfigFilePath);
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("CloudStoragePath", out var pathProp))
                {
                    string path = pathProp.GetString();
                    return string.IsNullOrWhiteSpace(path) ? null : path;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error reading cloud storage config: " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Saves the cloud storage path preference to the local config file.
        /// Pass <c>null</c> or an empty string to clear cloud storage and revert to local AppData.
        /// </summary>
        public static void SetCloudStoragePath(string path)
        {
            try
            {
                var config = new { CloudStoragePath = path ?? string.Empty };
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(CloudConfigFilePath, json);
                Logger.Info("Cloud storage path updated to: " + (string.IsNullOrEmpty(path) ? "(local only)" : path));
            }
            catch (Exception ex)
            {
                Logger.Error("Error saving cloud storage config: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the folder where OOFSponder should read and write user settings.
        /// Uses the configured cloud storage path when available; falls back to local AppData.
        /// </summary>
        public static string GetEffectiveSettingsFolder()
        {
            string cloudPath = GetCloudStoragePath();
            if (!string.IsNullOrEmpty(cloudPath))
            {
                try
                {
                    if (!Directory.Exists(cloudPath))
                    {
                        Directory.CreateDirectory(cloudPath);
                        Logger.Info("Created cloud storage folder: " + cloudPath);
                    }
                    return cloudPath;
                }
                catch (Exception ex)
                {
                    Logger.Error("Cannot access cloud storage path '" + cloudPath + "', falling back to local AppData: " + ex.Message);
                }
            }

            return Program.AppDataRoamingFolder;
        }

        /// <summary>
        /// Returns a human-readable description of the current effective settings location.
        /// </summary>
        public static string GetEffectiveSettingsFolderDescription()
        {
            string cloudPath = GetCloudStoragePath();
            if (!string.IsNullOrEmpty(cloudPath))
            {
                return $"Cloud storage: {cloudPath}";
            }
            return $"Local storage: {Program.AppDataRoamingFolder}";
        }
    }
}
