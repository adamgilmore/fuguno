using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuguno.Tfs
{
    public class WorkItemInfo
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Severity { get; set; }
        public string AssignedTo { get; set; }
    }
}
