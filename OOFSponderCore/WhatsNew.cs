using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOFScheduling
{
    public partial class WhatsNew : Form
    {
        //counter for expiring the form
        int iCountdown;

        public WhatsNew(int _iCountdown)
        {
            InitializeComponent();

            this.Icon = Properties.Resources.OOFSponderIcon;

            richTextBox1.LoadFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "whatsnew.rtf"));
            
            //set up the self-destruct
            iCountdown = _iCountdown;
                        //start the countdown
            if (iCountdown > 0)
            {
                tmDismiss.Interval = 1000;
                tmDismiss.Start();
            }

            //clear the tracker that allows this to show
            ClickOnceTracker.ClearFirstRunTracker();

            this.BringToFront();
            this.TopMost = true;
        }

        private void tmDismiss_Tick(object sender, EventArgs e)
        {
            btnOK.Text = "OK - Closing in " + iCountdown.ToString() + " secs";
            iCountdown--;
            if (iCountdown < 0)
                Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            tmDismiss.Stop();
            btnOK.Text = "OK";
        }

        private void WhatsNew_Resize(object sender, EventArgs e)
        {
            tmDismiss.Stop();
            btnOK.Text = "OK";
        }
    }
}
