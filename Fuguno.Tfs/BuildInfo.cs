namespace Fuguno.Tfs
{
    using System;
    using System.Collections.Generic;

    public class BuildInfo
    {
        public string Name { get; set; }
        public string BuildNumber { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public DateTime LastChangeTime { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedFor { get; set; }
        public TimeSpan ElapsedTime { get { return StartTime == DateTime.MinValue ? TimeSpan.Zero : LastChangeTime - StartTime; } }
        public long TotalTestCount { get; set; }
        public long TotalTestPassedCount { get; set; }
        public long TotalTestFailedCount { get; set; }
        public long TotalTestInconclusiveCount { get; set; }
        public List<TestRunInfo> TestRunInfos { get; set; }
    }
}
