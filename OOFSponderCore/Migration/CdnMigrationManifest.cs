using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OOFSponderCore.Migration
{
    // Mirrors the shape of /manifest/version.json served from the OOFSponder CDN.
    // Kept in sync with CdnVersionManifest in src/OOFSponderLauncher/UpdateService.cs.
    // Uses Newtonsoft.Json (the legacy ClickOnce project already references it) so we
    // do not need to add System.Text.Json to .NET Framework 4.7.2.

    internal class CdnMigrationManifest
    {
        [JsonProperty("schemaVersion")]
        public int SchemaVersion { get; set; }

        [JsonProperty("updatedUtc")]
        public DateTime UpdatedUtc { get; set; }

        [JsonProperty("rings")]
        public CdnMigrationRings Rings { get; set; }

        [JsonProperty("launcher")]
        public Dictionary<string, CdnMigrationLauncherEntry> Launcher { get; set; }
    }

    internal class CdnMigrationRings
    {
        [JsonProperty("production")]
        public CdnMigrationRingEntry Production { get; set; }

        [JsonProperty("insider")]
        public CdnMigrationRingEntry Insider { get; set; }

        [JsonProperty("alpha")]
        public CdnMigrationRingEntry Alpha { get; set; }
    }

    internal class CdnMigrationRingEntry
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("ring")]
        public string Ring { get; set; }

        [JsonProperty("releaseNotes")]
        public string ReleaseNotes { get; set; }

        [JsonProperty("sha256Hash")]
        public string Sha256Hash { get; set; }

        [JsonProperty("fileSizeBytes")]
        public long FileSizeBytes { get; set; }

        [JsonProperty("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("packageFileName")]
        public string PackageFileName { get; set; }

        [JsonProperty("packagePath")]
        public string PackagePath { get; set; }

        [JsonProperty("downloadUrl")]
        public string DownloadUrl { get; set; }
    }

    internal class CdnMigrationLauncherEntry
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("sha256Hash")]
        public string Sha256Hash { get; set; }

        [JsonProperty("fileSizeBytes")]
        public long FileSizeBytes { get; set; }

        [JsonProperty("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("downloadUrl")]
        public string DownloadUrl { get; set; }
    }
}
