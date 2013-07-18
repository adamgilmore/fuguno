namespace Fuguno.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    internal static class Helpers
    {
        internal static Dictionary<string, string> GetConnectionParameters(string connectionName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            var connectionParameters = connectionString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> parameterMap = new Dictionary<string, string>();
            foreach (var connectionParameter in connectionParameters)
            {
                var keyValue = connectionParameter.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                parameterMap.Add(keyValue[0].Trim(), keyValue[1].Trim());
            }

            return parameterMap;
        }
    }
}