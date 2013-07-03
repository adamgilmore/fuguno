namespace Fuguno.UI.Console
{
    using Fuguno.Tfs;
    using System.Configuration;
    using System.Threading;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var service = new BuildInfoService();

            while (true)
            {
                var buildInfo = service.GetLatestBuildInfo(
                    ConfigurationManager.AppSettings["TfsServerUri"],
                    ConfigurationManager.AppSettings["TfsCollectionName"],
                    ConfigurationManager.AppSettings["TfsProjectName"],
                    ConfigurationManager.AppSettings["TfsBuildDefinitionName"]);

                TimeSpan elapsed = buildInfo.StartTime == null ? new TimeSpan(0) : buildInfo.LastChangeTime - buildInfo.StartTime;
                Console.WriteLine("{0} {1} {2} {3} {4}mins", buildInfo.BuildNumber, buildInfo.Status, buildInfo.RequestedFor, buildInfo.StartTime, elapsed.Minutes);

                Thread.Sleep(5000);
            }
        }
    }
}
