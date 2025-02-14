using Microsoft.Extensions.Configuration;
using OOFSponder;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace OOFScheduling
{

    public class OOFData
    {
        internal DateTime PermaOOFDate { get; set; }
        static string DummyHTML = @"<BODY scroll=auto></BODY>";

        internal bool isEmptyOrDefaultOOFMessage(string input)
        {
            bool _result = false;
            if (input == string.Empty || input == DummyHTML)
            {
                _result = true;
            }

            return _result;
        }

        internal string UserSettingsSource { get; set; }

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
        private string _primaryOOFExternalMessage = string.Empty;
        internal string PrimaryOOFExternalMessage
        {
            get
            {
                return _primaryOOFExternalMessage;
            }

            set
            {
                //if a new value is being passed in, then persist to offline AppData storage
                //fail out if value is empty or the same as DummyHTML (the default prior to any editing)
                if (value != _primaryOOFExternalMessage && !isEmptyOrDefaultOOFMessage(value))
                {
                    //if _primaryOOFExternalMessage is an empty string, then this is the initial data load
                    //so it isn't an actual change in the OOF message
                    if (_primaryOOFExternalMessage != string.Empty)
                    {
                        Logger.Info("Primary OOF External has changed - persisting to AppData");
                        OOFData.Instance.SaveOOFMessageOffline(OOFData.OOFMessageType.PrimaryExternal, value);
                    }
                }
                _primaryOOFExternalMessage = value;
            }

        }

        private enum ExternalAudienceScope
        {
            All = 0, ContactsOnly=1, None=2
        }

        private ExternalAudienceScope _externalAudienceScope;

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
                //fail out if value is empty or the same as DummyHTML (the default prior to any editing)
                if (value != _primaryOOFInternalMessage && !isEmptyOrDefaultOOFMessage(value))
                {
                    //if _primaryOOFExternalMessage is an empty string, then this is the initial data load
                    //so it isn't an actual change in the OOF message
                    if (_primaryOOFInternalMessage != string.Empty)
                    {
                        Logger.Info("Primary OOF Internal has changed - persisting to AppData");
                        OOFData.Instance.SaveOOFMessageOffline(OOFData.OOFMessageType.PrimaryInternal, value);
                    }
                }

                _primaryOOFInternalMessage = value;
            }

        }
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
                //fail out if value is empty or the same as DummyHTML (the default prior to any editing)
                if (value != _secondaryOOFExternalMessage && !isEmptyOrDefaultOOFMessage(value))
                {
                    //if _primaryOOFExternalMessage is an empty string, then this is the initial data load
                    //so it isn't an actual change in the OOF message
                    if (_secondaryOOFExternalMessage != string.Empty)
                    {
                        Logger.Info("Secondary OOF External has changed - persisting to AppData");
                        OOFData.Instance.SaveOOFMessageOffline(OOFData.OOFMessageType.SecondaryExternal, value);
                    }
                }

                _secondaryOOFExternalMessage = value;
            }

        }

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
                //fail out if value is empty or the same as DummyHTML (the default prior to any editing)
                if (value != _secondaryOOFInternalMessage && !isEmptyOrDefaultOOFMessage(value))
                {
                    //if _primaryOOFExternalMessage is an empty string, then this is the initial data load
                    //so it isn't an actual change in the OOF message
                    if (_secondaryOOFInternalMessage != string.Empty)
                    {
                        Logger.Info("Secondary OOF Internal has changed - persisting to AppData");
                        OOFData.Instance.SaveOOFMessageOffline(OOFData.OOFMessageType.SecondaryInternal, value);
                    }
                }

                _secondaryOOFInternalMessage = value;
            }

        }

        internal static string OOFFileName(OOFMessageType messageType)
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
                    OOFSponder.Logger.Info("Instace null - instantiating via ReadProperties");
                    instance = new OOFData();
                    instance.ReadProperties();
                }
                return instance;
            }
        }

        internal bool HaveNecessaryData
        {
            get
            {
                bool _result = false;

                //we need the OOF messages and working hours
                //also, don't need to check SecondaryOOF messages for two reasons:
                //1) they won't always be set
                //2) the UI flow won't let you get here with permaOOF if they aren't set
                if (!isEmptyOrDefaultOOFMessage(OOFData.Instance.PrimaryOOFExternalMessage) && !isEmptyOrDefaultOOFMessage(OOFData.Instance.PrimaryOOFInternalMessage)
    && OOFData.Instance.WorkingHours != "")
                {
                    _result = true;
                }

                OOFSponder.Logger.Info("HaveNecessaryData: ", _result);
                return _result;
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
            OOFSponder.Logger.Info("UserSettingsSource: " + UserSettingsSource);
        }

        private void ReadProperties()
        {
            OOFSponder.Logger.Info("Reading settings");

            //new approach using appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                //later added files override previous ones
                //this is critical when we add new properties
                //we just need to add them to the base appsettings.json
                //and if the user's file doesn't have them, the new default gets added
                //from the updated appsettings.json
                .AddJsonFile("appsettings.json")
                .AddJsonFile(Path.Combine(SettingsHelpers.PerUserDataFolder(), SettingsHelpers.PerUserSettingsFile()), true)
                .Build();

            OOFSponderConfig.Root OOFSponderConfig = new OOFSponderConfig.Root();
            config.Bind(OOFSponderConfig);

            OOFSponder.Logger.Info("Successfully read properties and bound to class");
            OOFSponder.Logger.InfoPotentialPII("PrimaryOOFExternalMessage", OOFSponderConfig.OOFData.PrimaryOOFExternalMessage);

            instance.PermaOOFDate = OOFSponderConfig.OOFData.PermaOOFDate;
            instance.WorkingHours = OOFSponderConfig.OOFData.WorkingHours == baseValue ? string.Empty : OOFSponderConfig.OOFData.WorkingHours;

            //while reading in the Primary External, also store that value in a secondary Stored field for the Save comparison
            instance.PrimaryOOFExternalMessage = OOFSponderConfig.OOFData.PrimaryOOFExternalMessage == baseValue ? string.Empty : OOFSponderConfig.OOFData.PrimaryOOFExternalMessage;

            //while reading in the Primary Internal, also store that value in a secondary Stored field for the Save comparison
            instance.PrimaryOOFInternalMessage = OOFSponderConfig.OOFData.PrimaryOOFInternalMessage == baseValue ? string.Empty : OOFSponderConfig.OOFData.PrimaryOOFInternalMessage;

            //while reading in the Secondary External, also store that value in a secondary Stored field for the Save comparison
            instance.SecondaryOOFExternalMessage = OOFSponderConfig.OOFData.SecondaryOOFExternalMessage == baseValue ? string.Empty : OOFSponderConfig.OOFData.SecondaryOOFExternalMessage;

            //while reading in the Secondary Internal, also store that value in a secondary Stored field for the Save comparison
            instance.SecondaryOOFInternalMessage = OOFSponderConfig.OOFData.SecondaryOOFInternalMessage == baseValue ? string.Empty : OOFSponderConfig.OOFData.SecondaryOOFInternalMessage;

            instance.IsOnCallModeOn = OOFSponderConfig.OOFData.IsOnCallModeOn == baseBool ? false : OOFSponderConfig.OOFData.IsOnCallModeOn;
            instance.StartMinimized = OOFSponderConfig.OOFData.StartMinimized == baseBool ? false : OOFSponderConfig.OOFData.StartMinimized;

            instance.UserSettingsSource = OOFSponderConfig.UserSettingsSource;

            LogProperties();

            OOFSponder.Logger.Info("Successfully read settings");
        }

        ~OOFData()
        {
            Dispose(false);
        }

        public void WriteProperties(bool disposing = false)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            if (!this.HaveNecessaryData)
            {
                Logger.Warning("Missing necessary data, so not persisting settings!");
                return;
            }

            OOFSponder.Logger.Info("Persisting settings");

            //special logging and message box for the intermittent nulling of the message
            if (instance.PrimaryOOFExternalMessage == DummyHTML)
            {
#if DEBUG
                MessageBox.Show("OOF message has been nulled!!!");
#endif
                Logger.Error("NULLED: OOF message has been nulled!!!");
            }

            //new method using appsettings.json and JsonSerializer
            //map everything to an OOFSponderConfig object, then serialize to disk
            OOFSponderConfig.Root config = new OOFSponderConfig.Root();
            config.OOFData.PrimaryOOFExternalMessage = instance.PrimaryOOFExternalMessage;
            config.OOFData.PrimaryOOFInternalMessage = instance.PrimaryOOFInternalMessage;
            config.OOFData.SecondaryOOFExternalMessage = instance.SecondaryOOFExternalMessage;
            config.OOFData.SecondaryOOFInternalMessage = instance.SecondaryOOFInternalMessage;
            config.OOFData.PermaOOFDate = instance.PermaOOFDate;
            config.OOFData.WorkingHours = instance.WorkingHours;
            config.OOFData.IsOnCallModeOn = instance.IsOnCallModeOn;
            config.OOFData.StartMinimized = instance.StartMinimized;
            config.UserSettingsSource = instance.UserSettingsSource;


            // Serialize the person object to JSON
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = System.Text.Json.JsonSerializer.Serialize(config, options);

            // Write the JSON string to a file
            //user-specific appsettings.json
            string userappsettingsFile = Path.Combine(SettingsHelpers.PerUserDataFolder(), SettingsHelpers.PerUserSettingsFile());
            System.IO.File.WriteAllText(userappsettingsFile, jsonString);

            OOFSponder.Logger.Info("Persisted settings");

            if (disposing)
            {
                Dispose();
            }

        }

        internal enum OOFMessageType
        {
            PrimaryInternal = 0,
            PrimaryExternal = 1,
            SecondaryInternal = 2,
            SecondaryExternal = 3
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
                //
                Logger.Info("File created successfully: " + _fileName.Replace(_folderName, ""));

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
