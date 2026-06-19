using OOFSponderMac.Models;
using System;

namespace OOFSponderMac.ViewModels;

/// <summary>
/// Observable wrapper around a single <see cref="WorkingDay"/>
/// for use in the UI schedule grid.
/// </summary>
public sealed class WorkingDayViewModel : ViewModelBase
{
    private readonly WorkingDay _model;

    public WorkingDayViewModel(WorkingDay model) => _model = model;

    public WorkingDay Model => _model;

    public string DayName => _model.DayOfWeek.ToString();

    public bool IsOOF
    {
        get => _model.IsOOF;
        set
        {
            if (_model.IsOOF == value) return;
            _model.IsOOF = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TimePickersEnabled));
        }
    }

    /// <summary>Time pickers are only enabled when the day is a working day.</summary>
    public bool TimePickersEnabled => !IsOOF;

    public TimeSpan StartTime
    {
        get => _model.StartTime.TimeOfDay;
        set
        {
            _model.StartTime = DateTime.Today.Add(value);
            OnPropertyChanged();
        }
    }

    public TimeSpan EndTime
    {
        get => _model.EndTime.TimeOfDay;
        set
        {
            _model.EndTime = DateTime.Today.Add(value);
            OnPropertyChanged();
        }
    }

    /// <summary>Human-readable start time for display (HH:mm).</summary>
    public string StartTimeDisplay
    {
        get => _model.StartTime.ToString("HH:mm");
        set
        {
            if (TimeSpan.TryParse(value, out var ts))
            {
                _model.StartTime = DateTime.Today.Add(ts);
                OnPropertyChanged();
                OnPropertyChanged(nameof(StartTime));
            }
        }
    }

    /// <summary>Human-readable end time for display (HH:mm).</summary>
    public string EndTimeDisplay
    {
        get => _model.EndTime.ToString("HH:mm");
        set
        {
            if (TimeSpan.TryParse(value, out var ts))
            {
                _model.EndTime = DateTime.Today.Add(ts);
                OnPropertyChanged();
                OnPropertyChanged(nameof(EndTime));
            }
        }
    }
}
