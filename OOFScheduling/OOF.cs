using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOFScheduling
{
    public class OOF
    {
        public OOFMessage primaryOOF { get; set; }
        public OOFMessage secondaryOOF { get; set; }
        public bool isPermaOOFOn { get; set; }
        public DateTime permaOOFDate { get; set; }
    }

    public class OOFMessage
    {
        public string internalMessage { get; private set; }
        public string externalMessage { get; private set; }
    }
}
