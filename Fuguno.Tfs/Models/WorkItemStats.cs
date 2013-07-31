namespace Fuguno.Tfs.Models
{
    using System.Collections.Generic;

    public class WorkItemStats
    {
        private List<WorkItemStatsSeries> _series = new List<WorkItemStatsSeries>();
        private List<string> _ticks = new List<string>();

        public List<WorkItemStatsSeries> Series { get { return _series; } set { _series = value; } }
        public List<string> Ticks { get { return _ticks; } set { _ticks = value; } }
    }

    public class WorkItemStatsSeries
    {
        private List<int> _data = new List<int>();

        public string Label { get; set; }
        public List<int> Data { get { return _data; } set { _data = value; } }
    }
}
