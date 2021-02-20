using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.Helpers
{
    public static class ExtensionMethods
    {
        public static string ToBold(this string str)
        {
            str = $"**{str}**";
            return str;
        }
    }
}
