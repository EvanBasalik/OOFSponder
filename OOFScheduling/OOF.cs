using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOFScheduling
{
    public class OOFData
    {
        internal bool IsPermaOOFOn { get; set; }
        internal DateTime PermaOOFDate { get; set; }
        internal string WorkingHours { get; set; }
        internal string PrimaryOOFExternalMessage { get; set;}
        internal string PrimaryOOFInternalMessage { get; set; }
        internal string SecondaryOOFExternalMessage { get; set; }
        internal string SecondaryOOFInternalMessage { get; set; }

        private const string baseValue = "default";
        static OOFData instance;

        public static OOFData Instance
        {
            get
            {

                if (instance == null)
                {
                    instance = new OOFData();
                    instance.ReadProperties();
                }
                return instance;
            }
        }

        private void ReadProperties()
        {
            instance.IsPermaOOFOn = OOFScheduling.Properties.Settings.Default.IsPermaOOFOn;
            instance.PermaOOFDate = OOFScheduling.Properties.Settings.Default.PermaOOFDate;
            instance.WorkingHours = OOFScheduling.Properties.Settings.Default.workingHours == baseValue ? string.Empty : Properties.Settings.Default.workingHours;
            instance.PrimaryOOFExternalMessage = OOFScheduling.Properties.Settings.Default.PrimaryOOFExternal == baseValue ? string.Empty : Properties.Settings.Default.PrimaryOOFExternal;
            instance.PrimaryOOFInternalMessage = OOFScheduling.Properties.Settings.Default.PrimaryOOFInternal == baseValue ? string.Empty : Properties.Settings.Default.PrimaryOOFInternal;
            instance.SecondaryOOFExternalMessage = OOFScheduling.Properties.Settings.Default.SecondaryOOFExternal == baseValue ? string.Empty : Properties.Settings.Default.SecondaryOOFExternal;
            instance.SecondaryOOFInternalMessage = OOFScheduling.Properties.Settings.Default.SecondaryOOFInternal == baseValue ? string.Empty : Properties.Settings.Default.SecondaryOOFInternal;
        }

        internal string InternalOOFMessage
        {
            get
            {
                //decided whether to return primary or secondary message
                if (!instance.IsPermaOOFOn)
                {
                    return instance.PrimaryOOFInternalMessage;
                }
                else
                {
                    return instance.SecondaryOOFInternalMessage;
                }
            }
        }

        internal string ExternalOOFMessage
        {
            get
            {
                //decided whether to return primary or secondary message
                if (!instance.IsPermaOOFOn)
                {
                    return instance.PrimaryOOFExternalMessage;
                }
                else
                {
                    return instance.SecondaryOOFExternalMessage;
                }
            }
        }

        ~OOFData()
        {
            Dispose(false);
        }

        public void WriteProperties()
        {
            System.Diagnostics.Trace.TraceInformation("Persisted properties");

            Properties.Settings.Default.PrimaryOOFExternal = instance.PrimaryOOFExternalMessage;
            Properties.Settings.Default.PrimaryOOFInternal = instance.PrimaryOOFInternalMessage;
            Properties.Settings.Default.SecondaryOOFExternal = instance.SecondaryOOFExternalMessage;
            Properties.Settings.Default.SecondaryOOFInternal = instance.SecondaryOOFInternalMessage;
            Properties.Settings.Default.IsPermaOOFOn = instance.IsPermaOOFOn;
            Properties.Settings.Default.PermaOOFDate = instance.PermaOOFDate;
            Properties.Settings.Default.workingHours = instance.WorkingHours;
            Properties.Settings.Default.Save();
            Dispose();
        }

        #region IDisposable Members

        public void Dispose()
        {

            Dispose(true);

            // Turn off calling the finalizer
            System.GC.SuppressFinalize(this);

        }

        #endregion

        public void Dispose(bool disposing)
        {
            // Do not dispose of an owned managed object (one with a
            // finalizer) if called by member finalize,
            // as the owned managed objects finalize method
            // will be (or has been) called by finalization queue
            // processing already
            WriteProperties();
        }
    }

}
