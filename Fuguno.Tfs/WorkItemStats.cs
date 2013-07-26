using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuguno.Tfs
{
    public class WorkItemStat
    {
        public string Key { get; set; }
        public int Count { get; set; }
    }

    public class WorkItemStats
    {
        public IEnumerable<WorkItemStat> Data { get; set; }
    }
}
