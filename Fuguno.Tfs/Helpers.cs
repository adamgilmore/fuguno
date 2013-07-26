namespace Fuguno.Tfs
{
    using System.Text;

    internal static class Helpers
    {
        internal static string CreateAreaPathQueryFragment(string[] areaPaths)
        {
            var queryText = new StringBuilder();

            if (areaPaths != null & areaPaths.Length > 0)
            {
                queryText.Append(" AND ");

                if (areaPaths.Length > 1)
                    queryText.Append("(");

                bool prefixWithOr = false;
                foreach (var areaPath in areaPaths)
                {
                    if (prefixWithOr == true)
                        queryText.Append(" OR ");
                    else
                        prefixWithOr = true;
                    queryText.AppendFormat("[Area Path] under '{0}'", areaPath.Trim());
                }

                if (areaPaths.Length > 1)
                    queryText.Append(")");
            }

            return queryText.ToString();
        }
    }
}
