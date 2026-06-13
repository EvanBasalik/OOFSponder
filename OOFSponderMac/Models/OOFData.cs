using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Newtonsoft.Json;
using OOFSponderMac.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;

// Disambiguate types that clash with Microsoft.Graph
using SysDayOfWeek = System.DayOfWeek;
using SysFile      = System.IO.File;
using SysDirectory = System.IO.Directory;

namespace OOFSponderMac.Models;

/// <summary>
/// Core OOF data model and scheduling logic.
/// Cross-platform port of the Windows OOFScheduling.OOFData class.
/// </summary>
public class OOFData : IDisposable
{
    // ─── Singleton ───────────────────────────────────────────────────────────
    private static OOFData? _instance;
    public static OOFData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new OOFData();
                _instance.ReadProperties();
            }
            return _instance;
        }
    }

    // ─── Constants / statics ─────────────────────────────────────────────────
    private const string DefaultValue = "default";
    private const string DummyHTML = "<BODY scroll=auto></BODY>";
    internal static readonly string HTMLReadOnlyIndicator = "<BODY style=\"BACKGROUND-COLOR: lightgray\"";

    // ─── OOF message fields ───────────────────────────────────────────────────

    private string _primaryOOFExternalMessage = string.Empty;
    public string PrimaryOOFExternalMessage
    {
        get => _primaryOOFExternalMessage;
        set
        {
            if (value != _primaryOOFExternalMessage
                && !IsEmptyOrDefaultOOFMessage(value)
                && !value.Contains(HTMLReadOnlyIndicator))
            {
                if (_primaryOOFExternalMessage != string.Empty)
                    SaveOOFMessageOffline(OOFMessageType.PrimaryExternal, value);
            }
            if (!value.Contains(HTMLReadOnlyIndicator))
                _primaryOOFExternalMessage = value;
        }
    }

    private string _primaryOOFInternalMessage = string.Empty;
    public string PrimaryOOFInternalMessage
    {
        get => _primaryOOFInternalMessage;
        set
        {
            if (value != _primaryOOFInternalMessage && !IsEmptyOrDefaultOOFMessage(value))
            {
                if (_primaryOOFInternalMessage != string.Empty)
                    SaveOOFMessageOffline(OOFMessageType.PrimaryInternal, value);
            }
            _primaryOOFInternalMessage = value;
        }
    }

    private string _secondaryOOFExternalMessage = string.Empty;
    public string SecondaryOOFExternalMessage
    {
        get => _secondaryOOFExternalMessage;
        set
        {
            if (value != _secondaryOOFExternalMessage
                && !IsEmptyOrDefaultOOFMessage(value)
                && !value.Contains(HTMLReadOnlyIndicator))
            {
                if (_secondaryOOFExternalMessage != string.Empty)
                    SaveOOFMessageOffline(OOFMessageType.ExtendedExternal, value);
            }
            if (!value.Contains(HTMLReadOnlyIndicator))
                _secondaryOOFExternalMessage = value;
        }
    }

    private string _secondaryOOFInternalMessage = string.Empty;
    public string SecondaryOOFInternalMessage
    {
        get => _secondaryOOFInternalMessage;
        set
        {
            if (value != _secondaryOOFInternalMessage && !IsEmptyOrDefaultOOFMessage(value))
            {
                if (_secondaryOOFInternalMessage != string.Empty)
                    SaveOOFMessageOffline(OOFMessageType.ExtendedInternal, value);
            }
            _secondaryOOFInternalMessage = value;
        }
    }

    // ─── Schedule / settings fields ───────────────────────────────────────────

    private string _workingHours = string.Empty;
    public string WorkingHours
    {
        get => _workingHours;
        set
        {
            _workingHours = value;
            _workingDayCollection = null; // reset so it is re-parsed
        }
    }

    public DateTime PermaOOFDate { get; set; } = new DateTime(1900, 1, 1);
    public bool IsOnCallModeOn { get; set; }
    public bool StartMinimized { get; set; }
    public bool UseNewOOFMath { get; set; }
    public bool UnableToParseWorkingHours { get; set; }
    public string UserSettingsSource { get; set; } = string.Empty;
    public ExternalAudienceScope ExternalAudienceScope { get; set; } = ExternalAudienceScope.All;

    // ─── Working day collection ───────────────────────────────────────────────

    private Collection<WorkingDay>? _workingDayCollection;
    public Collection<WorkingDay> WorkingDayCollection
    {
        get
        {
            if (_workingDayCollection == null)
                _workingDayCollection = new Collection<WorkingDay>();

            if (_workingDayCollection.Count == 7)
                return _workingDayCollection;

            if (string.IsNullOrEmpty(WorkingHours) && _workingDayCollection.Count == 0)
            {
                CreateDefaultWorkingDays();
            }
            else if (_workingDayCollection.Count < 7 && !string.IsNullOrEmpty(WorkingHours))
            {
                ParseWorkingHours();
            }

            return _workingDayCollection;
        }
        set => _workingDayCollection = value;
    }

    private void CreateDefaultWorkingDays()
    {
        for (int i = 0; i < 7; i++)
        {
            _workingDayCollection!.Add(new WorkingDay
            {
                DayOfWeek = (SysDayOfWeek)i,
                StartTime = DateTime.Now.Date.AddHours(9),
                EndTime = DateTime.Now.Date.AddHours(17),
                IsOOF = false
            });
        }
    }

    private void ParseWorkingHours()
    {
        string[] workingTimes = WorkingHours.Split('|');
        if (workingTimes.Length == 7)
        {
            for (int i = 0; i < 7; i++)
            {
                string[] parts = workingTimes[i].Split('~');
                var day = new WorkingDay { DayOfWeek = (SysDayOfWeek)i };
                try
                {
                    day.StartTime = DateTime.Parse(parts[0]);
                    day.EndTime = DateTime.Parse(parts[1]);
                    day.IsOOF = parts[2] == "0";
                }
                catch
                {
                    day.StartTime = DateTime.Now.Date.AddHours(9);
                    day.EndTime = DateTime.Now.Date.AddHours(17);
                    day.IsOOF = false;
                    UnableToParseWorkingHours = true;
                }
                _workingDayCollection!.Add(day);
            }
        }
        else
        {
            _workingDayCollection!.Clear();
            CreateDefaultWorkingDays();
            UnableToParseWorkingHours = true;
        }
    }

    // ─── Scheduling helpers ───────────────────────────────────────────────────

    public bool IsPermaOOFOn => DateTime.Now < PermaOOFDate;

    public WorkingDay CurrentWorkingDay =>
        WorkingDayCollection[(int)DateTime.Now.DayOfWeek];

    public DateTime PreviousWorkingDayEnd
    {
        get
        {
            int daysBack = -1;
            while (daysBack >= -6)
            {
                if (!WorkingDayCollection[(int)DateTime.Now.AddDays(daysBack).DayOfWeek].IsOOF)
                    break;
                daysBack--;
            }
            var datePart = DateTime.Now.AddDays(daysBack);
            var timePart = WorkingDayCollection[(int)DateTime.Now.AddDays(daysBack).DayOfWeek].EndTime;
            return new DateTime(datePart.Year, datePart.Month, datePart.Day,
                timePart.Hour, timePart.Minute, 0, new CultureInfo("en-US", false).Calendar);
        }
    }

    public DateTime NextWorkingDayStart
    {
        get
        {
            var target = IsPermaOOFOn ? PermaOOFDate : DateTime.Now;
            int daysForward = 0;
            while (daysForward <= 6)
            {
                if (!WorkingDayCollection[(int)target.AddDays(daysForward).DayOfWeek].IsOOF)
                    break;
                daysForward++;
            }
            var datePart = target.AddDays(daysForward);
            var timePart = WorkingDayCollection[(int)target.AddDays(daysForward).DayOfWeek].StartTime;
            return new DateTime(datePart.Year, datePart.Month, datePart.Day,
                timePart.Hour, timePart.Minute, 0, new CultureInfo("en-US", false).Calendar);
        }
    }

    public DateTime NextWorkingDayEnd
    {
        get
        {
            var target = IsPermaOOFOn ? PermaOOFDate : DateTime.Now;
            int daysForward = 0;
            while (daysForward <= 6)
            {
                if (!WorkingDayCollection[(int)target.AddDays(daysForward).DayOfWeek].IsOOF)
                    break;
                daysForward++;
            }
            var datePart = target.AddDays(daysForward);
            var timePart = WorkingDayCollection[(int)target.AddDays(daysForward).DayOfWeek].EndTime;
            return new DateTime(datePart.Year, datePart.Month, datePart.Day,
                timePart.Hour, timePart.Minute, 0, new CultureInfo("en-US", false).Calendar);
        }
    }

    public void CalculateOOFTimes(out DateTime startTime, out DateTime endTime, bool enableOnCallMode)
    {
        startTime = DateTime.Now;
        endTime = DateTime.Now;

        var now = DateTime.Now;
        var currentDay = CurrentWorkingDay;
        var prevEnd = PreviousWorkingDayEnd;
        var nextStart = NextWorkingDayStart;
        var nextEnd = NextWorkingDayEnd;

        if (now > prevEnd && now < nextStart)
        {
            startTime = enableOnCallMode ? currentDay.StartTime : prevEnd;
            endTime = enableOnCallMode ? currentDay.EndTime : nextStart;
        }
        else if (now > currentDay.StartTime && now < currentDay.EndTime)
        {
            startTime = enableOnCallMode ? currentDay.StartTime : currentDay.EndTime;
            endTime = enableOnCallMode ? currentDay.EndTime : nextStart;
        }
        else
        {
            startTime = enableOnCallMode ? nextStart : currentDay.EndTime;
            endTime = enableOnCallMode ? nextEnd : nextStart;
        }
    }

    // ─── Data sufficiency check ───────────────────────────────────────────────

    public bool HaveNecessaryData
    {
        get
        {
            if (UseNewOOFMath && WorkingDayCollection.Count != 7) return false;
            if (!UseNewOOFMath && string.IsNullOrEmpty(WorkingHours)) return false;

            if (!IsPermaOOFOn
                && !IsEmptyOrDefaultOOFMessage(PrimaryOOFExternalMessage)
                && !IsEmptyOrDefaultOOFMessage(PrimaryOOFInternalMessage))
                return true;

            if (!IsPermaOOFOn
                && IsEmptyOrDefaultOOFMessage(PrimaryOOFExternalMessage)
                && ExternalAudienceScope == ExternalAudienceScope.None)
                return true;

            if (IsPermaOOFOn
                && IsEmptyOrDefaultOOFMessage(SecondaryOOFExternalMessage)
                && ExternalAudienceScope == ExternalAudienceScope.None)
                return true;

            if (IsPermaOOFOn
                && !IsEmptyOrDefaultOOFMessage(SecondaryOOFExternalMessage)
                && !IsEmptyOrDefaultOOFMessage(SecondaryOOFInternalMessage))
                return true;

            return false;
        }
    }

    private static bool IsEmptyOrDefaultOOFMessage(string input)
    {
        var cleaned = Regex.Replace(input, "<.*?>", string.Empty);
        cleaned = System.Net.WebUtility.HtmlDecode(cleaned);
        cleaned = new string(cleaned.Where(c => !char.IsWhiteSpace(c)).ToArray());
        return cleaned == string.Empty || cleaned == DummyHTML;
    }

    // ─── Persistence ─────────────────────────────────────────────────────────

    private void ReadProperties()
    {
        try
        {
            AppLogger.Info("Reading settings");

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile(Path.Combine(Program.AppDataFolder, "usersettings.json"), optional: true)
                .Build();

            var root = new OOFSettingsRoot();
            config.Bind(root);

            var d = root.OOFData;

            PermaOOFDate = d.PermaOOFDate;
            WorkingHours = d.WorkingHours == DefaultValue ? string.Empty : d.WorkingHours;
            PrimaryOOFExternalMessage = d.PrimaryOOFExternalMessage == DefaultValue ? string.Empty : d.PrimaryOOFExternalMessage;
            PrimaryOOFInternalMessage = d.PrimaryOOFInternalMessage == DefaultValue ? string.Empty : d.PrimaryOOFInternalMessage;
            SecondaryOOFExternalMessage = d.SecondaryOOFExternalMessage == DefaultValue ? string.Empty : d.SecondaryOOFExternalMessage;
            SecondaryOOFInternalMessage = d.SecondaryOOFInternalMessage == DefaultValue ? string.Empty : d.SecondaryOOFInternalMessage;
            IsOnCallModeOn = d.IsOnCallModeOn;
            StartMinimized = d.StartMinimized;
            ExternalAudienceScope = d.ExternalAudienceScope;
            UseNewOOFMath = d.UseNewOOFMath;
            UserSettingsSource = root.UserSettingsSource;
            WorkingDayCollection = d.WorkingDayCollection ?? new Collection<WorkingDay>();

            AppLogger.Info("Settings loaded successfully");
        }
        catch (Exception ex)
        {
            AppLogger.Error("Failed to read settings", ex);
        }
    }

    public void WriteProperties()
    {
        if (!HaveNecessaryData)
        {
            AppLogger.Warning("Missing necessary data – settings not persisted");
            return;
        }

        try
        {
            var root = new OOFSettingsRoot
            {
                UserSettingsSource = "OOFSponder_Mac_" + RuntimeInformation.FrameworkDescription,
                OOFData = new OOFSettingsData
                {
                    PrimaryOOFExternalMessage = PrimaryOOFExternalMessage,
                    PrimaryOOFInternalMessage = PrimaryOOFInternalMessage,
                    SecondaryOOFExternalMessage = SecondaryOOFExternalMessage,
                    SecondaryOOFInternalMessage = SecondaryOOFInternalMessage,
                    PermaOOFDate = PermaOOFDate,
                    WorkingHours = WorkingHours,
                    IsOnCallModeOn = IsOnCallModeOn,
                    StartMinimized = StartMinimized,
                    ExternalAudienceScope = ExternalAudienceScope,
                    UseNewOOFMath = UseNewOOFMath,
                    WorkingDayCollection = WorkingDayCollection
                }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(root,
                new JsonSerializerOptions { WriteIndented = true });
            SysFile.WriteAllText(
                Path.Combine(Program.AppDataFolder, "usersettings.json"), json);

            AppLogger.Info("Settings persisted");
        }
        catch (Exception ex)
        {
            AppLogger.Error("Failed to write settings", ex);
        }
    }

    internal enum OOFMessageType { PrimaryInternal, PrimaryExternal, ExtendedInternal, ExtendedExternal }

    private void SaveOOFMessageOffline(OOFMessageType messageType, string html)
    {
        try
        {
            var folder = Program.AppDataFolder;
            if (!SysDirectory.Exists(folder))
                SysDirectory.CreateDirectory(folder);

            var fileName = Path.Combine(folder,
                $"{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}_{messageType}.html");
            SysFile.WriteAllText(fileName, html);
            CleanupOldMessages(folder, messageType, keepCount: 10);
        }
        catch (Exception ex)
        {
            AppLogger.Error("Failed to save OOF message offline", ex);
        }
    }

    private static void CleanupOldMessages(string folder, OOFMessageType type, int keepCount)
    {
        try
        {
            var files = SysDirectory.GetFiles(folder)
                .Where(f => Path.GetFileName(f).Contains(type.ToString()))
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTime)
                .ToList();

            for (int i = keepCount; i < files.Count; i++)
                files[i].Delete();
        }
        catch { /* non-critical */ }
    }

    // ─── IDisposable ──────────────────────────────────────────────────────────

    public void Dispose()
    {
        WriteProperties();
        GC.SuppressFinalize(this);
    }
}

// ─── Settings configuration classes ──────────────────────────────────────────

internal class OOFSettingsRoot
{
    public OOFSettingsData OOFData { get; set; } = new();
    public string UserSettingsSource { get; set; } = string.Empty;
}

internal class OOFSettingsData
{
    public string WorkingHours { get; set; } = "default";
    public DateTime PermaOOFDate { get; set; } = new DateTime(1900, 1, 1);
    public string PrimaryOOFExternalMessage { get; set; } = "default";
    public string PrimaryOOFInternalMessage { get; set; } = "default";
    public string SecondaryOOFExternalMessage { get; set; } = "default";
    public string SecondaryOOFInternalMessage { get; set; } = "default";
    public ExternalAudienceScope ExternalAudienceScope { get; set; } = ExternalAudienceScope.All;
    public bool IsOnCallModeOn { get; set; }
    public bool StartMinimized { get; set; }
    public bool UseNewOOFMath { get; set; } = true;
    public Collection<WorkingDay>? WorkingDayCollection { get; set; }
}
