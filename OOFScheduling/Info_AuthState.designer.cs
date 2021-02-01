
namespace OOFScheduling
{
    partial class Info_AuthState
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Info_AuthState));
            this.authGrid = new System.Windows.Forms.DataGridView();
            this.DG_Property = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BT_SignIn = new System.Windows.Forms.Button();
            this.BT_Reauth = new System.Windows.Forms.Button();
            this.BT_SignOut = new System.Windows.Forms.Button();
            this.BT_Close = new System.Windows.Forms.Button();
            this.BT_Refresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.authGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // authGrid
            // 
            this.authGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.authGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.authGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.authGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.authGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DG_Property,
            this.Value});
            this.authGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.authGrid.Location = new System.Drawing.Point(0, 0);
            this.authGrid.Name = "authGrid";
            this.authGrid.ReadOnly = true;
            this.authGrid.Size = new System.Drawing.Size(665, 211);
            this.authGrid.TabIndex = 1;
            // 
            // DG_Property
            // 
            this.DG_Property.HeaderText = "Property";
            this.DG_Property.Name = "DG_Property";
            this.DG_Property.ReadOnly = true;
            this.DG_Property.Width = 21;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            this.Value.Width = 21;
            // 
            // BT_SignIn
            // 
            this.BT_SignIn.Location = new System.Drawing.Point(12, 217);
            this.BT_SignIn.Name = "BT_SignIn";
            this.BT_SignIn.Size = new System.Drawing.Size(127, 32);
            this.BT_SignIn.TabIndex = 2;
            this.BT_SignIn.Text = "Sign In";
            this.BT_SignIn.UseVisualStyleBackColor = true;
            this.BT_SignIn.Click += new System.EventHandler(this.BT_SignIn_Click);
            // 
            // BT_Reauth
            // 
            this.BT_Reauth.Location = new System.Drawing.Point(145, 217);
            this.BT_Reauth.Name = "BT_Reauth";
            this.BT_Reauth.Size = new System.Drawing.Size(127, 32);
            this.BT_Reauth.TabIndex = 3;
            this.BT_Reauth.Text = "Reauthenticate";
            this.BT_Reauth.UseVisualStyleBackColor = true;
            this.BT_Reauth.Click += new System.EventHandler(this.BT_Reauth_Click);
            // 
            // BT_SignOut
            // 
            this.BT_SignOut.Location = new System.Drawing.Point(393, 217);
            this.BT_SignOut.Name = "BT_SignOut";
            this.BT_SignOut.Size = new System.Drawing.Size(127, 32);
            this.BT_SignOut.TabIndex = 4;
            this.BT_SignOut.Text = "Sign Out";
            this.BT_SignOut.UseVisualStyleBackColor = true;
            this.BT_SignOut.Click += new System.EventHandler(this.BT_SignOut_Click);
            // 
            // BT_Close
            // 
            this.BT_Close.Location = new System.Drawing.Point(526, 217);
            this.BT_Close.Name = "BT_Close";
            this.BT_Close.Size = new System.Drawing.Size(127, 32);
            this.BT_Close.TabIndex = 5;
            this.BT_Close.Text = "Close";
            this.BT_Close.UseVisualStyleBackColor = true;
            this.BT_Close.Click += new System.EventHandler(this.BT_Close_Click);
            // 
            // BT_Refresh
            // 
            this.BT_Refresh.Location = new System.Drawing.Point(278, 217);
            this.BT_Refresh.Name = "BT_Refresh";
            this.BT_Refresh.Size = new System.Drawing.Size(109, 32);
            this.BT_Refresh.TabIndex = 6;
            this.BT_Refresh.Text = "Refresh";
            this.BT_Refresh.UseVisualStyleBackColor = true;
            this.BT_Refresh.Click += new System.EventHandler(this.BT_Refresh_Click);
            // 
            // Info_AuthState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 261);
            this.Controls.Add(this.BT_Refresh);
            this.Controls.Add(this.BT_Close);
            this.Controls.Add(this.BT_SignOut);
            this.Controls.Add(this.BT_Reauth);
            this.Controls.Add(this.BT_SignIn);
            this.Controls.Add(this.authGrid);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Info_AuthState";
            this.Text = "OOFSponder - Authentication";
            ((System.ComponentModel.ISupportInitialize)(this.authGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView authGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn DG_Property;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.Button BT_SignIn;
        private System.Windows.Forms.Button BT_Reauth;
        private System.Windows.Forms.Button BT_SignOut;
        private System.Windows.Forms.Button BT_Close;
        private System.Windows.Forms.Button BT_Refresh;
    }
}