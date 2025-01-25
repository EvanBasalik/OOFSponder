using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOFSponderConfig
{
    //{
    //  "OOFData": {
    //    "WorkingHours": "default",
    //    "PermaOOFDate": "1900-01-01",
    //    "PrimaryOOFExternalMessage": "default",
    //    "PrimaryOOFInternalMessage": "default",
    //    "SecondaryOOFExternalMessage": "default",
    //    "SecondaryOOFInternalMessage": "default",
    //    "IsOnCallModeOn": false,
    //    "StartMinimized": false
    //  },
    //  "UserSettingsSource": "OOFSponder_Core"
    //}

    public class OOFData
    {
        public string WorkingHours { get; set; }
        public DateTime PermaOOFDate { get; set; }
        public string PrimaryOOFExternalMessage { get; set; }
        public string PrimaryOOFInternalMessage { get; set; }
        public string SecondaryOOFExternalMessage { get; set; }
        public string SecondaryOOFInternalMessage { get; set; }
        public bool IsOnCallModeOn { get; set; }
        public bool StartMinimized { get; set; }
    }

    public class Root
    {
        public OOFData OOFData { get; set; }
        public string UserSettingsSource { get; set; }
    }
}
