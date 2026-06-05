using System;
using System.Windows.Forms;

namespace OOFSponderCore.Migration
{
    internal enum MigrationPromptResult
    {
        Yes,
        No,
        DontAskAgain
    }

    internal static class MigrationPrompt
    {
        public static MigrationPromptResult Show(string newVersion, string ring)
        {
            var versionLine = string.IsNullOrWhiteSpace(newVersion)
                ? string.Empty
                : ("\nNew version available: " + newVersion);

            var ringLine = string.IsNullOrWhiteSpace(ring)
                ? string.Empty
                : ("\nRing: " + ring);

            var message =
                "OOFSponder is moving off ClickOnce to a new self-updating launcher." +
                versionLine +
                ringLine +
                "\n\nMigrate now? You will only be asked once." +
                "\n\nYes  - install the new launcher, re-point Startup, and uninstall the ClickOnce copy" +
                "\nNo   - keep using the ClickOnce build for now (you'll be asked again next launch)" +
                "\nCancel - don't ask again on this machine";

            var result = MessageBox.Show(
                message,
                "OOFSponder migration",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);

            switch (result)
            {
                case DialogResult.Yes:    return MigrationPromptResult.Yes;
                case DialogResult.Cancel: return MigrationPromptResult.DontAskAgain;
                default:                  return MigrationPromptResult.No;
            }
        }
    }
}
