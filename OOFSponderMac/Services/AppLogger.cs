using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace OOFSponderMac.Services;

/// <summary>
/// Cross-platform file logger (no System.Windows.Forms dependency).
/// Rolls log files at 1 MB, retaining the three most recent.
/// </summary>
public static class AppLogger
{
    private static readonly string LogFilePath = Path.Combine(
        Program.AppDataFolder, "OOFSponder.log");

    private const long MaxLogSize = 1 * 1024 * 1024; // 1 MB
    private const int MaxRolledLogs = 3;

    private static readonly object Lock = new();

    public static void Info(string message) => WriteEntry(message, "info");
    public static void Warning(string message) => WriteEntry(message, "warning");
    public static void Error(string message) => WriteEntry(message, "error");
    public static void Error(string message, Exception ex) =>
        WriteEntry($"{message}: {ex.Message}" +
                   (ex.InnerException != null ? $" -> {ex.InnerException.Message}" : ""), "error");
    public static void Error(Exception ex) =>
        WriteEntry(ex.InnerException != null
            ? $"{ex.Message} due to {ex.InnerException.Message}"
            : ex.Message, "error");

    private static void WriteEntry(string message, string level)
    {
        lock (Lock)
        {
            try
            {
                RollIfNeeded(LogFilePath);
                using var writer = new StreamWriter(LogFilePath, append: true);
                writer.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss},{level},{Caller()},{Scrub(message)}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Logger failure: {ex.Message}");
            }
        }
    }

    private static string Scrub(string message) =>
        message.Replace(Program.AppDataFolder, "<data>");

    private static string Caller()
    {
        var frame = new StackFrame(3, false);
        return frame?.GetMethod()?.Name ?? "unknown";
    }

    private static void RollIfNeeded(string path)
    {
        try
        {
            if (!File.Exists(path)) return;
            if (new FileInfo(path).Length <= MaxLogSize) return;

            var dir = Path.GetDirectoryName(path)!;
            var stem = Path.GetFileNameWithoutExtension(path);
            var ext = Path.GetExtension(path);
            // Match rolled files of the form "{stem}.{digits}{ext}" (e.g. oofsponder.0.log, oofsponder.10.log)
            var rolledPattern = new Regex(
                $@"^{Regex.Escape(stem)}\.\d+{Regex.Escape(ext)}$",
                RegexOptions.IgnoreCase);
            var rolled = Directory.GetFiles(dir, $"{stem}*{ext}")
                .Where(f => rolledPattern.IsMatch(Path.GetFileName(f)))
                .OrderByDescending(f => f)
                .ToList();

            if (rolled.Count >= MaxRolledLogs)
            {
                File.Delete(rolled[MaxRolledLogs - 1]);
                rolled.RemoveAt(MaxRolledLogs - 1);
            }

            for (int i = rolled.Count; i > 0; i--)
                File.Move(rolled[i - 1], Path.Combine(dir, $"{stem}.{i}{ext}"));

            File.Move(path, Path.Combine(dir, $"{stem}.0{ext}"));
        }
        catch { /* rolling is best-effort */ }
    }
}
