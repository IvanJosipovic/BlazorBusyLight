using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBusyLight
{
    public class PresenceResponse
    {
        public string odatacontext { get; set; }
        public string id { get; set; }
        public string availability { get; set; }
        public string activity { get; set; }
    }
}
