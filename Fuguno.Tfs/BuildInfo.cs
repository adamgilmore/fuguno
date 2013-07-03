namespace Fuguno.Tfs
{
    using System;
    using System.Collections.Generic;

    public class BuildInfo
    {
        public string BuildNumber { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public DateTime LastChangeTime { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedFor { get; set; }
        public TimeSpan ElapsedTime { get { return StartTime == DateTime.MinValue ? TimeSpan.Zero : LastChangeTime - StartTime; } }
        public List<TestRunInfo> TestRunInfos { get; set; }
    }
}
