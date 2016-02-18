using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResDiffAnalyse
{
    public class CompareBySize : IComparer<ResInfo>
    {
        public int Compare(ResInfo x, ResInfo y)
        {
            return y.filesize.CompareTo(x.filesize);
        }

        //int IComparer<ResInfo>.Compare(ResInfo x, ResInfo y)
        //{
        //    return x.FileSize.CompareTo(y.FileSize);
        //}
    }
}
