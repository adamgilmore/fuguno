namespace Fuguno.Tfs.Models
{
    public class TestRunInfo
    {
        public int Total { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Completed { get; set; }
        public int Inconclusive { get; set; }
    }
}
