using System;
using System.Collections.ObjectModel;

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
    //    "ExternalAudienceScopeEnum": "All",
    //    "IsOnCallModeOn": false,
    //    "StartMinimized": false,
    //    "UseNewOOFMath": false
    //  },
    //  "UserSettingsSource": "OOFSponder_Core_{.NET version}"
    //}

    public class WorkingDayCollection
    {
        public string DayOfWeek { get; set; }
        public bool IsOnCallModeEnabled { get; set; }
        public bool IsOOF { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    internal class OOFData
    {
        public const string DeprecatedValue = "DeprecatedSetting";
        public string WorkingHours { get; set; }
        public DateTime PermaOOFDate { get; set; }
        public string PrimaryOOFExternalMessage { get; set; }
        public string PrimaryOOFInternalMessage { get; set; }
        public string SecondaryOOFExternalMessage { get; set; }
        public string SecondaryOOFInternalMessage { get; set; }
        public Microsoft.Graph.ExternalAudienceScope ExternalAudienceScope { get; set; }
        public bool IsOnCallModeOn { get; set; }
        public bool StartMinimized { get; set; }

        public bool UseNewOOFMath { get; set; }
        public Collection<OOFScheduling.WorkingDay> WorkingDayCollection { get; set; }
    }

    internal class Root
    {
        OOFData _OOFData;
        public OOFData OOFData
        {
            get
            {
                if (_OOFData == null)
                {
                    _OOFData = new OOFData();
                }

                return _OOFData;
            }
            set
            {
                _OOFData = value;
            }
        }

        //always emit OOFSponder_Core to indicate the new .NET Core variant
        public string UserSettingsSource { get; set; }
    }
}
