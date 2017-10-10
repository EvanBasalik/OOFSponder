using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOFScheduling
{
    public static class OOFData
    {
        public static OOFMessage PrimaryOOF { internal get; set; }
        public static OOFMessage SecondaryOOF { internal get; set; }
        public static bool IsPermaOOFOn { get; set; }
        public static DateTime PermaOOFDate { get; set; }
        public static string WorkingHours { get; set; }

        public static string InternalOOFMessage
        {
            get
            {
                //decided whether to return primary or secondary message
                if (!IsPermaOOFOn)
                {
                    return PrimaryOOF.internalMessage;
                }
                else
                {
                    return SecondaryOOF.internalMessage;
                }
            }
        }

        public static string ExternalOOFMessage
        {
            get
            {
                //decided whether to return primary or secondary message
                if (!IsPermaOOFOn)
                {
                    return PrimaryOOF.externalMessage;
                }
                else
                {
                    return SecondaryOOF.externalMessage;
                }
            }
        }
    }

    public class OOFMessage
    {
        public string internalMessage { get; set; }
        public string externalMessage { get; set; }
    }
}
