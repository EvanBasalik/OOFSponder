using Microsoft.Graph;
using OOFSponderMac.Models;
using OOFSponderMac.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OOFSponderMac.ViewModels;

public sealed class MainViewModel : ViewModelBase, IDisposable
{
    private readonly OOFData _data;
    private readonly OOFScheduler _scheduler;

    // ─── Constructor ──────────────────────────────────────────────────────────

    public MainViewModel()
    {
        _data = OOFData.Instance;
        _scheduler = new OOFScheduler();
        _scheduler.StatusChanged += msg => StatusText = msg;

        // Build working-day view models from the model
        WorkingDays = new ObservableCollection<WorkingDayViewModel>();
        foreach (var day in _data.WorkingDayCollection)
            WorkingDays.Add(new WorkingDayViewModel(day));

        // Load remaining fields from model
        _primaryOOFExternal = _data.PrimaryOOFExternalMessage;
        _primaryOOFInternal = _data.PrimaryOOFInternalMessage;
        _secondaryOOFExternal = _data.SecondaryOOFExternalMessage;
        _secondaryOOFInternal = _data.SecondaryOOFInternalMessage;
        _isPermaOOFMode = _data.IsPermaOOFOn;
        _permaOOFDate = _data.PermaOOFDate <= new DateTime(1900, 1, 2)
            ? DateTime.Today.AddDays(7)
            : _data.PermaOOFDate;
        _isOnCallModeOn = _data.IsOnCallModeOn;
        _startMinimized = _data.StartMinimized;
        _externalAudienceIndex = AudienceScopeToIndex(_data.ExternalAudienceScope);
        _signedInEmail = O365Service.DefaultUserUPN;

        SaveCommand = new AsyncRelayCommand(SaveAsync);
        SignInCommand = new AsyncRelayCommand(SignInAsync);
        SignOutCommand = new AsyncRelayCommand(SignOutAsync);
        ApplyNowCommand = new AsyncRelayCommand(ApplyNowAsync);

        _scheduler.Start();
    }

    // ─── Working schedule ─────────────────────────────────────────────────────

    public ObservableCollection<WorkingDayViewModel> WorkingDays { get; }

    // ─── Primary OOF messages ─────────────────────────────────────────────────

    private string _primaryOOFExternal;
    public string PrimaryOOFExternal
    {
        get => _primaryOOFExternal;
        set => SetField(ref _primaryOOFExternal, value);
    }

    private string _primaryOOFInternal;
    public string PrimaryOOFInternal
    {
        get => _primaryOOFInternal;
        set => SetField(ref _primaryOOFInternal, value);
    }

    // ─── Secondary / extended OOF messages ───────────────────────────────────

    private string _secondaryOOFExternal;
    public string SecondaryOOFExternal
    {
        get => _secondaryOOFExternal;
        set => SetField(ref _secondaryOOFExternal, value);
    }

    private string _secondaryOOFInternal;
    public string SecondaryOOFInternal
    {
        get => _secondaryOOFInternal;
        set => SetField(ref _secondaryOOFInternal, value);
    }

    private bool _isPermaOOFMode;
    public bool IsPermaOOFMode
    {
        get => _isPermaOOFMode;
        set
        {
            SetField(ref _isPermaOOFMode, value);
            OnPropertyChanged(nameof(PermaOOFVisible));
        }
    }

    public bool PermaOOFVisible => _isPermaOOFMode;

    private DateTime _permaOOFDate;
    public DateTime PermaOOFDate
    {
        get => _permaOOFDate;
        set => SetField(ref _permaOOFDate, value);
    }

    // ─── Settings ─────────────────────────────────────────────────────────────

    private bool _isOnCallModeOn;
    public bool IsOnCallModeOn
    {
        get => _isOnCallModeOn;
        set => SetField(ref _isOnCallModeOn, value);
    }

    private bool _startMinimized;
    public bool StartMinimized
    {
        get => _startMinimized;
        set => SetField(ref _startMinimized, value);
    }

    public string[] AudienceScopes { get; } = { "All external recipients", "Contacts only", "None" };

    private int _externalAudienceIndex;
    public int ExternalAudienceIndex
    {
        get => _externalAudienceIndex;
        set => SetField(ref _externalAudienceIndex, value);
    }

    // ─── Auth state ────────────────────────────────────────────────────────────

    private string _signedInEmail;
    public string SignedInEmail
    {
        get => _signedInEmail;
        set
        {
            SetField(ref _signedInEmail, value);
            OnPropertyChanged(nameof(SignedInText));
            OnPropertyChanged(nameof(IsSignedIn));
        }
    }

    public bool IsSignedIn => !string.IsNullOrEmpty(_signedInEmail);
    public string SignedInText => IsSignedIn
        ? $"Signed in as {_signedInEmail}"
        : "Not signed in";

    // ─── Status ────────────────────────────────────────────────────────────────

    private string _statusText = "Starting…";
    public string StatusText
    {
        get => _statusText;
        set => SetField(ref _statusText, value);
    }

    // ─── Commands ──────────────────────────────────────────────────────────────

    public ICommand SaveCommand { get; }
    public ICommand SignInCommand { get; }
    public ICommand SignOutCommand { get; }
    public ICommand ApplyNowCommand { get; }

    private Task SaveAsync()
    {
        AppLogger.Info("SaveAsync");
        PushToModel();
        _data.WriteProperties();
        StatusText = "Settings saved.";
        return Task.CompletedTask;
    }

    private async Task SignInAsync()
    {
        StatusText = "Signing in…";
        bool ok = await O365Service.SignInAsync();
        SignedInEmail = O365Service.DefaultUserUPN;
        StatusText = ok ? $"Signed in as {SignedInEmail}" : "Sign-in failed. Check the log.";
    }

    private async Task SignOutAsync()
    {
        await O365Service.SignOutAsync();
        SignedInEmail = string.Empty;
        StatusText = "Signed out.";
    }

    private Task ApplyNowAsync()
    {
        StatusText = "Applying OOF settings…";
        PushToModel();
        // Restart scheduler so it applies immediately
        _scheduler.Stop();
        _scheduler.Start();
        return Task.CompletedTask;
    }

    // ─── Model sync ───────────────────────────────────────────────────────────

    private void PushToModel()
    {
        _data.PrimaryOOFExternalMessage = _primaryOOFExternal;
        _data.PrimaryOOFInternalMessage = _primaryOOFInternal;
        _data.SecondaryOOFExternalMessage = _secondaryOOFExternal;
        _data.SecondaryOOFInternalMessage = _secondaryOOFInternal;
        _data.IsOnCallModeOn = _isOnCallModeOn;
        _data.StartMinimized = _startMinimized;
        _data.ExternalAudienceScope = IndexToAudienceScope(_externalAudienceIndex);
        _data.PermaOOFDate = _isPermaOOFMode ? _permaOOFDate : new DateTime(1900, 1, 1);

        // Update the WorkingDayCollection from each working-day VM
        _data.WorkingDayCollection.Clear();
        foreach (var vm in WorkingDays)
            _data.WorkingDayCollection.Add(vm.Model);
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────

    private static ExternalAudienceScope IndexToAudienceScope(int index) => index switch
    {
        0 => ExternalAudienceScope.All,
        1 => ExternalAudienceScope.ContactsOnly,
        _ => ExternalAudienceScope.None,
    };

    private static int AudienceScopeToIndex(ExternalAudienceScope scope) => scope switch
    {
        ExternalAudienceScope.All => 0,
        ExternalAudienceScope.ContactsOnly => 1,
        _ => 2,
    };

    // ─── IDisposable ──────────────────────────────────────────────────────────

    public void Dispose()
    {
        _scheduler.Dispose();
        _data.Dispose();
    }
}

// ─── Minimal ICommand adapter for async methods ───────────────────────────────

internal sealed class AsyncRelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private bool _isExecuting;

    public AsyncRelayCommand(Func<Task> execute) => _execute = execute;

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? _) => !_isExecuting;

    public async void Execute(object? _)
    {
        if (_isExecuting) return;
        _isExecuting = true;
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        try { await _execute(); }
        finally
        {
            _isExecuting = false;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
