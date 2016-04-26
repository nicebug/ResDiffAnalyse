using System.Collections.Generic;

namespace ResDiffAnalyse
{
    public class CompareBySize : IComparer<ResInfo>
    {
        //排序
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
