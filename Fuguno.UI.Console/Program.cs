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

                Console.WriteLine("BuildNumber={0} Status={1} RequestedBy={2} StartTime={3} LastChangeTime={4} FinishTime={5}",
                    buildInfo.BuildNumber,
                    buildInfo.Status,
                    buildInfo.RequestedBy,
                    buildInfo.StartTime,
                    buildInfo.LastChangeTime,
                    buildInfo.FinishTime);

                Thread.Sleep(5000);
            }
        }
    }
}
