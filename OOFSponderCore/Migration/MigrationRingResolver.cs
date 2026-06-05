using System;
using System.Collections.Generic;

namespace OOFSponderCore.Migration
{
    /// <summary>
    /// Hardcoded ring → CDN host mapping used by the migration installer. Kept in sync
    /// with <c>eng/cdn-endpoints.json</c> and <c>OOFSponderLauncher/CdnEndpointPolicy.cs</c>
    /// in the new repo.
    ///
    /// We deliberately hardcode rather than read JSON: the GitHub <c>OOFSponderCore</c>
    /// ClickOnce build is the boot-strap, so it has no shared file to read from.
    ///
    /// DO NOT hand-edit the two URL constants below. They are rewritten by
    /// <c>eng/Generate-MigrationRingResolver.ps1</c> from <c>eng/cdn-endpoints.json</c>,
    /// and both OneBranch pipelines run that script with <c>-Verify</c> after
    /// checkout — the build will fail if the on-disk values drift from the JSON.
    /// </summary>
    internal static class MigrationRingResolver
    {
        // Auto-generated from eng/cdn-endpoints.json by
        // eng/Generate-MigrationRingResolver.ps1. Do not edit by hand.
        public const string ProductionCdnBaseUrl =
            "https://oofsponder-dev-148884-mi-aca6h9ageaeretav.b02.azurefd.net";

        public const string PrereleaseCdnBaseUrl =
            "https://oofsponder-dev-182c8d-mi-gbgedmajg2dqc6a7.b01.azurefd.net";

        /// <summary>
        /// Normalises a ring string to <c>production</c> / <c>insider</c> / <c>alpha</c>.
        /// Returns <c>production</c> if the input cannot be parsed (fail-safe default).
        /// </summary>
        public static string NormalizeRing(string ring)
        {
            if (string.IsNullOrWhiteSpace(ring))
            {
                return "production";
            }

            var lower = ring.Trim().ToLowerInvariant();
            if (lower.Contains("alpha")) return "alpha";
            if (lower.Contains("insider")) return "insider";
            if (lower.Contains("production")) return "production";
            return "production";
        }

        /// <summary>
        /// Resolves the CDN base URL for a normalised ring. <c>production</c> goes to
        /// the PROD CDN; <c>alpha</c> and <c>insider</c> share the prerelease CDN.
        /// </summary>
        public static string GetCdnBaseUrlForRing(string ring)
        {
            switch (NormalizeRing(ring))
            {
                case "alpha":
                case "insider":
                    return PrereleaseCdnBaseUrl;
                case "production":
                default:
                    return ProductionCdnBaseUrl;
            }
        }

        /// <summary>
        /// Picks the manifest ring entry for a normalised ring.
        /// </summary>
        public static CdnMigrationRingEntry SelectRingEntry(CdnMigrationManifest manifest, string ring)
        {
            if (manifest?.Rings == null)
            {
                return null;
            }

            switch (NormalizeRing(ring))
            {
                case "alpha":     return manifest.Rings.Alpha;
                case "insider":   return manifest.Rings.Insider;
                case "production":
                default:          return manifest.Rings.Production;
            }
        }

        /// <summary>
        /// Picks launcher manifest entry by ring with a fallback to production.
        /// </summary>
        public static CdnMigrationLauncherEntry SelectLauncherEntry(CdnMigrationManifest manifest, string ring)
        {
            if (manifest?.Launcher == null || manifest.Launcher.Count == 0)
            {
                return null;
            }

            var normalized = NormalizeRing(ring);
            var map = new Dictionary<string, CdnMigrationLauncherEntry>(manifest.Launcher, StringComparer.OrdinalIgnoreCase);

            if (map.TryGetValue(normalized, out var entry) &&
                entry != null &&
                !string.IsNullOrWhiteSpace(entry.Version))
            {
                return entry;
            }

            if (map.TryGetValue("production", out var prod) &&
                prod != null &&
                !string.IsNullOrWhiteSpace(prod.Version))
            {
                return prod;
            }

            return null;
        }
    }
}
