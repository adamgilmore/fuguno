namespace Fuguno.Tfs
{
    using Microsoft.TeamFoundation.Server;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml;

    public class IterationInfoService : IIterationInfoService
    {
        string _tfsProjectName;
        string _tfsRootIterationPath;
        ICommonStructureService4 _commonStructureService;

        public IterationInfoService(string tfsServerUri, string tfsCollectionName, string tfsProjectName, string tfsRootIterationPath)
        {
            _tfsProjectName = tfsProjectName;
            _tfsRootIterationPath = tfsRootIterationPath;
            var tfsCollection = new TfsCollection(tfsServerUri, tfsCollectionName);
            _commonStructureService = tfsCollection.CommonStructureService;
        }

        public IterationInfo GetCurrentIterationInfo()
        {
            Trace.TraceInformation(string.Format("{0} -> GetCurrentIterationInfo", DateTime.Now));

            try
            {
                IterationInfo iterationInfo = null;

                var projectInfo = _commonStructureService.GetProjectFromName(_tfsProjectName);
                NodeInfo[] structures = _commonStructureService.ListStructures(projectInfo.Uri);
                var projectLifecycleInfo = structures.FirstOrDefault(n => n.StructureType.Equals("ProjectLifecycle"));
                var iterationXmlElement = _commonStructureService.GetNodesXml(new string[] { projectLifecycleInfo.Uri }, true);
                var rootIterationXmlNode = iterationXmlElement.SelectSingleNode(string.Format("//Node[@Path='{0}']", _tfsRootIterationPath));
                if (rootIterationXmlNode == null)
                {
                    Trace.TraceError(string.Format("{0} ! GetCurrentIterationInfo - can't find iterationPath='{1}'", DateTime.Now, _tfsRootIterationPath));
                }
                else
                {
                    List<IterationInfo> iterationInfos = new List<IterationInfo>();
                    GetIterationDates(rootIterationXmlNode, _tfsProjectName, ref iterationInfos);
                    if (iterationInfos != null)
                    {
                        iterationInfo = iterationInfos.FirstOrDefault(n => DateTime.Now >= n.StartDate && DateTime.Now <= n.EndDate);
                    }
                }
                return iterationInfo;
            }
            finally
            {
                Trace.TraceInformation(string.Format("{0} <- GetCurrentIterationInfo", DateTime.Now));
            }
        }

        private static void GetIterationDates(XmlNode node, string projectName, ref List<IterationInfo> iterationInfos)
        {
            if (node != null)
            {
                string iterationPath = node.Attributes["Path"].Value;
                if (!string.IsNullOrEmpty(iterationPath))
                {
                    // Attempt to read the start and end dates if they exist.
                    string strStartDate = (node.Attributes["StartDate"] != null) ? node.Attributes["StartDate"].Value : null;
                    string strEndDate = (node.Attributes["FinishDate"] != null) ? node.Attributes["FinishDate"].Value : null;

                    DateTime startDate, endDate;

                    if (!string.IsNullOrEmpty(strStartDate) && !string.IsNullOrEmpty(strEndDate))
                    {
                        bool datesValid = true;
 
                        // Both dates should be valid.
                        datesValid &= DateTime.TryParse(strStartDate, out startDate);
                        datesValid &= DateTime.TryParse(strEndDate, out endDate);

                        if (datesValid == true)
                        {
                            iterationInfos.Add(new IterationInfo
                            {
                                Name = node.Attributes["Name"].Value,
                                Path = iterationPath.Replace(string.Concat("\\", projectName, "\\Iteration"), projectName),
                                StartDate = startDate,
                                EndDate = endDate                       
                            });

                        }
                    }
             
                }
 
                // Visit any child nodes (sub-iterations).
                if (node.FirstChild != null)
                {
                    // The first child node is the <Children> tag, which we'll skip.
                    for (int nChild = 0; nChild < node.ChildNodes[0].ChildNodes.Count; nChild++)
                        GetIterationDates(node.ChildNodes[0].ChildNodes[nChild], projectName, ref iterationInfos);
                }
            }
        }
    }
}
