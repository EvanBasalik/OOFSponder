using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using OOFSponder;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace OOFScheduling
{

    public class OOFData
    {
        internal DateTime PermaOOFDate { get; set; }
        static string DummyHTML = @"<BODY scroll=auto></BODY>";

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
        private string StoredPrimaryOOFExternalMessage { get; set; }
        private string _primaryOOFExternalMessage = string.Empty;
        internal string PrimaryOOFExternalMessage { 
            get
            {
                return _primaryOOFExternalMessage;
            }
            
            set
            {
                //if a new value is being passed in, then persist to offline AppData storage
                //also update the stored value
                //fail out if value is empty or the same as DummyHTML (the default prior to any editing)
                if (value != StoredPrimaryOOFExternalMessage && value != "" && value != DummyHTML)
                {
                    Logger.Info("Primary OOF External has changed - persisting to AppData and updating stored value");
                    OOFData.Instance.StoredPrimaryOOFExternalMessage = value;
                    OOFData.Instance.SaveOOFMessageOffline(OOFData.OOFMessageType.PrimaryExternal, value);
                }
                _primaryOOFExternalMessage = value;
            }
        
        }
        private string StoredPrimaryOOFInternalMessage { get; set; }
        private string _primaryOOFInternalMessage = string.Empty;
        internal string PrimaryOOFInternalMessage
        {
            get
            {
                return _primaryOOFInternalMessage;
            }

            set
            {
                //if a new value is being passed in, then persist to offline AppData storage
                //also update the stored value
                //fail out if value is empty or the same as DummyHTML (the default prior to any editing)
                if (value != StoredPrimaryOOFInternalMessage && value != "" && value != DummyHTML)
                {
                    Logger.Info("Primary OOF Internal has changed - persisting to AppData and updating stored value");
                    OOFData.Instance.StoredPrimaryOOFInternalMessage = value;
                    OOFData.Instance.SaveOOFMessageOffline(OOFData.OOFMessageType.PrimaryInternal, value);
                }
                _primaryOOFInternalMessage = value;
            }

        }
        internal string StoredSecondaryOOFExternalMessage { get; set; }
        private string _secondaryOOFExternalMessage = string.Empty;
        internal string SecondaryOOFExternalMessage
        {
            get
            {
                return _secondaryOOFExternalMessage;
            }

            set
            {
                //if a new value is being passed in, then persist to offline AppData storage
                //also update the stored value
                //fail out if value is empty or the same as DummyHTML (the default prior to any editing)
                if (value != StoredSecondaryOOFExternalMessage && value != "" && value != DummyHTML)
                {
                    Logger.Info("Secondary OOF External has changed - persisting to AppData and updating stored value");
                    OOFData.Instance.StoredSecondaryOOFExternalMessage = value;
                    OOFData.Instance.SaveOOFMessageOffline(OOFData.OOFMessageType.SecondaryExternal, value);
                }
                _secondaryOOFExternalMessage = value;
            }

        }

        internal string StoredSecondaryOOFInternalMessage { get; set; }
        private string _secondaryOOFInternalMessage = string.Empty;
        internal string SecondaryOOFInternalMessage
        {
            get
            {
                return _secondaryOOFInternalMessage;
            }

            set
            {
                //if a new value is being passed in, then persist to offline AppData storage
                //also update the stored value
                //fail out if value is empty or the same as DummyHTML (the default prior to any editing)
                if (value != StoredSecondaryOOFInternalMessage && value != "" && value != DummyHTML)
                {
                    Logger.Info("Secondary OOF Interal has changed - persisting to AppData and updating stored value");
                    OOFData.Instance.StoredSecondaryOOFInternalMessage = value;
                    OOFData.Instance.SaveOOFMessageOffline(OOFData.OOFMessageType.SecondaryInternal, value);
                }
                _secondaryOOFInternalMessage = value;
            }

        }

        internal static string OOFFileName (OOFMessageType messageType) 
        {
            return Path.Combine(SettingsHelpers.PerUserDataFolder(), DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + messageType.ToString() + ".html");
        }

        internal Collection<OOFInstance> _OOFCollection;
        internal Collection<OOFInstance> OOFCollection
        {
            get
            {
                if (_OOFCollection == null)
                {
                    _OOFCollection = new Collection<OOFInstance>();
                }

                if (_OOFCollection.Count != 7)
                {
                    //convert the array of string objects to real objects
                    string[] workingTimes = WorkingHours.Split('|');
                    for (int i = 0; i < 7; i++)
                    {
                        string[] currentWorkingTime = workingTimes[i].Split('~');
                        OOFInstance OOFItem = new OOFInstance();
                        OOFItem.dayOfWeek = (DayOfWeek)i;
                        OOFItem.StartTime = DateTime.Parse(currentWorkingTime[0]);
                        OOFItem.EndTime = DateTime.Parse(currentWorkingTime[1]);
                        if (currentWorkingTime[2] == "0")
                        {
                            OOFItem.IsOOF = false;
                        }
                        else
                        {
                            OOFItem.IsOOF = true;
                        }
                        OOFItem.isOnCallModeEnabled = this.IsOnCallModeOn;

                        _OOFCollection.Add(OOFItem);
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
        private const bool baseBool = false;
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
                return this.OOFCollection[(int)DateTime.Now.DayOfWeek];
            }
        }

        internal DateTime previousOOFPeriodEnd
        {
            get
            {
                string datePart = DateTime.Now.AddDays(-1).ToShortDateString();
                string timePart = this.OOFCollection[(int)(DateTime.Now.AddDays(-1).DayOfWeek)].EndTime.ToShortTimeString();
                DateTime _previousOOFPeriodEnd = DateTime.Parse(datePart + " " + timePart);
                return _previousOOFPeriodEnd;
            }
        }

        internal DateTime nextOOFPeriodStart
        {
            get
            {
                string datePart = DateTime.Now.AddDays(1).ToShortDateString();
                string timePart = this.OOFCollection[(int)(DateTime.Now.AddDays(1).DayOfWeek)].StartTime.ToShortTimeString();
                DateTime _nextOOFPeriodStart = DateTime.Parse(datePart + " " + timePart);
                return _nextOOFPeriodStart;
            }
        }

        internal DateTime nextOOFPeriodEnd
        {
            get
            {
                string datePart = DateTime.Now.AddDays(1).ToShortDateString();
                string timePart = this.OOFCollection[(int)(DateTime.Now.AddDays(1).DayOfWeek)].EndTime.ToShortTimeString();
                DateTime _nextOOFPeriodEnd = DateTime.Parse(datePart + " " + timePart);
                return _nextOOFPeriodEnd;
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
        public bool StartMinimized { get; internal set; }
        public object get { get; private set; }

        private void LogProperties()
        {
            OOFSponder.Logger.InfoPotentialPII("PermaOOFDate", PermaOOFDate.ToString());
            OOFSponder.Logger.InfoPotentialPII("WorkingHours", WorkingHours);
            OOFSponder.Logger.InfoPotentialPII("PrimaryOOFExternalMessage", PrimaryOOFExternalMessage);
            OOFSponder.Logger.InfoPotentialPII("PrimaryOOFInternalMessage", PrimaryOOFInternalMessage);
            OOFSponder.Logger.InfoPotentialPII("SecondaryOOFExternalMessage", SecondaryOOFExternalMessage);
            OOFSponder.Logger.InfoPotentialPII("SecondaryOOFInternalMessage", SecondaryOOFInternalMessage);
            OOFSponder.Logger.Info("IsOnCallModeOn: " + IsOnCallModeOn);
            OOFSponder.Logger.Info("StartMinimized: " + StartMinimized);
        }

        private void ReadProperties()
        {
            OOFSponder.Logger.Info("Reading settings");

            //new approach using appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                //later added files override previous ones
                .AddJsonFile("appsettings.json")
                .AddJsonFile(Path.Combine(SettingsHelpers.PerUserDataFolder(), SettingsHelpers.PerUserSettingsFile()))
                .Build();

            var section = config.GetSection("OOFData");
            instance.PermaOOFDate = section.GetValue<DateTime>("PermaOOFDate");
            instance.WorkingHours = section.GetValue<string>("WorkingHours") == baseValue ? string.Empty : section.GetValue<string>("WorkingHours");

            //while reading in the Primary External, also store that value in a secondary Stored field for the Save comparison
            instance.PrimaryOOFExternalMessage = instance.StoredPrimaryOOFExternalMessage = section.GetValue<string>("PrimaryOOFExternalMessage") == baseValue ? string.Empty : section.GetValue<string>("PrimaryOOFExternalMessage");

            //while reading in the Primary Internal, also store that value in a secondary Stored field for the Save comparison
            instance.PrimaryOOFInternalMessage = instance.StoredPrimaryOOFInternalMessage = section.GetValue<string>("PrimaryOOFInternalMessage") == baseValue ? string.Empty : section.GetValue<string>("PrimaryOOFInternalMessage");

            //while reading in the Secondary External, also store that value in a secondary Stored field for the Save comparison
            instance.SecondaryOOFExternalMessage = instance.StoredSecondaryOOFExternalMessage = section.GetValue<string>("SecondaryOOFExternalMessage") == baseValue ? string.Empty : section.GetValue<string>("SecondaryOOFExternalMessage");

            //while reading in the Secondary Internal, also store that value in a secondary Stored field for the Save comparison
            instance.SecondaryOOFInternalMessage = instance.StoredSecondaryOOFInternalMessage = section.GetValue<string>("SecondaryOOFInternalMessage") == baseValue ? string.Empty : section.GetValue<string>("SecondaryOOFInternalMessage");

            instance.IsOnCallModeOn = section.GetValue<bool>("IsOnCallModeOn") == baseBool ? false : section.GetValue<bool>("IsOnCallModeOn");
            instance.StartMinimized = section.GetValue<bool>("StartMinimized") == baseBool ? false : section.GetValue<bool>("StartMinimized");


            //old approach using app.config
            //instance.PermaOOFDate = OOFScheduling.Properties.Settings.Default.PermaOOFDate;
            //instance.WorkingHours = OOFScheduling.Properties.Settings.Default.workingHours == baseValue ? string.Empty : Properties.Settings.Default.workingHours;

            //while reading in the Primary External, also store that value in a secondary Stored field for the Save comparison
            //instance.PrimaryOOFExternalMessage = instance.StoredPrimaryOOFExternalMessage = OOFScheduling.Properties.Settings.Default.PrimaryOOFExternal == baseValue ? string.Empty : Properties.Settings.Default.PrimaryOOFExternal;

            //while reading in the Primary Internal, also store that value in a secondary Stored field for the Save comparison
            //instance.PrimaryOOFInternalMessage = instance.StoredPrimaryOOFInternalMessage = OOFScheduling.Properties.Settings.Default.PrimaryOOFInternal == baseValue ? string.Empty : Properties.Settings.Default.PrimaryOOFInternal;

            //while reading in the Secondary External, also store that value in a secondary Stored field for the Save comparison
            //instance.SecondaryOOFExternalMessage = instance.StoredSecondaryOOFExternalMessage = OOFScheduling.Properties.Settings.Default.SecondaryOOFExternal == baseValue ? string.Empty : Properties.Settings.Default.SecondaryOOFExternal;

            //while reading in the Secondary Internal, also store that value in a secondary Stored field for the Save comparison
            //instance.SecondaryOOFInternalMessage = instance.StoredSecondaryOOFInternalMessage = OOFScheduling.Properties.Settings.Default.SecondaryOOFInternal == baseValue ? string.Empty : Properties.Settings.Default.SecondaryOOFInternal;

            //instance.IsOnCallModeOn = OOFScheduling.Properties.Settings.Default.enableOnCallMode == baseBool ? false : Properties.Settings.Default.enableOnCallMode;
            //instance.StartMinimized = OOFScheduling.Properties.Settings.Default.startMinimized == baseBool ? false : Properties.Settings.Default.startMinimized;

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

            //new method using appsettings.json
            SettingsHelpers.AddOrUpdateAppSetting("OOFData:PrimaryOOFExternalMessage", instance.PrimaryOOFExternalMessage);
            OOFSponder.Logger.Info("Persisted PrimaryOOFExternalMessage");

            SettingsHelpers.AddOrUpdateAppSetting("OOFData:PrimaryOOFInternalMessage", instance.PrimaryOOFInternalMessage);
            OOFSponder.Logger.Info("Persisted PrimaryOOFInternalMessage");

            SettingsHelpers.AddOrUpdateAppSetting("OOFData:SecondaryOOFExternalMessage", instance.SecondaryOOFExternalMessage);
            OOFSponder.Logger.Info("Persisted SecondaryOOFExternalMessage");

            SettingsHelpers.AddOrUpdateAppSetting("OOFData:SecondaryOOFInternalMessage", instance.SecondaryOOFInternalMessage);
            OOFSponder.Logger.Info("Persisted SecondaryOOFExternalMessage");

            SettingsHelpers.AddOrUpdateAppSetting("OOFData:PermaOOFDate", instance.PermaOOFDate);
            OOFSponder.Logger.Info("Persisted PermaOOFDate");

            SettingsHelpers.AddOrUpdateAppSetting("OOFData:WorkingHours", instance.WorkingHours);
            OOFSponder.Logger.Info("Persisted WorkingHours");

            SettingsHelpers.AddOrUpdateAppSetting("OOFData:IsOnCallModeOn", instance.IsOnCallModeOn);
            OOFSponder.Logger.Info("Persisted IsOnCallModeOn");

            SettingsHelpers.AddOrUpdateAppSetting("OOFData:StartMinimized", instance.StartMinimized);
            OOFSponder.Logger.Info("Persisted StartMinimized");

            //old method using appsettings.config
            //Properties.Settings.Default.PrimaryOOFExternal = instance.PrimaryOOFExternalMessage;
            //OOFSponder.Logger.Info("Persisted PrimaryOOFExternalMessage");

            //Properties.Settings.Default.PrimaryOOFInternal = instance.PrimaryOOFInternalMessage;
            //OOFSponder.Logger.Info("Persisted PrimaryOOFInternalMessage");

            //Properties.Settings.Default.SecondaryOOFExternal = instance.SecondaryOOFExternalMessage;
            //OOFSponder.Logger.Info("Persisted SecondaryOOFExternalMessage");

            //Properties.Settings.Default.SecondaryOOFInternal = instance.SecondaryOOFInternalMessage;
            //OOFSponder.Logger.Info("Persisted SecondaryOOFInternalMessage");

            //Properties.Settings.Default.PermaOOFDate = instance.PermaOOFDate;
            //OOFSponder.Logger.Info("Persisted PermaOOFDate");

            //Properties.Settings.Default.workingHours = instance.WorkingHours;
            //OOFSponder.Logger.Info("Persisted WorkingHours");

            //Properties.Settings.Default.enableOnCallMode = instance.IsOnCallModeOn;
            //OOFSponder.Logger.Info("Persisted enableOnCallMode = " + instance.IsOnCallModeOn.ToString());

            //Properties.Settings.Default.startMinimized = instance.StartMinimized;
            //OOFSponder.Logger.Info("Persisted startMinimized = " + instance.StartMinimized.ToString());

            //Properties.Settings.Default.Save();
            OOFSponder.Logger.Info("Persisted settings");

            if (disposing)
            {
                Dispose();
            }

        }

        internal enum OOFMessageType
        {
            PrimaryInternal=0,
            PrimaryExternal=1,
            SecondaryInternal=2,
            SecondaryExternal=3
        }

        internal bool SaveOOFMessageOffline(OOFMessageType messageType, string OOFMessageAsHTML)
        {
            bool _result = false;
            string _folderName = SettingsHelpers.PerUserDataFolder();
            string _fileName = OOFFileName(messageType);

            try
            {


                //first, create the folder if necessary
                if (!Directory.Exists(_folderName))
                {
                    // Create the directory
                    Directory.CreateDirectory(_folderName);
                    Logger.Info("Directory created successfully: " + _folderName);
                }

                File.WriteAllText(_fileName, OOFMessageAsHTML);
                Logger.Info("File created successfully: " + _fileName);

                _result = true;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }

            //clean up the old default files older than 3 iterations ago
            //if this fails, don't worry about it
            //the files are small and in an application-specific directory
            try
            {
                CleanUpOldOOFMessages(_folderName, messageType, 10);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return _result;

        }

        private bool CleanUpOldOOFMessages(string folderName, OOFMessageType OOFMessageToClean, int iterationsToKeep)
        {
            bool _result = false;

            try
            {
                // Get the files in the directory and order them by last write time in descending order
                var files = Directory.GetFiles(folderName)
                    .Where(f => Path.GetFileName(f).Contains(OOFMessageToClean.ToString()))
                    .Select(f => new FileInfo(f))
                    .OrderByDescending(f => f.LastWriteTime)
                    .ToList();


                // Keep the three most recent files and delete the rest
                for (int i = iterationsToKeep; i < files.Count(); i++)
                {
                    files[i].Delete();
                }

                _result = true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return _result;

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
        internal DayOfWeek dayOfWeek;
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
                return _startTime.EquivalentDateTime();
            }
            set => _startTime = value;
        }
        internal DateTime EndTime
        {
            get
            {
                //need to return the *actual* day and not just the day of week
                return _endTime.EquivalentDateTime();
            }
            set => _endTime = value;
        }
    }
    public static class DateTimeExtensions
    {
        //figures out the actual day from a generic day of the week
        public static DateTime EquivalentDateTime(this DateTime dtOld)
        {
            int num = (int)dtOld.DayOfWeek;
            int num2 = (int)DateTime.Today.DayOfWeek;
            return DateTime.Today.AddDays(num - num2).AddHours(dtOld.Hour).AddMinutes(dtOld.Minute);
        }
    }
}
