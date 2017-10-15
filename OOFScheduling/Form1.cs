using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Exchange.WebServices.Data;
using System.Timers;
using mshtml;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Threading;
using System.Reflection;

namespace OOFScheduling
{
    public partial class Form1 : Form
    {
        static string DummyHTML = @"<BODY scroll=auto></BODY>";

        //AppInsights
        Microsoft.ApplicationInsights.TelemetryClient AIClient = new Microsoft.ApplicationInsights.TelemetryClient();

        private ContextMenu trayMenu;

        //Track if force close or just hitting X to minimize
        private bool minimize = true;

        //Track if we have turned on the manual oof message
        private bool manualoof = false;

        //Track if we have a valid exchange connection
        private bool foundexchange = false;

        public Form1()
        {
            
            InitializeComponent();

            //workaround for strange form sizing on Evan's laptop
#if DEBUG
            this.Height = 1500;
#endif

            #region SetBuildInfo
            foreach (Assembly a in Thread.GetDomain().GetAssemblies())
            {
                if (a.GetName().Name == "OOFScheduling")
                {
                    lblBuild.Text = a.GetName().Version.ToString();
                    break;
                }
            }
            #endregion

            #region AddMenuItems
            AddMenuItems();
            #endregion
            OOFSponderInsights.ConfigureApplicationInsights();

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
            trayMenu.MenuItems.Add("Run Manually", RunManualMenu);
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Add menu to tray icon and show it.
            notifyIcon1.ContextMenu = trayMenu;
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
            #region Fill in property data if set
            //we could always get from CredMan, but setting it anyway to avoid
            //breaking other code dependencies that are checking to see if this is set
            if (Properties.Settings.Default.EmailAddress != "default")
            {
                emailAddressTB.Text = Properties.Settings.Default.EmailAddress;
            }

            //prep for async work
            System.Threading.Tasks.Task AuthTask = null;
            AuthTask = System.Threading.Tasks.Task.Run((Action)(() => { O365.MSALWork(O365.AADAction.ForceSignIn); }));

            //Can this get dropped by pulling in the OOF from the server during the CheckOOFStatus call?
            if (OOFData.Instance.IsPermaOOFOn)
            {
                SetUIforPermaOOF();
            }
            else
            {
                SetUIforPrimary();
            }
            htmlEditorControl1.BodyHtml = OOFData.Instance.ExternalOOFMessage;
            htmlEditorControl2.BodyHtml = OOFData.Instance.InternalOOFMessage;

            if (OOFData.Instance.WorkingHours!= "")
            {
                string[] workingHours = Properties.Settings.Default.workingHours.Split('|');

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

            //if PermaOOF isn't on, then set up UI to show Primary
            if (DateTime.Now > OOFData.Instance.PermaOOFDate)
            {
                SetUIforPrimary();
            }
            else
            {
                //else set up for Secondary
                SetUIforPermaOOF();
            }

            //set the PermaOOF date to something in the future
            OOFData.Instance.PermaOOFDate = DateTime.Now.AddDays(4);
            dtPermaOOF.Value = OOFData.Instance.PermaOOFDate;




            bool haveNecessaryData = false;


            //if CredMan is turned on, then we don't need the email or password
            //but we still need the OOF messages and working hours
            if (OOFData.Instance.ExternalOOFMessage != "" && OOFData.Instance.InternalOOFMessage != "" 
                && OOFData.Instance.WorkingHours != "")
            {
                haveNecessaryData = true;
            }

            if (haveNecessaryData)
            {
                toolStripStatusLabel1.Text = "Ready";
            }
            else
            {
                toolStripStatusLabel1.Text = "Please setup OOFsponder";
            }

            toolStripStatusLabel2.Text = "";
            #endregion
            Loopy();
            RunStatusCheck();

            //set up handlers to persist OOF messages
            htmlEditorControl1.Validated += htmlEditorValidated;
            htmlEditorControl2.Validated += htmlEditorValidated;

            //wait on async auth stuff if not null
            if (AuthTask != null)
            {
                AuthTask.Wait();
            }
        }

        private void AddMenuItems()
        {

            //for some reason, just touching the main form adds margins to everything
            //so doing this in code
            System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            clearToolStripMenuItem.Text = "Clear Stored Credentials";
            clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);


            this.fileToolStripMenuItem.DropDownItems.Insert(0,clearToolStripMenuItem);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ClearAllCreds();

        }

        private static void ClearAllCreds()
        {
            //also clear out the stored credentials
            Exchange101.Service.ClearCredentials();

            //autodiscover is only done if Properties.Settings.Default.EWSURL != "default"
            //so setting it to default is the equivalent of resetting credential properties
            Properties.Settings.Default.EWSURL = "default";
            Properties.Settings.Default.Save();

            //rather than fighting with system and UI state
            //just tell the user the exit and restart
            MessageBox.Show("Cleared credentials. Please exit and restart.", "OOFSponder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        //private async System.Threading.Tasks.Task RunGetCreds()
        //{
        //    UpdateStatusLabel(toolStripStatusLabel2, "Configuring Exchange, please wait.");
        //    notifyIcon1.Text = "Configuring Exchange, please wait.";

        //    try
        //    {
        //        await System.Threading.Tasks.Task.Run(() => GetCreds());
        //        UpdateStatusLabel(toolStripStatusLabel2, "Found your Exchange server!");
        //        notifyIcon1.Text = "Found your Exchange server!";
        //    }
        //    catch (System.Security.Authentication.AuthenticationException authEx)
        //    {
        //        //AuthenticationException should be handled lower in the stack, so just record it
        //        //and move on
        //        AIClient.TrackException(authEx);
        //    }
        //    catch (Exception ex)
        //    {
        //        AIClient.TrackException(ex);
        //        MessageBox.Show("Unable to find your Exchange server. Please try again later", "OOFSponder", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }




        //}

        //    private async System.Threading.Tasks.Task GetCreds()
        //{
        //    //if we have the EWSURL, then send it in
        //    Exchange101.UserData user = new Exchange101.UserData();
        //    if (Properties.Settings.Default.EWSURL != "default")
        //    {
        //        user.AutodiscoverUrl = new Uri(Properties.Settings.Default.EWSURL);
        //        Exchange101.Service.ConnectToService(user);
        //    }
        //    else
        //    {

        //        //should really have a more elegant way of doing this, but that is future work
        //        //if we get here, that means we don't have creds or URL
        //        //MessageBox.Show("No Exchange server identified yet. This may take a couple minutes");
        //        //therefore, turn on autodiscover tracing
        //        Exchange101.Service.ConnectToService(true);
        //        Properties.Settings.Default.EWSURL = Exchange101.Service.Instance.Url.ToString();
        //        //MessageBox.Show("Found your Exchange server!");

        //        Properties.Settings.Default.Save();
        //    }

        //    //map the collected info to the properties
        //    foundexchange = true;
        //    Properties.Settings.Default.EncryptPW = "UsingCredMan";
        //    Properties.Settings.Default.EmailAddress = Exchange101.UserData.user.EmailAddress;
        //    Properties.Settings.Default.Save();
        //}

        #region Set Oof Timed Loop
        void Loopy()
        {
            //Every 10 minutes for automation
            var timer = new System.Timers.Timer(600000);
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        private async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RunSetOof();
            RunStatusCheck();
        }
        #endregion

        #region Oof/EWS interaction
        #region Oof Status Check
        private async void RunStatusCheck()
        {
            if (Properties.Settings.Default.EmailAddress != "default" &&
                Properties.Settings.Default.EncryptPW != "default"
                && Properties.Settings.Default.EncryptPW == "UsingCredMan" 
                && Properties.Settings.Default.EWSURL != "default"
                && foundexchange
                )
            {
                string emailAddress = Properties.Settings.Default.EmailAddress;
                await System.Threading.Tasks.Task.Run(() => checkOOFStatus(emailAddress));
            }
        }

        public async System.Threading.Tasks.Task checkOOFStatus(string EmailAddress)
        {
            try
            {
                //variant using CredMan
                OofSettings myOOFSettings = Exchange101.Service.Instance.GetUserOofSettings(Properties.Settings.Default.EmailAddress);

                string currentStatus = "";

                if (myOOFSettings.State == OofState.Scheduled && (myOOFSettings.Duration.StartTime > DateTime.Now && myOOFSettings.Duration.EndTime < DateTime.Now))
                {
                    currentStatus = "OOF until " + myOOFSettings.Duration.EndTime.ToString();
                }
                else if (myOOFSettings.State == OofState.Scheduled && (myOOFSettings.Duration.StartTime < DateTime.Now || myOOFSettings.Duration.EndTime > DateTime.Now)) 
                {
                    currentStatus = "OOF starting at " + myOOFSettings.Duration.StartTime.ToString();
                }
                else if (myOOFSettings.State == OofState.Enabled)
                {
                    currentStatus = "Currently OOF";
                }
                else if (myOOFSettings.State == OofState.Disabled)
                {
                    currentStatus = "OOF Disabled";
                }

                //pull the existing OOF messages in
                //this accounts for where someone changes te message externally
                htmlEditorControl1.BodyHtml = myOOFSettings.ExternalReply;
                htmlEditorControl2.BodyHtml = myOOFSettings.InternalReply;

                //save them
                OOFData.Instance.ExternalOOFMessage = myOOFSettings.ExternalReply;
                OOFData.Instance.InternalOOFMessage = myOOFSettings.InternalReply;
                Properties.Settings.Default.Save();

                UpdateStatusLabel(toolStripStatusLabel2, "Current Status: " + currentStatus);
                notifyIcon1.Text = "Current Status: " + currentStatus;
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(100, "Login Error", "Cannot login to Exchange, please check your password!", ToolTipIcon.Error);
                UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - Email or Password incorrect or we cannot contact the server please check your settings and try again");
                            //don't send AI stuff if running in DEBUG
#if !DEBUG
                AIClient.TrackException(ex);
#endif
                return;
            }

        }
        #endregion
        #region Oof Manual Run
        private async void RunManualOOF()
        {
            if (Properties.Settings.Default.EmailAddress != "default" &&
                OOFData.Instance.ExternalOOFMessage != "default" &&
                OOFData.Instance.InternalOOFMessage != "default" &&
                Properties.Settings.Default.EncryptPW != "default"
                && Properties.Settings.Default.EncryptPW == "UsingCredMan"
                && Properties.Settings.Default.EWSURL != "default"
                && foundexchange
                )
            {
                string emailAddress = Properties.Settings.Default.EmailAddress;
                //Toggle Manual OOF
                await System.Threading.Tasks.Task.Run(() => setManualOOF(emailAddress, OOFData.Instance.ExternalOOFMessage, 
                    OOFData.Instance.InternalOOFMessage, !manualoof));
            }
        }

        public async System.Threading.Tasks.Task setManualOOF(string emailAddress, string oofMessageExternal, string oofMessageInternal, bool on)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString() + " - Sending to Exchange Server";

            try
            {
                //variant using CredMan
                OofSettings myOOFSettings = Exchange101.Service.Instance.GetUserOofSettings(Exchange101.UserData.user.EmailAddress);

                OofSettings myOOF = new OofSettings();

                // Set the OOF status to be a scheduled time period.
                if(on)
                    myOOF.State = OofState.Enabled;
                else
                    myOOF.State = OofState.Disabled;

                // Select the external audience that will receive OOF messages.
                myOOF.ExternalAudience = OofExternalAudience.All;

                // Set the OOF message for your internal audience.
                myOOF.InternalReply = new OofReply(oofMessageInternal);

                // Set the OOF message for your external audience.
                myOOF.ExternalReply = new OofReply(oofMessageExternal);

                string newinternal = Regex.Replace(myOOF.InternalReply, @"\r\n|\n\r|\n|\r", "\r\n");
                string currentinternal = Regex.Replace(myOOFSettings.InternalReply, @"\r\n|\n\r|\n|\r", "\r\n");
                string newexternal = Regex.Replace(myOOF.ExternalReply, @"\r\n|\n\r|\n|\r", "\r\n");
                string currentexternal = Regex.Replace(myOOFSettings.ExternalReply, @"\r\n|\n\r|\n|\r", "\r\n");

                if (myOOF.State != myOOFSettings.State ||
                    newinternal != currentinternal ||
                    newexternal != currentexternal)
                {
                    // Set value to Server
                    //variant using CredMan
                    Exchange101.Service.Instance.SetUserOofSettings(Exchange101.UserData.user.EmailAddress, myOOF);
                    UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - OOF Message set on Server");
                    RunStatusCheck();
                }
                else
                {
                    UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - No changes needed, OOF Message not set on Server");
                }
                if (on)
                    manualoof = true;
                else
                    manualoof = false;

                
                //don't send AI stuff if running in DEBUG
                //report to AppInsights
#if !DEBUG
                AIClient.TrackEvent("Set OOF manually");
#endif
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(100, "Login Error", "Cannot login to Exchange, please check your password!", ToolTipIcon.Error);
                UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - Email or Password incorrect");
                //don't send AI stuff if running in DEBUG
                //report to AppInsights
#if !DEBUG
                AIClient.TrackException(ex);
#endif
                return;
            }
        }
        #endregion
        #region Oof Set
        private async void RunSetOof()
        {
            bool haveNecessaryData = false;

            //if CredMan is turned on, then we don't need the email or password
            //but we still need the OOF messages and working hours
            //also, don't need to check SecondaryOOF messages for two reasons:
            //1) they won't always be set
            //2) the UI flow won't let you get here with permaOOF if they aren't set
            if (OOFData.Instance.ExternalOOFMessage != "default" &&
                OOFData.Instance.InternalOOFMessage != "default" &&
                OOFScheduling.Properties.Settings.Default.workingHours != "default" &&
                OOFScheduling.Properties.Settings.Default.EncryptPW == "UsingCredMan" &&
                OOFScheduling.Properties.Settings.Default.EWSURL != "default" &&
                foundexchange)
            {
                haveNecessaryData = true;
            }

            if (haveNecessaryData)
            {
                string emailAddress = Properties.Settings.Default.EmailAddress;
                //need to move these to *after* decided whether to use Primary or Secondary
                //string oofMessageExternal = Properties.Settings.Default.OOFHtmlExternal;
                //string oofMessageInternal = Properties.Settings.Default.OOFHtmlInternal;
                DateTime[] oofTimes = getOofTime(Properties.Settings.Default.workingHours);

                //if PermaOOF is turned on, need to adjust the end time
                if (OOFData.Instance.PermaOOFDate < oofTimes[0])
                {
                    //turn off permaOOF
                    //NOTE: this all should be abstracted in a Property somewhere
                    OOFData.Instance.IsPermaOOFOn = false;

                    //set all the UI stuff back to primary 
                    //to set up for normal OOF schedule
                    SetUIforPrimary();

                }

                //persist settings just in case
                string oofMessageExternal= htmlEditorControl1.BodyHtml;
                string oofMessageInternal= htmlEditorControl2.BodyHtml;
                if (!OOFData.Instance.IsPermaOOFOn)
                {
                    OOFData.Instance.PrimaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                    OOFData.Instance.PrimaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
                }
                else
                {
                    OOFData.Instance.SecondaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                    OOFData.Instance.SecondaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
                }

                OOFScheduling.Properties.Settings.Default.Save();


                //if PermaOOF isn't turned on, use the standard logic based on the stored schedule
                if ((oofTimes[0] != oofTimes[1]) && !OOFData.Instance.IsPermaOOFOn)
                {
                    await System.Threading.Tasks.Task.Run(() => setOOF(emailAddress, oofMessageExternal, oofMessageInternal, oofTimes[0], oofTimes[1]));
                }
                else
                //since permaOOF is on, need to adjust the end date such that is permaOOFDate
                //if permaOOF>oofTimes[0] and permaOOF<oofTimes[1], then AddDays((permaOOFDate - oofTimes[1]).Days
                //due to the way the math works out, need to add extra day if permaOOF>oofTimes[1]
                {
                    int adjustmentDays = 0;
                    if(OOFData.Instance.PermaOOFDate>oofTimes[0] && OOFData.Instance.PermaOOFDate < oofTimes[1])
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
#if !NOOOF
                    await System.Threading.Tasks.Task.Run(() => setOOF(emailAddress, oofMessageExternal, oofMessageInternal, oofTimes[0], oofTimes[1].AddDays((OOFData.Instance.PermaOOFDate - oofTimes[1]).Days + adjustmentDays)));
#endif
                }
            }
        }

        private async void RunSetOofO365()
        {
            bool haveNecessaryData = false;

            //if CredMan is turned on, then we don't need the email or password
            //but we still need the OOF messages and working hours
            //also, don't need to check SecondaryOOF messages for two reasons:
            //1) they won't always be set
            //2) the UI flow won't let you get here with permaOOF if they aren't set
            if (OOFData.Instance.ExternalOOFMessage != "default" &&
                OOFData.Instance.InternalOOFMessage != "default" &&
                OOFData.Instance.WorkingHours != "default" )
            {
                haveNecessaryData = true;
            }

            if (haveNecessaryData)
            {
                DateTime[] oofTimes = getOofTime(OOFData.Instance.WorkingHours);

                //if PermaOOF is turned on, need to adjust the end time
                if (OOFData.Instance.PermaOOFDate < oofTimes[0])
                {
                    //turn off permaOOF
                    //NOTE: this all should be abstracted in a Property somewhere
                    OOFData.Instance.IsPermaOOFOn = false;

                    //set all the UI stuff back to primary 
                    //to set up for normal OOF schedule
                    SetUIforPrimary();

                }

                //persist settings just in case
                string oofMessageExternal = htmlEditorControl1.BodyHtml;
                string oofMessageInternal = htmlEditorControl2.BodyHtml;
                if (!OOFData.Instance.IsPermaOOFOn)
                {
                    OOFData.Instance.PrimaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                    OOFData.Instance.PrimaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
                }
                else
                {
                    OOFData.Instance.SecondaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                    OOFData.Instance.SecondaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
                }

                //if PermaOOF isn't turned on, use the standard logic based on the stored schedule
                if ((oofTimes[0] != oofTimes[1]) && !OOFData.Instance.IsPermaOOFOn)
                {
                    await System.Threading.Tasks.Task.Run(() => setOOFO365(oofMessageExternal, oofMessageInternal, oofTimes[0], oofTimes[1]));
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
#if !NOOOF
                    await System.Threading.Tasks.Task.Run(() => setOOFO365(oofMessageExternal, oofMessageInternal, oofTimes[0], oofTimes[1].AddDays((OOFData.Instance.PermaOOFDate - oofTimes[1]).Days + adjustmentDays)));
#endif
                }
            }
        }

        public async System.Threading.Tasks.Task setOOF(string EmailAddress, string oofMessageExternal, string oofMessageInternal, DateTime StartTime, DateTime EndTime)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString() + " - Sending to Exchange Server";

            try
            {
                OofSettings myOOFSettings = Exchange101.Service.Instance.GetUserOofSettings(Exchange101.UserData.user.EmailAddress);

                OofSettings myOOF = new OofSettings();

                // Set the OOF status to be a scheduled time period.
                myOOF.State = OofState.Scheduled;

                // Select the time period during which to send OOF messages.
                myOOF.Duration = new TimeWindow(StartTime, EndTime);

                // Select the external audience that will receive OOF messages.
                myOOF.ExternalAudience = OofExternalAudience.All;

                // Set the OOF message for your internal audience.
                myOOF.InternalReply = new OofReply(oofMessageInternal);

                // Set the OOF message for your external audience.
                myOOF.ExternalReply = new OofReply(oofMessageExternal);

                string newinternal = Regex.Replace(myOOF.InternalReply, @"\r\n|\n\r|\n|\r", "\r\n");
                string currentinternal = Regex.Replace(myOOFSettings.InternalReply, @"\r\n|\n\r|\n|\r", "\r\n");
                string newexternal = Regex.Replace(myOOF.ExternalReply, @"\r\n|\n\r|\n|\r", "\r\n");
                string currentexternal = Regex.Replace(myOOFSettings.ExternalReply, @"\r\n|\n\r|\n|\r", "\r\n");

                if (myOOF.State != myOOFSettings.State ||
                    myOOF.Duration != myOOFSettings.Duration ||
                    newinternal != currentinternal ||
                    newexternal != currentexternal)
                {
                    // Set value to Server if we have the user address and URL
                    if (Exchange101.UserData.user.EmailAddress !=null)
                    {
                        //variant using CredMan
#if !NOOOF
                        Exchange101.Service.Instance.SetUserOofSettings(Exchange101.UserData.user.EmailAddress, myOOF);
#endif
                        UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - OOF Message set on Server");
                        RunStatusCheck();

                        //report back to AppInsights
                        AIClient.TrackEvent("Set OOF for user: " + AIClient.Context.User.Id.ToString());
                    }

                }
                else
                {
                    UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - No changes needed, OOF Message not set on Server");
                }
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(100, "Login Error", "Cannot login to Exchange, please check your password!", ToolTipIcon.Error);
                UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - Email or Password incorrect");
                //don't send AI stuff if running in DEBUG
                //report to AppInsights
#if !DEBUG
                AIClient.TrackException(ex);
#endif
                return;
            }
        }

        public async System.Threading.Tasks.Task setOOFO365(string oofMessageExternal, string oofMessageInternal, DateTime StartTime, DateTime EndTime)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString() + " - Sending to O365";

            try
            {
                OofSettings myOOFSettings = Exchange101.Service.Instance.GetUserOofSettings(Exchange101.UserData.user.EmailAddress);

                OofSettings myOOF = new OofSettings();

                // Set the OOF status to be a scheduled time period.
                myOOF.State = OofState.Scheduled;

                // Select the time period during which to send OOF messages.
                myOOF.Duration = new TimeWindow(StartTime, EndTime);

                // Select the external audience that will receive OOF messages.
                myOOF.ExternalAudience = OofExternalAudience.All;

                // Set the OOF message for your internal audience.
                myOOF.InternalReply = new OofReply(oofMessageInternal);

                // Set the OOF message for your external audience.
                myOOF.ExternalReply = new OofReply(oofMessageExternal);

                string newinternal = Regex.Replace(myOOF.InternalReply, @"\r\n|\n\r|\n|\r", "\r\n");
                string currentinternal = Regex.Replace(myOOFSettings.InternalReply, @"\r\n|\n\r|\n|\r", "\r\n");
                string newexternal = Regex.Replace(myOOF.ExternalReply, @"\r\n|\n\r|\n|\r", "\r\n");
                string currentexternal = Regex.Replace(myOOFSettings.ExternalReply, @"\r\n|\n\r|\n|\r", "\r\n");

                if (myOOF.State != myOOFSettings.State ||
                    myOOF.Duration != myOOFSettings.Duration ||
                    newinternal != currentinternal ||
                    newexternal != currentexternal)
                {
                    // Set value to Server if we have the user address and URL
                    if (Exchange101.UserData.user.EmailAddress != null)
                    {
                        //variant using CredMan
#if !NOOOF
                        Exchange101.Service.Instance.SetUserOofSettings(Exchange101.UserData.user.EmailAddress, myOOF);
#endif
                        UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - OOF Message set on Server");
                        RunStatusCheck();

                        //report back to AppInsights
                        AIClient.TrackEvent("Set OOF for user: " + AIClient.Context.User.Id.ToString());
                    }

                }
                else
                {
                    UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - No changes needed, OOF Message not set on Server");
                }
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(100, "Login Error", "Cannot login to Exchange, please check your password!", ToolTipIcon.Error);
                UpdateStatusLabel(toolStripStatusLabel1, DateTime.Now.ToString() + " - Email or Password incorrect");
                //don't send AI stuff if running in DEBUG
                //report to AppInsights
#if !DEBUG
                AIClient.TrackException(ex);
#endif
                return;
            }
        }

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
            string[] workingTimesArray = workingHours.Split('|');

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

            DateTime StartTime;
            if (currentWorkingTime[2] == "1")
            {
                StartTime = DateTime.Parse(currentCheckDate.ToString("D") + " " + currentWorkingTime[1]);
            }
            else
            {
                int daysback = -1;
                while (true)
                {
                    DateTime backday = currentCheckDate.AddDays(daysback);
                    string[] oldWorkingTime = workingTimesArray[(int)backday.DayOfWeek].Split('~');
                    if (oldWorkingTime[2] == "1")
                    {
                        StartTime = DateTime.Parse(backday.ToString("D") + " " + oldWorkingTime[1]);
                        break;
                    }
                    else
                    {
                        daysback--;
                    }
                }
            }

            string[] futureWorkingTime = workingTimesArray[(int)currentCheckDate.AddDays(1).DayOfWeek].Split('~');
            DateTime EndTime;
            if (futureWorkingTime[2] == "1")
            {
                EndTime = DateTime.Parse(currentCheckDate.AddDays(1).ToString("D") + " " + futureWorkingTime[0]);
            }
            else
            {
                int daysforward = 1;
                while (true)
                {
                    DateTime comingday = currentCheckDate.AddDays(1).AddDays(daysforward);
                    string[] oldWorkingTime = workingTimesArray[(int)comingday.DayOfWeek].Split('~');
                    if (oldWorkingTime[2] == "1")
                    {
                        EndTime = DateTime.Parse(comingday.ToString("D") + " " + oldWorkingTime[0]);
                        break;
                    }
                    else
                    {
                        daysforward++;
                    }
                }
            }

            OofTimes[0] = StartTime;
            OofTimes[1] = EndTime;

            return OofTimes;
        }

        private void saveSettings()
        {
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
        }

        #endregion

        #region Events
        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RunManualMenu(object sender, EventArgs e)
        {
            RunSetOof();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(100);
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (minimize && e.CloseReason != CloseReason.WindowsShutDown)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }

            //don't send AI stuff if running in DEBUG
            //report to AppInsights
#if !DEBUG
            if (AIClient != null)
            {
                AIClient.Flush(); // only for desktop apps
            }
#endif

        }

        private async void btnRunManually_Click(object sender, EventArgs e)
        {
            //don't send AI stuff if running in DEBUG
            //report to AppInsights
#if !DEBUG
            AIClient.TrackEvent("Setting OOF manually");
#endif
            RunSetOof();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveSettings();
        }
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSettings();
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            minimize = false;

            //flush AI data
            if (AIClient != null)
            {
                AIClient.Flush(); // only for desktop apps
            }

            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RunManualOOF();
            if (button3.Text == "Go OOF Now")
            {
                button3.Text = "Return from OOF";
            }
            else
            {
                button3.Text = "Go OOF Now";
            }
        }
        #endregion

        private void btnPermaOOF_Click(object sender, EventArgs e)
        {
            OOFSponderInsights.Track("Went PermaOOF");

            //persist the HTML text if it has been set
            //assume the latest text in the HTML controls should win
            if (htmlEditorControl1.BodyHtml != DummyHTML)
            {
                OOFData.Instance.SecondaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
            }
            if (htmlEditorControl2.BodyHtml != DummyHTML)
            {
                OOFData.Instance.SecondaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
            }

            //only set up for permaOOF if we have OOF messages
            if (Properties.Settings.Default.SecondaryOOFExternal == String.Empty | Properties.Settings.Default.SecondaryOOFInternal == String.Empty)
            {
                MessageBox.Show("Unable to turn on extended OOF - Secondary OOF messages not set", "OOFSponder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                //save the current text to permaOOF
                OOFData.Instance.IsPermaOOFOn = true;
                OOFData.Instance.PermaOOFDate = dtPermaOOF.Value;

                //actually go OOF now
                RunSetOof();

                SetUIforPermaOOF();
            }




        }

        private void secondaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AIClient.TrackEvent("Configured secondary OOF messages");

            //persist the existing OOF messages as Primary and then pull in the secondary
            OOFData.Instance.PrimaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
            OOFData.Instance.PrimaryOOFInternalMessage= htmlEditorControl2.BodyHtml;
            OOFData.Instance.IsPermaOOFOn = true;

            //now, set up the UI for PermaOOF
            SetUIforPermaOOF();
        }

        private void SetUIforPermaOOF()
        {
            primaryToolStripMenuItem.Checked = false;
            secondaryToolStripMenuItem.Checked = !primaryToolStripMenuItem.Checked;
            lblExternalMesage.Text = "Extended OOF External Message";
            lblInternalMessage.Text = "Extended OOF Internal Message";


            htmlEditorControl1.BodyHtml = OOFData.Instance.ExternalOOFMessage;
            htmlEditorControl2.BodyHtml = OOFData.Instance.InternalOOFMessage;

            Properties.Settings.Default.MessageOption = "2";
            Properties.Settings.Default.Save();

            //lastly, enable the permaOOF controls to help with some UI flow issues
            btnPermaOOF.Enabled = true;
            dtPermaOOF.Enabled = true;
        }

        private void primaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //since we are in the process of flipping from secondary to primary
            //we know that the UI is currently in Primary mode
            //so HTML controls have the Primary messages

            //persist the existing OOF messages as Secondary and then pull in the Primary
            OOFData.Instance.SecondaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
            OOFData.Instance.SecondaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
            OOFData.Instance.IsPermaOOFOn = false;

            //now, set up the UI for primary
            SetUIforPrimary();
        }

        private void SetUIforPrimary()
        {
            primaryToolStripMenuItem.Checked = true;
            secondaryToolStripMenuItem.Checked = !primaryToolStripMenuItem.Checked;
            lblExternalMesage.Text = "Primary External Message";
            lblInternalMessage.Text = "Primary Internal Message";

            htmlEditorControl1.BodyHtml = OOFData.Instance.ExternalOOFMessage;
            htmlEditorControl2.BodyHtml = OOFData.Instance.InternalOOFMessage;

            Properties.Settings.Default.MessageOption = "1";
            Properties.Settings.Default.Save();

            //lastly, disable the permaOOF controls to help with some UI flow issues
            btnPermaOOF.Enabled = false;
            dtPermaOOF.Enabled = false;
        }

        //common call for both controls, regardless of primary or secondary
        private void htmlEditorValidated(object sender, EventArgs e)
        {
            if (!OOFData.Instance.IsPermaOOFOn)
            {
                System.Diagnostics.Trace.WriteLine("PermaOOF off - persisting primary messages");
                OOFData.Instance.PrimaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                OOFData.Instance.PrimaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("PermaOOF on - persisting secondary messages");
                OOFData.Instance.SecondaryOOFExternalMessage = htmlEditorControl1.BodyHtml;
                OOFData.Instance.SecondaryOOFInternalMessage = htmlEditorControl2.BodyHtml;
            }
        }
    }
}
