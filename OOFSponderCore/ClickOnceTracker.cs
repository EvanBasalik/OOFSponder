using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OOFScheduling
{
    class ClickOnceTracker
    {

        internal static string trackerfolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        internal static string trackerfile = System.IO.Path.Combine(trackerfolder, "firstrunafterupgrade.txt");

        internal static bool IsFirstRun
        {
            get
            {
                if (File.Exists(trackerfile))
                    return true;

                return false;
            }
        }

        internal static bool ClearFirstRunTracker()
        {
            if (File.Exists(trackerfile))
            {
                File.Delete(trackerfile);
                return true;
            }

            return false;
        }
    }
}
