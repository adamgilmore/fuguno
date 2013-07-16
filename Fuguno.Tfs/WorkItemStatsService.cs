namespace Fuguno.Tfs
{
    using System.Text;

    public class WorkItemStatsService : IWorkItemStatsService
    {
        //private const string WorkItemCountQueryTemplate = "SELECT [Title] FROM WorkItems WHERE [Work Item Type] = '{0}'";

        //public long GetWorkItemCount(string workItemType, string[] areaPaths, string[] states)
        //{
        //    long workItemCount = 0;

        //    var queryText = new StringBuilder(string.Format(WorkItemCountQueryTemplate, workItemType));

        //    if (areaPaths != null & areaPaths.Length > 0)
        //    {
        //        if (areaPaths.Length > 1)
        //            queryText.Append("(");

        //        bool prefixWithOr = false;
        //        foreach (var areaPath in areaPaths)
        //        {
        //            if (prefixWithOr == true)
        //                queryText.Append(" OR ");
        //            else
        //                prefixWithOr = true;
        //            queryText.AppendFormat("[Area Path] under '{0}'", areaPath.Trim());
        //        }

        //        if (areaPaths.Length > 1)
        //            queryText.Append(")");
        //    }

        //    return workItemCount;
        //}
    }
}
