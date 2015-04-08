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
            this.oofUpdateTimer = new System.Windows.Forms.Timer(this.components);
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
            this.button1 = new System.Windows.Forms.Button();
            this.htmlEditorControl1 = new MSDN.Html.Editor.HtmlEditorControl();
            this.label8 = new System.Windows.Forms.Label();
            this.emailAddressTB = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.passwordTB = new System.Windows.Forms.MaskedTextBox();
            this.passwordConfirmTB = new System.Windows.Forms.MaskedTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
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
            // oofUpdateTimer
            // 
            this.oofUpdateTimer.Interval = 600000;
            this.oofUpdateTimer.Tick += new System.EventHandler(this.oofUpdateTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sunday";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Monday";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tuesday";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(300, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Wednesday";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(396, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Thursday";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(492, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Friday";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(588, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Saturday";
            // 
            // sundayStartTimepicker
            // 
            this.sundayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.sundayStartTimepicker.Location = new System.Drawing.Point(12, 65);
            this.sundayStartTimepicker.Name = "sundayStartTimepicker";
            this.sundayStartTimepicker.ShowUpDown = true;
            this.sundayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.sundayStartTimepicker.TabIndex = 7;
            this.sundayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // mondayStartTimepicker
            // 
            this.mondayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.mondayStartTimepicker.Location = new System.Drawing.Point(108, 65);
            this.mondayStartTimepicker.Name = "mondayStartTimepicker";
            this.mondayStartTimepicker.ShowUpDown = true;
            this.mondayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.mondayStartTimepicker.TabIndex = 8;
            this.mondayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // tuesdayStartTimepicker
            // 
            this.tuesdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tuesdayStartTimepicker.Location = new System.Drawing.Point(204, 65);
            this.tuesdayStartTimepicker.Name = "tuesdayStartTimepicker";
            this.tuesdayStartTimepicker.ShowUpDown = true;
            this.tuesdayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.tuesdayStartTimepicker.TabIndex = 9;
            this.tuesdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // wednesdayStartTimepicker
            // 
            this.wednesdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.wednesdayStartTimepicker.Location = new System.Drawing.Point(300, 65);
            this.wednesdayStartTimepicker.Name = "wednesdayStartTimepicker";
            this.wednesdayStartTimepicker.ShowUpDown = true;
            this.wednesdayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.wednesdayStartTimepicker.TabIndex = 10;
            this.wednesdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // thursdayStartTimepicker
            // 
            this.thursdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.thursdayStartTimepicker.Location = new System.Drawing.Point(396, 65);
            this.thursdayStartTimepicker.Name = "thursdayStartTimepicker";
            this.thursdayStartTimepicker.ShowUpDown = true;
            this.thursdayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.thursdayStartTimepicker.TabIndex = 11;
            this.thursdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // fridayStartTimepicker
            // 
            this.fridayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.fridayStartTimepicker.Location = new System.Drawing.Point(492, 65);
            this.fridayStartTimepicker.Name = "fridayStartTimepicker";
            this.fridayStartTimepicker.ShowUpDown = true;
            this.fridayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.fridayStartTimepicker.TabIndex = 12;
            this.fridayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // saturdayStartTimepicker
            // 
            this.saturdayStartTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.saturdayStartTimepicker.Location = new System.Drawing.Point(588, 65);
            this.saturdayStartTimepicker.Name = "saturdayStartTimepicker";
            this.saturdayStartTimepicker.ShowUpDown = true;
            this.saturdayStartTimepicker.Size = new System.Drawing.Size(90, 20);
            this.saturdayStartTimepicker.TabIndex = 13;
            this.saturdayStartTimepicker.Value = new System.DateTime(2015, 4, 6, 8, 0, 0, 0);
            // 
            // saturdayEndTimepicker
            // 
            this.saturdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.saturdayEndTimepicker.Location = new System.Drawing.Point(588, 91);
            this.saturdayEndTimepicker.Name = "saturdayEndTimepicker";
            this.saturdayEndTimepicker.ShowUpDown = true;
            this.saturdayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.saturdayEndTimepicker.TabIndex = 20;
            this.saturdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // fridayEndTimepicker
            // 
            this.fridayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.fridayEndTimepicker.Location = new System.Drawing.Point(492, 91);
            this.fridayEndTimepicker.Name = "fridayEndTimepicker";
            this.fridayEndTimepicker.ShowUpDown = true;
            this.fridayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.fridayEndTimepicker.TabIndex = 19;
            this.fridayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // thursdayEndTimepicker
            // 
            this.thursdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.thursdayEndTimepicker.Location = new System.Drawing.Point(396, 91);
            this.thursdayEndTimepicker.Name = "thursdayEndTimepicker";
            this.thursdayEndTimepicker.ShowUpDown = true;
            this.thursdayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.thursdayEndTimepicker.TabIndex = 18;
            this.thursdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // wednesdayEndTimepicker
            // 
            this.wednesdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.wednesdayEndTimepicker.Location = new System.Drawing.Point(300, 91);
            this.wednesdayEndTimepicker.Name = "wednesdayEndTimepicker";
            this.wednesdayEndTimepicker.ShowUpDown = true;
            this.wednesdayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.wednesdayEndTimepicker.TabIndex = 17;
            this.wednesdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // tuesdayEndTimepicker
            // 
            this.tuesdayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tuesdayEndTimepicker.Location = new System.Drawing.Point(204, 91);
            this.tuesdayEndTimepicker.Name = "tuesdayEndTimepicker";
            this.tuesdayEndTimepicker.ShowUpDown = true;
            this.tuesdayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.tuesdayEndTimepicker.TabIndex = 16;
            this.tuesdayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // mondayEndTimepicker
            // 
            this.mondayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.mondayEndTimepicker.Location = new System.Drawing.Point(108, 91);
            this.mondayEndTimepicker.Name = "mondayEndTimepicker";
            this.mondayEndTimepicker.ShowUpDown = true;
            this.mondayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.mondayEndTimepicker.TabIndex = 15;
            this.mondayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // sundayEndTimepicker
            // 
            this.sundayEndTimepicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.sundayEndTimepicker.Location = new System.Drawing.Point(12, 91);
            this.sundayEndTimepicker.Name = "sundayEndTimepicker";
            this.sundayEndTimepicker.ShowUpDown = true;
            this.sundayEndTimepicker.Size = new System.Drawing.Size(90, 20);
            this.sundayEndTimepicker.TabIndex = 14;
            this.sundayEndTimepicker.Value = new System.DateTime(2015, 4, 6, 17, 0, 0, 0);
            // 
            // sundayOffWorkCB
            // 
            this.sundayOffWorkCB.AutoSize = true;
            this.sundayOffWorkCB.Location = new System.Drawing.Point(15, 117);
            this.sundayOffWorkCB.Name = "sundayOffWorkCB";
            this.sundayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.sundayOffWorkCB.TabIndex = 21;
            this.sundayOffWorkCB.Text = "Off Work";
            this.sundayOffWorkCB.UseVisualStyleBackColor = true;
            this.sundayOffWorkCB.CheckedChanged += new System.EventHandler(this.sundayOffWorkCB_CheckedChanged);
            // 
            // mondayOffWorkCB
            // 
            this.mondayOffWorkCB.AutoSize = true;
            this.mondayOffWorkCB.Location = new System.Drawing.Point(111, 117);
            this.mondayOffWorkCB.Name = "mondayOffWorkCB";
            this.mondayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.mondayOffWorkCB.TabIndex = 22;
            this.mondayOffWorkCB.Text = "Off Work";
            this.mondayOffWorkCB.UseVisualStyleBackColor = true;
            this.mondayOffWorkCB.CheckedChanged += new System.EventHandler(this.mondayOffWorkCB_CheckedChanged);
            // 
            // tuesdayOffWorkCB
            // 
            this.tuesdayOffWorkCB.AutoSize = true;
            this.tuesdayOffWorkCB.Location = new System.Drawing.Point(207, 117);
            this.tuesdayOffWorkCB.Name = "tuesdayOffWorkCB";
            this.tuesdayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.tuesdayOffWorkCB.TabIndex = 23;
            this.tuesdayOffWorkCB.Text = "Off Work";
            this.tuesdayOffWorkCB.UseVisualStyleBackColor = true;
            this.tuesdayOffWorkCB.CheckedChanged += new System.EventHandler(this.tuesdayOffWorkCB_CheckedChanged);
            // 
            // wednesdayOffWorkCB
            // 
            this.wednesdayOffWorkCB.AutoSize = true;
            this.wednesdayOffWorkCB.Location = new System.Drawing.Point(303, 117);
            this.wednesdayOffWorkCB.Name = "wednesdayOffWorkCB";
            this.wednesdayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.wednesdayOffWorkCB.TabIndex = 24;
            this.wednesdayOffWorkCB.Text = "Off Work";
            this.wednesdayOffWorkCB.UseVisualStyleBackColor = true;
            this.wednesdayOffWorkCB.CheckedChanged += new System.EventHandler(this.wednesdayOffWorkCB_CheckedChanged);
            // 
            // thursdayOffWorkCB
            // 
            this.thursdayOffWorkCB.AutoSize = true;
            this.thursdayOffWorkCB.Location = new System.Drawing.Point(399, 117);
            this.thursdayOffWorkCB.Name = "thursdayOffWorkCB";
            this.thursdayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.thursdayOffWorkCB.TabIndex = 25;
            this.thursdayOffWorkCB.Text = "Off Work";
            this.thursdayOffWorkCB.UseVisualStyleBackColor = true;
            this.thursdayOffWorkCB.CheckedChanged += new System.EventHandler(this.thursdayOffWorkCB_CheckedChanged);
            // 
            // fridayOffWorkCB
            // 
            this.fridayOffWorkCB.AutoSize = true;
            this.fridayOffWorkCB.Location = new System.Drawing.Point(495, 117);
            this.fridayOffWorkCB.Name = "fridayOffWorkCB";
            this.fridayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.fridayOffWorkCB.TabIndex = 26;
            this.fridayOffWorkCB.Text = "Off Work";
            this.fridayOffWorkCB.UseVisualStyleBackColor = true;
            this.fridayOffWorkCB.CheckedChanged += new System.EventHandler(this.fridayOffWorkCB_CheckedChanged);
            // 
            // saturdayOffWorkCB
            // 
            this.saturdayOffWorkCB.AutoSize = true;
            this.saturdayOffWorkCB.Location = new System.Drawing.Point(591, 117);
            this.saturdayOffWorkCB.Name = "saturdayOffWorkCB";
            this.saturdayOffWorkCB.Size = new System.Drawing.Size(69, 17);
            this.saturdayOffWorkCB.TabIndex = 27;
            this.saturdayOffWorkCB.Text = "Off Work";
            this.saturdayOffWorkCB.UseVisualStyleBackColor = true;
            this.saturdayOffWorkCB.CheckedChanged += new System.EventHandler(this.saturdayOffWorkCB_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 23);
            this.button1.TabIndex = 29;
            this.button1.Text = "Run Manually";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // htmlEditorControl1
            // 
            this.htmlEditorControl1.InnerText = null;
            this.htmlEditorControl1.Location = new System.Drawing.Point(15, 140);
            this.htmlEditorControl1.Name = "htmlEditorControl1";
            this.htmlEditorControl1.Size = new System.Drawing.Size(663, 269);
            this.htmlEditorControl1.TabIndex = 31;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Email:";
            // 
            // emailAddressTB
            // 
            this.emailAddressTB.Location = new System.Drawing.Point(53, 10);
            this.emailAddressTB.Name = "emailAddressTB";
            this.emailAddressTB.Size = new System.Drawing.Size(100, 20);
            this.emailAddressTB.TabIndex = 33;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(159, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Password:";
            // 
            // passwordTB
            // 
            this.passwordTB.Location = new System.Drawing.Point(221, 10);
            this.passwordTB.Name = "passwordTB";
            this.passwordTB.Size = new System.Drawing.Size(100, 20);
            this.passwordTB.TabIndex = 35;
            this.passwordTB.UseSystemPasswordChar = true;
            // 
            // passwordConfirmTB
            // 
            this.passwordConfirmTB.Location = new System.Drawing.Point(428, 10);
            this.passwordConfirmTB.Name = "passwordConfirmTB";
            this.passwordConfirmTB.Size = new System.Drawing.Size(100, 20);
            this.passwordConfirmTB.TabIndex = 37;
            this.passwordConfirmTB.UseSystemPasswordChar = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(328, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "Confirm Password:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(108, 415);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 23);
            this.button2.TabIndex = 38;
            this.button2.Text = "Save Settings";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.passwordConfirmTB);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.passwordTB);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.emailAddressTB);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.htmlEditorControl1);
            this.Controls.Add(this.button1);
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
            this.Name = "Form1";
            this.Text = "OOFSponder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Timer oofUpdateTimer;
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
        private System.Windows.Forms.Button button1;
        private MSDN.Html.Editor.HtmlEditorControl htmlEditorControl1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox emailAddressTB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.MaskedTextBox passwordTB;
        private System.Windows.Forms.MaskedTextBox passwordConfirmTB;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button2;

    }
}

