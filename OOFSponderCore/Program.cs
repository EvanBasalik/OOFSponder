using System;
using System.Windows.Forms;

namespace OOFScheduling
{
    static class Program
    {

        //http://sanity-free.org/143/csharp_dotnet_single_instance_application.html


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                //http://covingtoninnovations.com/mc/SingleInstance.html
                bool gotMutex;
                //GUID is generated using the built-in VS capability
                System.Threading.Mutex m = new System.Threading.Mutex(true, "{6FE49292-F7B3-4EB7-B8F2-0CDDFE20B737}", out gotMutex);

                //http://covingtoninnovations.com/mc/SingleInstance.html
                if (gotMutex)
                {
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.SetCompatibleTextRenderingDefault(false);

                    //switch to the new form since cannot fix the HTMLEditor scaling on Form1
                    //Application.Run(new Form1());
                    Application.Run(new MainForm());
                    GC.KeepAlive(m);
                }
                else
                {
                    //http://www.codebetter.com/paullaudeman/2004/07/17/windows-forms-tip-ensure-only-one-instance-of-your-application-is-running-at-a-time/
                    // see if we can find the other app and Bring it to front
                    //NOTE: the link referenced for this specifies a string for the first value, but
                    //I ended up being able to just use the Window name
                    IntPtr hWnd = NativeMethods.FindWindow(null, "OOFSponder");

                    if (hWnd != IntPtr.Zero)
                    {
                        NativeMethods.WINDOWPLACEMENT placement = new NativeMethods.WINDOWPLACEMENT();
                        placement.length = System.Runtime.InteropServices.Marshal.SizeOf(placement);

                        NativeMethods.GetWindowPlacement(hWnd, ref placement);

                        if (placement.showCmd != NativeMethods.SW_MINIMIZE)
                        {
                            placement.showCmd = NativeMethods.SW_RESTORE;

                            NativeMethods.SetWindowPlacement(hWnd, ref placement);
                            NativeMethods.SetForegroundWindow(hWnd);
                        }
                    }

                    return;

                }
            }
            catch (Exception ex)
            {
                OOFSponder.Logger.Error("Fatal error on startup");
                OOFSponder.Logger.Error(ex);
                MessageBox.Show("Uh oh! Fatal error for OOFSponder. Please review the logs for potential reasons.");
                return;
            }


        }


    }
}
