using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Exchange.WebServices.Data;
using System.Security.Cryptography;
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

        //Track if PermaOOF (OOF until a specific day in the future)
        private bool permaOOF = false;
        private DateTime permaOOFDate;

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
            ConfigureApplicationInsights();
#if !CredMan
            //Show manual credentials on the bottom if we don't have credman
            label14.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            passwordConfirmTB.Visible = true;
            passwordTB.Visible = true;
            emailAddressTB.Visible = true;
            this.Height = 860;      
#endif
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
#if CredMan
            RunGetCreds();
#endif

            //Can this get dropped by pulling in the OOF from the server during the CheckOOFStatus call?
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
            dtPermaOOF.Value = DateTime.Now.AddDays(4);




            bool haveNecessaryData = false;

#if !CredMan
            //check to ensure we have all the necessary inputs
            if (Properties.Settings.Default.EmailAddress != "default" &&
    Properties.Settings.Default.OOFHtmlExternal != "default" &&
    Properties.Settings.Default.OOFHtmlInternal != "default" &&
    Properties.Settings.Default.workingHours != "default" &&
    Properties.Settings.Default.EncryptPW != "default")
            {
                haveNecessaryData = true;
            }
#else
            //if CredMan is turned on, then we don't need the email or password
            //but we still need the OOF messages and working hours
            if (OOFData.Instance.ExternalOOFMessage != "default" && OOFData.Instance.InternalOOFMessage != "default" 
                && OOFData.Instance.WorkingHours != "default")
            {
                haveNecessaryData = true;
            }
#endif

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

        private void ConfigureApplicationInsights()
        {
            AIClient.InstrumentationKey = "9eacd004-7944-4d2e-a978-d66104c67a49";
            // Set session data:
            AIClient.Context.User.Id = Environment.UserName;

            //use DEBUGAI if we actually want AppInsights from a DEBUG build
#if DEBUGAI
            AIClient.Context.User.Id = "DEBUG";
            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = true;
#endif
            AIClient.Context.Session.Id = Guid.NewGuid().ToString();
            AIClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

            //don't send AI stuff if running in DEBUG
#if !DEBUG
            //we are using this to track unique users
            AIClient.TrackEvent("User: " + AIClient.Context.User.Id.ToString());
#endif

        }

        private async void RunGetCreds()
        {
            UpdateStatusLabel(toolStripStatusLabel2, "Configuring Exchange, please wait.");
            notifyIcon1.Text = "Configuring Exchange, please wait.";

            try
            {
                await System.Threading.Tasks.Task.Run(() => GetCreds());
                UpdateStatusLabel(toolStripStatusLabel2, "Found your Exchange server!");
                notifyIcon1.Text = "Found your Exchange server!";
            }
            catch (System.Security.Authentication.AuthenticationException authEx)
            {
                //AuthenticationException should be handled lower in the stack, so just record it
                //and move on
                AIClient.TrackException(authEx);
            }
            catch (Exception ex)
            {
                AIClient.TrackException(ex);
                MessageBox.Show("Unable to find your Exchange server. Please try again later", "OOFSponder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        private async System.Threading.Tasks.Task GetCreds()
        {
            //if we have the EWSURL, then send it in
            Exchange101.UserData user = new Exchange101.UserData();
            if (Properties.Settings.Default.EWSURL != "default")
            {
                user.AutodiscoverUrl = new Uri(Properties.Settings.Default.EWSURL);
                Exchange101.Service.ConnectToService(user);
            }
            else
            {

                //should really have a more elegant way of doing this, but that is future work
                //if we get here, that means we don't have creds or URL
                //MessageBox.Show("No Exchange server identified yet. This may take a couple minutes");
                //therefore, turn on autodiscover tracing
                Exchange101.Service.ConnectToService(true);
                Properties.Settings.Default.EWSURL = Exchange101.Service.Instance.Url.ToString();
                //MessageBox.Show("Found your Exchange server!");

                Properties.Settings.Default.Save();
            }

            //map the collected info to the properties
            foundexchange = true;
            Properties.Settings.Default.EncryptPW = "UsingCredMan";
            Properties.Settings.Default.EmailAddress = Exchange101.UserData.user.EmailAddress;
            Properties.Settings.Default.Save();
        }

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
#if CredMan
                && Properties.Settings.Default.EncryptPW == "UsingCredMan" 
                && Properties.Settings.Default.EWSURL != "default"
                && foundexchange
#endif
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
#if !CredMan
                //variant using Web Credentials
                OofSettings myOOFSettings = ExchangeServiceConnection.Instance.service.GetUserOofSettings(EmailAddress);
#else
                //variant using CredMan
                OofSettings myOOFSettings = Exchange101.Service.Instance.GetUserOofSettings(Properties.Settings.Default.EmailAddress);
#endif

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
                htmlEditorControl1.BodyHtml = myOOFSettings.ExternalReply;
                htmlEditorControl2.BodyHtml = myOOFSettings.InternalReply;

                //save them
                //Properties.Settings.Default.OOFHtmlExternal = myOOFSettings.ExternalReply;
                //Properties.Settings.Default.OOFHtmlInternal = myOOFSettings.InternalReply;
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
#if CredMan
                && Properties.Settings.Default.EncryptPW == "UsingCredMan"
                && Properties.Settings.Default.EWSURL != "default"
                && foundexchange
#endif
                )
            {
                string emailAddress = Properties.Settings.Default.EmailAddress;
                string oofMessageExternal = OOFData.Instance.ExternalOOFMessage;
                string oofMessageInternal = OOFData.Instance.InternalOOFMessage;
                //Toggle Manual OOF
                await System.Threading.Tasks.Task.Run(() => setManualOOF(emailAddress, oofMessageExternal, oofMessageInternal, !manualoof));
            }
        }

        public async System.Threading.Tasks.Task setManualOOF(string emailAddress, string oofMessageExternal, string oofMessageInternal, bool on)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString() + " - Sending to Exchange Server";

            try
            {
#if !CredMan
                //variant using Web Credentials
                OofSettings myOOFSettings = ExchangeServiceConnection.Instance.service.GetUserOofSettings(emailAddress);
#else
                //variant using CredMan
                OofSettings myOOFSettings = Exchange101.Service.Instance.GetUserOofSettings(Exchange101.UserData.user.EmailAddress);
#endif

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
#if !CredMan
                    //variant using Web Credentials
                    ExchangeServiceConnection.Instance.service.SetUserOofSettings(emailAddress, myOOF);
#else
                    //variant using CredMan
                    Exchange101.Service.Instance.SetUserOofSettings(Exchange101.UserData.user.EmailAddress, myOOF);
#endif
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

#if !CredMan
            //check to ensure we have all the necessary inputs
            if (Properties.Settings.Default.EmailAddress != "default" &&
    Properties.Settings.Default.OOFHtmlExternal != "default" &&
    Properties.Settings.Default.OOFHtmlInternal != "default" &&
    Properties.Settings.Default.workingHours != "default" &&
    Properties.Settings.Default.EncryptPW != "default")
            {
                haveNecessaryData = true;
            }
#else
            //if CredMan is turned on, then we don't need the email or password
            //but we still need the OOF messages and working hours
            //also, don't need to check SecondaryOOF messages for two reasons:
            //1) they won't always be set
            //2) the UI flow won't let you get here with permaOOF if they aren't set
            if (OOFScheduling.Properties.Settings.Default.PrimaryOOFExternal != "default" &&
                OOFScheduling.Properties.Settings.Default.PrimaryOOFInternal != "default" &&
                OOFScheduling.Properties.Settings.Default.workingHours != "default" &&
                OOFScheduling.Properties.Settings.Default.EncryptPW == "UsingCredMan" &&
                OOFScheduling.Properties.Settings.Default.EWSURL != "default" &&
                foundexchange)
            {
                haveNecessaryData = true;
            }
#endif


            if (haveNecessaryData)
            {
                string emailAddress = Properties.Settings.Default.EmailAddress;
                //need to move these to *after* decided whether to use Primary or Secondary
                //string oofMessageExternal = Properties.Settings.Default.OOFHtmlExternal;
                //string oofMessageInternal = Properties.Settings.Default.OOFHtmlInternal;
                DateTime[] oofTimes = getOofTime(Properties.Settings.Default.workingHours);

                //if PermaOOF is turned on, need to adjust the end time
                if (permaOOFDate < oofTimes[0])
                {
                    //turn off permaOOF
                    //NOTE: this all should be abstracted in a Property somewhere
                    permaOOF = false;
                    Properties.Settings.Default.IsPermaOOFOn = permaOOF;
                    Properties.Settings.Default.Save();

                    //set all the UI stuff back to primary 
                    //to set up for normal OOF schedule
                    SetUIforPrimary();

                }

                //persist settings just in case
                string oofMessageExternal= htmlEditorControl1.BodyHtml;
                string oofMessageInternal= htmlEditorControl2.BodyHtml;
                if (!permaOOF)
                {
                    OOFScheduling.Properties.Settings.Default.PrimaryOOFExternal = htmlEditorControl1.BodyHtml;
                    OOFScheduling.Properties.Settings.Default.PrimaryOOFInternal = htmlEditorControl2.BodyHtml;
                }
                else
                {
                    OOFScheduling.Properties.Settings.Default.SecondaryOOFExternal = htmlEditorControl1.BodyHtml;
                    OOFScheduling.Properties.Settings.Default.SecondaryOOFInternal = htmlEditorControl2.BodyHtml;
                }

                OOFScheduling.Properties.Settings.Default.Save();


                //if PermaOOF isn't turned on, use the standard logic based on the stored schedule
                if ((oofTimes[0] != oofTimes[1]) && !permaOOF)
                {
                    await System.Threading.Tasks.Task.Run(() => setOOF(emailAddress, oofMessageExternal, oofMessageInternal, oofTimes[0], oofTimes[1]));
                }
                else
                //since permaOOF is on, need to adjust the end date such that is permaOOFDate
                //if permaOOF>oofTimes[0] and permaOOF<oofTimes[1], then AddDays((permaOOFDate - oofTimes[1]).Days
                //due to the way the math works out, need to add extra day if permaOOF>oofTimes[1]
                {
                    int adjustmentDays = 0;
                    if(permaOOFDate>oofTimes[0] && permaOOFDate<oofTimes[1])
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

                    await System.Threading.Tasks.Task.Run(() => setOOF(emailAddress, oofMessageExternal, oofMessageInternal, oofTimes[0], oofTimes[1].AddDays((permaOOFDate - oofTimes[1]).Days + adjustmentDays)));
                }
            }
        }

        public async System.Threading.Tasks.Task setOOF(string EmailAddress, string oofMessageExternal, string oofMessageInternal, DateTime StartTime, DateTime EndTime)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString() + " - Sending to Exchange Server";

            try
            {
#if !CredMan
                //variant using Web Credentials
                OofSettings myOOFSettings = ExchangeServiceConnection.Instance.service.GetUserOofSettings(EmailAddress);
#else
                OofSettings myOOFSettings = Exchange101.Service.Instance.GetUserOofSettings(Exchange101.UserData.user.EmailAddress);

#endif
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
#if !CredMan
                    //variant using Web Credentials
                    ExchangeServiceConnection.Instance.service.SetUserOofSettings(EmailAddress, myOOF);
#else
                        //variant using CredMan
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
#if !CredMan
            if (string.IsNullOrEmpty(emailAddressTB.Text))
            {
                MessageBox.Show("Please enter your email address");
            }
            else if (string.IsNullOrEmpty(passwordTB.Text))
            {
                MessageBox.Show("Please enter your password");
            }
            else if (string.IsNullOrEmpty(passwordConfirmTB.Text))
            {
                MessageBox.Show("Please confirm your password");
            }
            else if (passwordConfirmTB.Text != passwordTB.Text)
            {
                MessageBox.Show("Your password does not match the confirmed password, please confirm your password");
            }
            else
            {
                Properties.Settings.Default.EmailAddress = emailAddressTB.Text;
                Properties.Settings.Default.EncryptPW = DataProtectionApiWrapper.Encrypt(passwordTB.Text);
                Properties.Settings.Default.OOFHtmlExternal = htmlEditorControl1.BodyHtml;
                Properties.Settings.Default.OOFHtmlInternal = htmlEditorControl2.BodyHtml;
                Properties.Settings.Default.workingHours = ScheduleString();

                Properties.Settings.Default.Save();

                passwordConfirmTB.Text = "";
                passwordTB.Text = "";

                toolStripStatusLabel1.Text = "Settings Saved";
            }
#else
            if (primaryToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.PrimaryOOFExternal = htmlEditorControl1.BodyHtml;
                Properties.Settings.Default.PrimaryOOFInternal = htmlEditorControl2.BodyHtml;
            }
            else
            //since customer is editing Secondary message, save text in Secondary
            {
                Properties.Settings.Default.SecondaryOOFExternal = htmlEditorControl1.BodyHtml;
                Properties.Settings.Default.SecondaryOOFInternal = htmlEditorControl2.BodyHtml;
            }

            Properties.Settings.Default.workingHours = ScheduleString();

            Properties.Settings.Default.Save();

            toolStripStatusLabel1.Text = "Settings Saved";
#endif
            
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
            AIClient.TrackEvent("Went PermaOOF");

            //persist the HTML text if it has been set
            //assume the latest text in the HTML controls should win
            if (htmlEditorControl1.BodyHtml != DummyHTML)
            {
                Properties.Settings.Default.SecondaryOOFExternal = htmlEditorControl1.BodyHtml;
            }
            if (htmlEditorControl2.BodyHtml != DummyHTML)
            {
                Properties.Settings.Default.SecondaryOOFInternal = htmlEditorControl2.BodyHtml;
            }

            //only set up for permaOOF if we have OOF messages
            if (Properties.Settings.Default.SecondaryOOFExternal == String.Empty | Properties.Settings.Default.SecondaryOOFInternal == String.Empty)
            {
                MessageBox.Show("Unable to turn on extended OOF - Secondary OOF messages not set", "OOFSponder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                //save the current text to permaOOF
                permaOOF = true;
                Properties.Settings.Default.IsPermaOOFOn = permaOOF;
                permaOOFDate = dtPermaOOF.Value;
                Properties.Settings.Default.PermaOOFDate = dtPermaOOF.Value;
                Properties.Settings.Default.Save();

                //actually go OOF now
                RunSetOof();

                SetUIforPermaOOF();
            }




        }

        private void secondaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AIClient.TrackEvent("Configured secondary OOF messages");

            //persist the existing OOF messages as Primary and then pull in the secondary
            Properties.Settings.Default.PrimaryOOFExternal = htmlEditorControl1.BodyHtml;
            Properties.Settings.Default.PrimaryOOFInternal = htmlEditorControl2.BodyHtml;
            Properties.Settings.Default.Save();

            //now, set up the UI for PermaOOF
            SetUIforPermaOOF();
        }

        private void SetUIforPermaOOF()
        {
            primaryToolStripMenuItem.Checked = false;
            secondaryToolStripMenuItem.Checked = !primaryToolStripMenuItem.Checked;
            lblExternalMesage.Text = "Extended OOF External Message";
            lblInternalMessage.Text = "Extended OOF Internal Message";


            htmlEditorControl1.BodyHtml = Properties.Settings.Default.SecondaryOOFExternal;
            htmlEditorControl2.BodyHtml = Properties.Settings.Default.SecondaryOOFInternal;

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
            Properties.Settings.Default.SecondaryOOFExternal = htmlEditorControl1.BodyHtml;
            Properties.Settings.Default.SecondaryOOFInternal = htmlEditorControl2.BodyHtml;
            Properties.Settings.Default.Save();

            //now, set up the UI for primary
            SetUIforPrimary();
        }

        private void SetUIforPrimary()
        {
            primaryToolStripMenuItem.Checked = true;
            secondaryToolStripMenuItem.Checked = !primaryToolStripMenuItem.Checked;
            lblExternalMesage.Text = "Primary External Message";
            lblInternalMessage.Text = "Primary Internal Message";

            htmlEditorControl1.BodyHtml = Properties.Settings.Default.PrimaryOOFExternal;
            htmlEditorControl2.BodyHtml = Properties.Settings.Default.PrimaryOOFInternal;

            Properties.Settings.Default.MessageOption = "1";
            Properties.Settings.Default.Save();

            //lastly, disable the permaOOF controls to help with some UI flow issues
            btnPermaOOF.Enabled = false;
            dtPermaOOF.Enabled = false;
        }

        //common call for both controls, regardless of primary or secondary
        private void htmlEditorValidated(object sender, EventArgs e)
        {
            if (!permaOOF)
            {
                System.Diagnostics.Trace.WriteLine("PermaOOF off - persisting primary messages");
                OOFScheduling.Properties.Settings.Default.PrimaryOOFExternal = htmlEditorControl1.BodyHtml;
                OOFScheduling.Properties.Settings.Default.PrimaryOOFInternal = htmlEditorControl2.BodyHtml;
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("PermaOOF on - persisting secondary messages");
                OOFScheduling.Properties.Settings.Default.SecondaryOOFExternal = htmlEditorControl1.BodyHtml;
                OOFScheduling.Properties.Settings.Default.SecondaryOOFInternal = htmlEditorControl2.BodyHtml;
            }

            OOFScheduling.Properties.Settings.Default.Save();
        }
    }

    #region Old Credential and EWS Singleton class (Remove After CredMan Integration)
    public static class DataProtectionApiWrapper
    {
        /// <summary>
        /// Specifies the data protection scope of the DPAPI.
        /// </summary>
        private const DataProtectionScope Scope = DataProtectionScope.CurrentUser;
        private const string ProgramName = "OOFScheduling";

        public static string Encrypt(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            //encrypt data
            var data = Encoding.Unicode.GetBytes(text);
            byte[] encrypted = ProtectedData.Protect(data, Encoding.Unicode.GetBytes(ProgramName), Scope);

            //return as base64 string
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(this string cipher)
        {
            if (cipher == null)
            {
                throw new ArgumentNullException("cipher");
            }

            //parse base64 string
            byte[] data = Convert.FromBase64String(cipher);

            //decrypt data
            byte[] decrypted = ProtectedData.Unprotect(data, Encoding.Unicode.GetBytes(ProgramName), Scope);
            return Encoding.Unicode.GetString(decrypted);
        }

    }

    public sealed class ExchangeServiceConnection
    {
        private static volatile ExchangeServiceConnection instance;
        private static object syncRoot = new Object();
        public ExchangeService service;

        private ExchangeServiceConnection() {
            service = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
            string emailAddress = Properties.Settings.Default.EmailAddress;
            string pw = Properties.Settings.Default.EncryptPW;
            service.Credentials = new WebCredentials(emailAddress, DataProtectionApiWrapper.Decrypt(pw));
            service.UseDefaultCredentials = false;

            //Let's roll that beautiful bean footage
            service.TraceEnabled = true;
            service.TraceFlags = TraceFlags.All;

            if(service.Url == null)
            {
                service.AutodiscoverUrl(emailAddress, RedirectionUrlValidationCallback);
            }
        }

        public static ExchangeServiceConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (Properties.Settings.Default.EmailAddress != "default" &&
                            Properties.Settings.Default.EncryptPW != "default")
                        {
                            if (instance == null)
                                instance = new ExchangeServiceConnection();
                        }
                    }
                }

                return instance;
            }
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }
    }
    #endregion 
}
