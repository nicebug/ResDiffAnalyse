using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace ResDiffAnalyse
{
    class Program
    {
        public static List<ResInfo> oldVersionResInfos = null;
        public static List<ResInfo> newVersionResInfos = null;
        //新增加
        public static List<ResInfo> newAddResInfos = null;
        //做小计
        public static List<string> sizeInfos = null;
        //3个参数 第一个 旧版本的log文件Path
        //第2个  新版本的Log文件Path
        //第3个  输出结果文件Path
        static void Main(string[] args)
        {

            string oldLogFilePath;
            string newLogFilePath;
            string resultFileDirPath;
            if (args.Length == 3)
            {
                oldLogFilePath = args[0];
                newLogFilePath = args[1];
                resultFileDirPath = args[2];
            }
            else
            {
                oldLogFilePath = System.Environment.CurrentDirectory + "/oldversionlog.txt";
                newLogFilePath = System.Environment.CurrentDirectory + "/newversionlog.txt";
                resultFileDirPath = System.Environment.CurrentDirectory + "/result";
                if (!Directory.Exists(resultFileDirPath))
                {
                    Directory.CreateDirectory(resultFileDirPath);
                } 
            }
            if ((!File.Exists(oldLogFilePath)) || (!File.Exists(newLogFilePath)))
            {
                Console.WriteLine("input file not exist!---------------");
                return;
            }
            Console.WriteLine("dealwith old log file------------->");
            GetOldResInfos(oldLogFilePath);
            oldVersionResInfos.Sort(new CompareBySize());
            Console.WriteLine("dealwith new log file-------------->");
            GetNewResInfo(newLogFilePath);
            newVersionResInfos.Sort(new CompareBySize());
            Console.WriteLine("do out-------------->");
            sizeInfos = new List<string>();
            //
            sizeInfos.Add("-------------------新版本新增信息小计-------------------------------");
            DiffOutNewAddResInfos(resultFileDirPath);
            //新旧对比-----------
            DiffOutNewAddResInfoBySuffix(".png", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".shader", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".mat", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".anim", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".FBX", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".prefab", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".bytes", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".ogg", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".cs", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".tga", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".psd", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".jpg", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".cginc", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".PNG", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".txt", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".wav", resultFileDirPath);
            DiffOutNewAddResInfoBySuffix(".cubemap", resultFileDirPath);
            //对新版本统计
            sizeInfos.Add("-------------------新版本信息资源信息小计-------------------------------");
            LogOutNewVersionAssetBySuffix(".png", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".shader", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".mat", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".anim", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".FBX", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".prefab", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".bytes", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".ogg", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".cs", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".tga", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".jpg", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".psd", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".PNG", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".cginc", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".PNG", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".txt", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".wav", resultFileDirPath);
            LogOutNewVersionAssetBySuffix(".cubemap", resultFileDirPath);
            //
            LogOutNewVersionAssetList(resultFileDirPath);
            //对比新旧总资源差异
            DiffOutNewAllSizeChange(resultFileDirPath);
            Console.WriteLine("-------changed-------------");
            //输出没有新增但大小不一致的 
            sizeInfos.Add("-------------------新版本资源修改信息小计-------------------------------");
            DiffOutChangedSizeResInfo(resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".png", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".shader", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".mat", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".anim", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".FBX", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".prefab", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".bytes", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".ogg", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".cs", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".tga", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".jpg", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".psd", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".PNG", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".cginc", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".PNG", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".txt", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".wav", resultFileDirPath);
            DiffOutChangedResInfoBySuffix(".cubemap", resultFileDirPath);
            //输出已经不存在的资源
            sizeInfos.Add("-------------------旧版本资源删除信息小计-------------------------------");
            DiffOutNewVersionNotExistResInfos(resultFileDirPath);
            //写小计
            WriteSizeInfoFile(resultFileDirPath);
            Console.WriteLine("---------done----------");
        }

        //处理旧版本日志文件获取旧版本资源列表
        public static void GetOldResInfos(string logfilePath)
        {
            oldVersionResInfos = new List<ResInfo>();
            //
            StreamReader file = new StreamReader(logfilePath);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("Assets/") && (!line.Contains("warning")) && (!line.Contains("Filename:"))
                    && (!line.Contains("\"")))
                {
                    ResInfo res = new ResInfo(line);
                    if (res.isRealRes)
                    {
                        //过滤
                        bool isExisted = false;
                        ResInfo old = null;
                        for (int j = 0; j < oldVersionResInfos.Count; j++)
                        {
                            ResInfo rr = oldVersionResInfos[j];
                            if (string.Equals(rr.resPath, res.resPath))
                            {
                                isExisted = true;
                                old = rr;
                                break;
                            }
                        }
                        //不存在就将其放入列表
                        if (isExisted == false)
                        {
                            oldVersionResInfos.Add(res);
                        }
                        else
                        {
                            //比较大小 记录最大的
                            if ((old != null) && (old.GetSize() < res.GetSize()))
                            {
                                old.logStr = line;
                            }
                        }
                    }
                }
            }
            file.Close();
            Console.WriteLine("----------------->" + oldVersionResInfos.Count);
        }


        //处理新版本日志文件 获取新版本资源列表
        public static void GetNewResInfo(string logfilePath)
        {
            newVersionResInfos = new List<ResInfo>();
            StreamReader file = new StreamReader(logfilePath);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("Assets/") && (!line.Contains("warning")) && (!line.Contains("Filename:"))
                    && (!line.Contains("\"")))
                {
                    ResInfo res = new ResInfo(line);
                    if (res.isRealRes)
                    {
                        //过滤
                        bool isExisted = false;
                        ResInfo old = null;
                        for (int j = 0; j < newVersionResInfos.Count; j++)
                        {
                            ResInfo rr = newVersionResInfos[j];
                            if (string.Equals(rr.resPath, res.resPath))
                            {
                                isExisted = true;
                                old = rr;
                                break;
                            }
                        }
                        //
                        if (isExisted == false)
                        {
                            newVersionResInfos.Add(res);
                        }
                        else
                        {
                            //比较大小 记录最大的
                            if ((old != null) && (old.GetSize() < res.GetSize()))
                            {
                                old.logStr = line;
                            }
                        }
                    }

                }
            }
            file.Close();

            Console.WriteLine("----------------->" + newVersionResInfos.Count);
        }


        //输出新版本增加的资源列表
        public static void DiffOutNewAddResInfos(string outDir)
        {
            newAddResInfos = new List<ResInfo>();
            for (int i = 0; i < newVersionResInfos.Count; i++)
            {
                ResInfo newInfo = newVersionResInfos[i];
                bool isFind = false;
                for (int j = 0; j < oldVersionResInfos.Count; j++)
                {
                    ResInfo oldInfo = oldVersionResInfos[j];
                    if (string.Equals(newInfo.resPath, oldInfo.resPath))
                    {
                        isFind = true;
                        break;
                    }
                }
                if (isFind == false)
                {
                    newAddResInfos.Add(newInfo);
                }
            }
            newAddResInfos.Sort(new CompareBySize());

            string outFilePath = outDir + "/add_all.txt";
            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }
            float totalSize = 0.0f;
            StreamWriter file = new StreamWriter(outFilePath);
            foreach (ResInfo info in newAddResInfos)
            {
                file.WriteLine(info.logStr);
                
                totalSize += info.GetSize();
            }
            string end = "共计" + newAddResInfos.Count.ToString() + "文件->大小(kb):" + totalSize.ToString();
            file.WriteLine("===================================================");
            file.WriteLine(end);
            file.WriteLine("===================================================");
            file.Flush();
            file.Close();
            sizeInfos.Add("所有新增" + end);

        }
        //根据资源后缀类型输出新版本增加的资源列表
        public static void DiffOutNewAddResInfoBySuffix(string suffix, string outDir)
        {
            List<ResInfo> resinfos = new List<ResInfo>();

            for (int i = 0; i < newAddResInfos.Count; i++)
            {
                ResInfo newInfo = newAddResInfos[i];
                bool isFind = false;
                if (newInfo.resPath.Contains(suffix))
                {
                    isFind = true;
                }
                if (isFind == true)
                {
                    resinfos.Add(newInfo);
                }
            }
            //
            string outFilePath = outDir + "/" + "add" + suffix;
            outFilePath = outFilePath.Replace(".", "_");
            if (suffix.Equals(".PNG"))
            {
                outFilePath = outFilePath + "_0.txt";
            }
            else
            {
                outFilePath = outFilePath + ".txt";
            }
           

            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }
            float totalSize = 0.0f;

            StreamWriter file = new StreamWriter(outFilePath);
            foreach (ResInfo info in resinfos)
            {
                file.WriteLine(info.logStr);
                //
                totalSize += info.GetSize();
            }
            string end = "共计" + resinfos.Count.ToString() + "文件;大小(kb):" + totalSize.ToString();
            file.WriteLine("===================================================");
            file.WriteLine(end);
            file.WriteLine("===================================================");
            file.Flush();
            file.Close();
            //
            sizeInfos.Add("新增"+suffix + end);
        }
        //小计文本输出
        public static void WriteSizeInfoFile(string outDir)
        {
            if (sizeInfos == null) return;
            string outFilePath = outDir + "/" + "小计.txt";
            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }
            StreamWriter file = new StreamWriter(outFilePath);
            file.WriteLine("===================================================");
            foreach (string info in sizeInfos)
            {
                file.WriteLine(info);
            }
            file.WriteLine("===================================================");
            file.Flush();
            file.Close();
        }
        //分析出改变了大小的 资源
        public static void DiffOutChangedSizeResInfo(string outDir)
        {
            List<ResInfo> changedInfos = new List<ResInfo>();
            for (int i = 0; i < newVersionResInfos.Count; i++)
            {
                ResInfo newInfo = newVersionResInfos[i];
                bool isFind = false;
                for (int j = 0; j < oldVersionResInfos.Count; j++)
                {
                    ResInfo oldInfo = oldVersionResInfos[j];
                    if (string.Equals(newInfo.resPath, oldInfo.resPath))
                    {
                        //不等于
                        if (Math.Abs(newInfo.GetSize() - oldInfo.GetSize()) >= 0.1f)
                        {
                            newInfo.oldSize = oldInfo.GetSize();
                            isFind = true;
                            break;
                        }
                    }
                }
                if (isFind == true)
                {
                    changedInfos.Add(newInfo);
                }
            }
            string outFilePath = outDir + "/changed_all.txt";
            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }
            float totalChangeSize = 0.0f;
            StreamWriter file = new StreamWriter(outFilePath);
            foreach (ResInfo info in changedInfos)
            {
                file.WriteLine(info.resPath + "|new:|" + info.GetSize() + "|old:|" + info.oldSize);
                totalChangeSize += (info.GetSize() - info.oldSize);
            }
            string end = "修改原版本资源文件" + changedInfos.Count.ToString() + "个；修改变化总大小(kb):" + totalChangeSize.ToString();
            file.WriteLine("===================================================");
            file.WriteLine(end);
            file.WriteLine("===================================================");
            file.Flush();
            file.Close();
            sizeInfos.Add(end);
        }
        //根据后缀 分析出改变了大小的
        public static void DiffOutChangedResInfoBySuffix(string suffix, string outDir)
        {
            List<ResInfo> changedInfos = new List<ResInfo>();
            for (int i = 0; i < newVersionResInfos.Count; i++)
            {
                ResInfo newInfo = newVersionResInfos[i];
                bool isFind = false;
                for (int j = 0; j < oldVersionResInfos.Count; j++)
                {
                    ResInfo oldInfo = oldVersionResInfos[j];
                    if (string.Equals(newInfo.resPath, oldInfo.resPath))
                    {
                        //不等于 后缀相同
                        if ((Math.Abs(newInfo.GetSize() - oldInfo.GetSize()) >= 0.1f)&&(oldInfo.resPath.Contains(suffix)))
                        {
                            newInfo.oldSize = oldInfo.GetSize();
                            isFind = true;
                            break;
                        }
                    }
                }
                if (isFind == true)
                {
                    changedInfos.Add(newInfo);
                }
            }
            string outFilePath = outDir + "/" + "changed" + suffix;
            outFilePath = outFilePath.Replace(".", "_");
            if (suffix.Equals(".PNG"))
            {
                outFilePath = outFilePath + "_0.txt";
            }
            else
            {
                outFilePath = outFilePath + ".txt";
            }
            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }
            //---
            float totalChangeSize = 0.0f;
            StreamWriter file = new StreamWriter(outFilePath);
            foreach (ResInfo info in changedInfos)
            {
                file.WriteLine(info.resPath + "|new:|" + info.GetSize() + "|old:|" + info.oldSize);
                totalChangeSize += (info.GetSize() - info.oldSize);
            }
            string end = "修改原版本"+suffix+"资源文件" + changedInfos.Count.ToString() + "个；修改变化总大小(kb):" + totalChangeSize.ToString();
            file.WriteLine("===================================================");
            file.WriteLine(end);
            file.WriteLine("===================================================");
            file.Flush();
            file.Close();
            sizeInfos.Add(end);
        }

        //分析出新版本不存在的资源列表
        public static void DiffOutNewVersionNotExistResInfos(string outDir)
        {
            List<ResInfo> notExistList=new List<ResInfo>();
            for(int i=0;i<oldVersionResInfos.Count;i++){
                ResInfo old=oldVersionResInfos[i];

                //新的里面去找 还存在不
                bool isfind=false;
                for(int j=0;j<newVersionResInfos.Count;j++){
                    ResInfo newRes=newVersionResInfos[j];
                    if(string.Equals(old.resPath,newRes.resPath)){
                        isfind=true;
                        break;
                    }
                }

                if(isfind==false){
                    notExistList.Add(old);
                }
            }
            string outFilePath = outDir + "/notexisted.txt";
            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }
            float totalChangeSize = 0.0f;
            StreamWriter file = new StreamWriter(outFilePath);
            foreach (ResInfo info in notExistList)
            {
                file.WriteLine(info.logStr);
                totalChangeSize += info.GetSize();
            }
            string end = "原版本资源文件清理" + notExistList.Count.ToString() + "个；清理掉总大小(kb):" + totalChangeSize.ToString();
            file.WriteLine("===================================================");
            file.WriteLine(end);
            file.WriteLine("===================================================");
            file.Flush();
            file.Close();
            sizeInfos.Add(end);
        }

        //统计新旧版本的资源大小差异数据
        public static void DiffOutNewAllSizeChange(string outDir)
        {
            float oldallSize = 0.0f;
           
            foreach (ResInfo info in oldVersionResInfos)
            {
                oldallSize += info.GetSize();
            }
            sizeInfos.Add("旧版本所有资源合计"+oldVersionResInfos.Count.ToString()+ "个；大小合计(kb)--->" + oldallSize.ToString());

            float newallSize = 0.0f;
            foreach (ResInfo info in newVersionResInfos)
            {
                newallSize += info.GetSize();
            }
            sizeInfos.Add("新版本所有资源合计"+ newVersionResInfos.Count.ToString()+"个；大小合计(kb)--->" + newallSize.ToString());
            sizeInfos.Add("新版本所有资源增加" + (newVersionResInfos.Count - oldVersionResInfos.Count).ToString() + "个；大小增加(kb)" + (newallSize - oldallSize).ToString());
        }

        //输出最新版本所有资源
        public static void  LogOutNewVersionAssetList(string outDir){
            string outFilePath = outDir + "/newversion_all.txt";
            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }
            float totalSize = 0.0f;

            StreamWriter file = new StreamWriter(outFilePath);
            foreach (ResInfo info in newVersionResInfos)
            {
                file.WriteLine(info.logStr);
                //
                totalSize += info.GetSize();
            }
            string end = "新版本所有共计" + newVersionResInfos.Count.ToString() + "文件;大小(kb):" + totalSize.ToString();
            file.WriteLine("===================================================");
            file.WriteLine(end);
            file.WriteLine("===================================================");
            file.Flush();
            file.Close();

            sizeInfos.Add(end);
        }
        //根据资源后缀输出新版本的资源列表
        public static void LogOutNewVersionAssetBySuffix(string suffix, string outDir)
        {
            List<ResInfo> resinfos = new List<ResInfo>();

            for (int i = 0; i < newVersionResInfos.Count; i++)
            {
                ResInfo newInfo = newVersionResInfos[i];
                bool isFind = false;
                if (newInfo.resPath.Contains(suffix))
                {
                    isFind = true;
                }
                if (isFind == true)
                {
                    resinfos.Add(newInfo);
                }
            }
            //
            string outFilePath = outDir + "/" + "newversion" + suffix;
            outFilePath = outFilePath.Replace(".", "_");
            if (suffix.Equals(".PNG"))
            {
                outFilePath = outFilePath + "_0.txt";
            }
            else
            {
                outFilePath = outFilePath + ".txt";
            }
            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }
            float totalSize = 0.0f;

            StreamWriter file = new StreamWriter(outFilePath);
            foreach (ResInfo info in resinfos)
            {
                file.WriteLine(info.logStr);
                //
                totalSize += info.GetSize();
            }
            string end = "共计" + resinfos.Count.ToString() + "文件;大小(kb):" + totalSize.ToString();
            file.WriteLine("===================================================");
            file.WriteLine(end);
            file.WriteLine("===================================================");
            file.Flush();
            file.Close();
            //
            sizeInfos.Add(suffix + end);
        }

    }
}
