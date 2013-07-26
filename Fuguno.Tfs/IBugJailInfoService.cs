namespace Fuguno.Tfs
{
    using System.Collections.Generic;

    public interface IBugJailInfoService
    {
        IEnumerable<BugJailInfo> GetBugJail(string[] areaPaths);
    }
}
