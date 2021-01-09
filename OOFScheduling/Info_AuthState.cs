using Microsoft.Exchange.WebServices.Auth.Validation;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;
using Windows.Media.Capture;

namespace OOFScheduling
{
    public partial class Info_AuthState : Form
    {
        public Info_AuthState()
        {
            InitializeComponent();
            authGrid.Rows.Add(5);//Add initial rows that will be populated with information.
            callonload();//Call the job that will pull the auth information and populate the datagrid.

            //Check if logged in.
            //If user not logged in, prompt to sign in.
            if (O365.isLoggedIn == false) 
            { MessageBox.Show("Please sign in to view the data in this table using the buttons at the bottom of the next window.", "Not Authenticated"); }
            
        }

        public void callonload()
        {

            authGrid.Refresh();//Clear the state of the datagrid.
            
            if (O365.isLoggedIn == false)//Check if user is not logged in.
            {
                //Update Buttons
                BT_SignIn.Enabled = true;
                BT_SignOut.Enabled = false;
                BT_SignIn.Text = "Sign In";

                //Blank out the grid with no data.
                authGrid.Rows[0].Cells[0].Value = "Authenticated";
                authGrid.Rows[0].Cells[1].Value = "                                                                                                                                                                                       ";
                authGrid.Rows[1].Cells[0].Value = "Account";
                authGrid.Rows[1].Cells[1].Value = "";
                authGrid.Rows[2].Cells[0].Value = "AAD Tenant ID";
                authGrid.Rows[2].Cells[1].Value = "";
                authGrid.Rows[3].Cells[0].Value = "Access Token";
                authGrid.Rows[3].Cells[1].Value = "";
                authGrid.Rows[4].Cells[0].Value = "Token Expiration";
                authGrid.Rows[4].Cells[1].Value = "";
                authGrid.Rows[5].Cells[0].Value = "Authorization Correlation ID";
                authGrid.Rows[5].Cells[1].Value = "";
            }

            if (O365.isLoggedIn == true)//Check if the user is logged in.
            {
                //Disable the sign-in button and enable the sign-out button.
                BT_SignIn.Enabled = false;
                BT_SignOut.Enabled = true;
                BT_SignIn.Text = "Signed in";

                //Update the datagrid with values from the user session.
                authGrid.Rows[0].Cells[0].Value = "Authenticated";
                authGrid.Rows[0].Cells[1].Value = O365.isLoggedIn;
                authGrid.Rows[1].Cells[0].Value = "Account";
                authGrid.Rows[1].Cells[1].Value = O365.authResult.Account.Username;
                authGrid.Rows[2].Cells[0].Value = "AAD Tenant ID";
                authGrid.Rows[2].Cells[1].Value = O365.authResult.TenantId;
                authGrid.Rows[3].Cells[0].Value = "Access Token";
                authGrid.Rows[3].Cells[1].Value = O365.authResult.AccessToken;
                authGrid.Rows[4].Cells[0].Value = "Token Expiration";
                authGrid.Rows[4].Cells[1].Value = O365.authResult.ExpiresOn;
                authGrid.Rows[5].Cells[0].Value = "Authorization Correlation ID";
                authGrid.Rows[5].Cells[1].Value = O365.authResult.CorrelationId;
            }

            authGrid.EndEdit();//Close edits on the grid.

        }

        private void BT_Close_Click(object sender, System.EventArgs e)
        {
            this.Close();//Close the form.
        }

        private void BT_SignIn_Click(object sender, System.EventArgs e)
        {
            System.Threading.Tasks.Task AuthTask = null; //Declare auth task

            AuthTask = System.Threading.Tasks.Task.Run((Action)(() => { O365.MSALWork(O365.AADAction.SignIn); })); // AAD Login prompt.

            if (AuthTask.IsCompleted != true) //Check if the login is complete, if not, update button to say that login is in process.
            {
                BT_SignIn.Text = "Logging in";
                BT_SignIn.Enabled = false;
                AuthTask.Wait();
            }

            //Need to wait for the auth session.
            BT_SignIn.Enabled = false;
            BT_SignIn.Text = "Refreshing...";

            if (AuthTask.IsCompleted == true) //After authentication is complete, wait for 1 second to allow for refresh of the auth data.
            {
                System.Threading.Thread.Sleep(1000);
                callonload(); //Call reload of grid.
                BT_SignIn.Text = "Signed in";
            }
            

        }
        
        //BT_Reauth serves the function of AADAction.ForceSignIn - operation availiable while signed in or out.
        private void BT_Reauth_Click(object sender, EventArgs e) 
        {
            System.Threading.Tasks.Task AuthTask = null; //Declare auth task

            AuthTask = System.Threading.Tasks.Task.Run((Action)(() => { O365.MSALWork(O365.AADAction.ForceSignIn); })); // AAD Login prompt.

            if (AuthTask.IsCompleted != true) //Similar to the sign in action, the auth state can be renewed manually via ForceSignIn
            {
                BT_SignIn.Text = "Logging in";
                BT_SignIn.Enabled = false;
                AuthTask.Wait();
            }

            BT_SignIn.Enabled = false;
            BT_SignIn.Text = "Refreshing...";

            if (AuthTask.IsCompleted == true || O365.isLoggedIn == false)
            {
                MessageBox.Show("Authentication failed. Please try again.", "Warning"); //Error catch in case auth fails.
            }

            if (AuthTask.IsCompleted == true)
            {
                System.Threading.Thread.Sleep(1000);
                callonload();// Datagrid refresh.
                BT_SignIn.Text = "Signed in";
            }

        }

        //BT_Refresh will refresh the grid and update the table with auth data.
        private void BT_Refresh_Click(object sender, EventArgs e)
        {
            callonload();
        }

        //BT_SignOut will close out the session and clear the grid of auth data.
        private void BT_SignOut_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task AuthTask = null; //Declare task.

            AuthTask = System.Threading.Tasks.Task.Run((Action)(() => { O365.MSALWork(O365.AADAction.SignOut); })); //AAD Sign out - no prompt.

            if (AuthTask.IsCompleted != true) //While operation in progress, update button to show that we are signing out.
            {
                BT_SignOut.Text = "Signing out";
                BT_SignOut.Enabled = false;
                AuthTask.Wait();
            }

            BT_SignOut.Enabled = false;
            BT_SignOut.Text = "Refreshing...";//Sign out complete, wait 1 second for refresh below

            if (AuthTask.IsCompleted == true)
            {
                System.Threading.Thread.Sleep(1000);
                callonload();// Refresh grid.

                //Update buttons to properly state status.
                BT_SignIn.Enabled = true;
                BT_SignIn.Text = "Sign in";
                BT_SignOut.Text = "Sign out";
            }
        }
    }
}
