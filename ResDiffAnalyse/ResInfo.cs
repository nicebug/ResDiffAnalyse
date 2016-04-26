using System;
using System.Collections.Generic;
using System.Text;


namespace ResDiffAnalyse
{
    //unitylog 文件里面的一条记录
    //这种 16.0 mb	 7.7% Assets/UI/Atlases/dwComic/dwComic.png
    public class ResInfo
    {
        // like this: 16.0 mb	 7.7% Assets/UI/Atlases/dwComic/dwComic.png
        public string LogStr;
        // like this: Assets/UI/Atlases/dwComic/dwComic.png
        public string ResPath; 
        
        public bool IsRealRes = false;
        public float OldSize = 0.0f;

        public float Filesize = 0;
        
        public ResInfo(string oriLog)
        {
            LogStr = oriLog;
            //解析 16.0 mb	 7.7% Assets/UI/Atlases/dwComic/dwComic.png
            int pathIndex = LogStr.IndexOf("Assets/", 0, StringComparison.Ordinal);
            ResPath = LogStr.Substring(pathIndex);
            //
            JudgeIsRelRes();
            Filesize = GetSize();
        }

        //public float FileSize
        //{
        //    get { return Filesize; }
        //    set { Filesize = GetSize(); }
        //}

        private void JudgeIsRelRes()
        {
            IsRealRes = (LogStr.Contains(".cs") ||
                LogStr.Contains(".ogg") ||
                LogStr.Contains(".txt") ||
                LogStr.Contains(".bytes") ||
                LogStr.Contains(".prefab") ||
                LogStr.Contains(".png") ||
                LogStr.Contains(".anim") ||
                LogStr.Contains(".FBX") ||
                LogStr.Contains(".anim") ||
                LogStr.Contains(".wav") ||
                LogStr.Contains(".PNG") ||
                LogStr.Contains(".psd") ||
                LogStr.Contains(".jpg") ||
                LogStr.Contains(".cubemap") ||
                LogStr.Contains(".shader") ||
                LogStr.Contains(".mat") ||
                LogStr.Contains(".tga") ||
                LogStr.Contains(".cginc")) &&
                ((LogStr.Contains("kb") || (LogStr.Contains("mb")))
                && (LogStr.Contains("%"))
                );
        }

        public bool IsSameRes(ResInfo other)
        {
            bool retValue = string.Equals(ResPath, other.ResPath);
            return retValue;
        }

        public float GetSize()
        {
            var retValue = 0.0f;
            if (LogStr.Contains("kb"))
            {
                int kbIndex = LogStr.IndexOf("kb", StringComparison.Ordinal);
                string sizeStr = LogStr.Substring(0, kbIndex);
                if (float.TryParse(sizeStr, out retValue))
                {
                    return retValue;
                }
                else
                {
                    retValue = 0.0f;
                }

            }
            if (LogStr.Contains("mb"))
            {
                int kbIndex = LogStr.IndexOf("mb", StringComparison.Ordinal);
                string sizeStr = LogStr.Substring(0, kbIndex);
                if (float.TryParse(sizeStr, out retValue))
                {
                    return retValue * 1024.0f;
                }
                else
                {
                    retValue = 0.0f;
                }
            }
            return retValue;
        }

    }
}
