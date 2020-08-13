using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIPFSWatcher
{
    /// <summary>
    ///     https://stackoverflow.com/a/15406229
    /// </summary>
    public static class StringExt
    {
        public static string Left(this string @this, int count)
        {
            if (@this.Length <= count)
            {
                return @this;
            }
            else
            {
                return @this.Substring(0, count);
            }
        }
    }
}
