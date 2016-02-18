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
        public string logStr;
        // like this: Assets/UI/Atlases/dwComic/dwComic.png
        public string resPath; 
        
        public bool isRealRes = false;
        public float oldSize = 0.0f;

        public float filesize = 0;
        
        public ResInfo(string oriLog)
        {
            logStr = oriLog;
            //解析 16.0 mb	 7.7% Assets/UI/Atlases/dwComic/dwComic.png
            int pathIndex = logStr.IndexOf("Assets/", 0);
            resPath = logStr.Substring(pathIndex);
            //
            JudgeIsRelRes();
            filesize = GetSize();
        }

        //public float FileSize
        //{
        //    get { return filesize; }
        //    set { filesize = GetSize(); }
        //}

        private void JudgeIsRelRes()
        {
            isRealRes = (logStr.Contains(".cs") ||
                logStr.Contains(".ogg") ||
                logStr.Contains(".txt") ||
                logStr.Contains(".bytes") ||
                logStr.Contains(".prefab") ||
                logStr.Contains(".png") ||
                logStr.Contains(".anim") ||
                logStr.Contains(".FBX") ||
                logStr.Contains(".anim") ||
                logStr.Contains(".wav") ||
                logStr.Contains(".PNG") ||
                logStr.Contains(".psd") ||
                logStr.Contains(".jpg") ||
                logStr.Contains(".cubemap") ||
                logStr.Contains(".shader") ||
                logStr.Contains(".mat") ||
                logStr.Contains(".tga") ||
                logStr.Contains(".cginc")) &&
                ((logStr.Contains("kb") || (logStr.Contains("mb")))
                && (logStr.Contains("%"))
                );
        }

        public bool IsSameRes(ResInfo other)
        {
            bool retValue = false;
            if (string.Equals(this.resPath, other.resPath))
            {
                retValue = true;
            }
            return retValue;
        }

        public float GetSize()
        {
            float retValue = 0.0f;
            if (logStr.Contains("kb"))
            {
                int kbIndex = logStr.IndexOf("kb");
                string sizeStr = logStr.Substring(0, kbIndex);
                if (float.TryParse(sizeStr, out retValue))
                {
                    return retValue;
                }
                else
                {
                    retValue = 0.0f;
                }

            }
            if (logStr.Contains("mb"))
            {
                int kbIndex = logStr.IndexOf("mb");
                string sizeStr = logStr.Substring(0, kbIndex);
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
