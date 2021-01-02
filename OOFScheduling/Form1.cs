using System;
using System.Linq;
using System.Windows.Forms;
using System.Timers;
using mshtml;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Threading;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.Graph;

namespace OOFScheduling
{
    public partial class Form1 : Form
    {
        static string DummyHTML = @"<BODY scroll=auto></BODY>";

        private ContextMenu trayMenu;

        //Track if force close or just hitting X to minimize
        private bool minimize = true;

        public Form1()
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());

            InitializeComponent();

            #region SetBuildInfo
            foreach (Assembly a in Thread.GetDomain().GetAssemblies())
            {
                if (a.GetName().Name == "OOFScheduling")
                {
                    OOFData.version = lblBuild.Text = a.GetName().Version.ToString();
                    break;
                }
            }
            #endregion

            OOFSponderInsights.ConfigureApplicationInsights();

            OOFSponderInsights.Track("OOFSponderStart");

            //Set icon in code
            this.Icon = Properties.Resources.OOFSponderIcon;

            #region Add to Startup
            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(
                                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            //Path to launch shortcut
            string startPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs)
                               + @"\Microsoft\OOFSponder.appref-ms";

            rkApp.SetValue("OOFSponder", startPath);
            #endregion
            #region Tray Menu Initialize
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Add menu to tray icon and show it.
            notifyIcon1.ContextMenu = trayMenu;
            notifyIcon1.Icon = Properties.Resources.OOFSponderIcon;
            #endregion
            #region Read the list of teams to populate with templates
            //Read in the list of teams and build the dictionary list of team name
            //and template location

            //for each team, add an entry to the the Team Settings menu item (teamSettingsToolStripMenuItem)

            //If Use Team Settings is checked and a team is selected
            //then pull the remote files in as the text and then set the
            //controls to ReadOnly
            //Use a naming convention of "ExternalOOF.html" and "InternalOOF.html"?

            //Will also need to add some #Start/#End/#TimeZone logic
            //could even get fancy and have that be some sort of pop-up
            //so that each team could have its own
            //this is definitely future work :)

            #endregion


            //prep for async work
            System.Threading.Tasks.Task AuthTask = null;
            AuthTask = System.Threading.Tasks.Task.Run((Action)(() => { O365.MSALWork(O365.AADAction.SignIn); }));

#if DEBUGFLOW
            MessageBox.Show("Attach now", "OOFSponder", MessageBoxButtons.OK);
#endif

            if (OOFData.Instance.IsPermaOOFOn)
            {
                SetUIforSecondary();
            }
            else
            {
                SetUIforPrimary();
            }

            //Need to update the UI as appropriate based on On-Call mode
            SetUIforOnCallMode();

            if (OOFData.Instance.WorkingHours!= "")
            {
                string[] workingHours = OOFData.Instance.WorkingHours.Split('|');

                //Zero means you are off that day (not working) therefore the box is checked
                string[] dayHours = workingHours[0].Split('~');
                if (dayHours[2] == "0") {sundayOffWorkCB.Checked = true; } else {sundayOffWorkCB.Checked = false;}
                sundayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                sundayEndTimepicker.Value = DateTime.Parse(dayHours[1]);

                dayHours = workingHours[1].Split('~');
                if (dayHours[2] == "0") {mondayOffWorkCB.Checked = true; } else {mondayOffWorkCB.Checked = false;}
                mondayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                mondayEndTimepicker.Value = DateTime.Parse(dayHours[1]);

                dayHours = workingHours[2].Split('~');
                if (dayHours[2] == "0") {tuesdayOffWorkCB.Checked = true; } else {tuesdayOffWorkCB.Checked = false;}
                tuesdayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                tuesdayEndTimepicker.Value = DateTime.Parse(dayHours[1]);

                dayHours = workingHours[3].Split('~');
                if (dayHours[2] == "0") {wednesdayOffWorkCB.Checked = true; } else {wednesdayOffWorkCB.Checked = false;}
                wednesdayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                wednesdayEndTimepicker.Value = DateTime.Parse(dayHours[1]);

                dayHours = workingHours[4].Split('~');
                if (dayHours[2] == "0") {thursdayOffWorkCB.Checked = true; } else {thursdayOffWorkCB.Checked = false;}
                thursdayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                thursdayEndTimepicker.Value = DateTime.Parse(dayHours[1]);

                dayHours = workingHours[5].Split('~');
                if (dayHours[2] == "0") {fridayOffWorkCB.Checked = true; } else {fridayOffWorkCB.Checked = false;}
                fridayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                fridayEndTimepicker.Value = DateTime.Parse(dayHours[1]);

                dayHours = workingHours[6].Split('~');
                if (dayHours[2] == "0") { saturdayOffWorkCB.Checked = true; } else { saturdayOffWorkCB.Checked = false; }
                saturdayStartTimepicker.Value = DateTime.Parse(dayHours[0]);
                saturdayEndTimepicker.Value = DateTime.Parse(dayHours[1]);
            }

            bool haveNecessaryData = false;


            //we need the OOF messages and working hours
            if (OOFData.Instance.PrimaryOOFExternalMessage != "" && OOFData.Instance.PrimaryOOFInternalMessage != "" 
                && OOFData.Instance.WorkingHours != "")
            {
                haveNecessaryData = true;
            }

            if (haveNecessaryData)
            {
                toolStripStatusLabel1.Text = "Ready";
                OOFSponderInsights.TrackInfo("HaveNecessaryData");
            }
            else
            {
                toolStripStatusLabel1.Text = "Please setup OOFsponder";
                OOFSponderInsights.TrackInfo("MissingData");
            }

            toolStripStatusLabel2.Text = "";
            Loopy();

            //set up handlers to persist OOF messages
            htmlEditorControl1.Validated += htmlEditorValidated;
            htmlEditorControl2.Validated += htmlEditorValidated;

            //wait on async auth stuff if not null
            if (AuthTask != null)
            {
                AuthTask.Wait();
            }

            //trigger a check on current status
            System.Threading.Tasks.Task.Run(() => RunSetOofO365());

            radPrimary.CheckedChanged += new System.EventHandler(radPrimary_CheckedChanged);
            fileToolStripMenuItem.DropDownOpening += fileToolStripMenuItem_DropDownOpening;
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());
            if (!O365.isLoggedIn)
            {
                saveToolStripMenuItem.Tag = "LoggedOut";
                saveToolStripMenuItem.Text = "Sign in";
            }
            else
            {
                saveToolStripMenuItem.Tag = "LoggedIn";
                saveToolStripMenuItem.Text = "Sign out";
            }
        }

        void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());
            //prep for async work
            System.Threading.Tasks.Task AuthTask = null;
            
            if (saveToolStripMenuItem.Tag.ToString() == "LoggedIn")
            {
                AuthTask = System.Threading.Tasks.Task.Run((Action)(() => { O365.MSALWork(O365.AADAction.SignOut); }));
            }
            else
            {
                AuthTask = System.Threading.Tasks.Task.Run((Action)(() => { O365.MSALWork(O365.AADAction.SignIn); }));
            }

            //wait on async auth stuff if not null
            if (AuthTask != null)
            {
                AuthTask.Wait();
            }
        }

        #region Set Oof Timed Loop
        void Loopy()
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());
            //Every 10 minutes for automation
            var timer = new System.Timers.Timer(600000);
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        private async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());
            await System.Threading.Tasks.Task.Run(() => RunSetOofO365());
            //await checkOOFStatus();
        }
        #endregion

        #region Oof/EWS interaction

        #region Oof Set

        private async System.Threading.Tasks.Task<bool> RunSetOofO365()
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());
            bool haveNecessaryData = false;

            //if CredMan is turned on, then we don't need the email or password
            //but we still need the OOF messages and working hours
            //also, don't need to check SecondaryOOF messages for two reasons:
            //1) they won't always be set
            //2) the UI flow won't let you get here with permaOOF if they aren't set
            if (OOFData.Instance.PrimaryOOFExternalMessage != "" &&
                OOFData.Instance.PrimaryOOFInternalMessage != "" &&
                OOFData.Instance.WorkingHours != "" )
            {
                haveNecessaryData = true;
                OOFSponderInsights.Track("HaveNecessaryData");
            }

            bool result = false;
            if (haveNecessaryData)
            {
                DateTime[] oofTimes = getOofTime(OOFData.Instance.WorkingHours);

                ////if PermaOOF is turned on, need to adjust the end time
                //if (OOFData.Instance.PermaOOFDate < oofTimes[0])
                //{
                //    //set all the UI stuff back to primary 
                //    //to set up for normal OOF schedule
                //    SetUIforPrimary();

                //}

                //persist settings just in case
                string oofMessageExternal = htmlEditorControl1.BodyHtml;
                string oofMessageInternal = htmlEditorControl2.BodyHtml;
                //if (!OOFData.Instance.IsPermaOOFOn)
                //{
                //    OOFData.Instance.PrimaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                //    OOFData.Instance.PrimaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
                //}
                //else
                //{
                //    OOFData.Instance.SecondaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                //    OOFData.Instance.SecondaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
                //}

                //if PermaOOF isn't turned on, use the standard logic based on the stored schedule
                if ((oofTimes[0] != oofTimes[1]) && !OOFData.Instance.IsPermaOOFOn)
                {
                    OOFSponderInsights.Track("TrySetNormalOOF");
#if !NOOOF
                    result = await System.Threading.Tasks.Task.Run(() => TrySetOOF365(oofMessageExternal, oofMessageInternal, oofTimes[0], oofTimes[1]));
#else
                    result = true;
#endif
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
#if !NOOOF
                    result = await System.Threading.Tasks.Task.Run(() => TrySetOOF365(oofMessageExternal, oofMessageInternal, oofTimes[0], oofTimes[1].AddDays((OOFData.Instance.PermaOOFDate - oofTimes[1]).Days + adjustmentDays)));
#else
                    result = true;
#endif
                }
            }

            return result;
        }

        //public async System.Threading.Tasks.Task<bool> TrySetOOF365(string oofMessageExternal, string oofMessageInternal, DateTime StartTime, DateTime EndTime)
        //{
        //    OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());
        //    toolStripStatusLabel1.Text = DateTime.Now.ToString() + " - Sending to O365";

        //    //need to convert the times from local datetime to DateTimeTimeZone and UTC
        //    DateTimeTimeZone oofStart = new DateTimeTimeZone { DateTime = StartTime.ToUniversalTime().ToString("u").Replace("Z", ""), TimeZone = "UTC" };
        //    DateTimeTimeZone oofEnd = new DateTimeTimeZone { DateTime = EndTime.ToUniversalTime().ToString("u").Replace("Z", ""), TimeZone = "UTC" };

        //    //create local OOF object
        //    AutomaticRepliesSetting localOOF = new AutomaticRepliesSetting();
        //    localOOF.ExternalReplyMessage = oofMessageExternal;
        //    localOOF.InternalReplyMessage = oofMessageInternal;
        //    localOOF.ScheduledStartDateTime = oofStart;
        //    localOOF.ScheduledEndDateTime = oofEnd;
        //    localOOF.Status = AutomaticRepliesStatus.Scheduled;

        //    try
        //    {
        //        OOFSponderInsights.Track("Getting OOF settings from O365");
        //        string getOOFraw = await O365.GetHttpContentWithToken(O365.AutomatedReplySettingsURL);
        //        AutomaticRepliesSetting remoteOOF = JsonConvert.DeserializeObject<AutomaticRepliesSetting>(getOOFraw);
        //        OOFSponderInsights.Track("Successfully got OOF settings");

        //        bool externalReplyMessageEqual = remoteOOF.ExternalReplyMessage.CleanReplyMessage() == localOOF.ExternalReplyMessage.CleanReplyMessage();
        //        bool internalReplyMessageEqual = remoteOOF.InternalReplyMessage.CleanReplyMessage() == localOOF.InternalReplyMessage.CleanReplyMessage();

        //        //local and remote are both UTC, so just compare times
        //        //Not sure it can ever happen to have the DateTime empty, but wrap this in a TryCatch just in case
        //        //set both to false - that way, we don't care if either one blows up 
        //        //because if one is false, the overall evaluation is false anyway
        //        bool scheduledStartDateTimeEqual = false;
        //        bool scheduledEndDateTimeEqual = false;
        //        try
        //        {
        //            scheduledStartDateTimeEqual = DateTime.Parse(remoteOOF.ScheduledStartDateTime.DateTime) == DateTime.Parse(localOOF.ScheduledStartDateTime.DateTime);
        //            scheduledEndDateTimeEqual = DateTime.Parse(remoteOOF.ScheduledEndDateTime.DateTime) == DateTime.Parse(localOOF.ScheduledEndDateTime.DateTime);
        //        }
        //        catch (Exception)
        //        {
        //            //do nothing because we will just take the initialized false values;
        //        }

        //        if ( !externalReplyMessageEqual
        //                || !internalReplyMessageEqual
        //                || !scheduledStartDateTimeEqual
        //                || !scheduledEndDateTimeEqual
        //                )
        //        {
        //            OOFSponderInsights.Track("Local OOF doesn't match remote OOF");
        //            System.Net.Http.HttpResponseMessage result = await O365.PatchHttpContentWithToken(O365.MailboxSettingsURL, localOOF);

        //            if (result.StatusCode == System.Net.HttpStatusCode.OK)
        //            {
        //                UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - OOF message set - Start: " + StartTime + " - End: " + EndTime);

        //                //report back to AppInsights
        //                OOFSponderInsights.Track("Successfully set OOF");
        //                return true;
        //            }
        //            else
        //            {
        //                OOFSponderInsights.Track("Unable to set OOF");
        //                UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - Unable to set OOF message");
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            OOFSponderInsights.Track("Remote OOF matches - no changes");
        //            UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - No changes needed, OOF Message not changed - Start: " + StartTime + " - End: " + EndTime);
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        notifyIcon1.ShowBalloonTip(100, "OOF Exception", "Unable to set OOF: " + ex.Message, ToolTipIcon.Error);
        //        UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - Unable to set OOF");
        //        OOFSponderInsights.TrackException("Unable to set OOF: " + ex.Message, ex);

        //        return false;
        //    }
        //}

        #endregion
        #endregion

        #region Utilities
        public void UpdateStatusLabel(ToolStripStatusLabel ourLabel, String status_text)
        {
            MethodInvoker mi = new MethodInvoker(() => ourLabel.Text = status_text);

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
            if (!OOFData.Instance.IsOnCallModeOn)
            {
                CalculateOOFTimes(OOFData.Instance.WorkingHours.Split('|'), out StartTime, out EndTime);
            }
            else
            {
                CalculateOOFTimes2(out StartTime, out EndTime, OOFData.Instance.IsOnCallModeOn);
            }

            OofTimes[0] = StartTime;
            OofTimes[1] = EndTime;

            OOFSponderInsights.TrackInfo("Calculated OOF StartTime = " + StartTime.ToString());
            OOFSponderInsights.TrackInfo("Calculated OOF EndTime = " + EndTime.ToString());

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

            StartTime = DateTime.Now;
            EndTime = DateTime.Now;

            DateTime currentCheckDate = DateTime.Now;
            OOFInstance currentWorkingTime = OOFData.Instance.OOFCollection[(int)currentCheckDate.DayOfWeek];
            OOFInstance LastDayPeriod = OOFData.Instance.OOFCollection[(int)currentCheckDate.AddDays(-1).DayOfWeek];
            OOFInstance NextDayPeriod = OOFData.Instance.OOFCollection[(int)currentCheckDate.AddDays(1).DayOfWeek];

            //between the end of the previous OOF period and the start of the next one
            if (currentCheckDate > LastDayPeriod.StartTime && currentCheckDate < NextDayPeriod.StartTime)
            {
                if (enableOnCallMode)
                {
                    StartTime = NextDayPeriod.StartTime;
                    EndTime = NextDayPeriod.EndTime;
                }
                else
                {
                    StartTime = LastDayPeriod.EndTime;
                    EndTime = NextDayPeriod.StartTime;
                }
            }

            //between the start of the current period and the end of the current period
            if (currentCheckDate > currentWorkingTime.StartTime && currentCheckDate < currentWorkingTime.EndTime)
            {
                if (enableOnCallMode)
                {
                    StartTime = currentWorkingTime.StartTime;
                    EndTime = currentWorkingTime.EndTime;
                }
                else
                {
                    StartTime = NextDayPeriod.StartTime;
                    EndTime = NextDayPeriod.EndTime;
                }
            }

            if (currentCheckDate > currentWorkingTime.EndTime)
            {
                if (enableOnCallMode)
                {
                    StartTime = NextDayPeriod.StartTime;
                    EndTime = NextDayPeriod.EndTime;
                }
                else
                {
                    StartTime = currentWorkingTime.EndTime;
                    EndTime = NextDayPeriod.StartTime;
                }
            }



            //Hold now time. if we are doing traditional OOF and if our working time hasn't come yet but we are on the next day make now still be yesterday
            //Example: Your off at 5PM April 1st at 12:01 AM April 2nd we don't want to change our OOF to the one for April 2nd, we are still off from April 1st
            // To handle this if the time we get for the Beginning of our working time comes after the current time we fall back a day to use that days oof time.

            if (DateTime.Parse(currentCheckDate.ToString("D") + " " + currentWorkingTime.StartTime.ToShortTimeString()) > DateTime.Now)
            {
                currentCheckDate = DateTime.Now.AddDays(-1);
                currentWorkingTime = OOFData.Instance.OOFCollection[(int)currentCheckDate.DayOfWeek];
            }

            //figure out the StartTime for the OOF period
            if (currentWorkingTime.IsOOF)
            {
                if (!enableOnCallMode)
                {
                    //for standard OOF mode, the endTime is the start of the OOF period
                    StartTime = DateTime.Parse(currentCheckDate.ToString("D") + " " + currentWorkingTime.EndTime.ToShortTimeString());
                }
                else
                {
                    //if in OnCallMode, then we treat the startTime as the start of the OOF period
                    StartTime = DateTime.Parse(currentCheckDate.ToString("D") + " " +currentWorkingTime.StartTime.ToShortTimeString());
                }
            }
            else
            {
                int daysback = -1;
                //create a way to exit if someone has all 7 days marked as OOF
                while (daysback >= -30)
                {
                    DateTime backday = currentCheckDate.AddDays(daysback);
                    OOFInstance oldWorkingTime = OOFData.Instance.OOFCollection[(int)currentCheckDate.DayOfWeek];
                    StartTime = DateTime.Parse(backday.ToString("D") + " " + oldWorkingTime.EndTime);
                    if (oldWorkingTime.IsOOF)
                    {
                        break;
                    }
                    else
                    {
                        daysback--;
                    }
                }
            }

            //figure out the end time for the OOF period
            if (DateTime.Parse(currentCheckDate.ToString("D") + " " + currentWorkingTime.EndTime.ToShortTimeString()) > DateTime.Now)
            {
                currentCheckDate = DateTime.Now.AddDays(-1);
                currentWorkingTime = OOFData.Instance.OOFCollection[(int)currentCheckDate.DayOfWeek];
            }
            OOFInstance futureWorkingTime = OOFData.Instance.OOFCollection[(int)currentCheckDate.AddDays(1).DayOfWeek];
            if (futureWorkingTime.IsOOF)
            {

                //EndTime = DateTime.Parse(currentCheckDate.AddDays(1).ToString("D") + " " + futureWorkingTime.startTime);
                if (!enableOnCallMode)
                {
                    //for standard OOF mode, the endTime is the start of the OOF period
                    EndTime = DateTime.Parse(currentCheckDate.ToString("D") + " " + futureWorkingTime.StartTime.ToShortTimeString());
                }
                else
                {
                    //if in OnCallMode, then we treat the startTime as the start of the OOF period
                    EndTime = DateTime.Parse(currentCheckDate.ToString("D") + " " + futureWorkingTime.EndTime.ToShortTimeString());
                }
            }
            else
            {
                int daysforward = 1;
                //create a way to exit if someone has all 7 days marked as OOF
                while (daysforward <= 365)
                {
                    DateTime comingday = currentCheckDate.AddDays(1).AddDays(daysforward);
                    OOFInstance oldWorkingTime = OOFData.Instance.OOFCollection[(int)comingday.DayOfWeek];
                    EndTime = DateTime.Parse(comingday.ToString("D") + " " + oldWorkingTime.StartTime);
                    if (oldWorkingTime.IsOOF)
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

        private void saveSettings()
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());

            if (primaryToolStripMenuItem.Checked)
            {
                OOFData.Instance.PrimaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                OOFData.Instance.PrimaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
            }
            else
            //since customer is editing Secondary message, save text in Secondary
            {
                OOFData.Instance.SecondaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                OOFData.Instance.SecondaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
            }

            OOFData.Instance.WorkingHours = ScheduleString();

            OOFData.Instance.WriteProperties();

            toolStripStatusLabel1.Text = "Settings Saved";
            OOFSponderInsights.TrackInfo("Settings saved");

            //go implement the settings if possible
            System.Threading.Tasks.Task.Run(() => RunSetOofO365());
        }

        #endregion

        #region Events
        private void OnExit(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(100);
                this.ShowInTaskbar = false;
                this.Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            this.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (minimize && e.CloseReason != CloseReason.WindowsShutDown)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveSettings();
        }


        #region WorkingDaysControls
        private void sundayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            if (sundayOffWorkCB.Checked)
            {
                sundayStartTimepicker.Enabled = false;
                sundayEndTimepicker.Enabled = false;
            }
            else
            {
                sundayStartTimepicker.Enabled = true;
                sundayEndTimepicker.Enabled = true;
            }
        }

        private void mondayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            if (mondayOffWorkCB.Checked)
            {
                mondayStartTimepicker.Enabled = false;
                mondayEndTimepicker.Enabled = false;
            }
            else
            {
                mondayStartTimepicker.Enabled = true;
                mondayEndTimepicker.Enabled = true;
            }
        }

        private void tuesdayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            if (tuesdayOffWorkCB.Checked)
            {
                tuesdayStartTimepicker.Enabled = false;
                tuesdayEndTimepicker.Enabled = false;
            }
            else
            {
                tuesdayStartTimepicker.Enabled = true;
                tuesdayEndTimepicker.Enabled = true;
            }
        }

        private void wednesdayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            if (wednesdayOffWorkCB.Checked)
            {
                wednesdayStartTimepicker.Enabled = false;
                wednesdayEndTimepicker.Enabled = false;
            }
            else
            {
                wednesdayStartTimepicker.Enabled = true;
                wednesdayEndTimepicker.Enabled = true;
            }
        }

        private void thursdayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            if (thursdayOffWorkCB.Checked)
            {
                thursdayStartTimepicker.Enabled = false;
                thursdayEndTimepicker.Enabled = false;
            }
            else
            {
                thursdayStartTimepicker.Enabled = true;
                thursdayEndTimepicker.Enabled = true;
            }
        }

        private void fridayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            if (fridayOffWorkCB.Checked)
            {
                fridayStartTimepicker.Enabled = false;
                fridayEndTimepicker.Enabled = false;
            }
            else
            {
                fridayStartTimepicker.Enabled = true;
                fridayEndTimepicker.Enabled = true;
            }
        }

        private void saturdayOffWorkCB_CheckedChanged(object sender, EventArgs e)
        {
            if (saturdayOffWorkCB.Checked)
            {
                saturdayStartTimepicker.Enabled = false;
                saturdayEndTimepicker.Enabled = false;
            }
            else
            {
                saturdayStartTimepicker.Enabled = true;
                saturdayEndTimepicker.Enabled = true;
            }
        }
#endregion

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());
            saveSettings();
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());
            minimize = false;
            System.Windows.Forms.Application.Exit();
        }

        #endregion

        private async void btnPermaOOF_Click(object sender, EventArgs e)
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());

            //bail if permaOOF not in the future
            if (DateTime.Now >= dtPermaOOF.Value)
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
            if (((Button)sender).Tag.ToString()=="Enable")
            {
                OOFData.Instance.PermaOOFDate = dtPermaOOF.Value;
            }
            else
            {
                OOFData.Instance.PermaOOFDate = DateTime.Now;
            }

            //actually go OOF now
            result = await RunSetOofO365();

            if (result)
            {
                if (((Button)sender).Tag.ToString() == "Enable")
                {
                    OOFSponderInsights.Track("Enabled PermaOOF");
                }
                else
                {
                    OOFSponderInsights.Track("Disabled PermaOOF");
                }
            }
            else
            {
                //if we fail to set OOF, disable PermaOOF to reset the UI
                OOFData.Instance.PermaOOFDate = DateTime.Now;

            }

            SetUIforSecondary();
        }

        private void secondaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());

            //now, set up the UI for PermaOOF
            SetUIforSecondary();
        }

        private void SetUIforSecondary()
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());

            primaryToolStripMenuItem.Checked = false;
            secondaryToolStripMenuItem.Checked = !primaryToolStripMenuItem.Checked;
            lblExternalMesage.Text = "Extended OOF External Message";
            lblInternalMessage.Text = "Extended OOF Internal Message";

            htmlEditorControl1.BodyHtml = OOFData.Instance.SecondaryOOFExternalMessage;
            htmlEditorControl2.BodyHtml = OOFData.Instance.SecondaryOOFInternalMessage;

            //update the UI
            if (OOFData.Instance.IsPermaOOFOn)
            {
                btnPermaOOF.Text = "Disable Extended OOF";
                btnPermaOOF.Tag = "Disable";
                dtPermaOOF.Enabled = false;
            }
            else
            {
                btnPermaOOF.Text = "Enable Extended OOF";
                btnPermaOOF.Tag = "Enable";
                dtPermaOOF.Value = DateTime.Now.AddDays(1);
                dtPermaOOF.Enabled = true;
            }

            //lastly, enable the permaOOF controls to help with some UI flow issues
            btnPermaOOF.Enabled = true;

            OOFSponderInsights.Track("Configured for secondary");
        }

        private void primaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());

            //now, set up the UI for primary
            SetUIforPrimary();
        }

        private void SetUIforPrimary()
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());

            //since we are in the process of flipping from secondary to primary
            //we know that the UI is currently in Secondary mode (or first run)
            //so HTML controls have the Secondary messages

            //disable PermaOOF by setting date to time in the past
            OOFData.Instance.PermaOOFDate = DateTime.Now.AddMinutes(-1);

            primaryToolStripMenuItem.Checked = true;
            secondaryToolStripMenuItem.Checked = !primaryToolStripMenuItem.Checked;
            lblExternalMesage.Text = "Primary External Message";
            lblInternalMessage.Text = "Primary Internal Message";

            htmlEditorControl1.BodyHtml = OOFData.Instance.PrimaryOOFExternalMessage;
            htmlEditorControl2.BodyHtml = OOFData.Instance.PrimaryOOFInternalMessage;

            //lastly, disable the permaOOF controls to help with some UI flow issues
            btnPermaOOF.Enabled = false;
            dtPermaOOF.Enabled = false;
            dtPermaOOF.Value = DateTime.Now;

            OOFSponderInsights.Track("Configured for primary");
        }

        private void radPrimary_CheckedChanged(object sender, EventArgs e)
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());

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
    }

    internal static class Extensions
    {
        internal static string CleanReplyMessage(this string input)
        {
            return Regex.Replace(input, @"\r\n|\n\r|\n|\r", "\r\n");
        }
    }
}
