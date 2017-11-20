using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOFScheduling
{
    public class OOFData
    {
        internal DateTime PermaOOFDate { get; set; }
        internal string WorkingHours { get; set; }
        internal string PrimaryOOFExternalMessage { get; set;}
        internal string PrimaryOOFInternalMessage { get; set; }
        internal string SecondaryOOFExternalMessage { get; set; }
        internal string SecondaryOOFInternalMessage { get; set; }

        private const string baseValue = "default";
        internal static string version;
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

        internal bool IsPermaOOFOn
        {
            get
            {
                return DateTime.Now < PermaOOFDate;
            }
        }

        private void ReadProperties()
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());

            instance.PermaOOFDate = OOFScheduling.Properties.Settings.Default.PermaOOFDate;
            instance.WorkingHours = OOFScheduling.Properties.Settings.Default.workingHours == baseValue ? string.Empty : Properties.Settings.Default.workingHours;
            instance.PrimaryOOFExternalMessage = OOFScheduling.Properties.Settings.Default.PrimaryOOFExternal == baseValue ? string.Empty : Properties.Settings.Default.PrimaryOOFExternal;
            instance.PrimaryOOFInternalMessage = OOFScheduling.Properties.Settings.Default.PrimaryOOFInternal == baseValue ? string.Empty : Properties.Settings.Default.PrimaryOOFInternal;
            instance.SecondaryOOFExternalMessage = OOFScheduling.Properties.Settings.Default.SecondaryOOFExternal == baseValue ? string.Empty : Properties.Settings.Default.SecondaryOOFExternal;
            instance.SecondaryOOFInternalMessage = OOFScheduling.Properties.Settings.Default.SecondaryOOFInternal == baseValue ? string.Empty : Properties.Settings.Default.SecondaryOOFInternal;
        }

        //internal string InternalOOFMessage
        //{
        //    get
        //    {
        //        //decided whether to return primary or secondary message
        //        if (!instance.IsPermaOOFOn)
        //        {
        //            return instance.PrimaryOOFInternalMessage;
        //        }
        //        else
        //        {
        //            return instance.SecondaryOOFInternalMessage;
        //        }
        //    }

        //    set
        //    {
        //        //decided whether to return primary or secondary message
        //        if (!instance.IsPermaOOFOn)
        //        {
        //            instance.PrimaryOOFInternalMessage = value;
        //        }
        //        else
        //        {
        //            instance.SecondaryOOFInternalMessage = value;
        //        }
        //    }
        //}

        //internal string ExternalOOFMessage
        //{
        //    get
        //    {
        //        //decided whether to return primary or secondary message
        //        if (!instance.IsPermaOOFOn)
        //        {
        //            return instance.PrimaryOOFExternalMessage;
        //        }
        //        else
        //        {
        //            return instance.SecondaryOOFExternalMessage;
        //        }
        //    }

        //    set
        //    {
        //        //decided whether to return primary or secondary message
        //        if (!instance.IsPermaOOFOn)
        //        {
        //            instance.PrimaryOOFExternalMessage=value;
        //        }
        //        else
        //        {
        //            instance.SecondaryOOFExternalMessage=value;
        //        }
        //    }
        //}

        ~OOFData()
        {
            Dispose(false);
        }

        public void WriteProperties(bool disposing=false)
        {
            OOFSponderInsights.TrackInfo(OOFSponderInsights.CurrentMethod());

            System.Diagnostics.Trace.TraceInformation("Persisting properties");

            Properties.Settings.Default.PrimaryOOFExternal = instance.PrimaryOOFExternalMessage;
            System.Diagnostics.Trace.TraceInformation("Persisting PrimaryOOFExternalMessage");

            Properties.Settings.Default.PrimaryOOFInternal = instance.PrimaryOOFInternalMessage;
            System.Diagnostics.Trace.TraceInformation("Persisting PrimaryOOFInternalMessage");

            Properties.Settings.Default.SecondaryOOFExternal = instance.SecondaryOOFExternalMessage;
            System.Diagnostics.Trace.TraceInformation("Persisting SecondaryOOFExternalMessage");

            Properties.Settings.Default.SecondaryOOFInternal = instance.SecondaryOOFInternalMessage;
            System.Diagnostics.Trace.TraceInformation("Persisting SecondaryOOFInternalMessage");

            Properties.Settings.Default.PermaOOFDate = instance.PermaOOFDate;
            System.Diagnostics.Trace.TraceInformation("Persisting PermaOOFDate");

            Properties.Settings.Default.workingHours = instance.WorkingHours;
            System.Diagnostics.Trace.TraceInformation("Persisting WorkingHours");

            Properties.Settings.Default.Save();

            if (disposing)
            {
                Dispose();
            }

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
            WriteProperties(disposing);
        }
    }

}
