using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace stdtool
{
    public static class tools
    {
        public static string DoReplace(this string source, IDictionary<string, string> dic)
        {
            return dic.Aggregate(source, (current, pair) => current.Replace("${" + pair.Key + "}", pair.Value));
        }
        public static string GoString(this Guid guid)
        {
            return guid.ToString().Replace("-", "");
        }
    }
}