using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdall_extensions
{
    public static class Extensions
    {
        public static string CSVSafe(this string value)
        {
            if (value.Contains("\""))
            {
                value = value.Replace("\"", "\"\"");
                value = string.Format("\"{0}\"", value);
            }
            else if (value.Contains(",") || value.Contains(System.Environment.NewLine))
            {
                value = string.Format("\"{0}\"", value);
            }
            else if (value.StartsWith("0"))
            {
                value = string.Format("\'{0}\'", value);
            }


            return value;
        }
    }
}
