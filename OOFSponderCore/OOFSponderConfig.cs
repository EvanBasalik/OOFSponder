using System;

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
    //    "StartMinimized": false
    //  },
    //  "UserSettingsSource": "OOFSponder_Core_{.NET version}"
    //}

    internal class OOFData
    {
        public string WorkingHours { get; set; }
        public DateTime PermaOOFDate { get; set; }
        public string PrimaryOOFExternalMessage { get; set; }
        public string PrimaryOOFInternalMessage { get; set; }
        public string SecondaryOOFExternalMessage { get; set; }
        public string SecondaryOOFInternalMessage { get; set; }
        public Microsoft.Graph.ExternalAudienceScope ExternalAudienceScope { get; set; }
        public bool IsOnCallModeOn { get; set; }
        public bool StartMinimized { get; set; }
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
