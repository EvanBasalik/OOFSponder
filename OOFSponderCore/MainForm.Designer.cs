namespace OOFScheduling
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            lblInternalMessage = new System.Windows.Forms.Label();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            primaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            secondaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSavedOOFMessage = new System.Windows.Forms.ToolStripMenuItem();
            tsmiExternal = new System.Windows.Forms.ToolStripMenuItem();
            tsmiInternal = new System.Windows.Forms.ToolStripMenuItem();
            tsmiShowOOFMessageFolder = new System.Windows.Forms.ToolStripMenuItem();
            tsmiStartMinimized = new System.Windows.Forms.ToolStripMenuItem();
            enableOnCallModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            oOFSponderLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            oOFSponderLogFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            bETAEnableNewOOFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            lblBuild = new System.Windows.Forms.ToolStripStatusLabel();
            label13 = new System.Windows.Forms.Label();
            dtPermaOOF = new System.Windows.Forms.DateTimePicker();
            btnPermaOOF = new System.Windows.Forms.Button();
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            lblExternalMesage = new System.Windows.Forms.Label();
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            htmlEditorControl2 = new MSDN.Html.Editor.HtmlEditorControl();
            button2 = new System.Windows.Forms.Button();
            htmlEditorControl1 = new MSDN.Html.Editor.HtmlEditorControl();
            saturdayOffWorkCB = new System.Windows.Forms.CheckBox();
            fridayOffWorkCB = new System.Windows.Forms.CheckBox();
            thursdayOffWorkCB = new System.Windows.Forms.CheckBox();
            wednesdayOffWorkCB = new System.Windows.Forms.CheckBox();
            tuesdayOffWorkCB = new System.Windows.Forms.CheckBox();
            mondayOffWorkCB = new System.Windows.Forms.CheckBox();
            sundayOffWorkCB = new System.Windows.Forms.CheckBox();
            saturdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            fridayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            thursdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            wednesdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            tuesdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            mondayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            sundayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            saturdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            fridayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            thursdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            wednesdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            tuesdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            mondayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            sundayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            radPrimary = new System.Windows.Forms.RadioButton();
            radSecondary = new System.Windows.Forms.RadioButton();
            cboExternalAudienceScope = new System.Windows.Forms.ComboBox();
            statusStrip1.SuspendLayout();
            htmlEditorControl2.BeginInit();
            htmlEditorControl1.BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // lblInternalMessage
            // 
            lblInternalMessage.AutoSize = true;
            lblInternalMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblInternalMessage.Location = new System.Drawing.Point(9, 379);
            lblInternalMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblInternalMessage.Name = "lblInternalMessage";
            lblInternalMessage.Size = new System.Drawing.Size(104, 13);
            lblInternalMessage.TabIndex = 41;
            lblInternalMessage.Text = "&Internal Message";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.AccessibleDescription = "A menu item with text 'Message'";
            toolStripMenuItem1.AccessibleName = "Message selection";
            toolStripMenuItem1.CheckOnClick = true;
            toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { primaryToolStripMenuItem, secondaryToolStripMenuItem });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(232, 22);
            toolStripMenuItem1.Text = "&Message";
            // 
            // primaryToolStripMenuItem
            // 
            primaryToolStripMenuItem.AccessibleDescription = "A menu item with text 'Primary'";
            primaryToolStripMenuItem.AccessibleName = "Primary message";
            primaryToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            primaryToolStripMenuItem.Checked = true;
            primaryToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            primaryToolStripMenuItem.Name = "primaryToolStripMenuItem";
            primaryToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            primaryToolStripMenuItem.Text = "&Primary";
            primaryToolStripMenuItem.Click += primaryToolStripMenuItem_Click;
            // 
            // secondaryToolStripMenuItem
            // 
            secondaryToolStripMenuItem.AccessibleDescription = "A menu item with text 'Secondary'";
            secondaryToolStripMenuItem.AccessibleName = "Secondary message";
            secondaryToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            secondaryToolStripMenuItem.Name = "secondaryToolStripMenuItem";
            secondaryToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            secondaryToolStripMenuItem.Text = "&Secondary";
            secondaryToolStripMenuItem.Click += secondaryToolStripMenuItem_Click;
            // 
            // tsmiSavedOOFMessage
            // 
            tsmiSavedOOFMessage.AccessibleDescription = "A menu item that allows you to open a previously saved OOF message";
            tsmiSavedOOFMessage.AccessibleName = "Open saved OOF message";
            tsmiSavedOOFMessage.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            tsmiSavedOOFMessage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiExternal, tsmiInternal, tsmiShowOOFMessageFolder });
            tsmiSavedOOFMessage.Name = "tsmiSavedOOFMessage";
            tsmiSavedOOFMessage.Size = new System.Drawing.Size(232, 22);
            tsmiSavedOOFMessage.Text = "&Open saved OOF message...";
            // 
            // tsmiExternal
            // 
            tsmiExternal.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            tsmiExternal.Name = "tsmiExternal";
            tsmiExternal.Size = new System.Drawing.Size(222, 22);
            tsmiExternal.Tag = "External";
            tsmiExternal.Text = "External...";
            tsmiExternal.Click += tsmiSavedOOFMessage_Click;
            // 
            // tsmiInternal
            // 
            tsmiInternal.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            tsmiInternal.Name = "tsmiInternal";
            tsmiInternal.Size = new System.Drawing.Size(222, 22);
            tsmiInternal.Tag = "Internal";
            tsmiInternal.Text = "Internal...";
            tsmiInternal.Click += tsmiSavedOOFMessage_Click;
            // 
            // tsmiShowOOFMessageFolder
            // 
            tsmiShowOOFMessageFolder.AccessibleDescription = "A menu item that opens the AppData folder where OOFSponder saves OOF messages";
            tsmiShowOOFMessageFolder.AccessibleName = "Show OOF message folder";
            tsmiShowOOFMessageFolder.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            tsmiShowOOFMessageFolder.Name = "tsmiShowOOFMessageFolder";
            tsmiShowOOFMessageFolder.Size = new System.Drawing.Size(222, 22);
            tsmiShowOOFMessageFolder.Text = "Show OOF message folder...";
            tsmiShowOOFMessageFolder.Click += tsmiShowOOFMessageFolder_Click;
            // 
            // tsmiStartMinimized
            // 
            tsmiStartMinimized.AccessibleDescription = "A menu item that causes the app to start minimized when enabled";
            tsmiStartMinimized.AccessibleName = "Start minimized";
            tsmiStartMinimized.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            tsmiStartMinimized.CheckOnClick = true;
            tsmiStartMinimized.Name = "tsmiStartMinimized";
            tsmiStartMinimized.Size = new System.Drawing.Size(232, 22);
            tsmiStartMinimized.Text = "&Start minimized";
            // 
            // enableOnCallModeToolStripMenuItem
            // 
            enableOnCallModeToolStripMenuItem.AccessibleDescription = "A menu item with text '(BETA) Enable On-Call Mode'";
            enableOnCallModeToolStripMenuItem.AccessibleName = "(BETA) Enable On-Call Mode";
            enableOnCallModeToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            enableOnCallModeToolStripMenuItem.Name = "enableOnCallModeToolStripMenuItem";
            enableOnCallModeToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            enableOnCallModeToolStripMenuItem.Text = "(BETA) Enable On-Call Mode";
            enableOnCallModeToolStripMenuItem.Visible = false;
            enableOnCallModeToolStripMenuItem.Click += enableOnCallModeToolStripMenuItem_Click;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.AccessibleDescription = "File";
            fileToolStripMenuItem.AccessibleName = "File";
            fileToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1, tsmiSavedOOFMessage, tsmiStartMinimized, enableOnCallModeToolStripMenuItem, showLogsToolStripMenuItem, exitToolStripMenuItem, bETAEnableNewOOFToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(46, 22);
            fileToolStripMenuItem.Text = "&File...";
            fileToolStripMenuItem.MouseEnter += fileToolStripMenuItem_MouseEnter;
            fileToolStripMenuItem.MouseLeave += fileToolStripMenuItem_MouseLeave;
            // 
            // showLogsToolStripMenuItem
            // 
            showLogsToolStripMenuItem.AccessibleDescription = "A menu item with text 'Show logs'";
            showLogsToolStripMenuItem.AccessibleName = "Show logs";
            showLogsToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            showLogsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { oOFSponderLogToolStripMenuItem, oOFSponderLogFolderToolStripMenuItem });
            showLogsToolStripMenuItem.Name = "showLogsToolStripMenuItem";
            showLogsToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            showLogsToolStripMenuItem.Text = "Show &logs...";
            // 
            // oOFSponderLogToolStripMenuItem
            // 
            oOFSponderLogToolStripMenuItem.AccessibleDescription = "";
            oOFSponderLogToolStripMenuItem.AccessibleName = "Show OOFSponder log";
            oOFSponderLogToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            oOFSponderLogToolStripMenuItem.Name = "oOFSponderLogToolStripMenuItem";
            oOFSponderLogToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            oOFSponderLogToolStripMenuItem.Tag = "File";
            oOFSponderLogToolStripMenuItem.Text = "OOFSponder &log";
            oOFSponderLogToolStripMenuItem.Click += ShowLogs;
            // 
            // oOFSponderLogFolderToolStripMenuItem
            // 
            oOFSponderLogFolderToolStripMenuItem.AccessibleDescription = "";
            oOFSponderLogFolderToolStripMenuItem.AccessibleName = "OOF Sponder folder";
            oOFSponderLogFolderToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            oOFSponderLogFolderToolStripMenuItem.Name = "oOFSponderLogFolderToolStripMenuItem";
            oOFSponderLogFolderToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            oOFSponderLogFolderToolStripMenuItem.Tag = "Folder";
            oOFSponderLogFolderToolStripMenuItem.Text = "OOFSponder log folder";
            oOFSponderLogFolderToolStripMenuItem.Click += ShowLogs;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // bETAEnableNewOOFToolStripMenuItem
            // 
            bETAEnableNewOOFToolStripMenuItem.AccessibleName = "(BETA) Enable new OOF math";
            bETAEnableNewOOFToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            bETAEnableNewOOFToolStripMenuItem.Name = "bETAEnableNewOOFToolStripMenuItem";
            bETAEnableNewOOFToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            bETAEnableNewOOFToolStripMenuItem.Text = "(BETA) Enable New OOF Math";
            bETAEnableNewOOFToolStripMenuItem.Visible = false;
            bETAEnableNewOOFToolStripMenuItem.Click += bETAEnableNewOOFToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1, toolStripStatusLabel2, lblBuild });
            statusStrip1.Location = new System.Drawing.Point(0, 662);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 9, 0);
            statusStrip1.Size = new System.Drawing.Size(794, 24);
            statusStrip1.TabIndex = 45;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(118, 19);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(122, 19);
            toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // lblBuild
            // 
            lblBuild.Name = "lblBuild";
            lblBuild.Size = new System.Drawing.Size(78, 19);
            lblBuild.Text = "BuildNumber";
            // 
            // label13
            // 
            label13.AccessibleDescription = "A label saying 'Enter your Working Hours:'";
            label13.AccessibleName = "Enter your Working Hours";
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label13.Location = new System.Drawing.Point(14, -52);
            label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(157, 13);
            label13.TabIndex = 43;
            label13.Text = "Enter your Working Hours:";
            // 
            // dtPermaOOF
            // 
            dtPermaOOF.AccessibleDescription = "DateTime picker that specifies when PermaOOF should end";
            dtPermaOOF.AccessibleName = "Perma OOF date";
            dtPermaOOF.AccessibleRole = System.Windows.Forms.AccessibleRole.Clock;
            dtPermaOOF.Enabled = false;
            dtPermaOOF.Location = new System.Drawing.Point(559, 606);
            dtPermaOOF.Margin = new System.Windows.Forms.Padding(2);
            dtPermaOOF.Name = "dtPermaOOF";
            dtPermaOOF.Size = new System.Drawing.Size(215, 23);
            dtPermaOOF.TabIndex = 55;
            // 
            // btnPermaOOF
            // 
            btnPermaOOF.Location = new System.Drawing.Point(403, 604);
            btnPermaOOF.Margin = new System.Windows.Forms.Padding(2);
            btnPermaOOF.Name = "btnPermaOOF";
            btnPermaOOF.Size = new System.Drawing.Size(152, 26);
            btnPermaOOF.TabIndex = 54;
            btnPermaOOF.Tag = "Enable";
            btnPermaOOF.Text = "Go OOF now until:";
            btnPermaOOF.UseVisualStyleBackColor = true;
            btnPermaOOF.Click += btnPermaOOF_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // lblExternalMesage
            // 
            lblExternalMesage.AutoSize = true;
            lblExternalMesage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblExternalMesage.Location = new System.Drawing.Point(9, 152);
            lblExternalMesage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblExternalMesage.Name = "lblExternalMesage";
            lblExternalMesage.Size = new System.Drawing.Size(107, 13);
            lblExternalMesage.TabIndex = 40;
            lblExternalMesage.Text = "External Message";
            // 
            // notifyIcon1
            // 
            notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            notifyIcon1.BalloonTipText = "OOFScheduling will make sure your OOF message is set for you in the background!";
            notifyIcon1.BalloonTipTitle = "OOFScheduling is still working";
            notifyIcon1.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "OOFScheduling";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // htmlEditorControl2
            // 
            htmlEditorControl2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            htmlEditorControl2.InnerText = null;
            htmlEditorControl2.Location = new System.Drawing.Point(9, 396);
            htmlEditorControl2.Margin = new System.Windows.Forms.Padding(4);
            htmlEditorControl2.Name = "htmlEditorControl2";
            htmlEditorControl2.Size = new System.Drawing.Size(776, 200);
            htmlEditorControl2.TabIndex = 32;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(38, 604);
            button2.Margin = new System.Windows.Forms.Padding(4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(133, 26);
            button2.TabIndex = 38;
            button2.Text = "S&ave Settings";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // htmlEditorControl1
            // 
            htmlEditorControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            htmlEditorControl1.InnerText = null;
            htmlEditorControl1.Location = new System.Drawing.Point(9, 169);
            htmlEditorControl1.Margin = new System.Windows.Forms.Padding(4);
            htmlEditorControl1.Name = "htmlEditorControl1";
            htmlEditorControl1.Size = new System.Drawing.Size(776, 200);
            htmlEditorControl1.TabIndex = 31;
            // 
            // saturdayOffWorkCB
            // 
            saturdayOffWorkCB.AutoSize = true;
            saturdayOffWorkCB.Location = new System.Drawing.Point(696, 111);
            saturdayOffWorkCB.Margin = new System.Windows.Forms.Padding(4);
            saturdayOffWorkCB.Name = "saturdayOffWorkCB";
            saturdayOffWorkCB.Size = new System.Drawing.Size(74, 19);
            saturdayOffWorkCB.TabIndex = 27;
            saturdayOffWorkCB.Text = "Off Work";
            saturdayOffWorkCB.UseVisualStyleBackColor = true;
            // 
            // fridayOffWorkCB
            // 
            fridayOffWorkCB.AutoSize = true;
            fridayOffWorkCB.Location = new System.Drawing.Point(584, 111);
            fridayOffWorkCB.Margin = new System.Windows.Forms.Padding(4);
            fridayOffWorkCB.Name = "fridayOffWorkCB";
            fridayOffWorkCB.Size = new System.Drawing.Size(74, 19);
            fridayOffWorkCB.TabIndex = 24;
            fridayOffWorkCB.Text = "Off Work";
            fridayOffWorkCB.UseVisualStyleBackColor = true;
            // 
            // thursdayOffWorkCB
            // 
            thursdayOffWorkCB.AutoSize = true;
            thursdayOffWorkCB.Location = new System.Drawing.Point(472, 111);
            thursdayOffWorkCB.Margin = new System.Windows.Forms.Padding(4);
            thursdayOffWorkCB.Name = "thursdayOffWorkCB";
            thursdayOffWorkCB.Size = new System.Drawing.Size(74, 19);
            thursdayOffWorkCB.TabIndex = 21;
            thursdayOffWorkCB.Text = "Off Work";
            thursdayOffWorkCB.UseVisualStyleBackColor = true;
            // 
            // wednesdayOffWorkCB
            // 
            wednesdayOffWorkCB.AutoSize = true;
            wednesdayOffWorkCB.Location = new System.Drawing.Point(360, 111);
            wednesdayOffWorkCB.Margin = new System.Windows.Forms.Padding(4);
            wednesdayOffWorkCB.Name = "wednesdayOffWorkCB";
            wednesdayOffWorkCB.Size = new System.Drawing.Size(74, 19);
            wednesdayOffWorkCB.TabIndex = 18;
            wednesdayOffWorkCB.Text = "Off Work";
            wednesdayOffWorkCB.UseVisualStyleBackColor = true;
            // 
            // tuesdayOffWorkCB
            // 
            tuesdayOffWorkCB.AutoSize = true;
            tuesdayOffWorkCB.Location = new System.Drawing.Point(248, 111);
            tuesdayOffWorkCB.Margin = new System.Windows.Forms.Padding(4);
            tuesdayOffWorkCB.Name = "tuesdayOffWorkCB";
            tuesdayOffWorkCB.Size = new System.Drawing.Size(74, 19);
            tuesdayOffWorkCB.TabIndex = 15;
            tuesdayOffWorkCB.Text = "Off Work";
            tuesdayOffWorkCB.UseVisualStyleBackColor = true;
            // 
            // mondayOffWorkCB
            // 
            mondayOffWorkCB.AutoSize = true;
            mondayOffWorkCB.Location = new System.Drawing.Point(136, 111);
            mondayOffWorkCB.Margin = new System.Windows.Forms.Padding(4);
            mondayOffWorkCB.Name = "mondayOffWorkCB";
            mondayOffWorkCB.Size = new System.Drawing.Size(74, 19);
            mondayOffWorkCB.TabIndex = 12;
            mondayOffWorkCB.Text = "Off Work";
            mondayOffWorkCB.UseVisualStyleBackColor = true;
            // 
            // sundayOffWorkCB
            // 
            sundayOffWorkCB.AutoSize = true;
            sundayOffWorkCB.Location = new System.Drawing.Point(24, 111);
            sundayOffWorkCB.Margin = new System.Windows.Forms.Padding(4);
            sundayOffWorkCB.Name = "sundayOffWorkCB";
            sundayOffWorkCB.Size = new System.Drawing.Size(74, 19);
            sundayOffWorkCB.TabIndex = 9;
            sundayOffWorkCB.Text = "Off Work";
            sundayOffWorkCB.UseVisualStyleBackColor = true;
            // 
            // saturdayEndTimepicker
            // 
            saturdayEndTimepicker.AccessibleName = "Enter your working hours end time for Saturday";
            saturdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            saturdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            saturdayEndTimepicker.Location = new System.Drawing.Point(681, 81);
            saturdayEndTimepicker.Margin = new System.Windows.Forms.Padding(4);
            saturdayEndTimepicker.Name = "saturdayEndTimepicker";
            saturdayEndTimepicker.ShowUpDown = true;
            saturdayEndTimepicker.Size = new System.Drawing.Size(104, 23);
            saturdayEndTimepicker.TabIndex = 26;
            saturdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // fridayEndTimepicker
            // 
            fridayEndTimepicker.AccessibleName = "Enter your working hours end time for Friday";
            fridayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            fridayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            fridayEndTimepicker.Location = new System.Drawing.Point(569, 81);
            fridayEndTimepicker.Margin = new System.Windows.Forms.Padding(4);
            fridayEndTimepicker.Name = "fridayEndTimepicker";
            fridayEndTimepicker.ShowUpDown = true;
            fridayEndTimepicker.Size = new System.Drawing.Size(104, 23);
            fridayEndTimepicker.TabIndex = 23;
            fridayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // thursdayEndTimepicker
            // 
            thursdayEndTimepicker.AccessibleName = "Enter your working hours end time for Thursday";
            thursdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            thursdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            thursdayEndTimepicker.Location = new System.Drawing.Point(457, 81);
            thursdayEndTimepicker.Margin = new System.Windows.Forms.Padding(4);
            thursdayEndTimepicker.Name = "thursdayEndTimepicker";
            thursdayEndTimepicker.ShowUpDown = true;
            thursdayEndTimepicker.Size = new System.Drawing.Size(104, 23);
            thursdayEndTimepicker.TabIndex = 20;
            thursdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // wednesdayEndTimepicker
            // 
            wednesdayEndTimepicker.AccessibleName = "Enter your working hours end time for Wednesday";
            wednesdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            wednesdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            wednesdayEndTimepicker.Location = new System.Drawing.Point(345, 81);
            wednesdayEndTimepicker.Margin = new System.Windows.Forms.Padding(4);
            wednesdayEndTimepicker.Name = "wednesdayEndTimepicker";
            wednesdayEndTimepicker.ShowUpDown = true;
            wednesdayEndTimepicker.Size = new System.Drawing.Size(104, 23);
            wednesdayEndTimepicker.TabIndex = 17;
            wednesdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // tuesdayEndTimepicker
            // 
            tuesdayEndTimepicker.AccessibleName = "Enter your working hours end time for Tuesday";
            tuesdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            tuesdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            tuesdayEndTimepicker.Location = new System.Drawing.Point(233, 81);
            tuesdayEndTimepicker.Margin = new System.Windows.Forms.Padding(4);
            tuesdayEndTimepicker.Name = "tuesdayEndTimepicker";
            tuesdayEndTimepicker.ShowUpDown = true;
            tuesdayEndTimepicker.Size = new System.Drawing.Size(104, 23);
            tuesdayEndTimepicker.TabIndex = 14;
            tuesdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // mondayEndTimepicker
            // 
            mondayEndTimepicker.AccessibleName = "Enter your working hours end time for Monday";
            mondayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            mondayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            mondayEndTimepicker.Location = new System.Drawing.Point(121, 81);
            mondayEndTimepicker.Margin = new System.Windows.Forms.Padding(4);
            mondayEndTimepicker.Name = "mondayEndTimepicker";
            mondayEndTimepicker.ShowUpDown = true;
            mondayEndTimepicker.Size = new System.Drawing.Size(104, 23);
            mondayEndTimepicker.TabIndex = 11;
            mondayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // sundayEndTimepicker
            // 
            sundayEndTimepicker.AccessibleName = "Enter your working hours end time for Sunday";
            sundayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            sundayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            sundayEndTimepicker.Location = new System.Drawing.Point(9, 81);
            sundayEndTimepicker.Margin = new System.Windows.Forms.Padding(4);
            sundayEndTimepicker.Name = "sundayEndTimepicker";
            sundayEndTimepicker.ShowUpDown = true;
            sundayEndTimepicker.Size = new System.Drawing.Size(104, 23);
            sundayEndTimepicker.TabIndex = 8;
            sundayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // saturdayStartTimepicker
            // 
            saturdayStartTimepicker.AccessibleName = "Enter your working hours start time for Saturday";
            saturdayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            saturdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            saturdayStartTimepicker.Location = new System.Drawing.Point(681, 51);
            saturdayStartTimepicker.Margin = new System.Windows.Forms.Padding(4);
            saturdayStartTimepicker.Name = "saturdayStartTimepicker";
            saturdayStartTimepicker.ShowUpDown = true;
            saturdayStartTimepicker.Size = new System.Drawing.Size(104, 23);
            saturdayStartTimepicker.TabIndex = 25;
            saturdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // fridayStartTimepicker
            // 
            fridayStartTimepicker.AccessibleName = "Enter your working hours start time for Friday";
            fridayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            fridayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            fridayStartTimepicker.Location = new System.Drawing.Point(569, 51);
            fridayStartTimepicker.Margin = new System.Windows.Forms.Padding(4);
            fridayStartTimepicker.Name = "fridayStartTimepicker";
            fridayStartTimepicker.ShowUpDown = true;
            fridayStartTimepicker.Size = new System.Drawing.Size(104, 23);
            fridayStartTimepicker.TabIndex = 22;
            fridayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // thursdayStartTimepicker
            // 
            thursdayStartTimepicker.AccessibleName = "Enter your working hours start time for Thursday";
            thursdayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            thursdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            thursdayStartTimepicker.Location = new System.Drawing.Point(457, 51);
            thursdayStartTimepicker.Margin = new System.Windows.Forms.Padding(4);
            thursdayStartTimepicker.Name = "thursdayStartTimepicker";
            thursdayStartTimepicker.ShowUpDown = true;
            thursdayStartTimepicker.Size = new System.Drawing.Size(104, 23);
            thursdayStartTimepicker.TabIndex = 19;
            thursdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // wednesdayStartTimepicker
            // 
            wednesdayStartTimepicker.AccessibleName = "Enter your working hours start time for Wednesday";
            wednesdayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            wednesdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            wednesdayStartTimepicker.Location = new System.Drawing.Point(345, 51);
            wednesdayStartTimepicker.Margin = new System.Windows.Forms.Padding(4);
            wednesdayStartTimepicker.Name = "wednesdayStartTimepicker";
            wednesdayStartTimepicker.ShowUpDown = true;
            wednesdayStartTimepicker.Size = new System.Drawing.Size(104, 23);
            wednesdayStartTimepicker.TabIndex = 16;
            wednesdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // tuesdayStartTimepicker
            // 
            tuesdayStartTimepicker.AccessibleName = "Enter your working hours start time for Tuesday";
            tuesdayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            tuesdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            tuesdayStartTimepicker.Location = new System.Drawing.Point(233, 51);
            tuesdayStartTimepicker.Margin = new System.Windows.Forms.Padding(4);
            tuesdayStartTimepicker.Name = "tuesdayStartTimepicker";
            tuesdayStartTimepicker.ShowUpDown = true;
            tuesdayStartTimepicker.Size = new System.Drawing.Size(104, 23);
            tuesdayStartTimepicker.TabIndex = 13;
            tuesdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // mondayStartTimepicker
            // 
            mondayStartTimepicker.AccessibleName = "Enter your working hours start time for Monday";
            mondayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            mondayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            mondayStartTimepicker.Location = new System.Drawing.Point(121, 51);
            mondayStartTimepicker.Margin = new System.Windows.Forms.Padding(4);
            mondayStartTimepicker.Name = "mondayStartTimepicker";
            mondayStartTimepicker.ShowUpDown = true;
            mondayStartTimepicker.Size = new System.Drawing.Size(104, 23);
            mondayStartTimepicker.TabIndex = 10;
            mondayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // sundayStartTimepicker
            // 
            sundayStartTimepicker.AccessibleName = "Enter your working hours start time for Sunday";
            sundayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            sundayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            sundayStartTimepicker.Location = new System.Drawing.Point(9, 51);
            sundayStartTimepicker.Margin = new System.Windows.Forms.Padding(4);
            sundayStartTimepicker.Name = "sundayStartTimepicker";
            sundayStartTimepicker.ShowUpDown = true;
            sundayStartTimepicker.Size = new System.Drawing.Size(104, 23);
            sundayStartTimepicker.TabIndex = 7;
            sundayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // label7
            // 
            label7.AccessibleDescription = "A label with the text Saturday";
            label7.AccessibleName = "Saturday working hours";
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(707, 32);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(53, 15);
            label7.TabIndex = 6;
            label7.Text = "Saturday";
            // 
            // label6
            // 
            label6.AccessibleDescription = "A label with the text Friday";
            label6.AccessibleName = "Friday working hours";
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(602, 32);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(39, 15);
            label6.TabIndex = 5;
            label6.Text = "Friday";
            // 
            // label5
            // 
            label5.AccessibleDescription = "A label with the text Thursday";
            label5.AccessibleName = "Thursday working hours";
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(481, 32);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(56, 15);
            label5.TabIndex = 4;
            label5.Text = "Thursday";
            // 
            // label4
            // 
            label4.AccessibleDescription = "A label with the text Wednesday";
            label4.AccessibleName = "Wednesday working hours";
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(363, 32);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(68, 15);
            label4.TabIndex = 3;
            label4.Text = "Wednesday";
            // 
            // label3
            // 
            label3.AccessibleDescription = "A label with the text Tuesday";
            label3.AccessibleName = "Tuesday working hours";
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(260, 32);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(51, 15);
            label3.TabIndex = 2;
            label3.Text = "Tuesday";
            // 
            // label2
            // 
            label2.AccessibleDescription = "A label with the text Monday";
            label2.AccessibleName = "Monday working hours";
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(148, 32);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(51, 15);
            label2.TabIndex = 1;
            label2.Text = "Monday";
            // 
            // label1
            // 
            label1.AccessibleDescription = "A label with the text Sunday";
            label1.AccessibleName = "Sunday working hours";
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(38, 32);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(46, 15);
            label1.TabIndex = 0;
            label1.Text = "Sunday";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            menuStrip1.Size = new System.Drawing.Size(794, 24);
            menuStrip1.TabIndex = 42;
            menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(53, 22);
            aboutToolStripMenuItem.Text = "&Help...";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // radPrimary
            // 
            radPrimary.AutoSize = true;
            radPrimary.Checked = true;
            radPrimary.Location = new System.Drawing.Point(178, 608);
            radPrimary.Margin = new System.Windows.Forms.Padding(4);
            radPrimary.Name = "radPrimary";
            radPrimary.Size = new System.Drawing.Size(93, 19);
            radPrimary.TabIndex = 56;
            radPrimary.TabStop = true;
            radPrimary.Text = "Primary OOF";
            radPrimary.UseVisualStyleBackColor = true;
            // 
            // radSecondary
            // 
            radSecondary.AutoSize = true;
            radSecondary.Location = new System.Drawing.Point(276, 608);
            radSecondary.Margin = new System.Windows.Forms.Padding(4);
            radSecondary.Name = "radSecondary";
            radSecondary.Size = new System.Drawing.Size(107, 19);
            radSecondary.TabIndex = 57;
            radSecondary.TabStop = true;
            radSecondary.Text = "Secondary OOF";
            radSecondary.UseVisualStyleBackColor = true;
            // 
            // cboExternalAudienceScope
            // 
            cboExternalAudienceScope.FormattingEnabled = true;
            cboExternalAudienceScope.Items.AddRange(new object[] { "All", "Contacts Only", "None" });
            cboExternalAudienceScope.Location = new System.Drawing.Point(629, 149);
            cboExternalAudienceScope.Name = "cboExternalAudienceScope";
            cboExternalAudienceScope.Size = new System.Drawing.Size(121, 23);
            cboExternalAudienceScope.TabIndex = 59;
            cboExternalAudienceScope.SelectedIndexChanged += cboExternalAudienceScope_SelectedIndexChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new System.Drawing.Size(794, 686);
            Controls.Add(cboExternalAudienceScope);
            Controls.Add(lblInternalMessage);
            Controls.Add(statusStrip1);
            Controls.Add(label13);
            Controls.Add(dtPermaOOF);
            Controls.Add(btnPermaOOF);
            Controls.Add(lblExternalMesage);
            Controls.Add(htmlEditorControl2);
            Controls.Add(button2);
            Controls.Add(saturdayOffWorkCB);
            Controls.Add(fridayOffWorkCB);
            Controls.Add(thursdayOffWorkCB);
            Controls.Add(wednesdayOffWorkCB);
            Controls.Add(tuesdayOffWorkCB);
            Controls.Add(mondayOffWorkCB);
            Controls.Add(sundayOffWorkCB);
            Controls.Add(saturdayEndTimepicker);
            Controls.Add(fridayEndTimepicker);
            Controls.Add(thursdayEndTimepicker);
            Controls.Add(wednesdayEndTimepicker);
            Controls.Add(tuesdayEndTimepicker);
            Controls.Add(mondayEndTimepicker);
            Controls.Add(sundayEndTimepicker);
            Controls.Add(saturdayStartTimepicker);
            Controls.Add(fridayStartTimepicker);
            Controls.Add(thursdayStartTimepicker);
            Controls.Add(wednesdayStartTimepicker);
            Controls.Add(tuesdayStartTimepicker);
            Controls.Add(mondayStartTimepicker);
            Controls.Add(sundayStartTimepicker);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            Controls.Add(radPrimary);
            Controls.Add(radSecondary);
            Controls.Add(htmlEditorControl1);
            MaximumSize = new System.Drawing.Size(1300, 725);
            Name = "MainForm";
            Text = "OOFSponder";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ResizeEnd += MainForm_ResizeEnd;
            Resize += MainForm_Resize;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            htmlEditorControl2.EndInit();
            htmlEditorControl1.EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblInternalMessage;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem primaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiSavedOOFMessage;
        private System.Windows.Forms.ToolStripMenuItem tsmiExternal;
        private System.Windows.Forms.ToolStripMenuItem tsmiInternal;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowOOFMessageFolder;
        private System.Windows.Forms.ToolStripMenuItem tsmiStartMinimized;
        private System.Windows.Forms.ToolStripMenuItem enableOnCallModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oOFSponderLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oOFSponderLogFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bETAEnableNewOOFToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblBuild;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker dtPermaOOF;
        private System.Windows.Forms.Button btnPermaOOF;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label lblExternalMesage;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private MSDN.Html.Editor.HtmlEditorControl htmlEditorControl2;
        private System.Windows.Forms.Button button2;
        private MSDN.Html.Editor.HtmlEditorControl htmlEditorControl1;
        private System.Windows.Forms.CheckBox saturdayOffWorkCB;
        private System.Windows.Forms.CheckBox fridayOffWorkCB;
        private System.Windows.Forms.CheckBox thursdayOffWorkCB;
        private System.Windows.Forms.CheckBox wednesdayOffWorkCB;
        private System.Windows.Forms.CheckBox tuesdayOffWorkCB;
        private System.Windows.Forms.CheckBox mondayOffWorkCB;
        private System.Windows.Forms.CheckBox sundayOffWorkCB;
        private System.Windows.Forms.DateTimePicker saturdayEndTimepicker;
        private System.Windows.Forms.DateTimePicker fridayEndTimepicker;
        private System.Windows.Forms.DateTimePicker thursdayEndTimepicker;
        private System.Windows.Forms.DateTimePicker wednesdayEndTimepicker;
        private System.Windows.Forms.DateTimePicker tuesdayEndTimepicker;
        private System.Windows.Forms.DateTimePicker mondayEndTimepicker;
        private System.Windows.Forms.DateTimePicker sundayEndTimepicker;
        private System.Windows.Forms.DateTimePicker saturdayStartTimepicker;
        private System.Windows.Forms.DateTimePicker fridayStartTimepicker;
        private System.Windows.Forms.DateTimePicker thursdayStartTimepicker;
        private System.Windows.Forms.DateTimePicker wednesdayStartTimepicker;
        private System.Windows.Forms.DateTimePicker tuesdayStartTimepicker;
        private System.Windows.Forms.DateTimePicker mondayStartTimepicker;
        private System.Windows.Forms.DateTimePicker sundayStartTimepicker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.RadioButton radPrimary;
        private System.Windows.Forms.RadioButton radSecondary;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboExternalAudienceScope;
    }
}