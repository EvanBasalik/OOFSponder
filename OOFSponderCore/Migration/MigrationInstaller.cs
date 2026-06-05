using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOFSponderCore.Migration
{
    /// <summary>
    /// One-shot migrator that moves a user from the GitHub ClickOnce install of
    /// OOFSponder (the <c>OOFSponderCore</c> project published via
    /// <c>SupportingFiles\release.ps1</c> + <c>ClickOnceProfile</c>) to the new
    /// <c>OOFSponderLauncher</c>-based CDN install. Designed to be called from
    /// <c>Program.Main</c> in the GitHub <c>OOFSponderCore</c> app, before
    /// <c>Application.Run(new MainForm())</c>. Safe to call on every launch: it
    /// no-ops after the first successful migration.
    ///
    /// Note: <c>OOFSponderCore</c> already persists user state to
    /// <c>%AppData%\OOFSponder\usersettings.json</c> using the same
    /// <c>OOFSponderConfig.Root</c> schema the launcher consumes, so no settings
    /// translation is performed by this migrator. The existing file is left in
    /// place and read by the new install on first run.
    /// </summary>
    internal static class MigrationInstaller
    {
        private const string AppName = "OOFSponder";
        private const string LauncherExeName = "OOFSponderLauncher.exe";
        private const string MigratedMarkerFileName = "migrated.json";
        private const string DontAskAgainMarkerFileName = "migration.dontask";
        private const string RunKeyName = "OOFSponder";
        private const string RunKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string UninstallKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        // The launcher bootstrap always comes from PROD CDN, regardless of core ring.
        // Core ring still follows the legacy ClickOnce ring and is written to appsettings.
        private const string LauncherBootstrapRing = "production";

        // The legacy ClickOnce shortcut the GitHub build drops here:
        //   %AppData%\Microsoft\Windows\Start Menu\Programs\Evan Basalik\OOFSponder.appref-ms
        // (Publisher folder matches OOFSponderCore/AutoStartup.cs PublisherName.)
        private const string LegacyApprefRelativePath = @"Evan Basalik\OOFSponder.appref-ms";

        // Minimum schema version we know how to read. Bumping this in the new launcher
        // forces a re-migration even if migrated.json already exists.
        private const int MigrationSchemaVersion = 1;

        public static async Task<bool> RunIfNeededAsync()
        {
            try
            {
                if (!IsClickOnceInstall())
                {
                    SafeLog("Migration: not a ClickOnce install; skipping migration.");
                    return false;
                }

                var appDataDir = GetAppDataDir();
                Directory.CreateDirectory(appDataDir);

                if (HasMigratedMarker(appDataDir))
                {
                    SafeLog("Migration: migrated marker found; migration already complete.");
                    return false;
                }

                if (HasDontAskAgainMarker(appDataDir))
                {
                    SafeLog("Migration: don't-ask-again marker found; skipping migration.");
                    return false;
                }

                var coreRing = ResolveRingFromAppref();
                SafeLog("Migration: resolved core ring '" + coreRing + "'.");

                var coreCdnBase = MigrationRingResolver.GetCdnBaseUrlForRing(coreRing);
                SafeLog("Migration: core CDN base URL is '" + coreCdnBase + "'.");

                SafeLog("Migration: downloading launcher manifest from production CDN.");
                var manifest = await DownloadManifestAsync(MigrationRingResolver.ProductionCdnBaseUrl).ConfigureAwait(true);
                var launcherEntry = MigrationRingResolver.SelectLauncherEntry(manifest, LauncherBootstrapRing);
                if (launcherEntry == null || string.IsNullOrWhiteSpace(launcherEntry.Version))
                {
                    SafeLog("Migration: no launcher manifest entry for ring '" + LauncherBootstrapRing + "'.");
                    return false;
                }
                SafeLog("Migration: launcher manifest entry found; version='" + launcherEntry.Version + "'.");

                var prompt = MigrationPrompt.Show(launcherEntry.Version, coreRing);
                if (prompt == MigrationPromptResult.DontAskAgain)
                {
                    SafeLog("Migration: user chose don't-ask-again.");
                    WriteDontAskAgainMarker(appDataDir);
                    return false;
                }
                if (prompt != MigrationPromptResult.Yes)
                {
                    SafeLog("Migration: user declined migration prompt.");
                    return false;
                }
                SafeLog("Migration: user accepted migration prompt.");

                var installDir = GetTargetInstallDir();
                SafeLog("Migration: target install dir is '" + installDir + "'.");
                EnsureCleanTargetDir(installDir);

                var launcherUrl = ResolveLauncherUrl(MigrationRingResolver.ProductionCdnBaseUrl, launcherEntry);
                if (string.IsNullOrWhiteSpace(launcherUrl))
                {
                    SafeLog("Migration: launcher entry has no usable download URL.");
                    return false;
                }
                SafeLog("Migration: downloading launcher from '" + launcherUrl + "'.");

                var launcherBytes = await DownloadAndVerifyLauncherAsync(launcherUrl, launcherEntry.Sha256Hash).ConfigureAwait(true);
                if (launcherBytes == null)
                {
                    return false;
                }
                SafeLog("Migration: launcher downloaded and SHA-256 verified (" + launcherBytes.Length + " bytes).");

                var launcherDest = Path.Combine(installDir, LauncherExeName);
                File.WriteAllBytes(launcherDest, launcherBytes);
                SafeLog("Migration: launcher written to '" + launcherDest + "'.");

                WriteAppSettingsJson(installDir, coreRing, coreCdnBase);
                SafeLog("Migration: appsettings.json written.");

                // No user-settings translation is required: OOFSponderCore already
                // persists user state to %AppData%\OOFSponder\usersettings.json using
                // the same OOFSponderConfig.Root schema the launcher consumes. The
                // file is left in place and picked up by the new install as-is.

                RepointStartupToLauncher(installDir);
                SafeLog("Migration: startup registry key re-pointed to launcher.");

                RemoveLegacyApprefShortcut();
                SafeLog("Migration: legacy appref-ms shortcut removed.");

                TryUninstallClickOnce();
                SafeLog("Migration: ClickOnce uninstall triggered.");

                WriteMigratedMarker(appDataDir, coreRing, launcherEntry.Version, installDir);
                SafeLog("Migration: migrated marker written.");

                StartLauncher(installDir);
                SafeLog("Migration: launcher process started; requesting app exit.");

                // Ask the legacy app to exit so the launcher can take over cleanly.
                Application.Exit();
                return true;
            }
            catch (Exception ex)
            {
                SafeLog("Migration failed: " + ex);
                return false;
            }
        }

        // ---- detection ----------------------------------------------------------------

        /// <summary>
        /// Returns <c>true</c> when the running assembly lives under the per-user ClickOnce
        /// shadow-copy root (<c>%LocalAppData%\Apps\2.0\</c>). Avoids referencing
        /// <c>System.Deployment.Application</c> so we don't add a framework reference to the
        /// legacy csproj just for detection.
        /// </summary>
        private static bool IsClickOnceInstall()
        {
            try
            {
#if DEBUG
                return true; // skip detection logic in debug builds to allow testing migration flow without ClickOnce
#endif

                var exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                             ?? string.Empty;
                var clickOnceRoot = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    @"Apps\2.0");
                return exeDir.StartsWith(clickOnceRoot, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Reads the legacy <c>OOFSponder.appref-ms</c> from the user's Programs folder and
        /// looks at the embedded ClickOnce update URL to figure out which ring this user
        /// is on. The .appref-ms is a UTF-16 text file containing the deployment URL,
        /// which contains the ring name (matching <c>ClickOnceHelper.ApplicationDeployment.Ring</c>
        /// in the new code).
        /// </summary>
        private static string ResolveRingFromAppref()
        {
            try
            {
                var apprefPath = GetLegacyApprefPath();
                if (File.Exists(apprefPath))
                {
                    var contents = File.ReadAllText(apprefPath, System.Text.Encoding.Unicode);
                    return MigrationRingResolver.NormalizeRing(contents);
                }
            }
            catch
            {
                // fall through to default
            }
            return "production";
        }

        // ---- download / verify --------------------------------------------------------

        private static async Task<CdnMigrationManifest> DownloadManifestAsync(string cdnBase)
        {
            // Ensure modern TLS is enabled just in case the host machine defaults to
            // older protocols.
            try
            {
                ServicePointManager.SecurityProtocol |=
                    SecurityProtocolType.Tls12 | (SecurityProtocolType)0x3000 /* Tls13 if available */;
            }
            catch
            {
                // ignore - older Windows may not support Tls13 enum value
            }

            using (var http = new HttpClient { Timeout = TimeSpan.FromMinutes(2) })
            {
                var url = cdnBase.TrimEnd('/') + "/manifest/version.json";
                var json = await http.GetStringAsync(url).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<CdnMigrationManifest>(json);
            }
        }

        private static string ResolveLauncherUrl(string cdnBase, CdnMigrationLauncherEntry entry)
        {
            if (!string.IsNullOrWhiteSpace(entry.DownloadUrl))
            {
                return entry.DownloadUrl;
            }
            if (!string.IsNullOrWhiteSpace(entry.Path))
            {
                return cdnBase.TrimEnd('/') + "/" + entry.Path.TrimStart('/');
            }
            if (!string.IsNullOrWhiteSpace(entry.FileName))
            {
                return cdnBase.TrimEnd('/') + "/launcher/" + entry.FileName;
            }
            return null;
        }

        private static async Task<byte[]> DownloadAndVerifyLauncherAsync(string launcherUrl, string expectedSha256)
        {
            if (string.IsNullOrWhiteSpace(expectedSha256))
            {
                SafeLog("Migration: aborting launcher download. Manifest entry is missing Sha256Hash; refusing to install unverified launcher from " + launcherUrl);
                return null;
            }

            using (var http = new HttpClient { Timeout = TimeSpan.FromMinutes(10) })
            {
                var bytes = await http.GetByteArrayAsync(launcherUrl).ConfigureAwait(false);

                var actual = ComputeSha256Hex(bytes);
                if (!actual.Equals(expectedSha256, StringComparison.OrdinalIgnoreCase))
                {
                    SafeLog("Migration: launcher SHA-256 mismatch. Expected " + expectedSha256 + ", got " + actual);
                    return null;
                }

                return bytes;
            }
        }

        private static string ComputeSha256Hex(byte[] data)
        {
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(data);
                var sb = new System.Text.StringBuilder(hash.Length * 2);
                foreach (var b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }

        // ---- install layout / config --------------------------------------------------

        private static string GetTargetInstallDir()
        {
            // Match the launcher's default user-writable install location
            // (ApplicationConfig.AppDirectory): %APPDATA%\OOFSponderStandalone
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, "OOFSponderStandalone");
        }

        private static void EnsureCleanTargetDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                return;
            }

            // If the directory exists but is empty, that's fine. If it contains files,
            // the user already has a manual install — bail out instead of clobbering it.
            if (Directory.EnumerateFileSystemEntries(dir).Any())
            {
                throw new InvalidOperationException(
                    "Target install directory already contains files: " + dir +
                    ". Aborting migration to avoid overwriting an existing install.");
            }
        }

        private static void WriteAppSettingsJson(string installDir, string ring, string cdnBaseUrl)
        {
            // Shape MUST match LauncherConfiguration in src/OOFSponderLauncher.
            var config = new
            {
                _comment = "Bootstrapped by OOFSponder ClickOnce migration on " +
                           DateTime.UtcNow.ToString("u"),
                UpdateService = new
                {
                    CdnBaseUrl = cdnBaseUrl,
                    OOFSponderRing = ring,
                    CheckIntervalMinutes = 60,
                    EnableAutoUpdate = true
                },
                Launcher = new
                {
                    Ring = LauncherBootstrapRing
                },
                Application = new
                {
                    AppName = AppName,
                    AppDirectory = installDir
                },
                DotNetRuntime = new
                {
                    RequiredMajorVersion = 10,
                    RequiredMinorVersion = 0,
                    DownloadUrl = "https://aka.ms/dotnet/10.0/windowsdesktop-runtime-win-x64.exe"
                }
            };

            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(Path.Combine(installDir, "appsettings.json"), json);
        }

        // ---- startup re-pointing ------------------------------------------------------

        private static void RepointStartupToLauncher(string installDir)
        {
            var launcherPath = Path.Combine(installDir, LauncherExeName);
            using (var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true))
            {
                if (key == null)
                {
                    return;
                }
                key.SetValue(RunKeyName, "\"" + launcherPath + "\"", RegistryValueKind.String);
            }
        }

        private static void RemoveLegacyApprefShortcut()
        {
            try
            {
                var apprefPath = GetLegacyApprefPath();
                if (File.Exists(apprefPath))
                {
                    File.Delete(apprefPath);
                }

                // Also remove from the user's Startup folder if a previous version copied it there.
                var startupAppref = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                    "OOFSponder.appref-ms");
                if (File.Exists(startupAppref))
                {
                    File.Delete(startupAppref);
                }
            }
            catch
            {
                // Best-effort cleanup.
            }
        }

        private static string GetLegacyApprefPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Programs),
                LegacyApprefRelativePath);
        }

        // ---- ClickOnce uninstall ------------------------------------------------------

        private static void TryUninstallClickOnce()
        {
            try
            {
                using (var uninstallRoot = Registry.CurrentUser.OpenSubKey(UninstallKeyPath))
                {
                    if (uninstallRoot == null)
                    {
                        return;
                    }

                    foreach (var subName in uninstallRoot.GetSubKeyNames())
                    {
                        using (var sub = uninstallRoot.OpenSubKey(subName))
                        {
                            if (sub == null) continue;

                            var displayName = sub.GetValue("DisplayName") as string;
                            if (string.IsNullOrEmpty(displayName)) continue;
                            if (displayName.IndexOf("OOFSponder", StringComparison.OrdinalIgnoreCase) < 0)
                            {
                                continue;
                            }

                            var uninstallString = sub.GetValue("UninstallString") as string;
                            if (string.IsNullOrEmpty(uninstallString)) continue;

                            // ClickOnce UninstallString looks like:
                            //   rundll32.exe dfshim.dll,ShArpMaintain ...
                            // It accepts no real "silent" flag, so we fire-and-forget; the
                            // user will see the standard ClickOnce confirm dialog.
                            try
                            {
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = "cmd.exe",
                                    Arguments = "/c " + uninstallString,
                                    CreateNoWindow = true,
                                    UseShellExecute = false,
                                    WindowStyle = ProcessWindowStyle.Hidden
                                });
                            }
                            catch (Exception ex)
                            {
                                SafeLog("Migration: ClickOnce uninstall failed: " + ex.Message);
                            }
                            return;
                        }
                    }
                }
            }
            catch
            {
                // Best-effort. The launcher will still take over even if the old entry remains.
            }
        }

        // ---- launcher handoff ---------------------------------------------------------

        private static void StartLauncher(string installDir)
        {
            var launcherPath = Path.Combine(installDir, LauncherExeName);
            if (!File.Exists(launcherPath))
            {
                SafeLog("Migration: launcher not present at " + launcherPath);
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = launcherPath,
                WorkingDirectory = installDir,
                UseShellExecute = true
            });
        }

        // ---- markers ------------------------------------------------------------------

        private static string GetAppDataDir()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                AppName);
        }

        private static bool HasMigratedMarker(string appDataDir)
        {
            return File.Exists(Path.Combine(appDataDir, MigratedMarkerFileName));
        }

        private static bool HasDontAskAgainMarker(string appDataDir)
        {
            return File.Exists(Path.Combine(appDataDir, DontAskAgainMarkerFileName));
        }

        private static void WriteDontAskAgainMarker(string appDataDir)
        {
            try
            {
                File.WriteAllText(
                    Path.Combine(appDataDir, DontAskAgainMarkerFileName),
                    DateTime.UtcNow.ToString("u"));
            }
            catch { }
        }

        private static void WriteMigratedMarker(string appDataDir, string ring, string version, string installDir)
        {
            try
            {
                var payload = new
                {
                    schemaVersion = MigrationSchemaVersion,
                    migratedUtc = DateTime.UtcNow,
                    ring = ring,
                    installedVersion = version,
                    installDir = installDir
                };
                File.WriteAllText(
                    Path.Combine(appDataDir, MigratedMarkerFileName),
                    JsonConvert.SerializeObject(payload, Formatting.Indented));
            }
            catch (Exception ex)
            {
                SafeLog("Migration: failed to write migrated marker: " + ex.Message);
            }
        }

        // ---- misc ---------------------------------------------------------------------

        private static void TryDelete(string path)
        {
            try { if (File.Exists(path)) File.Delete(path); } catch { }
        }

        private static void SafeLog(string message)
        {
            try
            {
                OOFSponder.Logger.Info(message, false);
            }
            catch
            {
                try
                {
                    Debug.WriteLine(message);
                }
                catch
                {
                }
            }
        }
    }
}
