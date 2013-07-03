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

                Console.WriteLine("S={0} RF={1} RB={2} ST={3} CT={4} FT={5} #={6}",
                    buildInfo.Status,
                    buildInfo.RequestedFor,
                    buildInfo.RequestedBy,
                    buildInfo.StartTime,
                    buildInfo.LastChangeTime,
                    buildInfo.FinishTime,
                    buildInfo.BuildNumber);

                Thread.Sleep(5000);
            }
        }
    }
}
