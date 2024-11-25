using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace OOFScheduling
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            sundayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            mondayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            tuesdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            wednesdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            thursdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            fridayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            saturdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            saturdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            fridayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            thursdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            wednesdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            tuesdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            mondayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            sundayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            sundayOffWorkCB = new System.Windows.Forms.CheckBox();
            mondayOffWorkCB = new System.Windows.Forms.CheckBox();
            tuesdayOffWorkCB = new System.Windows.Forms.CheckBox();
            wednesdayOffWorkCB = new System.Windows.Forms.CheckBox();
            thursdayOffWorkCB = new System.Windows.Forms.CheckBox();
            fridayOffWorkCB = new System.Windows.Forms.CheckBox();
            saturdayOffWorkCB = new System.Windows.Forms.CheckBox();
            button2 = new System.Windows.Forms.Button();
            lblExternalMesage = new System.Windows.Forms.Label();
            lblInternalMessage = new System.Windows.Forms.Label();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            primaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            secondaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            signoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            enableOnCallModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            bETAEnableNewOOFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            label13 = new System.Windows.Forms.Label();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            lblBuild = new System.Windows.Forms.ToolStripStatusLabel();
            passwordConfirmTB = new System.Windows.Forms.MaskedTextBox();
            label10 = new System.Windows.Forms.Label();
            passwordTB = new System.Windows.Forms.MaskedTextBox();
            label9 = new System.Windows.Forms.Label();
            emailAddressTB = new System.Windows.Forms.TextBox();
            label8 = new System.Windows.Forms.Label();
            btnPermaOOF = new System.Windows.Forms.Button();
            dtPermaOOF = new System.Windows.Forms.DateTimePicker();
            radPrimary = new System.Windows.Forms.RadioButton();
            radSecondary = new System.Windows.Forms.RadioButton();
            htmlEditorControl2 = new MSDN.Html.Editor.HtmlEditorControl();
            htmlEditorControl1 = new MSDN.Html.Editor.HtmlEditorControl();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
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
            // label1
            // 
            label1.AccessibleDescription = "A label with the text Sunday";
            label1.AccessibleName = "Sunday working hours";
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(24, 90);
            label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(81, 30);
            label1.TabIndex = 0;
            label1.Text = "Sunday";
            // 
            // label2
            // 
            label2.AccessibleDescription = "A label with the text Monday";
            label2.AccessibleName = "Monday working hours";
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(216, 90);
            label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(89, 30);
            label2.TabIndex = 1;
            label2.Text = "Monday";
            // 
            // label3
            // 
            label3.AccessibleDescription = "A label with the text Tuesday";
            label3.AccessibleName = "Tuesday working hours";
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(408, 90);
            label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(89, 30);
            label3.TabIndex = 2;
            label3.Text = "Tuesday";
            // 
            // label4
            // 
            label4.AccessibleDescription = "A label with the text Wednesday";
            label4.AccessibleName = "Wednesday working hours";
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(600, 90);
            label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(120, 30);
            label4.TabIndex = 3;
            label4.Text = "Wednesday";
            // 
            // label5
            // 
            label5.AccessibleDescription = "A label with the text Thursday";
            label5.AccessibleName = "Thursday working hours";
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(792, 90);
            label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(97, 30);
            label5.TabIndex = 4;
            label5.Text = "Thursday";
            // 
            // label6
            // 
            label6.AccessibleDescription = "A label with the text Friday";
            label6.AccessibleName = "Friday working hours";
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(984, 90);
            label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(68, 30);
            label6.TabIndex = 5;
            label6.Text = "Friday";
            // 
            // label7
            // 
            label7.AccessibleDescription = "A label with the text Saturday";
            label7.AccessibleName = "Saturday working hours";
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(1176, 90);
            label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(94, 30);
            label7.TabIndex = 6;
            label7.Text = "Saturday";
            // 
            // sundayStartTimepicker
            // 
            sundayStartTimepicker.AccessibleName = "Enter your working hours start time for Sunday";
            sundayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            sundayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            sundayStartTimepicker.Location = new System.Drawing.Point(24, 127);
            sundayStartTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            sundayStartTimepicker.Name = "sundayStartTimepicker";
            sundayStartTimepicker.ShowUpDown = true;
            sundayStartTimepicker.Size = new System.Drawing.Size(176, 35);
            sundayStartTimepicker.TabIndex = 7;
            sundayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // mondayStartTimepicker
            // 
            mondayStartTimepicker.AccessibleName = "Enter your working hours start time for Monday";
            mondayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            mondayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            mondayStartTimepicker.Location = new System.Drawing.Point(216, 127);
            mondayStartTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            mondayStartTimepicker.Name = "mondayStartTimepicker";
            mondayStartTimepicker.ShowUpDown = true;
            mondayStartTimepicker.Size = new System.Drawing.Size(176, 35);
            mondayStartTimepicker.TabIndex = 10;
            mondayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // tuesdayStartTimepicker
            // 
            tuesdayStartTimepicker.AccessibleName = "Enter your working hours start time for Tuesday";
            tuesdayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            tuesdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            tuesdayStartTimepicker.Location = new System.Drawing.Point(408, 127);
            tuesdayStartTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            tuesdayStartTimepicker.Name = "tuesdayStartTimepicker";
            tuesdayStartTimepicker.ShowUpDown = true;
            tuesdayStartTimepicker.Size = new System.Drawing.Size(176, 35);
            tuesdayStartTimepicker.TabIndex = 13;
            tuesdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // wednesdayStartTimepicker
            // 
            wednesdayStartTimepicker.AccessibleName = "Enter your working hours start time for Wednesday";
            wednesdayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            wednesdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            wednesdayStartTimepicker.Location = new System.Drawing.Point(600, 127);
            wednesdayStartTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            wednesdayStartTimepicker.Name = "wednesdayStartTimepicker";
            wednesdayStartTimepicker.ShowUpDown = true;
            wednesdayStartTimepicker.Size = new System.Drawing.Size(176, 35);
            wednesdayStartTimepicker.TabIndex = 16;
            wednesdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // thursdayStartTimepicker
            // 
            thursdayStartTimepicker.AccessibleName = "Enter your working hours start time for Thursday";
            thursdayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            thursdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            thursdayStartTimepicker.Location = new System.Drawing.Point(792, 127);
            thursdayStartTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            thursdayStartTimepicker.Name = "thursdayStartTimepicker";
            thursdayStartTimepicker.ShowUpDown = true;
            thursdayStartTimepicker.Size = new System.Drawing.Size(176, 35);
            thursdayStartTimepicker.TabIndex = 19;
            thursdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // fridayStartTimepicker
            // 
            fridayStartTimepicker.AccessibleName = "Enter your working hours start time for Friday";
            fridayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            fridayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            fridayStartTimepicker.Location = new System.Drawing.Point(984, 127);
            fridayStartTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            fridayStartTimepicker.Name = "fridayStartTimepicker";
            fridayStartTimepicker.ShowUpDown = true;
            fridayStartTimepicker.Size = new System.Drawing.Size(176, 35);
            fridayStartTimepicker.TabIndex = 22;
            fridayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // saturdayStartTimepicker
            // 
            saturdayStartTimepicker.AccessibleName = "Enter your working hours start time for Saturday";
            saturdayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            saturdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            saturdayStartTimepicker.Location = new System.Drawing.Point(1176, 127);
            saturdayStartTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            saturdayStartTimepicker.Name = "saturdayStartTimepicker";
            saturdayStartTimepicker.ShowUpDown = true;
            saturdayStartTimepicker.Size = new System.Drawing.Size(176, 35);
            saturdayStartTimepicker.TabIndex = 25;
            saturdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // saturdayEndTimepicker
            // 
            saturdayEndTimepicker.AccessibleName = "Enter your working hours end time for Saturday";
            saturdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            saturdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            saturdayEndTimepicker.Location = new System.Drawing.Point(1176, 187);
            saturdayEndTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            saturdayEndTimepicker.Name = "saturdayEndTimepicker";
            saturdayEndTimepicker.ShowUpDown = true;
            saturdayEndTimepicker.Size = new System.Drawing.Size(176, 35);
            saturdayEndTimepicker.TabIndex = 26;
            saturdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // fridayEndTimepicker
            // 
            fridayEndTimepicker.AccessibleName = "Enter your working hours end time for Friday";
            fridayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            fridayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            fridayEndTimepicker.Location = new System.Drawing.Point(984, 187);
            fridayEndTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            fridayEndTimepicker.Name = "fridayEndTimepicker";
            fridayEndTimepicker.ShowUpDown = true;
            fridayEndTimepicker.Size = new System.Drawing.Size(176, 35);
            fridayEndTimepicker.TabIndex = 23;
            fridayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // thursdayEndTimepicker
            // 
            thursdayEndTimepicker.AccessibleName = "Enter your working hours end time for Thursday";
            thursdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            thursdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            thursdayEndTimepicker.Location = new System.Drawing.Point(792, 187);
            thursdayEndTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            thursdayEndTimepicker.Name = "thursdayEndTimepicker";
            thursdayEndTimepicker.ShowUpDown = true;
            thursdayEndTimepicker.Size = new System.Drawing.Size(176, 35);
            thursdayEndTimepicker.TabIndex = 20;
            thursdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // wednesdayEndTimepicker
            // 
            wednesdayEndTimepicker.AccessibleName = "Enter your working hours end time for Wednesday";
            wednesdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            wednesdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            wednesdayEndTimepicker.Location = new System.Drawing.Point(600, 187);
            wednesdayEndTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            wednesdayEndTimepicker.Name = "wednesdayEndTimepicker";
            wednesdayEndTimepicker.ShowUpDown = true;
            wednesdayEndTimepicker.Size = new System.Drawing.Size(176, 35);
            wednesdayEndTimepicker.TabIndex = 17;
            wednesdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // tuesdayEndTimepicker
            // 
            tuesdayEndTimepicker.AccessibleName = "Enter your working hours end time for Tuesday";
            tuesdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            tuesdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            tuesdayEndTimepicker.Location = new System.Drawing.Point(408, 187);
            tuesdayEndTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            tuesdayEndTimepicker.Name = "tuesdayEndTimepicker";
            tuesdayEndTimepicker.ShowUpDown = true;
            tuesdayEndTimepicker.Size = new System.Drawing.Size(176, 35);
            tuesdayEndTimepicker.TabIndex = 14;
            tuesdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // mondayEndTimepicker
            // 
            mondayEndTimepicker.AccessibleName = "Enter your working hours end time for Monday";
            mondayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            mondayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            mondayEndTimepicker.Location = new System.Drawing.Point(216, 187);
            mondayEndTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            mondayEndTimepicker.Name = "mondayEndTimepicker";
            mondayEndTimepicker.ShowUpDown = true;
            mondayEndTimepicker.Size = new System.Drawing.Size(176, 35);
            mondayEndTimepicker.TabIndex = 11;
            mondayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // sundayEndTimepicker
            // 
            sundayEndTimepicker.AccessibleName = "Enter your working hours end time for Sunday";
            sundayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            sundayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            sundayEndTimepicker.Location = new System.Drawing.Point(24, 187);
            sundayEndTimepicker.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            sundayEndTimepicker.Name = "sundayEndTimepicker";
            sundayEndTimepicker.ShowUpDown = true;
            sundayEndTimepicker.Size = new System.Drawing.Size(176, 35);
            sundayEndTimepicker.TabIndex = 8;
            sundayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // sundayOffWorkCB
            // 
            sundayOffWorkCB.AutoSize = true;
            sundayOffWorkCB.Location = new System.Drawing.Point(30, 247);
            sundayOffWorkCB.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            sundayOffWorkCB.Name = "sundayOffWorkCB";
            sundayOffWorkCB.Size = new System.Drawing.Size(123, 34);
            sundayOffWorkCB.TabIndex = 9;
            sundayOffWorkCB.Text = "Off Work";
            sundayOffWorkCB.UseVisualStyleBackColor = true;
            sundayOffWorkCB.CheckedChanged += sundayOffWorkCB_CheckedChanged;
            // 
            // mondayOffWorkCB
            // 
            mondayOffWorkCB.AutoSize = true;
            mondayOffWorkCB.Location = new System.Drawing.Point(222, 247);
            mondayOffWorkCB.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            mondayOffWorkCB.Name = "mondayOffWorkCB";
            mondayOffWorkCB.Size = new System.Drawing.Size(123, 34);
            mondayOffWorkCB.TabIndex = 12;
            mondayOffWorkCB.Text = "Off Work";
            mondayOffWorkCB.UseVisualStyleBackColor = true;
            mondayOffWorkCB.CheckedChanged += mondayOffWorkCB_CheckedChanged;
            // 
            // tuesdayOffWorkCB
            // 
            tuesdayOffWorkCB.AutoSize = true;
            tuesdayOffWorkCB.Location = new System.Drawing.Point(414, 247);
            tuesdayOffWorkCB.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            tuesdayOffWorkCB.Name = "tuesdayOffWorkCB";
            tuesdayOffWorkCB.Size = new System.Drawing.Size(123, 34);
            tuesdayOffWorkCB.TabIndex = 15;
            tuesdayOffWorkCB.Text = "Off Work";
            tuesdayOffWorkCB.UseVisualStyleBackColor = true;
            tuesdayOffWorkCB.CheckedChanged += tuesdayOffWorkCB_CheckedChanged;
            // 
            // wednesdayOffWorkCB
            // 
            wednesdayOffWorkCB.AutoSize = true;
            wednesdayOffWorkCB.Location = new System.Drawing.Point(606, 247);
            wednesdayOffWorkCB.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            wednesdayOffWorkCB.Name = "wednesdayOffWorkCB";
            wednesdayOffWorkCB.Size = new System.Drawing.Size(123, 34);
            wednesdayOffWorkCB.TabIndex = 18;
            wednesdayOffWorkCB.Text = "Off Work";
            wednesdayOffWorkCB.UseVisualStyleBackColor = true;
            wednesdayOffWorkCB.CheckedChanged += wednesdayOffWorkCB_CheckedChanged;
            // 
            // thursdayOffWorkCB
            // 
            thursdayOffWorkCB.AutoSize = true;
            thursdayOffWorkCB.Location = new System.Drawing.Point(798, 247);
            thursdayOffWorkCB.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            thursdayOffWorkCB.Name = "thursdayOffWorkCB";
            thursdayOffWorkCB.Size = new System.Drawing.Size(123, 34);
            thursdayOffWorkCB.TabIndex = 21;
            thursdayOffWorkCB.Text = "Off Work";
            thursdayOffWorkCB.UseVisualStyleBackColor = true;
            thursdayOffWorkCB.CheckedChanged += thursdayOffWorkCB_CheckedChanged;
            // 
            // fridayOffWorkCB
            // 
            fridayOffWorkCB.AutoSize = true;
            fridayOffWorkCB.Location = new System.Drawing.Point(990, 247);
            fridayOffWorkCB.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            fridayOffWorkCB.Name = "fridayOffWorkCB";
            fridayOffWorkCB.Size = new System.Drawing.Size(123, 34);
            fridayOffWorkCB.TabIndex = 24;
            fridayOffWorkCB.Text = "Off Work";
            fridayOffWorkCB.UseVisualStyleBackColor = true;
            fridayOffWorkCB.CheckedChanged += fridayOffWorkCB_CheckedChanged;
            // 
            // saturdayOffWorkCB
            // 
            saturdayOffWorkCB.AutoSize = true;
            saturdayOffWorkCB.Location = new System.Drawing.Point(1182, 247);
            saturdayOffWorkCB.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            saturdayOffWorkCB.Name = "saturdayOffWorkCB";
            saturdayOffWorkCB.Size = new System.Drawing.Size(123, 34);
            saturdayOffWorkCB.TabIndex = 27;
            saturdayOffWorkCB.Text = "Off Work";
            saturdayOffWorkCB.UseVisualStyleBackColor = true;
            saturdayOffWorkCB.CheckedChanged += saturdayOffWorkCB_CheckedChanged;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(24, 1024);
            button2.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(228, 53);
            button2.TabIndex = 38;
            button2.Text = "Save Settings";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // lblExternalMesage
            // 
            lblExternalMesage.AutoSize = true;
            lblExternalMesage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblExternalMesage.Location = new System.Drawing.Point(28, 305);
            lblExternalMesage.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblExternalMesage.Name = "lblExternalMesage";
            lblExternalMesage.Size = new System.Drawing.Size(185, 25);
            lblExternalMesage.TabIndex = 40;
            lblExternalMesage.Text = "External Message";
            // 
            // lblInternalMessage
            // 
            lblInternalMessage.AutoSize = true;
            lblInternalMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblInternalMessage.Location = new System.Drawing.Point(28, 670);
            lblInternalMessage.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblInternalMessage.Name = "lblInternalMessage";
            lblInternalMessage.Size = new System.Drawing.Size(178, 25);
            lblInternalMessage.TabIndex = 41;
            lblInternalMessage.Text = "Internal Message";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(12, 5, 0, 5);
            menuStrip1.Size = new System.Drawing.Size(1366, 44);
            menuStrip1.TabIndex = 42;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.AccessibleDescription = "File";
            fileToolStripMenuItem.AccessibleName = "File";
            fileToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1, signoutToolStripMenuItem, enableOnCallModeToolStripMenuItem, showLogsToolStripMenuItem, exitToolStripMenuItem, bETAEnableNewOOFToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(62, 34);
            fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.AccessibleDescription = "A menu item with text 'Message'";
            toolStripMenuItem1.AccessibleName = "Message selection";
            toolStripMenuItem1.CheckOnClick = true;
            toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { primaryToolStripMenuItem, secondaryToolStripMenuItem });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(409, 40);
            toolStripMenuItem1.Text = "Message";
            // 
            // primaryToolStripMenuItem
            // 
            primaryToolStripMenuItem.AccessibleDescription = "A menu item with text 'Primary'";
            primaryToolStripMenuItem.AccessibleName = "Primary message";
            primaryToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            primaryToolStripMenuItem.Checked = true;
            primaryToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            primaryToolStripMenuItem.Name = "primaryToolStripMenuItem";
            primaryToolStripMenuItem.Size = new System.Drawing.Size(227, 40);
            primaryToolStripMenuItem.Text = "Primary";
            primaryToolStripMenuItem.Click += primaryToolStripMenuItem_Click;
            // 
            // secondaryToolStripMenuItem
            // 
            secondaryToolStripMenuItem.AccessibleDescription = "A menu item with text 'Secondary'";
            secondaryToolStripMenuItem.AccessibleName = "Secondary message";
            secondaryToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            secondaryToolStripMenuItem.Name = "secondaryToolStripMenuItem";
            secondaryToolStripMenuItem.Size = new System.Drawing.Size(227, 40);
            secondaryToolStripMenuItem.Text = "Secondary";
            secondaryToolStripMenuItem.Click += secondaryToolStripMenuItem_Click;
            // 
            // signoutToolStripMenuItem
            // 
            signoutToolStripMenuItem.AccessibleDescription = "A menu item with text 'Sign out'";
            signoutToolStripMenuItem.AccessibleName = "Sign out";
            signoutToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            signoutToolStripMenuItem.Name = "signoutToolStripMenuItem";
            signoutToolStripMenuItem.Size = new System.Drawing.Size(409, 40);
            signoutToolStripMenuItem.Text = "Sign out";
            signoutToolStripMenuItem.Click += signOutToolStripMenuItem_Click;
            // 
            // enableOnCallModeToolStripMenuItem
            // 
            enableOnCallModeToolStripMenuItem.AccessibleDescription = "A menu item with text '(BETA) Enable On-Call Mode'";
            enableOnCallModeToolStripMenuItem.AccessibleName = "(BETA) Enable On-Call Mode";
            enableOnCallModeToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            enableOnCallModeToolStripMenuItem.Name = "enableOnCallModeToolStripMenuItem";
            enableOnCallModeToolStripMenuItem.Size = new System.Drawing.Size(409, 40);
            enableOnCallModeToolStripMenuItem.Text = "(BETA) Enable On-Call Mode";
            enableOnCallModeToolStripMenuItem.Click += enableOnCallModeToolStripMenuItem_Click;
            // 
            // showLogsToolStripMenuItem
            // 
            showLogsToolStripMenuItem.AccessibleDescription = "A menu item with text 'Show logs'";
            showLogsToolStripMenuItem.AccessibleName = "Show logs";
            showLogsToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            showLogsToolStripMenuItem.Name = "showLogsToolStripMenuItem";
            showLogsToolStripMenuItem.Size = new System.Drawing.Size(409, 40);
            showLogsToolStripMenuItem.Text = "Show logs";
            showLogsToolStripMenuItem.Click += showLogsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(409, 40);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // bETAEnableNewOOFToolStripMenuItem
            // 
            bETAEnableNewOOFToolStripMenuItem.AccessibleName = "(BETA) Enable new OOF math";
            bETAEnableNewOOFToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            bETAEnableNewOOFToolStripMenuItem.Name = "bETAEnableNewOOFToolStripMenuItem";
            bETAEnableNewOOFToolStripMenuItem.Size = new System.Drawing.Size(409, 40);
            bETAEnableNewOOFToolStripMenuItem.Text = "(BETA) Enable New OOF Math";
            bETAEnableNewOOFToolStripMenuItem.Click += bETAEnableNewOOFToolStripMenuItem_Click;
            // 
            // label13
            // 
            label13.AccessibleDescription = "A label saying 'Enter your Working Hours:'";
            label13.AccessibleName = "Enter your Working Hours";
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label13.Location = new System.Drawing.Point(24, 58);
            label13.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(267, 25);
            label13.TabIndex = 43;
            label13.Text = "Enter your Working Hours:";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1, toolStripStatusLabel2, lblBuild });
            statusStrip1.Location = new System.Drawing.Point(0, 1816);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 28, 0);
            statusStrip1.Size = new System.Drawing.Size(1366, 43);
            statusStrip1.TabIndex = 45;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(206, 34);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(210, 34);
            toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // lblBuild
            // 
            lblBuild.Name = "lblBuild";
            lblBuild.Size = new System.Drawing.Size(135, 34);
            lblBuild.Text = "BuildNumber";
            // 
            // passwordConfirmTB
            // 
            passwordConfirmTB.Location = new System.Drawing.Point(884, 1779);
            passwordConfirmTB.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            passwordConfirmTB.Name = "passwordConfirmTB";
            passwordConfirmTB.Size = new System.Drawing.Size(196, 35);
            passwordConfirmTB.TabIndex = 52;
            passwordConfirmTB.UseSystemPasswordChar = true;
            passwordConfirmTB.Visible = false;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(684, 1786);
            label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(184, 30);
            label10.TabIndex = 51;
            label10.Text = "Confirm Password:";
            label10.Visible = false;
            // 
            // passwordTB
            // 
            passwordTB.Location = new System.Drawing.Point(470, 1779);
            passwordTB.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            passwordTB.Name = "passwordTB";
            passwordTB.Size = new System.Drawing.Size(196, 35);
            passwordTB.TabIndex = 50;
            passwordTB.UseSystemPasswordChar = true;
            passwordTB.Visible = false;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(346, 1786);
            label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(104, 30);
            label9.TabIndex = 49;
            label9.Text = "Password:";
            label9.Visible = false;
            // 
            // emailAddressTB
            // 
            emailAddressTB.Location = new System.Drawing.Point(134, 1779);
            emailAddressTB.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            emailAddressTB.Name = "emailAddressTB";
            emailAddressTB.Size = new System.Drawing.Size(196, 35);
            emailAddressTB.TabIndex = 48;
            emailAddressTB.Visible = false;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(52, 1786);
            label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(68, 30);
            label8.TabIndex = 47;
            label8.Text = "Email:";
            label8.Visible = false;
            // 
            // btnPermaOOF
            // 
            btnPermaOOF.Location = new System.Drawing.Point(649, 1024);
            btnPermaOOF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnPermaOOF.Name = "btnPermaOOF";
            btnPermaOOF.Size = new System.Drawing.Size(260, 53);
            btnPermaOOF.TabIndex = 54;
            btnPermaOOF.Tag = "Enable";
            btnPermaOOF.Text = "Go OOF now until:";
            btnPermaOOF.UseVisualStyleBackColor = true;
            btnPermaOOF.Click += btnPermaOOF_Click;
            // 
            // dtPermaOOF
            // 
            dtPermaOOF.Enabled = false;
            dtPermaOOF.Location = new System.Drawing.Point(917, 1033);
            dtPermaOOF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            dtPermaOOF.Name = "dtPermaOOF";
            dtPermaOOF.Size = new System.Drawing.Size(366, 35);
            dtPermaOOF.TabIndex = 55;
            // 
            // radPrimary
            // 
            radPrimary.AutoSize = true;
            radPrimary.Checked = true;
            radPrimary.Location = new System.Drawing.Point(264, 1033);
            radPrimary.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            radPrimary.Name = "radPrimary";
            radPrimary.Size = new System.Drawing.Size(156, 34);
            radPrimary.TabIndex = 56;
            radPrimary.TabStop = true;
            radPrimary.Text = "Primary OOF";
            radPrimary.UseVisualStyleBackColor = true;
            // 
            // radSecondary
            // 
            radSecondary.AutoSize = true;
            radSecondary.Location = new System.Drawing.Point(432, 1033);
            radSecondary.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            radSecondary.Name = "radSecondary";
            radSecondary.Size = new System.Drawing.Size(182, 34);
            radSecondary.TabIndex = 56;
            radSecondary.TabStop = true;
            radSecondary.Text = "Secondary OOF";
            radSecondary.UseVisualStyleBackColor = true;
            // 
            // htmlEditorControl2
            // 
            htmlEditorControl2.InnerText = null;
            htmlEditorControl2.Location = new System.Drawing.Point(24, 702);
            htmlEditorControl2.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            htmlEditorControl2.Name = "htmlEditorControl2";
            htmlEditorControl2.Size = new System.Drawing.Size(1334, 300);
            htmlEditorControl2.TabIndex = 39;
            // 
            // htmlEditorControl1
            // 
            htmlEditorControl1.InnerText = null;
            htmlEditorControl1.Location = new System.Drawing.Point(24, 342);
            htmlEditorControl1.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            htmlEditorControl1.Name = "htmlEditorControl1";
            htmlEditorControl1.Size = new System.Drawing.Size(1334, 300);
            htmlEditorControl1.TabIndex = 31;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new System.Drawing.Size(1396, 1136);
            Controls.Add(dtPermaOOF);
            Controls.Add(btnPermaOOF);
            Controls.Add(passwordConfirmTB);
            Controls.Add(label10);
            Controls.Add(passwordTB);
            Controls.Add(label9);
            Controls.Add(emailAddressTB);
            Controls.Add(label8);
            Controls.Add(statusStrip1);
            Controls.Add(label13);
            Controls.Add(lblInternalMessage);
            Controls.Add(lblExternalMesage);
            Controls.Add(htmlEditorControl2);
            Controls.Add(button2);
            Controls.Add(htmlEditorControl1);
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
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(1420, 1200);
            MinimumSize = new System.Drawing.Size(1420, 300);
            Name = "Form1";
            Text = "OOFSponder";
            FormClosing += Form1_FormClosing;
            Resize += Form1_Resize;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void SaveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker sundayStartTimepicker;
        private System.Windows.Forms.DateTimePicker mondayStartTimepicker;
        private System.Windows.Forms.DateTimePicker tuesdayStartTimepicker;
        private System.Windows.Forms.DateTimePicker wednesdayStartTimepicker;
        private System.Windows.Forms.DateTimePicker thursdayStartTimepicker;
        private System.Windows.Forms.DateTimePicker fridayStartTimepicker;
        private System.Windows.Forms.DateTimePicker saturdayStartTimepicker;
        private System.Windows.Forms.DateTimePicker saturdayEndTimepicker;
        private System.Windows.Forms.DateTimePicker fridayEndTimepicker;
        private System.Windows.Forms.DateTimePicker thursdayEndTimepicker;
        private System.Windows.Forms.DateTimePicker wednesdayEndTimepicker;
        private System.Windows.Forms.DateTimePicker tuesdayEndTimepicker;
        private System.Windows.Forms.DateTimePicker mondayEndTimepicker;
        private System.Windows.Forms.DateTimePicker sundayEndTimepicker;
        private System.Windows.Forms.CheckBox sundayOffWorkCB;
        private System.Windows.Forms.CheckBox mondayOffWorkCB;
        private System.Windows.Forms.CheckBox tuesdayOffWorkCB;
        private System.Windows.Forms.CheckBox wednesdayOffWorkCB;
        private System.Windows.Forms.CheckBox thursdayOffWorkCB;
        private System.Windows.Forms.CheckBox fridayOffWorkCB;
        private System.Windows.Forms.CheckBox saturdayOffWorkCB;
        private MSDN.Html.Editor.HtmlEditorControl htmlEditorControl1;
        private System.Windows.Forms.Button button2;
        private MSDN.Html.Editor.HtmlEditorControl htmlEditorControl2;
        private System.Windows.Forms.Label lblExternalMesage;
        private System.Windows.Forms.Label lblInternalMessage;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem signoutToolStripMenuItem;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.MaskedTextBox passwordConfirmTB;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.MaskedTextBox passwordTB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox emailAddressTB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolStripStatusLabel lblBuild;
        private System.Windows.Forms.Button btnPermaOOF;
        private System.Windows.Forms.DateTimePicker dtPermaOOF;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem primaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableOnCallModeToolStripMenuItem;
        private System.Windows.Forms.RadioButton radPrimary;
        private System.Windows.Forms.RadioButton radSecondary;
        private System.Windows.Forms.ToolStripMenuItem showLogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bETAEnableNewOOFToolStripMenuItem;
    }
}

