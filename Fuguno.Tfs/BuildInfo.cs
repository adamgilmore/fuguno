using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuguno.Tfs
{
    public class BuildInfo
    {
        public string BuildNumber { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public DateTime LastChangeTime { get; set; }
        public string RequestedBy { get; set; }
    }
}
