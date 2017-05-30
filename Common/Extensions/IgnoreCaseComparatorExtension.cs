using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    /// <summary>
    /// 扩展String.Contains不区分大小写
    /// </summary>
    public class IgnoreCaseComparatorExtension : IEqualityComparer<string>
    {
        public int GetHashCode(string t)
        {
            return t.GetHashCode();
        }

        public bool Equals(string x, string y)
        {
            return x.Trim().ToUpper() == y.Trim().ToUpper();
        }
    }
}
