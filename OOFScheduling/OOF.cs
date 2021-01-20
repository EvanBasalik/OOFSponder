using mshtml;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace OOFScheduling
{
    public class OOFData
    {
        internal DateTime PermaOOFDate { get; set; }

        private string _workingHours;
        internal string WorkingHours
        {
            get
            {
                return _workingHours;
            }
            set
            {
                _workingHours = value;
                //if we update WorkingHours, then blow away OOFCollection
                _OOFCollection = null;
            }
        }
        internal string PrimaryOOFExternalMessage { get; set; }
        internal string PrimaryOOFInternalMessage { get; set; }
        internal string SecondaryOOFExternalMessage { get; set; }
        internal string SecondaryOOFInternalMessage { get; set; }

        internal Collection<OOFInstance> _OOFCollection;
        internal Collection<OOFInstance> OOFCollection
        {
            get
            {
                if (_OOFCollection == null)
                {
                    _OOFCollection = new Collection<OOFInstance>();
                }

                if (_OOFCollection.Count == 0)
                {
                    //first, convert the array of string objects to real objects
                    //next, continue to pattern 7 days into the future
                    //plus, extend the pattern 7 days into the past
                    string[] workingTimes = WorkingHours.Split('|');
                    for (int i = 0; i < 7; i++)
                    {
                        string[] currentWorkingTime = workingTimes[i].Split('~');
                        OOFInstance OOFItemCurrent = new OOFInstance();
                        OOFItemCurrent.DayOfWeek = (DayOfWeek)i;
                        OOFItemCurrent.StartTime = DateTime.Parse(currentWorkingTime[0]).EquivalentDateTime(OOFItemCurrent.DayOfWeek);
                        OOFItemCurrent.EndTime = DateTime.Parse(currentWorkingTime[1]).EquivalentDateTime(OOFItemCurrent.DayOfWeek);
                        if (currentWorkingTime[2] == "0")
                        {
                            OOFItemCurrent.IsOOF = false;
                        }
                        else
                        {
                            OOFItemCurrent.IsOOF = true;
                        }
                        OOFItemCurrent.isOnCallModeEnabled = this.IsOnCallModeOn;

                        OOFInstance OOFItemFuture = new OOFInstance(OOFItemCurrent, 7);
                        OOFInstance OOFItemPast = new OOFInstance(OOFItemCurrent, -7);

                        _OOFCollection.Add(OOFItemCurrent);
                        _OOFCollection.Add(OOFItemPast);
                        _OOFCollection.Add(OOFItemFuture);
                    }
                }

                return _OOFCollection;
            }
        }

        //Track whether or not to run in OnCallMode
        //When in this mode, the OOF times get flipped and instead of 
        //tracking days on/days off, they will track a start/end for OOF *during* the working day
        internal bool IsOnCallModeOn { get; set; }

        private const string baseValue = "default";
        internal static string version;
        static OOFData instance;

        public static OOFData Instance
        {
            get
            {

                if (instance == null)
                {
                    instance = new OOFData();
                    instance.ReadProperties();
                }
                return instance;
            }
        }

        internal OOFInstance currentOOFPeriod
        {
            get
            {
                //need to find the OOFInstance that matches today
                OOFInstance _currentOOFInstance = this.OOFCollection
                                                    .OrderBy(OOF => OOF.StartTime)
                                                    .Where(OOF => OOF.StartTime.Date == DateTime.Now.Date)
                                                    .First();
                return _currentOOFInstance;
            }
        }

        internal DateTime previousOOFPeriodEnd
        {
            get
            {

                OOFInstance _previousOOFInstance = null;

                //find the latest instance before this one
                if (IsOnCallModeOn)
                {
                    _previousOOFInstance = this.OOFCollection
                                    .OrderBy(OOF => OOF.StartTime)
                                    .Where(OOF => OOF.EndTime < currentOOFPeriod.StartTime && OOF.IsOOF)
                                    .Last();
                }
                else
                {
                    _previousOOFInstance = this.OOFCollection
                               .OrderBy(OOF => OOF.StartTime)
                               .Where(OOF => OOF.EndTime < currentOOFPeriod.StartTime)
                               .Last();
                }

                return _previousOOFInstance.EndTime;
            }
        }

        internal OOFInstance nextOOFPeriod
        {
            get
            {
                
                //find the next instance after the current one
                OOFInstance _nextOOFInstance = null;
                if (IsOnCallModeOn)
                {
                    _nextOOFInstance = this.OOFCollection
                                           .OrderBy(OOF => OOF.StartTime)
                                           .Where(OOF => OOF.StartTime > currentOOFPeriod.EndTime && OOF.IsOOF)
                                           .First();
                }
                else
                {
                    _nextOOFInstance = this.OOFCollection
                                           .OrderBy(OOF => OOF.StartTime)
                                           .Where(OOF => OOF.StartTime > currentOOFPeriod.EndTime)
                                           .First();
                }

                return _nextOOFInstance;
            }
        }

        internal bool IsPermaOOFOn
        {
            get
            {
                return DateTime.Now < PermaOOFDate;
            }
        }

        public bool useNewOOFMath { get; internal set; }

        private void LogProperties()
        {
            OOFSponder.Logger.InfoPotentialPII("PermaOOFDate", PermaOOFDate.ToString());
            OOFSponder.Logger.InfoPotentialPII("WorkingHours", WorkingHours);
            OOFSponder.Logger.InfoPotentialPII("PrimaryOOFExternalMessage", PrimaryOOFExternalMessage);
            OOFSponder.Logger.InfoPotentialPII("PrimaryOOFInternalMessage", PrimaryOOFInternalMessage);
            OOFSponder.Logger.InfoPotentialPII("SecondaryOOFExternalMessage", SecondaryOOFExternalMessage);
            OOFSponder.Logger.InfoPotentialPII("SecondaryOOFInternalMessage", SecondaryOOFInternalMessage);
            OOFSponder.Logger.Info("IsOnCallModeOn: " + IsOnCallModeOn);
        }

        private void ReadProperties()
        {
            OOFSponder.Logger.Info("Reading settings");

            instance.PermaOOFDate = OOFScheduling.Properties.Settings.Default.PermaOOFDate;
            instance.WorkingHours = OOFScheduling.Properties.Settings.Default.workingHours == baseValue ? string.Empty : Properties.Settings.Default.workingHours;
            instance.PrimaryOOFExternalMessage = OOFScheduling.Properties.Settings.Default.PrimaryOOFExternal == baseValue ? string.Empty : Properties.Settings.Default.PrimaryOOFExternal;
            instance.PrimaryOOFInternalMessage = OOFScheduling.Properties.Settings.Default.PrimaryOOFInternal == baseValue ? string.Empty : Properties.Settings.Default.PrimaryOOFInternal;
            instance.SecondaryOOFExternalMessage = OOFScheduling.Properties.Settings.Default.SecondaryOOFExternal == baseValue ? string.Empty : Properties.Settings.Default.SecondaryOOFExternal;
            instance.SecondaryOOFInternalMessage = OOFScheduling.Properties.Settings.Default.SecondaryOOFInternal == baseValue ? string.Empty : Properties.Settings.Default.SecondaryOOFInternal;
            instance.IsOnCallModeOn = OOFScheduling.Properties.Settings.Default.enableOnCallMode;

            LogProperties();

            OOFSponder.Logger.Info("Successfully read settings");
        }

        ~OOFData()
        {
            Dispose(false);
        }

        public void WriteProperties(bool disposing = false)
        {
            OOFSponder.Logger.Info("Persisting settings");

            Properties.Settings.Default.PrimaryOOFExternal = instance.PrimaryOOFExternalMessage;
            OOFSponder.Logger.Info("Persisted PrimaryOOFExternalMessage");

            Properties.Settings.Default.PrimaryOOFInternal = instance.PrimaryOOFInternalMessage;
            OOFSponder.Logger.Info("Persisted PrimaryOOFInternalMessage");

            Properties.Settings.Default.SecondaryOOFExternal = instance.SecondaryOOFExternalMessage;
            OOFSponder.Logger.Info("Persisted SecondaryOOFExternalMessage");

            Properties.Settings.Default.SecondaryOOFInternal = instance.SecondaryOOFInternalMessage;
            OOFSponder.Logger.Info("Persisted SecondaryOOFInternalMessage");

            Properties.Settings.Default.PermaOOFDate = instance.PermaOOFDate;
            OOFSponder.Logger.Info("Persisted PermaOOFDate");

            Properties.Settings.Default.workingHours = instance.WorkingHours;
            OOFSponder.Logger.Info("Persisted WorkingHours");

            Properties.Settings.Default.enableOnCallMode = instance.IsOnCallModeOn;
            OOFSponder.Logger.Info("Persisted enableOnCallMode = " + instance.IsOnCallModeOn.ToString());

            Properties.Settings.Default.Save();
            OOFSponder.Logger.Info("Persisted settings");

            if (disposing)
            {
                Dispose();
            }

        }

        #region IDisposable Members

        public void Dispose()
        {

            Dispose(true);

            // Turn off calling the finalizer
            System.GC.SuppressFinalize(this);

        }

        #endregion

        public void Dispose(bool disposing)
        {
            // Do not dispose of an owned managed object (one with a
            // finalizer) if called by member finalize,
            // as the owned managed objects finalize method
            // will be (or has been) called by finalization queue
            // processing already
            WriteProperties(disposing);
        }
    }
    public class OOFInstance
    {
        private DateTime _startTime;
        private DateTime _endTime;
        private DayOfWeek _dayofWeek;
        internal bool isOnCallModeEnabled = false;

        private bool _isOOF;
        public bool IsOOF
        {
            get
            {
                if (isOnCallModeEnabled)
                {
                    return !_isOOF;
                }
                else
                {
                    return _isOOF;
                }
            }

            set => _isOOF = value;
        }

        internal DateTime StartTime
        {
            get
            {
                //need to return the *actual* day and not just the day of week
                return _startTime;
            }
            set => _startTime = value;
        }
        internal DateTime EndTime
        {
            get
            {
                //need to return the *actual* day and not just the day of week
                return _endTime;
            }
            set => _endTime = value;
        }

        internal DayOfWeek DayOfWeek { get => _dayofWeek; set => _dayofWeek = value; }

        internal OOFInstance()
        {

        }

        internal OOFInstance (OOFInstance OOFInstance, int DaysToShift)
        {
            _startTime = OOFInstance.StartTime.AddDays(DaysToShift);
            _endTime = OOFInstance.EndTime.AddDays(DaysToShift); ;
            _dayofWeek = OOFInstance.DayOfWeek;
            _isOOF = OOFInstance.IsOOF;

        }
    }

    public static class DateTimeExtensions
    {
        //figures out the actual day from a generic day of the week
        public static DateTime EquivalentDateTime(this DateTime dtOld, DayOfWeek dayOfWeek)
        {
            int referenceDayofWeek = (int)dayOfWeek;
            int todayDayofWeek = (int)DateTime.Today.DayOfWeek;
            int deltaDayofWeek = referenceDayofWeek - todayDayofWeek;
            //if (deltaDayofWeek <0) {deltaDayofWeek += 7;} //if the day of the week is *before* today, then we are really talking about next week
            DateTime _localDT = DateTime.Today.AddDays(deltaDayofWeek).AddHours(dtOld.Hour).AddMinutes(dtOld.Minute);
            return _localDT;
        }
    }
}
