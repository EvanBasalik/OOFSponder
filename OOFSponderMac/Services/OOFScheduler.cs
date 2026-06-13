using Microsoft.Graph;
using OOFSponderMac.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OOFSponderMac.Services;

/// <summary>
/// Background service that checks every 10 minutes whether the OOF
/// status in Exchange Online needs to be updated, and applies changes.
///
/// This is the cross-platform equivalent of the Windows timer loop
/// in MainForm.
/// </summary>
public sealed class OOFScheduler : IDisposable
{
    // How often to check (matches the Windows version)
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(10);

    private CancellationTokenSource? _cts;
    private Task? _loopTask;

    public event Action<string>? StatusChanged;

    // ─── Lifecycle ─────────────────────────────────────────────────────────

    public void Start()
    {
        if (_loopTask != null && !_loopTask.IsCompleted)
            return;

        _cts = new CancellationTokenSource();
        _loopTask = RunLoopAsync(_cts.Token);
        AppLogger.Info("OOFScheduler started");
    }

    public void Stop()
    {
        _cts?.Cancel();
        _loopTask = null;
        AppLogger.Info("OOFScheduler stopped");
    }

    // ─── Main loop ──────────────────────────────────────────────────────────

    private async Task RunLoopAsync(CancellationToken ct)
    {
        // Run once immediately on start, then on the interval
        while (!ct.IsCancellationRequested)
        {
            try
            {
                await ApplyOOFAsync();
            }
            catch (Exception ex)
            {
                AppLogger.Error("OOFScheduler loop error", ex);
                RaiseStatus($"Error: {ex.Message}");
            }

            try { await Task.Delay(CheckInterval, ct); }
            catch (OperationCanceledException) { break; }
        }
    }

    // ─── OOF logic ──────────────────────────────────────────────────────────

    private async Task ApplyOOFAsync()
    {
        AppLogger.Info("ApplyOOFAsync: checking OOF status");
        var data = OOFData.Instance;

        if (!data.HaveNecessaryData)
        {
            AppLogger.Info("ApplyOOFAsync: missing required data – skipping");
            RaiseStatus("Waiting for settings – please fill in working hours and OOF messages.");
            return;
        }

        if (!await O365Service.IsSignedInAsync().ConfigureAwait(false))
        {
            AppLogger.Info("ApplyOOFAsync: not signed in – attempting silent sign-in");
            bool ok = await O365Service.SignInAsync();
            if (!ok)
            {
                RaiseStatus("Not signed in – please sign in to apply OOF settings.");
                return;
            }
        }

        // Determine OOF window
        data.CalculateOOFTimes(out DateTime startTime, out DateTime endTime,
            data.IsOnCallModeOn);

        bool shouldBeOOF = DetermineIfShouldBeOOF(data, startTime, endTime);

        // Build the Graph setting object
        var oof = BuildOOFSetting(data, shouldBeOOF, startTime, endTime);

        // Send to Graph
#if !NOOOF
        await O365Service.PatchMailboxSettingsAsync(oof);
#endif

        string status = shouldBeOOF
            ? $"OOF active – resumes {endTime:ddd d MMM HH:mm}"
            : $"In office – next OOF at {startTime:ddd d MMM HH:mm}";

        RaiseStatus(status);
        AppLogger.Info($"ApplyOOFAsync: {status}");
    }

    private static bool DetermineIfShouldBeOOF(OOFData data, DateTime startTime, DateTime endTime)
    {
        if (data.IsPermaOOFOn)
            return true;

        var now = DateTime.Now;
        return now >= startTime && now < endTime;
    }

    private static AutomaticRepliesSetting BuildOOFSetting(
        OOFData data, bool oofEnabled, DateTime startTime, DateTime endTime)
    {
        var oof = new AutomaticRepliesSetting
        {
            ExternalAudience = data.ExternalAudienceScope,
            Status = oofEnabled
                ? AutomaticRepliesStatus.Scheduled
                : AutomaticRepliesStatus.Disabled
        };

        bool useSecondary = data.IsPermaOOFOn;

        oof.InternalReplyMessage = useSecondary
            ? data.SecondaryOOFInternalMessage
            : data.PrimaryOOFInternalMessage;

        oof.ExternalReplyMessage = useSecondary
            ? data.SecondaryOOFExternalMessage
            : data.PrimaryOOFExternalMessage;

        if (oofEnabled && !data.IsPermaOOFOn)
        {
            oof.ScheduledStartDateTime = new DateTimeTimeZone
            {
                DateTime = startTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                TimeZone = TimeZoneInfo.Local.Id
            };
            oof.ScheduledEndDateTime = new DateTimeTimeZone
            {
                DateTime = endTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                TimeZone = TimeZoneInfo.Local.Id
            };
        }

        return oof;
    }

    private void RaiseStatus(string message) =>
        StatusChanged?.Invoke(message);

    // ─── IDisposable ────────────────────────────────────────────────────────

    public void Dispose()
    {
        Stop();
        _cts?.Dispose();
    }
}
