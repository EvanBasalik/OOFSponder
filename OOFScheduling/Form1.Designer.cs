﻿using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.sundayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            this.mondayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            this.tuesdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            this.wednesdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            this.thursdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            this.fridayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            this.saturdayStartTimepicker = new System.Windows.Forms.DateTimePicker();
            this.saturdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            this.fridayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            this.thursdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            this.wednesdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            this.tuesdayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            this.mondayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            this.sundayEndTimepicker = new System.Windows.Forms.DateTimePicker();
            this.sundayOffWorkCB = new System.Windows.Forms.CheckBox();
            this.mondayOffWorkCB = new System.Windows.Forms.CheckBox();
            this.tuesdayOffWorkCB = new System.Windows.Forms.CheckBox();
            this.wednesdayOffWorkCB = new System.Windows.Forms.CheckBox();
            this.thursdayOffWorkCB = new System.Windows.Forms.CheckBox();
            this.fridayOffWorkCB = new System.Windows.Forms.CheckBox();
            this.saturdayOffWorkCB = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lblExternalMesage = new System.Windows.Forms.Label();
            this.lblInternalMessage = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.primaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.signoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableOnCallModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label13 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblBuild = new System.Windows.Forms.ToolStripStatusLabel();
            this.passwordConfirmTB = new System.Windows.Forms.MaskedTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.passwordTB = new System.Windows.Forms.MaskedTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.emailAddressTB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnPermaOOF = new System.Windows.Forms.Button();
            this.dtPermaOOF = new System.Windows.Forms.DateTimePicker();
            this.radPrimary = new System.Windows.Forms.RadioButton();
            this.radSecondary = new System.Windows.Forms.RadioButton();
            this.htmlEditorControl2 = new MSDN.Html.Editor.HtmlEditorControl();
            this.htmlEditorControl1 = new MSDN.Html.Editor.HtmlEditorControl();
            this.bETAEnableNewOOFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "OOFScheduling will make sure your OOF message is set for you in the background!";
            this.notifyIcon1.BalloonTipTitle = "OOFScheduling is still working";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "OOFScheduling";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sunday";
            this.label1.AccessibleName = this.label1.Text + " working hours";
            this.label1.AccessibleDescription = "A label with the text " + this.label1.Text;

            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Monday";
            this.label2.AccessibleName = this.label2.Text + " working hours";
            this.label2.AccessibleDescription = "A label with the text " + this.label2.Text;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tuesday";
            this.label3.AccessibleName = this.label3.Text + " working hours";
            this.label3.AccessibleDescription = "A label with the text " + this.label3.Text;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(300, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Wednesday";
            this.label4.AccessibleName = this.label4.Text + " working hours";
            this.label4.AccessibleDescription = "A label with the text " + this.label4.Text;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(396, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Thursday";
            this.label5.AccessibleName = this.label5.Text + " working hours";
            this.label5.AccessibleDescription = "A label with the text " + this.label5.Text;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(492, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Friday";
            this.label6.AccessibleName = this.label6.Text + " working hours";
            this.label6.AccessibleDescription = "A label with the text " + this.label6.Text;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(588, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Saturday";
            this.label7.AccessibleName = this.label7.Text + " working hours";
            this.label7.AccessibleDescription = "A label with the text " + this.label7.Text;
            // 
            // sundayStartTimepicker
            // 
            this.sundayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.sundayStartTimepicker.Location = new System.Drawing.Point(12, 55);
            this.sundayStartTimepicker.Name = "sundayStartTimepicker";
            this.sundayStartTimepicker.ShowUpDown = true;
            this.sundayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.sundayStartTimepicker.TabIndex = 7;
            this.sundayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            this.sundayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.sundayStartTimepicker.AccessibleName = "Enter your Start Time for Sunday";
            // 
            // mondayStartTimepicker
            // 
            this.mondayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.mondayStartTimepicker.Location = new System.Drawing.Point(108, 55);
            this.mondayStartTimepicker.Name = "mondayStartTimepicker";
            this.mondayStartTimepicker.ShowUpDown = true;
            this.mondayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.mondayStartTimepicker.TabIndex = 10;
            this.mondayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            this.mondayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.mondayStartTimepicker.AccessibleName = "Enter your Start Time for Monday";
            // 
            // tuesdayStartTimepicker
            // 
            this.tuesdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tuesdayStartTimepicker.Location = new System.Drawing.Point(204, 55);
            this.tuesdayStartTimepicker.Name = "tuesdayStartTimepicker";
            this.tuesdayStartTimepicker.ShowUpDown = true;
            this.tuesdayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.tuesdayStartTimepicker.TabIndex = 13;
            this.tuesdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            this.tuesdayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.tuesdayStartTimepicker.AccessibleName = "Enter your Start Time for Tuesday";
            // 
            // wednesdayStartTimepicker
            // 
            this.wednesdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.wednesdayStartTimepicker.Location = new System.Drawing.Point(300, 55);
            this.wednesdayStartTimepicker.Name = "wednesdayStartTimepicker";
            this.wednesdayStartTimepicker.ShowUpDown = true;
            this.wednesdayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.wednesdayStartTimepicker.TabIndex = 16;
            this.wednesdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            this.wednesdayStartTimepicker.AccessibleRole =  System.Windows.Forms.AccessibleRole.SpinButton;
            this.wednesdayStartTimepicker.AccessibleName = "Enter your Start Time for Wednesday";
            // 
            // thursdayStartTimepicker
            // 
            this.thursdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.thursdayStartTimepicker.Location = new System.Drawing.Point(396, 55);
            this.thursdayStartTimepicker.Name = "thursdayStartTimepicker";
            this.thursdayStartTimepicker.ShowUpDown = true;
            this.thursdayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.thursdayStartTimepicker.TabIndex = 19;
            this.thursdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            this.thursdayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.thursdayStartTimepicker.AccessibleName = "Enter your Start Time for Thursday";
            // 
            // fridayStartTimepicker
            // 
            this.fridayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.fridayStartTimepicker.Location = new System.Drawing.Point(492, 55);
            this.fridayStartTimepicker.Name = "fridayStartTimepicker";
            this.fridayStartTimepicker.ShowUpDown = true;
            this.fridayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.fridayStartTimepicker.TabIndex = 22;
            this.fridayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            this.fridayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.fridayStartTimepicker.AccessibleName = "Enter your Start Time for Friday";
            // 
            // saturdayStartTimepicker
            // 
            this.saturdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.saturdayStartTimepicker.Location = new System.Drawing.Point(588, 55);
            this.saturdayStartTimepicker.Name = "saturdayStartTimepicker";
            this.saturdayStartTimepicker.ShowUpDown = true;
            this.saturdayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.saturdayStartTimepicker.TabIndex = 25;
            this.saturdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            this.saturdayStartTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.saturdayStartTimepicker.AccessibleName = "Enter your Start Time for Saturday";
            // 
            // saturdayEndTimepicker
            // 
            this.saturdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.saturdayEndTimepicker.Location = new System.Drawing.Point(588, 81);
            this.saturdayEndTimepicker.Name = "saturdayEndTimepicker";
            this.saturdayEndTimepicker.ShowUpDown = true;
            this.saturdayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.saturdayEndTimepicker.TabIndex = 26;
            this.saturdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            this.saturdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.saturdayEndTimepicker.AccessibleName = "Enter your End Time for Saturday";
            // 
            // fridayEndTimepicker
            // 
            this.fridayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.fridayEndTimepicker.Location = new System.Drawing.Point(492, 81);
            this.fridayEndTimepicker.Name = "fridayEndTimepicker";
            this.fridayEndTimepicker.ShowUpDown = true;
            this.fridayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.fridayEndTimepicker.TabIndex = 23;
            this.fridayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            this.fridayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.fridayEndTimepicker.AccessibleName = "Enter your End Time for Friday";
            // 
            // thursdayEndTimepicker
            // 
            this.thursdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.thursdayEndTimepicker.Location = new System.Drawing.Point(396, 81);
            this.thursdayEndTimepicker.Name = "thursdayEndTimepicker";
            this.thursdayEndTimepicker.ShowUpDown = true;
            this.thursdayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.thursdayEndTimepicker.TabIndex = 20;
            this.thursdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            this.thursdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.thursdayEndTimepicker.AccessibleName = "Enter your End Time for Thursday";
            // 
            // wednesdayEndTimepicker
            // 
            this.wednesdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.wednesdayEndTimepicker.Location = new System.Drawing.Point(300, 81);
            this.wednesdayEndTimepicker.Name = "wednesdayEndTimepicker";
            this.wednesdayEndTimepicker.ShowUpDown = true;
            this.wednesdayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.wednesdayEndTimepicker.TabIndex = 17;
            this.wednesdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            this.wednesdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.wednesdayEndTimepicker.AccessibleName = "Enter your End Time for Wednesday";
            // 
            // tuesdayEndTimepicker
            // 
            this.tuesdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tuesdayEndTimepicker.Location = new System.Drawing.Point(204, 81);
            this.tuesdayEndTimepicker.Name = "tuesdayEndTimepicker";
            this.tuesdayEndTimepicker.ShowUpDown = true;
            this.tuesdayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.tuesdayEndTimepicker.TabIndex = 14;
            this.tuesdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            this.tuesdayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.tuesdayEndTimepicker.AccessibleName = "Enter your End Time for Tuesday";
            // 
            // mondayEndTimepicker
            // 
            this.mondayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.mondayEndTimepicker.Location = new System.Drawing.Point(108, 81);
            this.mondayEndTimepicker.Name = "mondayEndTimepicker";
            this.mondayEndTimepicker.ShowUpDown = true;
            this.mondayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.mondayEndTimepicker.TabIndex = 11;
            this.mondayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            this.mondayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.mondayEndTimepicker.AccessibleName = "Enter your End Time for Monday";
            // 
            // sundayEndTimepicker
            // 
            this.sundayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.sundayEndTimepicker.Location = new System.Drawing.Point(12, 81);
            this.sundayEndTimepicker.Name = "sundayEndTimepicker";
            this.sundayEndTimepicker.ShowUpDown = true;
            this.sundayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.sundayEndTimepicker.TabIndex = 8;
            this.sundayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            this.sundayEndTimepicker.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.sundayEndTimepicker.AccessibleName = "Enter your End Time for Sunday";
            // 
            // sundayOffWorkCB
            // 
            this.sundayOffWorkCB.AutoSize = true;
            this.sundayOffWorkCB.Location = new System.Drawing.Point(15, 107);
            this.sundayOffWorkCB.Name = "sundayOffWorkCB";
            this.sundayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.sundayOffWorkCB.TabIndex = 9;
            this.sundayOffWorkCB.Text = "Off Work";
            this.sundayOffWorkCB.UseVisualStyleBackColor = true;
            this.sundayOffWorkCB.CheckedChanged += new System.EventHandler(this.sundayOffWorkCB_CheckedChanged);
            // 
            // mondayOffWorkCB
            // 
            this.mondayOffWorkCB.AutoSize = true;
            this.mondayOffWorkCB.Location = new System.Drawing.Point(111, 107);
            this.mondayOffWorkCB.Name = "mondayOffWorkCB";
            this.mondayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.mondayOffWorkCB.TabIndex = 12;
            this.mondayOffWorkCB.Text = "Off Work";
            this.mondayOffWorkCB.UseVisualStyleBackColor = true;
            this.mondayOffWorkCB.CheckedChanged += new System.EventHandler(this.mondayOffWorkCB_CheckedChanged);
            // 
            // tuesdayOffWorkCB
            // 
            this.tuesdayOffWorkCB.AutoSize = true;
            this.tuesdayOffWorkCB.Location = new System.Drawing.Point(207, 107);
            this.tuesdayOffWorkCB.Name = "tuesdayOffWorkCB";
            this.tuesdayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.tuesdayOffWorkCB.TabIndex = 15;
            this.tuesdayOffWorkCB.Text = "Off Work";
            this.tuesdayOffWorkCB.UseVisualStyleBackColor = true;
            this.tuesdayOffWorkCB.CheckedChanged += new System.EventHandler(this.tuesdayOffWorkCB_CheckedChanged);
            // 
            // wednesdayOffWorkCB
            // 
            this.wednesdayOffWorkCB.AutoSize = true;
            this.wednesdayOffWorkCB.Location = new System.Drawing.Point(303, 107);
            this.wednesdayOffWorkCB.Name = "wednesdayOffWorkCB";
            this.wednesdayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.wednesdayOffWorkCB.TabIndex = 18;
            this.wednesdayOffWorkCB.Text = "Off Work";
            this.wednesdayOffWorkCB.UseVisualStyleBackColor = true;
            this.wednesdayOffWorkCB.CheckedChanged += new System.EventHandler(this.wednesdayOffWorkCB_CheckedChanged);
            // 
            // thursdayOffWorkCB
            // 
            this.thursdayOffWorkCB.AutoSize = true;
            this.thursdayOffWorkCB.Location = new System.Drawing.Point(399, 107);
            this.thursdayOffWorkCB.Name = "thursdayOffWorkCB";
            this.thursdayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.thursdayOffWorkCB.TabIndex = 21;
            this.thursdayOffWorkCB.Text = "Off Work";
            this.thursdayOffWorkCB.UseVisualStyleBackColor = true;
            this.thursdayOffWorkCB.CheckedChanged += new System.EventHandler(this.thursdayOffWorkCB_CheckedChanged);
            // 
            // fridayOffWorkCB
            // 
            this.fridayOffWorkCB.AutoSize = true;
            this.fridayOffWorkCB.Location = new System.Drawing.Point(495, 107);
            this.fridayOffWorkCB.Name = "fridayOffWorkCB";
            this.fridayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.fridayOffWorkCB.TabIndex = 24;
            this.fridayOffWorkCB.Text = "Off Work";
            this.fridayOffWorkCB.UseVisualStyleBackColor = true;
            this.fridayOffWorkCB.CheckedChanged += new System.EventHandler(this.fridayOffWorkCB_CheckedChanged);
            // 
            // saturdayOffWorkCB
            // 
            this.saturdayOffWorkCB.AutoSize = true;
            this.saturdayOffWorkCB.Location = new System.Drawing.Point(591, 107);
            this.saturdayOffWorkCB.Name = "saturdayOffWorkCB";
            this.saturdayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.saturdayOffWorkCB.TabIndex = 27;
            this.saturdayOffWorkCB.Text = "Off Work";
            this.saturdayOffWorkCB.UseVisualStyleBackColor = true;
            this.saturdayOffWorkCB.CheckedChanged += new System.EventHandler(this.saturdayOffWorkCB_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 719);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 23);
            this.button2.TabIndex = 38;
            this.button2.Text = "Save Settings";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblExternalMesage
            // 
            this.lblExternalMesage.AutoSize = true;
            this.lblExternalMesage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExternalMesage.Location = new System.Drawing.Point(14, 132);
            this.lblExternalMesage.Name = "lblExternalMesage";
            this.lblExternalMesage.Size = new System.Drawing.Size(107, 13);
            this.lblExternalMesage.TabIndex = 40;
            this.lblExternalMesage.Text = "External Message";
            // 
            // lblInternalMessage
            // 
            this.lblInternalMessage.AutoSize = true;
            this.lblInternalMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInternalMessage.Location = new System.Drawing.Point(14, 428);
            this.lblInternalMessage.Name = "lblInternalMessage";
            this.lblInternalMessage.Size = new System.Drawing.Size(104, 13);
            this.lblInternalMessage.TabIndex = 41;
            this.lblInternalMessage.Text = "Internal Message";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(689, 24);
            this.menuStrip1.TabIndex = 42;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.signoutToolStripMenuItem,
            this.enableOnCallModeToolStripMenuItem,
            this.showLogsToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.bETAEnableNewOOFToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.fileToolStripMenuItem.AccessibleName = "File";
            this.fileToolStripMenuItem.AccessibleDescription = this.fileToolStripMenuItem.AccessibleName;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.CheckOnClick = true;
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.primaryToolStripMenuItem,
            this.secondaryToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(231, 22);
            this.toolStripMenuItem1.Text = "Message";
            this.toolStripMenuItem1.AccessibleDefaultActionDescription = "Sets the OOF message to the primary or secondary";
            this.toolStripMenuItem1.AccessibleName = "Message selection"; // + " " + "1 of 5";
            this.toolStripMenuItem1.AccessibleDescription = "A menu item with text 'Message'";
            // 
            // primaryToolStripMenuItem
            // 
            this.primaryToolStripMenuItem.Checked = true;
            this.primaryToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.primaryToolStripMenuItem.Name = "primaryToolStripMenuItem";
            this.primaryToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.primaryToolStripMenuItem.Text = "Primary";
            this.primaryToolStripMenuItem.Click += new System.EventHandler(this.primaryToolStripMenuItem_Click);
            this.primaryToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.primaryToolStripMenuItem.AccessibleDefaultActionDescription = "Sets the OOF message to the primary message";
            this.primaryToolStripMenuItem.AccessibleName = "Primary message"; // + " " + "1 of 2";
            this.primaryToolStripMenuItem.AccessibleDescription = "A menu item with text 'Primary'";
            // 
            // secondaryToolStripMenuItem
            // 
            this.secondaryToolStripMenuItem.Name = "secondaryToolStripMenuItem";
            this.secondaryToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.secondaryToolStripMenuItem.Text = "Secondary";
            this.secondaryToolStripMenuItem.Click += new System.EventHandler(this.secondaryToolStripMenuItem_Click);
            this.secondaryToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.secondaryToolStripMenuItem.AccessibleDefaultActionDescription = "Sets the OOF message to the secondary message";
            this.secondaryToolStripMenuItem.AccessibleName = "Secondary message"; // + " " + "2 of 2";
            this.secondaryToolStripMenuItem.AccessibleDescription = "A menu item with text 'Secondary'";
            // 
            // signoutToolStripMenuItem
            // 
            this.signoutToolStripMenuItem.Name = "signoutToolStripMenuItem";
            this.signoutToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.signoutToolStripMenuItem.Text = "Sign out";
            this.signoutToolStripMenuItem.Click += new System.EventHandler(this.signOutToolStripMenuItem_Click);
            this.signoutToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.signoutToolStripMenuItem.AccessibleDefaultActionDescription = "Signs you out";
            this.signoutToolStripMenuItem.AccessibleName = "Sign out"; // + " " + "2 of 5";
            this.signoutToolStripMenuItem.AccessibleDescription = "A menu item with text 'Sign out'";

            // 
            // enableOnCallModeToolStripMenuItem
            // 
            this.enableOnCallModeToolStripMenuItem.Name = "enableOnCallModeToolStripMenuItem";
            this.enableOnCallModeToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.enableOnCallModeToolStripMenuItem.Text = "(BETA) Enable On-Call Mode";
            this.enableOnCallModeToolStripMenuItem.Click += new System.EventHandler(this.enableOnCallModeToolStripMenuItem_Click);
            this.enableOnCallModeToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.enableOnCallModeToolStripMenuItem.AccessibleDefaultActionDescription = "Enables on-call mode";
            this.enableOnCallModeToolStripMenuItem.AccessibleName = "(BETA) Enable On-Call Mode"; // + " " + "3 of 5";
            this.enableOnCallModeToolStripMenuItem.AccessibleDescription = "A menu item with text '(BETA) Enable On-Call Mode'";
            // 
            // showLogsToolStripMenuItem
            // 
            this.showLogsToolStripMenuItem.Name = "showLogsToolStripMenuItem";
            this.showLogsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.showLogsToolStripMenuItem.Text = "Show logs";
            this.showLogsToolStripMenuItem.Click += new System.EventHandler(this.showLogsToolStripMenuItem_Click);
            this.showLogsToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.showLogsToolStripMenuItem.AccessibleDefaultActionDescription = "Open the folder containing the log file";
            this.showLogsToolStripMenuItem.AccessibleName = this.showLogsToolStripMenuItem.Text; // + " " + "4 of 5";
            this.showLogsToolStripMenuItem.AccessibleDescription = "A menu item with text '" + this.showLogsToolStripMenuItem.Text + "'";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            this.showLogsToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.showLogsToolStripMenuItem.AccessibleDefaultActionDescription = "Exit";
            this.showLogsToolStripMenuItem.AccessibleName = this.showLogsToolStripMenuItem.Text; // + " " + "5 of 5";
            this.showLogsToolStripMenuItem.AccessibleDescription = "A menu item with text '" + this.showLogsToolStripMenuItem.Text + "'";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(12, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(157, 13);
            this.label13.TabIndex = 43;
            this.label13.Text = "Enter your Working Hours:";
            this.label13.AccessibleName = "Enter your Working Hours";
            this.label13.AccessibleDescription = "A label saying '" + this.label13.Text + "'";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.lblBuild});
            this.statusStrip1.Location = new System.Drawing.Point(0, 747);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(689, 24);
            this.statusStrip1.TabIndex = 45;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(122, 19);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // lblBuild
            // 
            this.lblBuild.Name = "lblBuild";
            this.lblBuild.Size = new System.Drawing.Size(78, 19);
            this.lblBuild.Text = "BuildNumber";
            // 
            // passwordConfirmTB
            // 
            this.passwordConfirmTB.Location = new System.Drawing.Point(442, 771);
            this.passwordConfirmTB.Name = "passwordConfirmTB";
            this.passwordConfirmTB.Size = new System.Drawing.Size(100, 20);
            this.passwordConfirmTB.TabIndex = 52;
            this.passwordConfirmTB.UseSystemPasswordChar = true;
            this.passwordConfirmTB.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(342, 774);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 13);
            this.label10.TabIndex = 51;
            this.label10.Text = "Confirm Password:";
            this.label10.Visible = false;
            // 
            // passwordTB
            // 
            this.passwordTB.Location = new System.Drawing.Point(235, 771);
            this.passwordTB.Name = "passwordTB";
            this.passwordTB.Size = new System.Drawing.Size(100, 20);
            this.passwordTB.TabIndex = 50;
            this.passwordTB.UseSystemPasswordChar = true;
            this.passwordTB.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(173, 774);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 49;
            this.label9.Text = "Password:";
            this.label9.Visible = false;
            // 
            // emailAddressTB
            // 
            this.emailAddressTB.Location = new System.Drawing.Point(67, 771);
            this.emailAddressTB.Name = "emailAddressTB";
            this.emailAddressTB.Size = new System.Drawing.Size(100, 20);
            this.emailAddressTB.TabIndex = 48;
            this.emailAddressTB.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 774);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 47;
            this.label8.Text = "Email:";
            this.label8.Visible = false;
            // 
            // btnPermaOOF
            // 
            this.btnPermaOOF.Location = new System.Drawing.Point(308, 719);
            this.btnPermaOOF.Margin = new System.Windows.Forms.Padding(2);
            this.btnPermaOOF.Name = "btnPermaOOF";
            this.btnPermaOOF.Size = new System.Drawing.Size(130, 23);
            this.btnPermaOOF.TabIndex = 54;
            this.btnPermaOOF.Tag = "Enable";
            this.btnPermaOOF.Text = "Go OOF now until:";
            this.btnPermaOOF.UseVisualStyleBackColor = true;
            this.btnPermaOOF.Click += new System.EventHandler(this.btnPermaOOF_Click);
            // 
            // dtPermaOOF
            // 
            this.dtPermaOOF.Enabled = false;
            this.dtPermaOOF.Location = new System.Drawing.Point(438, 721);
            this.dtPermaOOF.Margin = new System.Windows.Forms.Padding(2);
            this.dtPermaOOF.Name = "dtPermaOOF";
            this.dtPermaOOF.Size = new System.Drawing.Size(185, 20);
            this.dtPermaOOF.TabIndex = 55;
            // 
            // radPrimary
            // 
            this.radPrimary.AutoSize = true;
            this.radPrimary.Checked = true;
            this.radPrimary.Location = new System.Drawing.Point(110, 722);
            this.radPrimary.Name = "radPrimary";
            this.radPrimary.Size = new System.Drawing.Size(84, 17);
            this.radPrimary.TabIndex = 56;
            this.radPrimary.TabStop = true;
            this.radPrimary.Text = "Primary OOF";
            this.radPrimary.UseVisualStyleBackColor = true;
            // 
            // radSecondary
            // 
            this.radSecondary.AutoSize = true;
            this.radSecondary.Location = new System.Drawing.Point(202, 722);
            this.radSecondary.Name = "radSecondary";
            this.radSecondary.Size = new System.Drawing.Size(101, 17);
            this.radSecondary.TabIndex = 56;
            this.radSecondary.TabStop = true;
            this.radSecondary.Text = "Secondary OOF";
            this.radSecondary.UseVisualStyleBackColor = true;
            // 
            // htmlEditorControl2
            // 
            this.htmlEditorControl2.InnerText = null;
            this.htmlEditorControl2.Location = new System.Drawing.Point(12, 444);
            this.htmlEditorControl2.Name = "htmlEditorControl2";
            this.htmlEditorControl2.Size = new System.Drawing.Size(667, 269);
            this.htmlEditorControl2.TabIndex = 39;
            // 
            // htmlEditorControl1
            // 
            this.htmlEditorControl1.InnerText = null;
            this.htmlEditorControl1.Location = new System.Drawing.Point(12, 148);
            this.htmlEditorControl1.Name = "htmlEditorControl1";
            this.htmlEditorControl1.Size = new System.Drawing.Size(667, 277);
            this.htmlEditorControl1.TabIndex = 31;
            // 
            // bETAEnableNewOOFToolStripMenuItem
            // 
            this.bETAEnableNewOOFToolStripMenuItem.Name = "bETAEnableNewOOFToolStripMenuItem";
            this.bETAEnableNewOOFToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.bETAEnableNewOOFToolStripMenuItem.Text = "(BETA) Enable New OOF Math";
            this.bETAEnableNewOOFToolStripMenuItem.Click += new System.EventHandler(this.bETAEnableNewOOFToolStripMenuItem_Click);
            this.bETAEnableNewOOFToolStripMenuItem.AccessibleName = "(BETA) Enable new OOF math";
            this.bETAEnableNewOOFToolStripMenuItem.AccessibleDescription = this.fileToolStripMenuItem.AccessibleName;
            this.bETAEnableNewOOFToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 771);
            this.Controls.Add(this.dtPermaOOF);
            this.Controls.Add(this.btnPermaOOF);
            this.Controls.Add(this.passwordConfirmTB);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.passwordTB);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.emailAddressTB);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lblInternalMessage);
            this.Controls.Add(this.lblExternalMesage);
            this.Controls.Add(this.htmlEditorControl2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.htmlEditorControl1);
            this.Controls.Add(this.saturdayOffWorkCB);
            this.Controls.Add(this.fridayOffWorkCB);
            this.Controls.Add(this.thursdayOffWorkCB);
            this.Controls.Add(this.wednesdayOffWorkCB);
            this.Controls.Add(this.tuesdayOffWorkCB);
            this.Controls.Add(this.mondayOffWorkCB);
            this.Controls.Add(this.sundayOffWorkCB);
            this.Controls.Add(this.saturdayEndTimepicker);
            this.Controls.Add(this.fridayEndTimepicker);
            this.Controls.Add(this.thursdayEndTimepicker);
            this.Controls.Add(this.wednesdayEndTimepicker);
            this.Controls.Add(this.tuesdayEndTimepicker);
            this.Controls.Add(this.mondayEndTimepicker);
            this.Controls.Add(this.sundayEndTimepicker);
            this.Controls.Add(this.saturdayStartTimepicker);
            this.Controls.Add(this.fridayStartTimepicker);
            this.Controls.Add(this.thursdayStartTimepicker);
            this.Controls.Add(this.wednesdayStartTimepicker);
            this.Controls.Add(this.tuesdayStartTimepicker);
            this.Controls.Add(this.mondayStartTimepicker);
            this.Controls.Add(this.sundayStartTimepicker);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.radPrimary);
            this.Controls.Add(this.radSecondary);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(705, 810);
            this.MinimumSize = new System.Drawing.Size(705, 810);
            this.Name = "Form1";
            this.Text = "OOFSponder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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

