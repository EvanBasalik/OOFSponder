﻿using Microsoft.Extensions.Configuration;
using OOFSponder;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace OOFScheduling
{

    public class OOFData
    {
        public DateTime PermaOOFDate { get; set; }
        static string DummyHTML = @"<BODY scroll=auto></BODY>";
        internal static readonly string HTMLReadOnlyIndicator = "<BODY style=\"BACKGROUND-COLOR: lightgray\"";

        internal enum CurrentUIState
        {
            Primary = 0,
            Secondary = 1
        }

        internal bool isEmptyOrDefaultOOFMessage(string input)
        {
            bool _result = false;

            string output1 = input.RemoveHTML();
            string output = new string(output1.Where(c => !char.IsWhiteSpace(c)).ToArray());

            if (output == string.Empty || output == DummyHTML)
            {
                _result = true;
            }

            return _result;
        }

        public string UserSettingsSource { get; set; }

        private string _workingHours;
        public string WorkingHours
        {
            get
            {
                return _workingHours;
            }
            set
            {
                _workingHours = value;
                //if we update WorkingHours, then blow away WorkingDayCollection
                _WorkingDayCollection = null;
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
                //also add special case for the UI read-only stuff

                if (value != _primaryOOFExternalMessage && !isEmptyOrDefaultOOFMessage(value) && !value.Contains(HTMLReadOnlyIndicator))
                {
                    //if _primaryOOFExternalMessage is an empty string, then this is the initial data load
                    //so it isn't an actual change in the OOF message
                    if (_primaryOOFExternalMessage != string.Empty)
                    {
                        Logger.Info("Primary OOF External has changed - persisting to AppData");
                        OOFData.Instance.SaveOOFMessageOffline(OOFData.OOFMessageType.PrimaryExternal, value);
                    }
                }

                //if value contains the special code, then it is for visualization only
                //and shouldn't be saved
                if (!value.Contains(HTMLReadOnlyIndicator))
                {
                    _primaryOOFExternalMessage = value;
                }
            }

        }

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
                //also add special case for the UI read-only stuff
                if (value != _secondaryOOFExternalMessage && !isEmptyOrDefaultOOFMessage(value) && !value.Contains(HTMLReadOnlyIndicator))
                {
                    //if _primaryOOFExternalMessage is an empty string, then this is the initial data load
                    //so it isn't an actual change in the OOF message
                    if (_secondaryOOFExternalMessage != string.Empty)
                    {
                        Logger.Info("Secondary OOF External has changed - persisting to AppData");
                        OOFData.Instance.SaveOOFMessageOffline(OOFData.OOFMessageType.ExtendedExternal, value);
                    }
                }

                //if value contains the special code, then it is for visualization only
                //and shouldn't be saved
                if (!value.Contains(HTMLReadOnlyIndicator))
                {
                    _secondaryOOFExternalMessage = value;
                }
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
                        OOFData.Instance.SaveOOFMessageOffline(OOFData.OOFMessageType.ExtendedInternal, value);
                    }
                }

                _secondaryOOFInternalMessage = value;
            }

        }

        internal static string OOFFileName(OOFMessageType messageType)
        {
            return Path.Combine(Logger.PerUserDataFolder(), DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + messageType.ToString() + ".html");
        }

        internal Collection<WorkingDay> _WorkingDayCollection;
        public Collection<WorkingDay> WorkingDayCollection
        {
            get
            {
                if (_WorkingDayCollection == null)
                {
                    _WorkingDayCollection = new Collection<WorkingDay>();
                }

                //if it already has 7 days, then that means it was read in from the config file
                //as a persisted value
                //In that case, we can just return the collection
                if (_WorkingDayCollection.Count == 7)
                {
                    //if we have all 7 days, then return the collection
                    Logger.Info("Have 7 WorkingDays, so just returning the collection");
                    return _WorkingDayCollection;
                }

                if (WorkingHours == string.Empty && _WorkingDayCollection.Count == 0)
                {
                    //if WorkingHours has never been set and we and we don't have WorkingDayCollection
                    //then create one with the defaults
                    Logger.Info("WorkingHours = default and no existing WorkingDayCollection, so defaulting to base collection of WorkingDays");
                    for (int i = 0; i < 7; i++)
                    {
                        // Default to 9am-5pm working hours, not OOF
                        WorkingDay OOFItem = new WorkingDay();
                        OOFItem.DayOfWeek = (DayOfWeek)i;
                        OOFItem.StartTime = DateTime.Now.Date.AddHours(9);
                        OOFItem.EndTime = DateTime.Now.Date.AddHours(17);
                        OOFItem.IsOOF = false;
                        _WorkingDayCollection.Add(OOFItem);
                    }
                }

                //if we don't have all the working days, then try to parse the WorkingHours string
                //if we aren't able to parse, fill in defaults
                if (_WorkingDayCollection.Count < 7 && WorkingHours != string.Empty)
                {
                    //convert the array of string objects to real objects
                    string[] workingTimes = WorkingHours.Split('|');

                    //if we have a value for all 7 days, then try to parse each day
                    if (workingTimes.Length == 7)
                    {
                        Logger.Info("Have 7 workingTimes, so converting to WorkingDay objects");
                        for (int i = 0; i < 7; i++)
                        {
                            string[] currentWorkingTime = workingTimes[i].Split('~');
                            WorkingDay OOFItem = new WorkingDay();
                            OOFItem.DayOfWeek = (DayOfWeek)i;

                            //try to parse the results of splitting the WorkingHours string
                            //if it fails, then default to 9am-5pm working hours, not OOF
                            try
                            {
                                OOFItem.StartTime = DateTime.Parse(currentWorkingTime[0]);
                                OOFItem.EndTime = DateTime.Parse(currentWorkingTime[1]);
                                if (currentWorkingTime[2] == "0")
                                {
                                    OOFItem.IsOOF = true;
                                }
                                else
                                {
                                    OOFItem.IsOOF = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error("Unable to parse WorkingHours string into valid date for: " + OOFItem.DayOfWeek.ToString(), ex);
                                // Default to 9am-5pm working hours, not OOF
                                OOFItem.StartTime = DateTime.Now.Date.AddHours(9);
                                OOFItem.EndTime = DateTime.Now.Date.AddHours(17);
                                OOFItem.IsOOF = false;

                                //set a flag so we know to tell the user that we couldn't parse the WorkingHours string
                                this.UnabletoParseWorkingHours = true;
                            }

                            _WorkingDayCollection.Add(OOFItem);
                        }

                    }
                    //if we don't have all 7, then assume WorkingHours or WorkingDays is busted and create a new one with defaults
                    else
                    {
                        //first, clear out the collection so we don't end up with more than 14
                        _WorkingDayCollection.Clear();

                        Logger.Info("Couldn't parse WorkingHours, so defaulting to base collection of WorkingDays");
                        for (int i = 0; i < 7; i++)
                        {
                            // Default to 9am-5pm working hours, not OOF
                            WorkingDay OOFItem = new WorkingDay();
                            OOFItem.DayOfWeek = (DayOfWeek)i;
                            OOFItem.StartTime = DateTime.Now.Date.AddHours(8);
                            OOFItem.EndTime = DateTime.Now.Date.AddHours(17);
                            OOFItem.IsOOF = false;
                            _WorkingDayCollection.Add(OOFItem);
                        }

                        //set a flag so we know to tell the user that we couldn't parse the WorkingHours string
                        this.UnabletoParseWorkingHours = true;
                    }


                }

                return _WorkingDayCollection;
            }

            set { _WorkingDayCollection = value; }
        }

        //Track whether or not to run in OnCallMode
        //When in this mode, the OOF times get flipped and instead of 
        //tracking days on/days off, they will track a start/end for OOF *during* the working day
        public bool IsOnCallModeOn { get; set; }

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
        public void CalculateOOFTimes2(out DateTime StartTime, out DateTime EndTime, bool enableOnCallMode)
        {
            OOFSponder.Logger.Info("Using CalculationOOFTimes2");

            StartTime = DateTime.Now;
            EndTime = DateTime.Now;

            DateTime currentCheckDate = DateTime.Now;
            OOFSponder.Logger.Info($"currentCheckDate = {currentCheckDate}");

            WorkingDay currentWorkingDay = OOFData.Instance.currentWorkingDay;
            OOFSponder.Logger.Info($"currentWorkingDay.StartTime = {currentWorkingDay.StartTime}");
            OOFSponder.Logger.Info($"currentWorkingDay.Endtime = {currentWorkingDay.EndTime}");

            DateTime previousWorkingDayEnd = OOFData.Instance.PreviousWorkingDayEnd;
            OOFSponder.Logger.Info($"previousWorkingDayEnd = {previousWorkingDayEnd}");

            DateTime nextWorkingDayStart = OOFData.Instance.nextWorkingDayStart;
            OOFSponder.Logger.Info($"nextWorkingDayStart = {nextWorkingDayStart}");

            DateTime nextWorkingDayEnd = OOFData.Instance.nextWorkingDayEnd;
            OOFSponder.Logger.Info($"nextWorkingDayEnd = {nextWorkingDayEnd}");

            OOFSponder.Logger.Info($"enableOnCallMode = {enableOnCallMode}");

            if (currentCheckDate > previousWorkingDayEnd && currentCheckDate < nextWorkingDayStart)
            {
                OOFSponder.Logger.Info("currentCheckDate greater than previousWorkingDayEnd and less than nextWorkingDayStart");
                if (enableOnCallMode)
                {
                    StartTime = currentWorkingDay.StartTime;
                    EndTime = currentWorkingDay.EndTime;
                }
                else
                {
                    StartTime = previousWorkingDayEnd;
                    EndTime = nextWorkingDayStart;
                }
            }
            else if (currentCheckDate > currentWorkingDay.StartTime && currentCheckDate < currentWorkingDay.EndTime)
            {
                OOFSponder.Logger.Info("currentCheckDate greater than currentWorkingDay.StartTime and less than currentWorkingDay.EndTime");
                if (enableOnCallMode)
                {
                    StartTime = currentWorkingDay.StartTime;
                    EndTime = currentWorkingDay.EndTime;
                }
                else
                {
                    StartTime = currentWorkingDay.EndTime;
                    EndTime = nextWorkingDayStart;
                }
            }
            else
            {
                OOFSponder.Logger.Info("currentCheckDate greater than currentWorkingDay.EndTime");
                if (enableOnCallMode)
                {
                    StartTime = nextWorkingDayStart;
                    EndTime = nextWorkingDayEnd;
                }
                else
                {
                    StartTime = currentWorkingDay.EndTime;
                    EndTime = nextWorkingDayStart;
                }
            }
        }
        internal bool HaveNecessaryData
        {
            get
            {
                bool _result = false;

                if (useNewOOFMath)
                {
                    if (OOFData.Instance.WorkingDayCollection.Count != 7)
                    {
                        OOFSponder.Logger.Info("HaveNecessaryData - WorkingDayCollection:" + _result);
                        return _result;
                    }
                }
                else
                {
                    if (OOFData.Instance.WorkingHours == "")
                    {
                        OOFSponder.Logger.Info("HaveNecessaryData - WorkingHours:" + _result);
                        return _result;
                    }
                }


                //we need the OOF messages and working hours
                //also, don't need to check SecondaryOOF messages for two reasons:
                //1) they won't always be set
                //2) the UI flow won't let you get here with permaOOF if they aren't set
                if (!IsPermaOOFOn && !isEmptyOrDefaultOOFMessage(OOFData.Instance.PrimaryOOFExternalMessage) && !isEmptyOrDefaultOOFMessage(OOFData.Instance.PrimaryOOFInternalMessage))
                {
                    _result = true;
                    OOFSponder.Logger.Info("HaveNecessaryData: Normal OOF");
                    return _result;
                }

                //check the secondary condition where External Message Audience Scope == None
                //and PermaOOF is NOT on
                //in that case, it is OK for PrimaryOOFExternalMessage to be empty
                if (!OOFData.instance.IsPermaOOFOn && isEmptyOrDefaultOOFMessage(OOFData.instance.PrimaryOOFExternalMessage) && OOFData.instance.ExternalAudienceScope == Microsoft.Graph.ExternalAudienceScope.None)
                {
                    _result = true;
                    OOFSponder.Logger.Info("HaveNecessaryData: Normal OOF with Scope=None");
                    return _result;
                }

                //check the tertiary condition where External Message Audience Scope == None
                //and PermaOOF is on
                //in that case, it is OK for SecondaryOOFExternalMessage to be empty
                if (OOFData.instance.IsPermaOOFOn && isEmptyOrDefaultOOFMessage(OOFData.instance.SecondaryOOFExternalMessage) && OOFData.instance.ExternalAudienceScope == Microsoft.Graph.ExternalAudienceScope.None)
                {
                    _result = true;
                    OOFSponder.Logger.Info("HaveNecessaryData: PermaOOF with Scope=None");
                    return _result;
                }

                if (useNewOOFMath)
                {
                    if (OOFData.instance.IsPermaOOFOn && !isEmptyOrDefaultOOFMessage(OOFData.Instance.SecondaryOOFExternalMessage) && !isEmptyOrDefaultOOFMessage(OOFData.instance.SecondaryOOFInternalMessage)
    && OOFData.Instance.WorkingDayCollection.Count == 7)
                    {
                        _result = true;
                        OOFSponder.Logger.Info("HaveNecessaryData: PermaOOF");
                        return _result;
                    }
                }
                else
                {
                    if (OOFData.instance.IsPermaOOFOn && !isEmptyOrDefaultOOFMessage(OOFData.Instance.SecondaryOOFExternalMessage) && !isEmptyOrDefaultOOFMessage(OOFData.instance.SecondaryOOFInternalMessage)
    && OOFData.Instance.WorkingHours != "")
                    {
                        _result = true;
                        OOFSponder.Logger.Info("HaveNecessaryData: PermaOOF");
                        return _result;
                    }
                }

                OOFSponder.Logger.Info("HaveNecessaryData: " + _result);
                return _result;
            }
        }

        internal WorkingDay currentWorkingDay
        {
            get
            {
                //
                return this.WorkingDayCollection[(int)DateTime.Now.DayOfWeek];
            }
        }

        internal DateTime PreviousWorkingDayEnd
        {
            get
            {
                //need to find the EndTime of the last working day
                int daysback = -1;
                //create a way to exit if someone has all 7 days marked as OOF
                while (daysback >= -6)
                {
                    if (!WorkingDayCollection[(int)(DateTime.Now.AddDays(daysback).DayOfWeek)].IsOOF)
                    {
                        break;
                    }
                    else
                    {
                        daysback--;
                    }
                }

                DateTime datePart = DateTime.Now.AddDays(daysback);
                DateTime timePart = WorkingDayCollection[(int)(DateTime.Now.AddDays(daysback).DayOfWeek)].EndTime;
                long ticks = new DateTime(
                                        datePart.Year,
                                        datePart.Month,
                                        datePart.Day,
                                        timePart.Hour,
                                        timePart.Minute,
                                        0,
                                        new CultureInfo("en-US", false).Calendar).Ticks;
                DateTime _previousWorkingDayEnd = new DateTime(ticks);

                //string datePartOld = DateTime.Now.AddDays(daysback).ToShortDateString();
                //string timePartOld = this.WorkingDayCollection[(int)(DateTime.Now.AddDays(daysback).DayOfWeek)].EndTime.ToShortTimeString();
                //DateTime _previousOOFPeriodEndOld = DateTime.Parse(datePartOld + " " + timePartOld);
                return _previousWorkingDayEnd;
            }
        }

        internal DateTime nextWorkingDayStart
        {
            get
            {
                //need to find the StartTime of the next working day

                //start with today
                DateTime targetDateTime = DateTime.Now;
                int daysforward = 0;

                //if PermaOOF is on, then the target date is actually the day of PermaOOF
                //and look starting on PermaOOF
                if (IsPermaOOFOn)
                {
                    targetDateTime = PermaOOFDate;
                }

                //create a way to exit if someone has all 7 days marked as OOF
                while (daysforward <= 6)
                {

                    if (!WorkingDayCollection[(int)(targetDateTime.AddDays(daysforward).DayOfWeek)].IsOOF)
                    {
                        break;
                    }
                    else
                    {
                        daysforward++;
                    }
                }

                DateTime datePart = targetDateTime.AddDays(daysforward);
                DateTime timePart = WorkingDayCollection[(int)(targetDateTime.AddDays(daysforward).DayOfWeek)].StartTime;
                long ticks = new DateTime(
                                        datePart.Year,
                                        datePart.Month,
                                        datePart.Day,
                                        timePart.Hour,
                                        timePart.Minute,
                                        0,
                                        new CultureInfo("en-US", false).Calendar).Ticks;
                DateTime _nextWorkingDayStart = new DateTime(ticks);

                //string datePartOld = DateTime.Now.AddDays(daysforward).ToShortDateString();
                //string timePartOld = this.WorkingDayCollection[(int)(DateTime.Now.AddDays(daysforward).DayOfWeek)].StartTime.ToShortTimeString();
                //DateTime _nextOOFPeriodStartOld = DateTime.Parse(datePartOld + " " + timePartOld);
                return _nextWorkingDayStart;
            }
        }

        internal DateTime nextWorkingDayEnd
        {
            get
            {
                //need to find the EndTime of the next working day

                //by default start with today
                //and look starting tomorrow
                DateTime targetDateTime = DateTime.Now;
                int daysforward = 0;

                //if PermaOOF is on, then the target date is actually the day of PermaOOF
                //and look starting on PermaOOF
                if (IsPermaOOFOn)
                {
                    targetDateTime = PermaOOFDate;
                    daysforward = 0;
                }

                //create a way to exit if someone has all 7 days marked as OOF
                while (daysforward <= 6)
                {

                    if (!WorkingDayCollection[(int)(targetDateTime.AddDays(daysforward).DayOfWeek)].IsOOF)
                    {
                        break;
                    }
                    else
                    {
                        daysforward++;
                    }
                }

                DateTime datePart = targetDateTime.AddDays(daysforward);
                DateTime timePart = WorkingDayCollection[(int)(targetDateTime.AddDays(daysforward).DayOfWeek)].EndTime;
                long ticks = new DateTime(
                                        datePart.Year,
                                        datePart.Month,
                                        datePart.Day,
                                        timePart.Hour,
                                        timePart.Minute,
                                        0,
                                        new CultureInfo("en-US", false).Calendar).Ticks;
                DateTime _nextOOFPeriodEnd = new DateTime(ticks);

                string datePartOld = DateTime.Now.AddDays(1).ToShortDateString();
                string timePartOld = this.WorkingDayCollection[(int)(DateTime.Now.AddDays(1).DayOfWeek)].EndTime.ToShortTimeString();
                DateTime _nextOOFPeriodEndOld = DateTime.Parse(datePartOld + " " + timePartOld);
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

        public Microsoft.Graph.ExternalAudienceScope _externalAudienceScope;
        public bool UnabletoParseWorkingHours = false;

        public Microsoft.Graph.ExternalAudienceScope ExternalAudienceScope { get => _externalAudienceScope; set => _externalAudienceScope = value; }

        private void LogProperties()
        {
            //manually log the ones with potential PII
            OOFSponder.Logger.InfoPotentialPII("PrimaryOOFExternalMessage", PrimaryOOFExternalMessage);
            OOFSponder.Logger.InfoPotentialPII("PrimaryOOFInternalMessage", PrimaryOOFInternalMessage);
            OOFSponder.Logger.InfoPotentialPII("SecondaryOOFExternalMessage", SecondaryOOFExternalMessage);
            OOFSponder.Logger.InfoPotentialPII("SecondaryOOFInternalMessage", SecondaryOOFInternalMessage);

            //for everything else, use ObjectDumper
            //exclude the properties from above
            String _instanceString = ObjectDumper.Dump(instance, new DumpOptions()
            {
                //IndentChar = '\t',
                //IndentSize = 1,
                //LineBreakChar = Environment.NewLine,
                //DumpStyle = DumpStyle.Console,
                ExcludeProperties = new string[]
                {
                    nameof(instance.PrimaryOOFExternalMessage),
                    nameof(instance.PrimaryOOFInternalMessage),
                    nameof(instance.SecondaryOOFExternalMessage),
                    nameof(instance.SecondaryOOFInternalMessage)
                }
            });
            OOFSponder.Logger.Info(_instanceString);
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
                .AddJsonFile(Path.Combine(Logger.PerUserDataFolder(), SettingsHelpers.PerUserSettingsFile()), true)
                .Build();

            OOFSponderConfig.Root OOFSponderConfig = new OOFSponderConfig.Root();
            config.Bind(OOFSponderConfig);

            OOFSponder.Logger.Info("Successfully read properties and bound to class");

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

            instance.ExternalAudienceScope = OOFSponderConfig.OOFData.ExternalAudienceScope;

            instance.useNewOOFMath = OOFSponderConfig.OOFData.UseNewOOFMath;

            instance.WorkingDayCollection = OOFSponderConfig.OOFData.WorkingDayCollection;

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

            //TODO: rewrite this for real failures
            //special logging and message box for the intermittent nulling of the message
            if (instance.PrimaryOOFExternalMessage == DummyHTML)
            {
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
            config.OOFData.ExternalAudienceScope = (Microsoft.Graph.ExternalAudienceScope)instance.ExternalAudienceScope;
            config.UserSettingsSource = "OOFSponder_Core_" + RuntimeInformation.FrameworkDescription;
            config.OOFData.WorkingDayCollection = instance.WorkingDayCollection;
            config.OOFData.UseNewOOFMath = instance.useNewOOFMath;

            //TODO: reenable once I have confidence in the new workingDayCollection approach
            ////special code to deprecate WorkingHours if the new WorkingDayCollection
            ////is properly populate
            //if (instance.WorkingDayCollection.Count == 7)
            //{
            //    config.OOFData.WorkingHours = "No longer in use and can be deleted";
            //    Logger.Info("Wrote WorkingHours deprecation message");
            //}
            //else
            //{
            //    Logger.Info("WorkingHours still in use");
            //}

            // Serialize the person object to JSON
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = System.Text.Json.JsonSerializer.Serialize(config, options);

            // Write the JSON string to a file
            //user-specific appsettings.json
            string userappsettingsFile = Path.Combine(Logger.PerUserDataFolder(), SettingsHelpers.PerUserSettingsFile());
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
            ExtendedInternal = 2,
            ExtendedExternal = 3
        }

        internal bool SaveOOFMessageOffline(OOFMessageType messageType, string OOFMessageAsHTML)
        {
            bool _result = false;
            string _folderName = Logger.PerUserDataFolder();
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
    public class WorkingDay
    {
        private DateTime _startTime;
        private DateTime _endTime;
        public DayOfWeek DayOfWeek { get; set; }

        private bool _isOOF;
        public bool IsOOF
        {
            get
            {
                return _isOOF;
            }

            set => _isOOF = value;
        }

        public DateTime StartTime
        {
            get
            {
                //need to return the *actual* day and not just the day of week
                return _startTime.EquivalentDateTime();
            }
            set => _startTime = value;
        }
        public DateTime EndTime
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
