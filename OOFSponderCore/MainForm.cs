using Microsoft.Graph;
using Microsoft.Win32;
using Newtonsoft.Json;
using OOFSponder;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows.Forms;

namespace OOFScheduling
{
    public partial class MainForm : Form
    {
        private ContextMenuStrip trayMenu;

        //track if we are in the base instantiation
        //if we are, don't pop the OOFSponder toast on minimize
        private bool inInitialization = false;

        //controls whether or not the we show the form by default
        private bool allowshowdisplay = false;

        public MainForm()
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            inInitialization = true;

            InitializeComponent();

            // TODO figure out why the cross-thread access to dtPermaOOF is still happening
            //temporary fix since I cannot find where it is happening
            //with dtPermaOOF
            if (Debugger.IsAttached)
            {
                CheckForIllegalCrossThreadCalls = false;
            }

#if !DEBUG
            // Display release notes so user knows what's new
            if (ClickOnceTracker.IsFirstRun)
            {
                WhatsNew wn = new WhatsNew(10);
                wn.Show();
            }
#endif

            #region Minimize decision and setup
            //if we have all the inputs and "start minimized" is checked in the menu, then minimize
            //if we are missing some necessar input, then need to show the window regardless
            Logger.Info("StartMinimized:" + OOFData.Instance.StartMinimized.ToString());
            Logger.Info("HaveNecessaryData:" + OOFData.Instance.HaveNecessaryData.ToString());

            if (OOFData.Instance.StartMinimized && OOFData.Instance.HaveNecessaryData)
            {

                //if in DEBUG, then always show the form
#if !DEBUG
                //don't call Show() here b/c we run minimized
                this.WindowState = FormWindowState.Minimized;
#else
                allowshowdisplay = true;
                this.WindowState = FormWindowState.Normal;
                this.Show();
#endif
                //but do make the tray icon visible
                notifyIcon1.Visible = true;
            }
            else
            {
                allowshowdisplay = true;
                this.WindowState = FormWindowState.Normal;
                this.Show();
            }
            Logger.Info("Starting WindowState:" + this.WindowState.ToString());
            #endregion

            //need to wire up the day checkboxes first so that it takes effect
            //when we set the value
            //get a list of the checkbox controls so we can apply special event handling to the OffWork ones
            var listOfCheckBoxControls = GetControlsOfSpecificType(this, typeof(CheckBox));
            foreach (var checkBox in listOfCheckBoxControls)
            {
                if (checkBox.Name.Contains("OffWorkCB"))
                {
                    ((CheckBox)checkBox).CheckedChanged += OffWorkCB_CheckedChanged;

                    checkBox.AccessibleName = checkBox.Name.Replace("OffWorkCB", "").FirstCharToUpper() + "Off Work";
                }
            }

            //and now we can set the values
            if (OOFData.Instance.WorkingHours != "")
            {
                string[] workingHours = OOFData.Instance.WorkingHours.Split('|');
                Logger.InfoPotentialPII("workingHours", OOFData.Instance.WorkingHours);

                //Zero means you are off that day (not working) therefore the box is checked
                string[] dayHours = workingHours[0].Split('~');
                if (dayHours[2] == "0") { sundayOffWorkCB.Checked = true; } else { sundayOffWorkCB.Checked = false; }
                sundayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                sundayEndTimepicker.Value = DateTime.Parse(dayHours[1]);
                sundayEndTimepicker.ValueChanged += DateTimePicker_ValueChanged;
                sundayStartTimepicker.ValueChanged += DateTimePicker_ValueChanged;

                dayHours = workingHours[1].Split('~');
                if (dayHours[2] == "0") { mondayOffWorkCB.Checked = true; } else { mondayOffWorkCB.Checked = false; }
                mondayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                mondayEndTimepicker.Value = DateTime.Parse(dayHours[1]);
                mondayEndTimepicker.ValueChanged += DateTimePicker_ValueChanged;
                mondayStartTimepicker.ValueChanged += DateTimePicker_ValueChanged;

                dayHours = workingHours[2].Split('~');
                if (dayHours[2] == "0") { tuesdayOffWorkCB.Checked = true; } else { tuesdayOffWorkCB.Checked = false; }
                tuesdayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                tuesdayEndTimepicker.Value = DateTime.Parse(dayHours[1]);
                tuesdayEndTimepicker.ValueChanged += DateTimePicker_ValueChanged;
                tuesdayStartTimepicker.ValueChanged += DateTimePicker_ValueChanged;

                dayHours = workingHours[3].Split('~');
                if (dayHours[2] == "0") { wednesdayOffWorkCB.Checked = true; } else { wednesdayOffWorkCB.Checked = false; }
                wednesdayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                wednesdayEndTimepicker.Value = DateTime.Parse(dayHours[1]);
                wednesdayEndTimepicker.ValueChanged += DateTimePicker_ValueChanged;
                wednesdayStartTimepicker.ValueChanged += DateTimePicker_ValueChanged;

                dayHours = workingHours[4].Split('~');
                if (dayHours[2] == "0") { thursdayOffWorkCB.Checked = true; } else { thursdayOffWorkCB.Checked = false; }
                thursdayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                thursdayEndTimepicker.Value = DateTime.Parse(dayHours[1]);
                thursdayEndTimepicker.ValueChanged += DateTimePicker_ValueChanged;
                thursdayStartTimepicker.ValueChanged += DateTimePicker_ValueChanged;

                dayHours = workingHours[5].Split('~');
                if (dayHours[2] == "0") { fridayOffWorkCB.Checked = true; } else { fridayOffWorkCB.Checked = false; }
                fridayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                fridayEndTimepicker.Value = DateTime.Parse(dayHours[1]);
                fridayEndTimepicker.ValueChanged += DateTimePicker_ValueChanged;
                fridayStartTimepicker.ValueChanged += DateTimePicker_ValueChanged;

                dayHours = workingHours[6].Split('~');
                if (dayHours[2] == "0") { saturdayOffWorkCB.Checked = true; } else { saturdayOffWorkCB.Checked = false; }
                saturdayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                saturdayEndTimepicker.Value = DateTime.Parse(dayHours[1]);
                saturdayEndTimepicker.ValueChanged += DateTimePicker_ValueChanged;
                saturdayStartTimepicker.ValueChanged += DateTimePicker_ValueChanged;
            }
            else
            {
                Logger.Warning("Don't have WorkingHours");
            }

            //set the time format for the LastDateTimePickers
            int index = 0;
            foreach (LastDateTimePicker item in this.Controls.OfType<LastDateTimePicker>())
            {
                //need to map the format to the culture
                // Get the current culture info
                CultureInfo currentCulture = CultureInfo.CurrentCulture;

                //get the shortDateTimeString and then add padding as necessary
                string shortDateTimeString = currentCulture.DateTimeFormat.ShortTimePattern;
                shortDateTimeString = shortDateTimeString.PadLeft(9);

                //on the first time through, log the format for debugging purposes
                if (index == 0)
                {
                    Logger.Info("shortDateTimeString with spaces: " + shortDateTimeString);
                }

                item.Format = DateTimePickerFormat.Custom;
                item.CustomFormat = shortDateTimeString;

                index++;
            }

            //populate the dropdown with the possible ExternalAudienceScope values
            var enumValues = Enum.GetValues(typeof(Microsoft.Graph.ExternalAudienceScope)).Cast<Microsoft.Graph.ExternalAudienceScope>();
            foreach (var value in enumValues)
            {
                cboExternalAudienceScope.Items.Add(EnumHelper.GetEnumDescription(value));
            }

            cboExternalAudienceScope.SelectedIndex = (int)OOFData.Instance.ExternalAudienceScope;
            //need to decide if the External Message needs to be marked ReadOnly
            DecideonHTMLReadOnly();

            //pull all the runtime accessibility work into one place
            //wire up to respond to changes
            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);

            //detect when being shut down due to the system shutting down so we can gracefully close
            SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);

            //actually do the work
            DoAccessibilityUIWork();

            #region SetBuildInfo

            //if this is a click-once deployment, grab the version from there
            //otherwise, use the assembly version
            if (Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion") != null)
            {
                OOFData.version = lblBuild.Text = Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion");
            }
            else
            {
                OOFData.version = lblBuild.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
            }

            OOFSponder.Logger.Info("Assembly version: " + Assembly.GetEntryAssembly().GetName().Version.ToString());

            #endregion

            OOFSponderInsights.ConfigureApplicationInsights();

            OOFSponder.Logger.Info("OOFSponderStart");

            //Set icon in code
            this.Icon = Properties.Resources.OOFSponderIcon;

            #region Add to Startup

            AutoStartup.AddToStartup();

            #endregion

            #region Tray Menu Initialize
            OOFSponder.Logger.Info("Initializing tray menu");

            // Create a simple tray menu with only one item
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add(new ToolStripMenuItem("Exit", null, OnExit));

            // Add menu to tray icon and show it.
            notifyIcon1.ContextMenuStrip = trayMenu;
            notifyIcon1.Icon = Properties.Resources.OOFSponderIcon;

            OOFSponder.Logger.Info("Done initializing tray menu");
            #endregion

            //prep for async work
            System.Threading.Tasks.Task AuthTask = null;
            AuthTask = System.Threading.Tasks.Task.Run((Action)(() => { O365.MSALWork(O365.AADAction.SignIn); }));

#if DEBUGFLOW
            MessageBox.Show("Attach now", "OOFSponder", MessageBoxButtons.OK);
#endif

#if NOOOF
            //if a NOOOF build, then update the Save Settings button visibly
            button2.Text = "Save NoOOF";
#endif
            cboExternalAudienceScope.SelectedItem = OOFData.Instance.ExternalAudienceScope.ToString();
            if (OOFData.Instance.IsPermaOOFOn)
            {
                SetUIforSecondary();
            }
            else
            {
                SetUIforPrimary();
            }

            //now that we have set up the UI properly, can add the handlers for primary/secondary
            radPrimary.CheckedChanged += new System.EventHandler(radPrimary_CheckedChanged);

            //Need to update the UI as appropriate based on On-Call mode
            SetUIforOnCallMode();

            //we need the OOF messages and working hours
            if (!OOFData.Instance.HaveNecessaryData)
            {
                //we are missing data, so log the three we are checking
                if (!OOFData.Instance.IsPermaOOFOn)
                {
                    OOFSponder.Logger.InfoPotentialPII("PrimaryOOFExternalMessage", OOFData.Instance.PrimaryOOFExternalMessage);
                    OOFSponder.Logger.InfoPotentialPII("PrimaryOOFInternalMessage", OOFData.Instance.PrimaryOOFInternalMessage);
                }
                else
                {
                    OOFSponder.Logger.InfoPotentialPII("SecondaryOOFExternalMessage", OOFData.Instance.SecondaryOOFExternalMessage);
                    OOFSponder.Logger.InfoPotentialPII("SecondaryOOFInternalMessage", OOFData.Instance.SecondaryOOFInternalMessage);
                }

                OOFSponder.Logger.InfoPotentialPII("WorkingHours", OOFData.Instance.WorkingHours);
            }

            if (OOFData.Instance.HaveNecessaryData)
            {
                toolStripStatusLabel1.Text = "Ready";
                OOFSponder.Logger.Info("HaveNecessaryData");
            }
            else
            {
                toolStripStatusLabel1.Text = "Please setup OOFsponder";
                OOFSponder.Logger.Info("MissingData");
            }

            toolStripStatusLabel2.Text = "";
            Loopy();

            //set up handlers to persist OOF messages
            htmlEditorControl1.Validated += htmlEditorValidated;
            htmlEditorControl2.Validated += htmlEditorValidated;

            //ensure the UI is right for AudienceScope
            //do this really late in load due to dependencies making it tough
            //to know when the PrimaryMessage won't change any more
            DecideonHTMLReadOnly();

            #region StartMinimized Setup
            //make sure to set the state of the Start Minimized menu item appropriately
            tsmiStartMinimized.Checked = OOFData.Instance.StartMinimized;

            //now that the value is properly set, can add the handler
            tsmiStartMinimized.Click += tsmiStartMinimized_CheckStateChanged;
            #endregion

            //wait on async auth stuff if not null
            if (AuthTask != null)
            {
                AuthTask.Wait();
            }

            //fileToolStripMenuItem.DropDownOpening += fileToolStripMenuItem_DropDownOpening;

            //now that everything is loaded, trigger a check on current status
            System.Threading.Tasks.Task.Run(() => RunSetOofO365());

            //unset the base instantiation tracker
            inInitialization = false;
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay);
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            //right now, the ony user preference we respond to
            //is HighContrast and that is contained in the DoAccessiblityUIWork() method
            DoAccessibilityUIWork();
        }

        private void DoAccessibilityUIWork()
        {
            //set the AccessibleName for all the working day checkboxes
            var listOfCheckBoxControls = GetControlsOfSpecificType(this, typeof(CheckBox));
            foreach (var checkBox in listOfCheckBoxControls)
            {
                if (checkBox.Name.Contains("OffWorkCB"))
                {
                    checkBox.AccessibleName = "Check if you are off work on " + checkBox.Name.Replace("OffWorkCB", "").FirstCharToUpper();
                }
            }

            //set the AccessibleName for the OOF message controls
            //External
            htmlEditorControl1.AccessibleName = "External OOF Message";
            foreach (Control item in htmlEditorControl1.Controls)
            {
                if (item.GetType() == typeof(ToolStrip))
                {
                    item.AccessibleName = "External " + item.AccessibleName;
                }
            }


            //Internal
            htmlEditorControl2.AccessibleName = "Internal OOF Message";
            foreach (Control item in htmlEditorControl2.Controls)
            {
                if (item.GetType() == typeof(ToolStrip))
                {
                    item.AccessibleName = "Internal " + item.AccessibleName;
                }
            }

            //similar to the File menuitem, the DateTimePickers aren't properly rendering in 
            //High Contrast, so manually setting the ForeColor to ControlLight
            if (SystemInformation.HighContrast)
            {
                OOFSponder.Logger.Info("HighContrast mode = true, so setting DataTimePickers to ControlLight");
                foreach (DateTimePicker item in this.Controls.OfType<DateTimePicker>())
                {
                    item.ForeColor = SystemColors.ControlLight;
                }
            }
            else
            {
                OOFSponder.Logger.Info("HighContrast mode = false, so setting DataTimePickers to ControlText");
                foreach (DateTimePicker item in this.Controls.OfType<DateTimePicker>())
                {
                    item.ForeColor = SystemColors.WindowText;
                }
            }
        }

        //private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        //{
        //    OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());
        //    if (!O365.isLoggedIn)
        //    {
        //        signoutToolStripMenuItem.Tag = "LoggedOut";
        //        signoutToolStripMenuItem.Text = "Sign in";
        //    }
        //    else
        //    {
        //        signoutToolStripMenuItem.Tag = "LoggedIn";
        //        signoutToolStripMenuItem.Text = "Sign out";
        //    }
        //}

        void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());
            ////prep for async work
            //System.Threading.Tasks.Task AuthTask = null;

            //if (signoutToolStripMenuItem.Tag.ToString() == "LoggedIn")
            //{
            //    AuthTask = System.Threading.Tasks.Task.Run((Action)(() => { O365.MSALWork(O365.AADAction.SignOut); }));
            //}
            //else
            //{
            //    AuthTask = System.Threading.Tasks.Task.Run((Action)(() => { O365.MSALWork(O365.AADAction.SignIn); }));
            //}

            ////wait on async auth stuff if not null
            //if (AuthTask != null)
            //{
            //    AuthTask.Wait();
            //}
        }

        #region Set Oof Timed Loop
        void Loopy()
        {
            OOFSponder.Logger.Info("Setting up Loopy");

            //normal logic
            //Every 10 minutes for automation
            var timer = new System.Timers.Timer(600000);

#if FASTLOOP
            //Every 30 seconds for rapid testing
            timer = new System.Timers.Timer(30000);
#endif

#if MEDIUMLOOP
            //Every 5 min for human-based testing
            timer = new System.Timers.Timer(300000);
#endif
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        private async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            OOFSponder.Logger.Info("Loopy elapsed - saving settings and running RunSetOofO365");
            saveSettings();

            //no longer necessary - we are doing it inside the saveSettings call above
            //await System.Threading.Tasks.Task.Run(() => RunSetOofO365());
            //await checkOOFStatus();
        }
        #endregion

        #region Oof/EWS interaction

        #region Oof Set

        private async System.Threading.Tasks.Task<bool> RunSetOofO365()
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            bool result = false;
            if (OOFData.Instance.HaveNecessaryData)
            {
                OOFSponder.Logger.Info("Getting OOF times");
                DateTime[] oofTimes = getOofTime(OOFData.Instance.WorkingHours);
                OOFSponder.Logger.Info("Got OOF times");

                //persist settings just in case
                string oofMessageExternal = htmlEditorControl1.BodyHtml;
                string oofMessageInternal = htmlEditorControl2.BodyHtml;

                //if not logged in, give the user a chance to log in
                OOFSponder.Logger.Info("Checking to make sure the user is logged in before doing OOF work");
                if (!O365.isLoggedIn)
                {
                    OOFSponder.Logger.Error("Not logged in when trying to Save Settings. Giving them one more try.");

                    OOFSponder.Logger.Info("Trying to log in again inside " + OOFSponderInsights.CurrentMethod());
                    System.Threading.Tasks.Task AuthTask = null;
                    AuthTask = System.Threading.Tasks.Task.Run((Action)(() => { O365.MSALWork(O365.AADAction.SignIn); }));
                    AuthTask.Wait(10000);
                    OOFSponder.Logger.Info("Successfully logged in inside " + OOFSponderInsights.CurrentMethod());

                    //if still not logged in, bail
                    if (!O365.isLoggedIn)
                    {
                        OOFSponder.Logger.Error("STILL not logged in, so stopping from saving");
                        MessageBox.Show("Not logged in!. Please hit Save Settings again and log in with a valid user");
                        return false;
                    }
                }

                OOFSponder.Logger.Info("Getting ready to set either PermaOOF or NormalOOF");
                //if PermaOOF isn't turned on, use the standard logic based on the stored schedule
                if ((oofTimes[0] != oofTimes[1]) && !OOFData.Instance.IsPermaOOFOn)
                {
                    OOFSponder.Logger.Info("TrySetNormalOOF");
                    result = await System.Threading.Tasks.Task.Run(() => TrySetOOF365(oofMessageExternal, oofMessageInternal, oofTimes[0], oofTimes[1]));
                }
                else
                //since permaOOF is on, need to adjust the end date such that is permaOOFDate
                //if permaOOF>oofTimes[0] and permaOOF<oofTimes[1], then AddDays((permaOOFDate - oofTimes[1]).Days
                //due to the way the math works out, need to add extra day if permaOOF>oofTimes[1]
                {
                    int adjustmentDays = 0;
                    if (OOFData.Instance.PermaOOFDate > oofTimes[0] && OOFData.Instance.PermaOOFDate < oofTimes[1])
                    {
                        adjustmentDays = 1;
                    }

                    //in order to accomodate someone going OOF mid-schedule
                    //check if now is before the next scheduled "OFF" slot
                    //if it is, then adjust start time to NOW
                    if (oofTimes[0] > DateTime.Now)
                    {
                        oofTimes[0] = DateTime.Now;
                    }

                    OOFSponderInsights.Track("TrySetPermaOOF");

                    result = await System.Threading.Tasks.Task.Run(() => TrySetOOF365(oofMessageExternal, oofMessageInternal, oofTimes[0], oofTimes[1].AddDays((OOFData.Instance.PermaOOFDate - oofTimes[1]).Days + adjustmentDays)));
                }
            }

            return result;
        }

        public async System.Threading.Tasks.Task<bool> TrySetOOF365(string oofMessageExternal, string oofMessageInternal, DateTime StartTime, DateTime EndTime)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());
            //need to account for background threads
            //toolStripStatusLabel1.Text = DateTime.Now.ToString() + " - Sending to O365";
            UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - Sending to O365");

            //need to convert the times from local datetime to DateTimeTimeZone and UTC
            DateTimeTimeZone oofStart = new DateTimeTimeZone { DateTime = StartTime.ToUniversalTime().ToString("u").Replace("Z", ""), TimeZone = "UTC" };
            DateTimeTimeZone oofEnd = new DateTimeTimeZone { DateTime = EndTime.ToUniversalTime().ToString("u").Replace("Z", ""), TimeZone = "UTC" };

            //create local OOF object
            //TODO: why not use OOFData.Instance??
            AutomaticRepliesSetting localOOF = new AutomaticRepliesSetting();
            localOOF.ExternalReplyMessage = oofMessageExternal;
            localOOF.InternalReplyMessage = oofMessageInternal;
            localOOF.ScheduledStartDateTime = oofStart;
            localOOF.ScheduledEndDateTime = oofEnd;
            localOOF.Status = AutomaticRepliesStatus.Scheduled;
            localOOF.ExternalAudience = OOFData.Instance.ExternalAudienceScope;

            try
            {
                OOFSponder.Logger.Info("Getting OOF settings from O365");
                string getOOFraw = await O365.GetHttpContentWithToken(O365.AutomatedReplySettingsURL);

                if (getOOFraw == string.Empty)
                {
                    //need to account for background threads
                    //toolStripStatusLabel1.Text = DateTime.Now.ToString() + " - unable to set OOF";
                    UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - unable to set OOF");

                    return false;
                }

                AutomaticRepliesSetting remoteOOF = JsonConvert.DeserializeObject<AutomaticRepliesSetting>(getOOFraw);

                //generalized check for an invalid grant
                //if Status is null, we weren't able to get the OOF settings - don't care why to some extent
                if (remoteOOF.Status == null)
                {
                    OOFSponder.Logger.Error("Unable to get OOF settings: " + getOOFraw);
                    OOFSponder.Logger.Error("Hint - most common cause for the above is old OOFSponder auth flow with tenant without admin consent");

                    //need to account for background thread
                    //toolStripStatusLabel1.Text = DateTime.Now.ToString() + " - unable to set OOF";
                    UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - unable to set OOF");

                    return false;
                }
                else
                {
                    OOFSponder.Logger.Info("Successfully got OOF settings");
                }


                bool internalReplyMessageEqual = false;
                try
                {
                    internalReplyMessageEqual = remoteOOF.InternalReplyMessage.CleanReplyMessage() == localOOF.InternalReplyMessage.CleanReplyMessage();
                }
                catch (Exception)
                {

                    throw new Exception("Internal message comparison failure");
                }

                bool externalReplyMessageEqual = false;
                try
                {
                    externalReplyMessageEqual = remoteOOF.ExternalReplyMessage.CleanReplyMessage() == localOOF.ExternalReplyMessage.CleanReplyMessage();
                }
                catch (Exception)
                {
                    throw new Exception("External message comparison failure");
                }

                //get the culture info so we can properly parse

                //local and remote are both UTC, so just compare times
                //Not sure it can ever happen to have the DateTime empty, but wrap this in a TryCatch just in case
                bool scheduledStartDateTimeEqual = false;
                try
                {
                    //remote OOF comes back in UTC time zone
                    //02/21/2025 01:00:00
                    string format = "MM/dd/yyyy HH:mm:ss";
                    scheduledStartDateTimeEqual = DateTime.ParseExact(remoteOOF.ScheduledStartDateTime.DateTime,
                                                                            format, CultureInfo.InvariantCulture)
                                                    == DateTime.Parse(localOOF.ScheduledStartDateTime.DateTime);
                }
                catch (Exception ex)
                {
                    throw new Exception("Start time comparison failure");
                }

                bool scheduledEndDateTimeEqual = false;
                try
                {
                    string format = "MM/dd/yyyy HH:mm:ss";
                    scheduledEndDateTimeEqual = DateTime.ParseExact(remoteOOF.ScheduledEndDateTime.DateTime,
                                                                            format, CultureInfo.InvariantCulture)
                                                    == DateTime.Parse(localOOF.ScheduledEndDateTime.DateTime);
                }
                catch (Exception ex)
                {
                    throw new Exception("End time comparison failure");
                }

                //don't know if this value could ever be null, so set to false by default
                bool externalAudienceScopeEqual = false;
                try
                {
                    externalAudienceScopeEqual = remoteOOF.ExternalAudience == localOOF.ExternalAudience;
                }
                catch (Exception ex)
                {
                    throw new Exception("External message comparison failure");
                }


                if (!externalReplyMessageEqual
                        || !internalReplyMessageEqual
                        || !scheduledStartDateTimeEqual
                        || !scheduledEndDateTimeEqual
                        || !externalAudienceScopeEqual
                        )
                {
                    OOFSponder.Logger.Info("Local OOF doesn't match remote OOF");

#if NOOOF
                    Logger.Warning("Running NOOOF mode - not actually setting OOF");
                    return true;
#endif
                    System.Net.Http.HttpResponseMessage result = await O365.PatchHttpContentWithToken(O365.MailboxSettingsURL, localOOF);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - OOF message set - Start: " + StartTime + " - End: " + EndTime);

                        OOFSponder.Logger.Info("Successfully set OOF");
                        return true;
                    }
                    else
                    {
                        OOFSponder.Logger.Error("Unable to set OOF. HttpStatusCode: " + result.StatusCode.ToString());
                        UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - Unable to set OOF message");
                        return false;
                    }
                }
                else
                {
                    OOFSponder.Logger.Info("Remote OOF matches - no changes");
                    UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - No changes needed, OOF Message not changed - Start: " + StartTime + " - End: " + EndTime);
                    return true;
                }
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(100, "OOF Exception", "Unable to set OOF: " + ex.Message, ToolTipIcon.Error);
                UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - Unable to set OOF");
                OOFSponder.Logger.Error("Unable to set OOF: " + ex.Message, ex);

                return false;
            }
        }

        #endregion
        #endregion

        #region Utilities
        public void UpdateStatusLabel(ToolStripStatusLabel ourLabel, String status_text)
        {
            System.Windows.Forms.MethodInvoker mi = new System.Windows.Forms.MethodInvoker(() => ourLabel.Text = status_text);

            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private string ScheduleString()
        {
            string[] scheduleString = new string[7];

            string checkstring = sundayOffWorkCB.Checked ? "0" : "1";
            scheduleString[0] = sundayStartTimepicker.Value.ToString("HH:mm tt") + "~" + sundayEndTimepicker.Value.ToString("HH:mm tt") + "~" + checkstring;

            checkstring = mondayOffWorkCB.Checked ? "0" : "1";
            scheduleString[1] = mondayStartTimepicker.Value.ToString("HH:mm tt") + "~" + mondayEndTimepicker.Value.ToString("HH:mm tt") + "~" + checkstring;

            checkstring = tuesdayOffWorkCB.Checked ? "0" : "1";
            scheduleString[2] = tuesdayStartTimepicker.Value.ToString("HH:mm tt") + "~" + tuesdayEndTimepicker.Value.ToString("HH:mm tt") + "~" + checkstring;

            checkstring = wednesdayOffWorkCB.Checked ? "0" : "1";
            scheduleString[3] = wednesdayStartTimepicker.Value.ToString("HH:mm tt") + "~" + wednesdayEndTimepicker.Value.ToString("HH:mm tt") + "~" + checkstring;

            checkstring = thursdayOffWorkCB.Checked ? "0" : "1";
            scheduleString[4] = thursdayStartTimepicker.Value.ToString("HH:mm tt") + "~" + thursdayEndTimepicker.Value.ToString("HH:mm tt") + "~" + checkstring;

            checkstring = fridayOffWorkCB.Checked ? "0" : "1";
            scheduleString[5] = fridayStartTimepicker.Value.ToString("HH:mm tt") + "~" + fridayEndTimepicker.Value.ToString("HH:mm tt") + "~" + checkstring;

            checkstring = saturdayOffWorkCB.Checked ? "0" : "1";
            scheduleString[6] = saturdayStartTimepicker.Value.ToString("HH:mm tt") + "~" + saturdayEndTimepicker.Value.ToString("HH:mm tt") + "~" + checkstring;

            return string.Join("|", scheduleString);
        }

        private DateTime[] getOofTime(string workingHours)
        {
            DateTime[] OofTimes = new DateTime[2];
            DateTime StartTime, EndTime;

            //add new variant that can handle OnCallMode - don't convert old code to this at this time due to the risk
            if (!OOFData.Instance.IsOnCallModeOn && !OOFData.Instance.useNewOOFMath)
            {
                CalculateOOFTimes(OOFData.Instance.WorkingHours.Split('|'), out StartTime, out EndTime);
            }
            else
            {
                CalculateOOFTimes2(out StartTime, out EndTime, OOFData.Instance.IsOnCallModeOn);
            }

            OofTimes[0] = StartTime;
            OofTimes[1] = EndTime;

            OOFSponder.Logger.Info("Calculated OOF StartTime = " + StartTime.ToString());
            OOFSponder.Logger.Info("Calculated OOF EndTime = " + EndTime.ToString());

            return OofTimes;
        }

        //legacy method - STILL USED WHEN ONCALLMODE NOT ENABLED!!!!!!!
        void CalculateOOFTimes(string[] workingTimesArray, out DateTime StartTime, out DateTime EndTime)
        {
            //Hold now time (if our working time hasn't come yet but we are on the next day make now still be yesterday
            //Example: Your off at 5PM April 1st at 12:01 AM April 2nd we don't want to change our OOF to the one for April 2nd, we are still off from April 1st
            // To handle this if the time we get for the Beginning of our working time comes after the current time we fall back a day to use that days oof time.
            DateTime currentCheckDate = DateTime.Now;
            string[] currentWorkingTime = workingTimesArray[(int)currentCheckDate.DayOfWeek].Split('~');

            if (DateTime.Parse(currentCheckDate.ToString("D") + " " + currentWorkingTime[0]) > DateTime.Now)
            {
                currentCheckDate = DateTime.Now.AddDays(-1);
                currentWorkingTime = workingTimesArray[(int)currentCheckDate.DayOfWeek].Split('~');
            }

            StartTime = DateTime.Now;
            if (currentWorkingTime[2] == "1")
            {
                StartTime = DateTime.Parse(currentCheckDate.ToString("D") + " " + currentWorkingTime[1]);
            }
            else
            {
                int daysback = -1;
                //create a way to exit if someone has all 7 days marked as OOF
                while (daysback >= -30)
                {
                    DateTime backday = currentCheckDate.AddDays(daysback);
                    string[] oldWorkingTime = workingTimesArray[(int)backday.DayOfWeek].Split('~');
                    StartTime = DateTime.Parse(backday.ToString("D") + " " + oldWorkingTime[1]);
                    if (oldWorkingTime[2] == "1")
                    {
                        break;
                    }
                    else
                    {
                        daysback--;
                    }
                }
            }

            string[] futureWorkingTime = workingTimesArray[(int)currentCheckDate.AddDays(1).DayOfWeek].Split('~');
            EndTime = DateTime.Now;
            if (futureWorkingTime[2] == "1")
            {
                EndTime = DateTime.Parse(currentCheckDate.AddDays(1).ToString("D") + " " + futureWorkingTime[0]);
            }
            else
            {
                int daysforward = 1;
                //create a way to exit if someone has all 7 days marked as OOF
                while (daysforward <= 365)
                {
                    DateTime comingday = currentCheckDate.AddDays(1).AddDays(daysforward);
                    string[] oldWorkingTime = workingTimesArray[(int)comingday.DayOfWeek].Split('~');
                    EndTime = DateTime.Parse(comingday.ToString("D") + " " + oldWorkingTime[0]);
                    if (oldWorkingTime[2] == "1")
                    {
                        break;
                    }
                    else
                    {
                        daysforward++;
                    }
                }
            }
        }

        //add new variant that can handle OnCallMode - don't convert old code to this at this time due to the risk
        void CalculateOOFTimes2(out DateTime StartTime, out DateTime EndTime, bool enableOnCallMode)
        {
            OOFSponder.Logger.Info("Using CalculationOOFTimes2");

            StartTime = DateTime.Now;
            EndTime = DateTime.Now;


            DateTime currentCheckDate = DateTime.Now;
            OOFSponder.Logger.Info("currentCheckDate = " + currentCheckDate.ToString());

            OOFInstance currentWorkingTime = OOFData.Instance.currentOOFPeriod;
            OOFSponder.Logger.Info("currentWorkingTime.StartTime = " + currentWorkingTime.StartTime);
            OOFSponder.Logger.Info("currentWorkingtime.Endtime = " + currentWorkingTime.EndTime);

            DateTime previousDayPeriodEnd = OOFData.Instance.previousOOFPeriodEnd;
            OOFSponder.Logger.Info("previousDayPeriodEnd = " + previousDayPeriodEnd);

            DateTime nextDayPeriodStart = OOFData.Instance.nextOOFPeriodStart;
            OOFSponder.Logger.Info("nextDayPeriodState = " + nextDayPeriodStart);

            DateTime nextDayPeriodEnd = OOFData.Instance.nextOOFPeriodEnd;
            OOFSponder.Logger.Info("nextDayPeriodEnd =" + nextDayPeriodEnd);

            OOFSponder.Logger.Info("enableOnCallMode = " + enableOnCallMode);

            //between the end of the previous OOF period and the start of the next one
            if (currentCheckDate > previousDayPeriodEnd && currentCheckDate < currentWorkingTime.StartTime)
            {
                OOFSponder.Logger.Info("currentCheckDate greater than previousDayPeriodEnd and less than currentWorkingTime.StartTime");
                if (enableOnCallMode)
                {
                    StartTime = currentWorkingTime.StartTime;
                    EndTime = currentWorkingTime.EndTime;
                }
                else
                {
                    StartTime = previousDayPeriodEnd;
                    EndTime = currentWorkingTime.StartTime;
                }
            }

            //between the start of the current period and the end of the current period
            if (currentCheckDate > currentWorkingTime.StartTime && currentCheckDate < currentWorkingTime.EndTime)
            {
                OOFSponder.Logger.Info("currentCheckDate greater than currentWorkingTime.StartTime and less than currentWorkingTime.EndTime");
                if (enableOnCallMode)
                {
                    StartTime = currentWorkingTime.StartTime;
                    EndTime = currentWorkingTime.EndTime;
                }
                else
                {
                    StartTime = nextDayPeriodStart;
                    EndTime = nextDayPeriodEnd;
                }
            }

            if (currentCheckDate > currentWorkingTime.EndTime)
            {
                OOFSponder.Logger.Info("currentCheckDate greater than currentWorkingTime.EndTime");
                if (enableOnCallMode)
                {
                    StartTime = nextDayPeriodStart;
                    EndTime = nextDayPeriodEnd;
                }
                else
                {
                    StartTime = currentWorkingTime.EndTime;
                    EndTime = nextDayPeriodStart;
                }
            }
        }

        bool stopExit = false;
        private void saveSettings(bool onExit = false)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            if (primaryToolStripMenuItem.Checked)
            {
                OOFSponder.Logger.Info("Saving Primary OOF message");
                OOFData.Instance.PrimaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                OOFData.Instance.PrimaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
            }
            else
            //since customer is editing Secondary message, save text in Secondary
            {
                OOFSponder.Logger.Info("Saving Secondary OOF message");
                OOFData.Instance.SecondaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                OOFData.Instance.SecondaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
            }

            //persist if they want the UI minimized on start up
            OOFData.Instance.StartMinimized = tsmiStartMinimized.Checked;

            //persist the external message scope
            Enum.TryParse(cboExternalAudienceScope.SelectedItem.ToString().Replace(" ", ""), out ExternalAudienceScope result);
            OOFData.Instance.ExternalAudienceScope = result;

            OOFData.Instance.WorkingHours = ScheduleString();

            //check to make sure we have the minimum data
            if (!OOFData.Instance.HaveNecessaryData)
            {
                Logger.Warning("Missing necessary data, so not persisting settings!");

                //if we are just shutting down, then do everything silently
                //otherwise, show some dialog boxes to allow the user to correct
                //missing data
                if (!systemShuttingDown)
                {

                    //if exiting, give the option to OK (exit) or Cancel (go back)
                    //if just the result of a Save, then just show OK
                    MessageBoxButtons buttons;
                    if (onExit)
                    {
                        buttons = MessageBoxButtons.OKCancel;
                    }
                    else
                    {
                        buttons = MessageBoxButtons.OK;
                    }

                    DialogResult resultMsgBox = MessageBox.Show(Resources.MissingNecessaryData,
                        Resources.MissingNecessaryDataMessageBoxTitle,
                        buttons, MessageBoxIcon.Error);

                    //if Cancel -> return back to main form
                    if (resultMsgBox == DialogResult.Cancel)
                    {
                        //in case this came from an Exit call, break the Exit
                        stopExit = true;
                        return;
                    }

                    //if OK and not from Exit -> don't allow save
                    if (resultMsgBox == DialogResult.OK && !onExit)
                    {
                        return;
                    }

                    //if OK and from Exit -> allow to Exit to contine
                    //however, once we get into writing the actual settings
                    //the save won't happen - just silently
                }
            }


            OOFSponder.Logger.Info("Saving settings");
            OOFData.Instance.WriteProperties();

            toolStripStatusLabel1.Text = "Settings Saved";
            OOFSponder.Logger.Info("Settings saved");

            //go implement the settings if possible
            System.Threading.Tasks.Task.Run(() => RunSetOofO365());
        }

        #endregion

        #region Events

        private static bool systemShuttingDown = false;
        static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            // Perform necessary cleanup here
            systemShuttingDown = true;

            e.Cancel = false; // Set to true to cancel shutdown/logoff
        }

        private void OnExit(object sender, EventArgs e)
        {
            OOFSponder.Logger.Info("Exiting - triggered by system tray Exit");
            // Use this since we are a console app
            System.Environment.Exit(1);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            //move this here so it doesn't get called for every tiny resize
            //if we are moving to Minimized, then make everything hidden
            //plus show system tray stuff
            if (this.WindowState == FormWindowState.Minimized)
            {
                OOFSponder.Logger.Info("Main window just minimized, so show tooltip, etc.");
                notifyIcon1.Visible = true;

                //if this is coming from the initial data load, don't pop the ballon
                if (!inInitialization) notifyIcon1.ShowBalloonTip(100);

                this.ShowInTaskbar = false;
            }

            //if we are moving to Normal, then make everything visible
            //plus hide system tray stuff
            if (this.WindowState == FormWindowState.Normal)
            {
                OOFSponder.Logger.Info("Main window just to normal, so show in taskbar");
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
                this.Show();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //show the main window
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.allowshowdisplay = true;
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
                this.Show();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OOFSponder.Logger.Info("FormClosing triggered: " + e.CloseReason.ToString());

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                //this.Hide();

                OOFSponder.Logger.Info("FormClosing due to UserClosing - canceled Close and minimized instead");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveSettings();
        }

        /// <summary>
        /// Triggers a Show, then a Hide to address the weird HTLMEditor painting issue
        /// </summary>
        private void FixHtmlEditorControlSizingIssue()
        {
            //foreach (Control ctl in this.Controls)
            //{
            //    if (ctl.Name.Contains("html"))
            //    {
            //        foreach (Control ctl2 in ctl.Controls)
            //        {
            //            foreach (Control ctl3 in ctl2.Controls)
            //            {
            //                if (ctl3.Name.Contains("editor"))
            //                {
            //                    Debug.WriteLine("found editor control");
            //                    ctl3.Refresh();
            //                    ctl3.PerformLayout();
            //                }
            //            }
            //        }
            //    } 
            //}
            //this.Size = new Size(this.Size.Width +1,this.Size.Height+ 1);
            //this.Refresh();
            //this.PerformLayout();
            //this.Refresh();
            //this.Show();
            //because of problems with the scaling inheritance, have to set this in code
            //this.Size = new System.Drawing.Size(1500, 1400);
            this.Hide();
            this.Show();
        }


        #region WorkingDaysControls

        public static System.Collections.Generic.List<Control> GetControlsOfSpecificType(Control container, Type type)
        {
            var controls = new System.Collections.Generic.List<Control>();

            foreach (Control ctrl in container.Controls)
            {
                if (ctrl.GetType() == type)
                    controls.Add(ctrl);

                controls.AddRange(GetControlsOfSpecificType(ctrl, type));
            }

            return controls;
        }

        //generic handler for making sure that the various daily CheckBoxes
        //enable/disable the various daily DateTimePickers as appropriate
        private void OffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            var listofDataTimePickers = GetControlsOfSpecificType(this, typeof(LastDateTimePicker));
            foreach (var dateTimePicker in listofDataTimePickers)
            {
                CheckBox cb = ((CheckBox)sender);
                LastDateTimePicker dt = ((LastDateTimePicker)dateTimePicker);
                string cbName = cb.Name.Replace("OffWorkCB", "");
                string dtpName = dt.Name.Replace("StartTimepicker", "").Replace("EndTimepicker", "");
                if (cbName == dtpName)
                {
                    if (!OOFData.Instance.IsOnCallModeOn)
                    {
                        dt.Enabled = !cb.Checked;
                    }
                }
            }

        }

        //these are all deprecated, but leaving to not have to mess with the base event handlers
        private void sundayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            //if (sundayOffWorkCB.Checked)
            //{
            //    sundayStartTimepicker.Enabled = false;
            //    sundayEndTimepicker.Enabled = false;
            //}
            //else
            //{
            //    sundayStartTimepicker.Enabled = true;
            //    sundayEndTimepicker.Enabled = true;
            //}
        }

        private void mondayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            //if (mondayOffWorkCB.Checked)
            //{
            //    mondayStartTimepicker.Enabled = false;
            //    mondayEndTimepicker.Enabled = false;
            //}
            //else
            //{
            //    mondayStartTimepicker.Enabled = true;
            //    mondayEndTimepicker.Enabled = true;
            //}
        }

        private void tuesdayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            //if (tuesdayOffWorkCB.Checked)
            //{
            //    tuesdayStartTimepicker.Enabled = false;
            //    tuesdayEndTimepicker.Enabled = false;
            //}
            //else
            //{
            //    tuesdayStartTimepicker.Enabled = true;
            //    tuesdayEndTimepicker.Enabled = true;
            //}
        }

        private void wednesdayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            //if (wednesdayOffWorkCB.Checked)
            //{
            //    wednesdayStartTimepicker.Enabled = false;
            //    wednesdayEndTimepicker.Enabled = false;
            //}
            //else
            //{
            //    wednesdayStartTimepicker.Enabled = true;
            //    wednesdayEndTimepicker.Enabled = true;
            //}
        }

        private void thursdayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            //if (thursdayOffWorkCB.Checked)
            //{
            //    thursdayStartTimepicker.Enabled = false;
            //    thursdayEndTimepicker.Enabled = false;
            //}
            //else
            //{
            //    thursdayStartTimepicker.Enabled = true;
            //    thursdayEndTimepicker.Enabled = true;
            //}
        }

        private void fridayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            //if (fridayOffWorkCB.Checked)
            //{
            //    fridayStartTimepicker.Enabled = false;
            //    fridayEndTimepicker.Enabled = false;
            //}
            //else
            //{
            //    fridayStartTimepicker.Enabled = true;
            //    fridayEndTimepicker.Enabled = true;
            //}
        }

        private void saturdayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            //if (saturdayOffWorkCB.Checked)
            //{
            //    saturdayStartTimepicker.Enabled = false;
            //    saturdayEndTimepicker.Enabled = false;
            //}
            //else
            //{
            //    saturdayStartTimepicker.Enabled = true;
            //    saturdayEndTimepicker.Enabled = true;
            //}
        }
        #endregion

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            //do one last save in case we missed any changes
            saveSettings(true);

            //inside saveSettings, if they clicked cancel
            //we should stop the Exit
            if (stopExit)
            {
                return;
            }

            //do one last flush in case anything not persisted
            foreach (TextWriterTraceListener logger in Trace.Listeners)
            {
                logger.Flush();
            }

            System.Windows.Forms.Application.Exit();
        }

        #endregion

        private async void btnPermaOOF_Click(object sender, EventArgs e)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            int debugPermaOOF = 0;
#if DEBUG
            //if in DEBUG, set it a few more days out from the default to make
            //it easier to see visually it was set
            if (((Button)sender).Tag.ToString() == "Enable")
            {
                debugPermaOOF = 4;
            }
#endif

            //to be thread-safe, grab the value of dtPermaOOF here and store it
            DateTime dtPermaOOFValue = new DateTime();
            if (dtPermaOOF.InvokeRequired)
            {
                dtPermaOOF.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                {
                    dtPermaOOF.Value = dtPermaOOF.Value.AddDays(debugPermaOOF);
                    dtPermaOOFValue = dtPermaOOF.Value;
                }));
            }
            else
            {
                dtPermaOOF.Value = dtPermaOOF.Value.AddDays(debugPermaOOF);
                dtPermaOOFValue = dtPermaOOF.Value;
            }

            //bail if permaOOF not in the future
            if (DateTime.Now >= dtPermaOOFValue)
            {
                MessageBox.Show("You must pick a date in future", "OOFSponder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //only set up for permaOOF if we have OOF messages
            if (OOFData.Instance.SecondaryOOFExternalMessage == String.Empty | OOFData.Instance.SecondaryOOFInternalMessage == String.Empty)
            {
                MessageBox.Show("Unable to turn on extended OOF - Secondary OOF messages not set", "OOFSponder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool result = false;
            if (((Button)sender).Tag.ToString() == "Enable")
            {
                OOFData.Instance.PermaOOFDate = dtPermaOOFValue;
            }
            else
            {
                //flipping back to normal operations
                //don't need to enable button. That's handled later
                OOFData.Instance.PermaOOFDate = DateTime.Now;
                //if (dtPermaOOF.InvokeRequired)
                //{
                //    dtPermaOOF.Invoke(new System.Windows.Forms.MethodInvoker(delegate () { dtPermaOOF.Value = OOFData.Instance.PermaOOFDate; }));
                //}
                //else
                //{
                //    dtPermaOOF.Value = OOFData.Instance.PermaOOFDate;
                //}
            }

            //actually go OOF now
            result = await RunSetOofO365();

            if (result)
            {
                if (((Button)sender).Tag.ToString() == "Enable")
                {
                    SetUIforSecondary();
                    OOFSponderInsights.Track("Enabled PermaOOF");
                }
                else
                {
                    SetUIforPrimary();
                    OOFSponderInsights.Track("Disabled PermaOOF");
                }

                //if everything is set, save settings
                saveSettings();
            }
            else
            {
                MessageBox.Show("Failed to set OOF message. Please check your settings and try again");
                //if we fail to set OOF, disable PermaOOF to reset the UI
                OOFData.Instance.PermaOOFDate = DateTime.Now;

            }

        }

        private void secondaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            //now, set up the UI for PermaOOF
            SetUIforSecondary();
        }

        private void SetUIforSecondary()
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            primaryToolStripMenuItem.Checked = false;
            secondaryToolStripMenuItem.Checked = !primaryToolStripMenuItem.Checked;
            radSecondary.Checked = true;


            //set the tags on the Internal/External OOF message load items to Secondary
            //do this before setting the AccessibleName and AccessibleDescription so we can use the tag
            tsmiExternal.Tag = tsmiInternal.Tag = "Secondary";

            //Accessibility settings
            lblExternalMesage.Text = htmlEditorControl1.AccessibleDescription = htmlEditorControl1.AccessibleName = "Extended OOF External Message";
            lblInternalMessage.Text = htmlEditorControl2.AccessibleDescription = htmlEditorControl2.AccessibleName = "Extended OOF Internal Message";
            DoAccessibilityWorkforOpenSavedOOFMenuItems();

            htmlEditorControl1.BodyHtml = OOFData.Instance.SecondaryOOFExternalMessage;
            htmlEditorControl2.BodyHtml = OOFData.Instance.SecondaryOOFInternalMessage;

            //update the UI according to PermaOOF
            SetPermaOOFUIControlState();

            OOFSponderInsights.Track("Configured for secondary");
        }

        private void SetPermaOOFUIControlState()
        {
            //update the UI
            if (OOFData.Instance.IsPermaOOFOn)
            {
                btnPermaOOF.Text = "Disable Extended OOF";
                btnPermaOOF.Tag = "Disable";

                //need to be thread-safe
                if (dtPermaOOF.InvokeRequired)
                {
                    dtPermaOOF.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        UpdatedtPermaOOFSettings();
                    }));
                }
                else
                {
                    UpdatedtPermaOOFSettings();
                }

            }
            else
            {
                btnPermaOOF.Text = "Enable Extended OOF";
                btnPermaOOF.Tag = "Enable";

                //need to be thread-safe
                if (dtPermaOOF.InvokeRequired)
                {
                    dtPermaOOF.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        UpdatedtPermaOOFSettings();
                    }));
                }
                else
                {
                    UpdatedtPermaOOFSettings();
                }
            }
        }

        private void UpdatedtPermaOOFSettings()
        {
            if (OOFData.Instance.IsPermaOOFOn)
            {
                //set the date to PermaOOF date
                //and disable the control
                dtPermaOOF.Value = OOFData.Instance.PermaOOFDate;
                dtPermaOOF.Enabled = false;
            }
            else
            {
                //set it to an arbitrary time in the future
                //and enable the control
                dtPermaOOF.Value = DateTime.Now.AddDays(1);
                dtPermaOOF.Enabled = true;
            }


        }

        private void primaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            //now, set up the UI for primary
            SetUIforPrimary();
        }

        private void SetUIforPrimary()
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            //since we are in the process of flipping from secondary to primary
            //we know that the UI is currently in Secondary mode (or first run)
            //so HTML controls have the Secondary messages

            primaryToolStripMenuItem.Checked = true;
            secondaryToolStripMenuItem.Checked = !primaryToolStripMenuItem.Checked;
            radPrimary.Checked = true;

            //set the tags on the Internal/External OOF message load items to Primary
            //do this before setting the AccessibleName and AccessibleDescription so we can use the tag
            tsmiExternal.Tag = tsmiInternal.Tag = "Primary";

            //Accessibility settings
            lblExternalMesage.Text = htmlEditorControl1.AccessibleDescription = htmlEditorControl1.AccessibleName = "Primary OOF External Message";
            lblInternalMessage.Text = htmlEditorControl2.AccessibleDescription = htmlEditorControl2.AccessibleName = "Primary OOF Internal Message";
            DoAccessibilityWorkforOpenSavedOOFMenuItems();

            htmlEditorControl1.BodyHtml = OOFData.Instance.PrimaryOOFExternalMessage;
            htmlEditorControl2.BodyHtml = OOFData.Instance.PrimaryOOFInternalMessage;

            //update the UI according to PermaOOF
            SetPermaOOFUIControlState();

            ////need to be thread-safe
            //if (dtPermaOOF.InvokeRequired)
            //{
            //    dtPermaOOF.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
            //    {
            //        dtPermaOOF.Enabled = false;
            //        dtPermaOOF.Value = DateTime.Now;
            //    }));
            //}
            //else
            //{
            //    dtPermaOOF.Enabled = false;
            //    dtPermaOOF.Value = DateTime.Now;
            //}

            OOFSponderInsights.Track("Configured for primary");
        }

        private void DoAccessibilityWorkforOpenSavedOOFMenuItems()
        {
            tsmiExternal.AccessibleName = "Open saved " + tsmiExternal.Tag + " external OOF message";
            tsmiExternal.AccessibleDescription = "Opens a file dialog to allow picking a saved " + tsmiExternal.Tag +
                    " external OOF message";
            tsmiExternal.Text = tsmiExternal.Tag + " " + "External...";
            tsmiInternal.AccessibleName = "Open saved " + tsmiInternal.Tag + " internal OOF message";
            tsmiInternal.AccessibleDescription = "Opens a file dialog to allow picking a saved " + tsmiInternal.Tag +
                    " internal OOF message";
            tsmiInternal.Text = tsmiInternal.Tag + " " + "Internal...";
        }

        private void radPrimary_CheckedChanged(object sender, EventArgs e)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            if (radPrimary.Checked)
            {
                //Persist the opposite message
                OOFData.Instance.SecondaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                OOFData.Instance.SecondaryOOFInternalMessage = htmlEditorControl2.BodyHtml;

                SetUIforPrimary();
            }
            else
            {
                //Persist the opposite message
                OOFData.Instance.PrimaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                OOFData.Instance.PrimaryOOFInternalMessage = htmlEditorControl2.BodyHtml;

                SetUIforSecondary();
            }
        }

        //common call for both controls, regardless of primary or secondary
        private void htmlEditorValidated(object sender, EventArgs e)
        {
            if (radPrimary.Checked)
            {
                OOFSponderInsights.Track("PermaOOF off - persisting primary messages");
                OOFData.Instance.PrimaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                OOFData.Instance.PrimaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
            }
            else
            {
                OOFSponderInsights.Track("PermaOOF on - persisting secondary messages");
                OOFData.Instance.SecondaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                OOFData.Instance.SecondaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
            }
        }

        //enable/disable OnCallMode
        private void enableOnCallModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OOFData.Instance.IsOnCallModeOn = !OOFData.Instance.IsOnCallModeOn;
            SetUIforOnCallMode();
        }

        //do all the work to update the UI when enabling/disabling OnCallMode
        private void SetUIforOnCallMode()
        {
            OOFSponder.Logger.Info("Attempting to set OnCallModeUI for OnCallMode=" + OOFData.Instance.IsOnCallModeOn);
            enableOnCallModeToolStripMenuItem.Checked = OOFData.Instance.IsOnCallModeOn;

            //rename all the working day checkbox labels - keep the control names
            //a bit confusing, sure - but better than recreating a whole new set of controls
            if (OOFData.Instance.IsOnCallModeOn)
            {
                sundayOffWorkCB.Text = "On-Call";
                mondayOffWorkCB.Text = "On-Call";
                tuesdayOffWorkCB.Text = "On-Call";
                wednesdayOffWorkCB.Text = "On-Call";
                thursdayOffWorkCB.Text = "On-Call";
                fridayOffWorkCB.Text = "On-Call";
                saturdayOffWorkCB.Text = "On-Call";
            }
            else
            {
                sundayOffWorkCB.Text = "Off Work";
                mondayOffWorkCB.Text = "Off Work";
                tuesdayOffWorkCB.Text = "Off Work";
                wednesdayOffWorkCB.Text = "Off Work";
                thursdayOffWorkCB.Text = "Off Work";
                fridayOffWorkCB.Text = "Off Work";
                saturdayOffWorkCB.Text = "Off Work";
            }

            OOFSponder.Logger.Info("Successfully set OnCallModeUI for OnCallMode=" + OOFData.Instance.IsOnCallModeOn);
        }

        private void ShowLogs(object sender, EventArgs e)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);

            ////should be this name, but we'll get it dynamically later just to check
            //string loggerFileName = Logger.FileName;

            //getting ready to show logs, so force a flush
            foreach (TextWriterTraceListener logger in Trace.Listeners)
            {
                logger.Flush();

                //don't need this any more b/c we have this info in Logger.FileName
                //FieldInfo fieldInfo = typeof(TextWriterTraceListener).GetField("_fileName", BindingFlags.NonPublic | BindingFlags.Instance);
                //loggerFileName = (string)fieldInfo.GetValue(logger);
            }

            //default to opening the file, but if the user
            //picked the folder open in the UI, then switch to just the folder
            string FileorFoldertoOpen = Logger.FileName;
            if (((ToolStripMenuItem)sender).Tag.ToString() == "Folder")
            {
                FileorFoldertoOpen = System.IO.Path.GetDirectoryName(Logger.FileName);
            }

            var psi = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = FileorFoldertoOpen,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }

        private void bETAEnableNewOOFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bETAEnableNewOOFToolStripMenuItem.Checked = !bETAEnableNewOOFToolStripMenuItem.Checked;
            OOFData.Instance.useNewOOFMath = bETAEnableNewOOFToolStripMenuItem.Checked;
        }

        private void tsmiStartMinimized_CheckStateChanged(object sender, EventArgs e)
        {
            //Adding b/c I think this might be where the nulled messages are coming from
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            //force a save if the StartMinimize is adjusted
            saveSettings();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void tsmiSavedOOFMessage_Click(object sender, EventArgs e)
        {

            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            string SavedOOFMessageHTML = string.Empty;

            //only show files related to the target message
            ToolStripMenuItem tsmi = ((ToolStripMenuItem)sender);
            string _tsmiText = tsmi.Text.Replace(tsmi.Tag + " ", "").Replace("...", "");
            string filenameFilter = tsmi.Tag + _tsmiText;
            string filenameFilterDescription = tsmi.Tag + " " + _tsmiText;

            //only show HTML files
            openFileDialog.Filter = filenameFilterDescription + "|*" + filenameFilter + ".html|All HTML Files|*.html";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Title = "Select an existing OOF message file";
            openFileDialog.InitialDirectory = SettingsHelpers.PerUserDataFolder();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SavedOOFMessageHTML = System.IO.File.ReadAllText(openFileDialog.FileName);
                    switch (tsmi.Text.Replace(tsmi.Tag + " ", "").Replace("...", ""))
                    {
                        case "External":
                            htmlEditorControl1.BodyHtml = SavedOOFMessageHTML;
                            break;
                        case "Internal":
                            htmlEditorControl2.BodyHtml = SavedOOFMessageHTML;
                            break;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file. Original error: " + ex.Message);
                }
            }
        }

        private void tsmiShowOOFMessageFolder_Click(object sender, EventArgs e)
        {
            OOFSponder.Logger.Info(OOFSponderInsights.CurrentMethod());

            //get the AppData folder and open
            //this is for generic manipulation of the saved messages
            string FileorFoldertoOpen = System.IO.Path.GetDirectoryName(SettingsHelpers.PerUserDataFolder());

            var psi = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = FileorFoldertoOpen,
                UseShellExecute = true,
                Verb = "open"
            };
            System.Diagnostics.Process.Start(psi);
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            //for some reason, even though the control colors report as
            //Control and ControlText, it doesn't look right under High Contrast
            //so do some manual fix up to provide contrast. It looks a bit weird
            //visually, but at least the contrast works
            if (SystemInformation.HighContrast)
            {
                //as suggested by the Accessibility folks, but this isn't necessarily
                //going to work across all possible color combinations
                //fileToolStripMenuItem.BackColor = Color.White;
                //fileToolStripMenuItem.ForeColor = Color.Black;

                //use SystemColors to ensure it works no matter what
                //just make sure we have a darker background than the foreground
                fileToolStripMenuItem.ForeColor = SystemColors.ControlDark;
                fileToolStripMenuItem.BackColor = SystemColors.ControlLight;
            }


        }

        private void fileToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {

            //for some reason, even though the control colors report as
            //Control and ControlText, it doesn't look right under High Contrast
            //so had to do some manual fix up in fileToolStripMenuItem_MouseEnter.
            //Revert back to standard SystemColors here
            if (SystemInformation.HighContrast)
            {
                fileToolStripMenuItem.BackColor = SystemColors.Control;
                fileToolStripMenuItem.ForeColor = SystemColors.ControlText;
            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox a = new AboutBox();
            a.Show();
        }

        private void cboExternalAudienceScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if in the initial load, don't react
            if (inInitialization)
            {
                return;
            }

            OOFData.Instance.ExternalAudienceScope = (Microsoft.Graph.ExternalAudienceScope)cboExternalAudienceScope.SelectedIndex;

            DecideonHTMLReadOnly();

            //persist since settings change
            saveSettings();
        }

        private void DecideonHTMLReadOnly()
        {
            // Determine whether the External Message entry box should be editable based on Audience Scope
            bool isEditable = OOFData.Instance.ExternalAudienceScope != ExternalAudienceScope.None;

            // Enable or disable the External Message entry box
            htmlEditorControl1.Enabled = isEditable;

            // Update the HTML content to reflect the read-only state
            if (isEditable)
            {
                //if it editable, then just reload the saved text
                //reload primary/secondary based on which one is active
                //just copy from appropriate Internal
                htmlEditorControl1.BodyHtml = radPrimary.Checked ?
                    OOFData.Instance.PrimaryOOFInternalMessage : OOFData.Instance.SecondaryOOFInternalMessage;
            }
            else
            {
                foreach (Panel item in htmlEditorControl1.Controls.OfType<Panel>())
                {
                    //we know there is only one in the control, so can cheat
                    foreach (WebBrowser wb in item.Controls.OfType<WebBrowser>())
                    {
                        //really would prefer to use the transparent panel, but cannot 
                        //get it show on top. Oh well - maybe another day
                        //got this from Copilot!
                        wb.Document.InvokeScript("execScript", new object[] { "document.body.style.backgroundColor = 'lightgray';", "JavaScript" });
                    }

                }
                // htmlEditorControl1.BodyHtml = htmlEditorControl1.BodyHtml.Replace("scroll=auto", "scroll=auto style='" + OOFData.HTMLReadOnlyIndicator + "'");
            }
        }

        private bool navigatingDateTimePicker = false;
        private int incrementMinutes = 15;

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            LastDateTimePicker picker = sender as LastDateTimePicker;


            if (usedKeys)
            {
                //reset the usedKeys bool so that a spinner operation works
                usedKeys = false;
                return;
            }

            if (picker != null)
            {
                if (!navigatingDateTimePicker)
                {
                    /* First set the navigating flag to true so this method doesn't get called again while updating */
                    navigatingDateTimePicker = true;

                    /* using timespan because that's the only way I know how to round times well */
                    TimeSpan tempTS = picker.Value - picker.Value.Date;
                    TimeSpan roundedTimeSpan;

                    TimeSpan TDBug = picker.Value - picker.LastValue;
                    if (TDBug.TotalMinutes == 59)
                    {
                        // first: if we are going back and skipping an hour it needs an adjustment
                        roundedTimeSpan = TimeSpan.FromMinutes(incrementMinutes * Math.Floor((tempTS.TotalMinutes - 60) / incrementMinutes));
                        picker.Value = picker.Value.Date + roundedTimeSpan;
                    }
                    else if (picker.Value > picker.LastValue)
                    {
                        // round up to the nearest interval
                        roundedTimeSpan = TimeSpan.FromMinutes(incrementMinutes * Math.Ceiling(tempTS.TotalMinutes / incrementMinutes));
                        picker.Value = picker.Value.Date + roundedTimeSpan;
                    }
                    else
                    {
                        // round down to the nearest interval from prev
                        roundedTimeSpan = TimeSpan.FromMinutes(incrementMinutes * Math.Floor(tempTS.TotalMinutes / incrementMinutes));
                        picker.Value = picker.Value.Date + roundedTimeSpan;
                    }
                    navigatingDateTimePicker = false;
                }
            }
        }

        //use this to allow manual input of times that aren't aligned to the spinner increment
        private bool usedKeys = false;
        private void LastDateTimePicker_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            LastDateTimePicker picker = sender as LastDateTimePicker;
            usedKeys = true;
        }
    }



}
