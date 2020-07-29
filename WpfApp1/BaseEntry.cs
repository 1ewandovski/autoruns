using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace WpfApp1
{
    internal class BaseEntry
    {
        public BaseEntry(bool _isentry,string _entryname,string _description,string _publisher,string _imagepath,DateTime _time,bool _isempty) //每一行的内容
        {
            IsEntry = _isentry;
            EntryName = _entryname;
            Description = _description;
            Publisher = _publisher;
            ImagePath = _imagepath;
            Time = _time;
            IsEmpty = _isempty;
        }
        public bool IsEntry { get; set; }
        public string EntryName { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string ImagePath { get; set; }
        public DateTime Time { get; set; }
        public bool IsEmpty { get; set; }
        
    }
    internal class BaseFunctions
    {
        public ArrayList list = new ArrayList();
        /*protected string GetFileName(string filename)
        {
            return string.Empty;
        }*/

        protected string GetDescription(string file)    //获取描述
        {
            if (!System.IO.File.Exists(file)) return string.Empty;
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(file);
            
            return info.FileDescription;
        }

        protected string GetSysPath(string path)
        {
            string res = path;
            if (System.IO.File.Exists(res)) return res;
            //if (res.StartsWith("C:\\windows\\")) return res;
            //if (res.StartsWith("System32")) return "C:\\windows\\" + res;
            //if (res.StartsWith("system32")) return "C:\\windows\\" + res;
            if (res.StartsWith("System32")) res= "C:\\Windows\\" + res;
            else if (res.StartsWith("system32")) res= "C:\\Windows\\" + res;
            if (System.IO.File.Exists(res)) return res;
            /*if (res.StartsWith("\\SystemRoot"))
            {
                //int frag = res.IndexOf("\\SystemRoot");
                return "C:\\windows\\" + res.Substring(11);
            }*/
            else return res;
        }

        protected void AddEntryList(bool isentry,string entryname, string description,string publisher, string imagepath, DateTime time,bool isempty=false)  //添加进list，默认为false
        {
            list.Add(new BaseEntry(isentry,entryname,description,publisher,imagepath, time,isempty));
        }

        protected DateTime GetFileTime(string path) //获取修改时间
        {
            FileInfo f = new FileInfo(path);
           
            return f.LastWriteTime;
        }

        protected string GetPathInSystem(string value)  //获取系统路径
        {
            if (System.IO.File.Exists(value)) return value;
            string path = Environment.GetEnvironmentVariable("PATH");
            string[] systemPaths = path.Split(';');
            foreach (string leadingFolder in systemPaths)
            {
                string maybePath = leadingFolder + '\\' + value;
                if (System.IO.File.Exists(maybePath))
                    return maybePath;
            }
            return "";
        }
        protected string GetShortCutValue(string value)  //获取缩写
        {
            int frag1 = value.LastIndexOf('\\');
            int frag2 = value.LastIndexOf('\"');
            string result = string.Empty;
            if (frag1 < frag2)
                result = value.Substring(frag1 + 1, frag2-frag1-1);
            else
                result = value.Substring(frag1 + 1);
            return result;
        }

        protected string GetPublisher(string path)  //获取发布者
        {
            try
            {
                X509Certificate2 crt = new X509Certificate2(path);
                string subject = crt.Subject;
                int frag1 = subject.IndexOf("CN=") + 3;
                int frag2 = subject.IndexOf("=", frag1);
                while (subject[frag2--] != ',') ;
                return subject.Substring(frag1, frag2 - frag1 + 1).Trim('\"');
                //return subject;
            }
            catch (Exception e)
            {

            }
            return "no signature";
        }
        // Get the normal file path out of the Registry Key Value Content
        protected string GetValueContentAsPath(string value)  //获取路径
        {
            string res = value;
            // strip from double quote
            if (res != string.Empty && res[0] == '\"')
                res = value.Substring(1, value.LastIndexOf('\"') - 1);

            // strip parameters at the end
            if (!res.EndsWith(".exe") && res.Contains(".exe"))
                res = res.Substring(0, res.IndexOf(".exe") + 4);

            // strip strange characters in the beginning
            if (res.StartsWith("\\??\\")) res = res.Substring(4);

            if (res.StartsWith(@"\")) res = res.Substring(1);
            if (res.IndexOf(@"SystemRoot", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string SystemRoot = Environment.GetEnvironmentVariable("SystemRoot");
                res = SystemRoot + res.Substring(res.IndexOf(@"\"));
            }

            // deal with environment variables
            if (res.StartsWith(@"%"))
            {
                int index = res.IndexOf(@"%", 1);
                string envVar = res.Substring(1, index - 1);
                string envVarValue = Environment.GetEnvironmentVariable(envVar);
                res = envVarValue + res.Substring(index + 1);
            }

            return res;
        }

        /*public ArrayList Dump()  //将list中的对象转为字符串输出
        {
            //string result = string.Empty;
            ArrayList templist = new ArrayList();
            foreach (BaseEntry element in list)
            {
                if (element.IsEntry)
                templist.Add(element.EntryName + '\t'+ element.Time);
                else
                templist.Add('\t'+element.EntryName + '\t' +element.Description+'\t'+element.ImagePath+'\t'+ element.Time);
            }
            return templist;
        }*/
        public ArrayList DumpV1(bool flag)  //将list中的对象转为字符串输出
        {
            //string result = string.Empty;
            ArrayList templist = new ArrayList();
            foreach (BaseEntry element in list)
            {
                if (flag && element.IsEmpty) continue;
                if (element.IsEntry)
                {
                    templist.Add(element.EntryName);
                    templist.Add(element.Time);
                    templist.Add(string.Empty);
                    templist.Add(string.Empty);
                    templist.Add(string.Empty);
                }
                else
                {
                    templist.Add(element.EntryName);
                    templist.Add(element.Description);
                    templist.Add(element.Publisher);
                    templist.Add(element.ImagePath);
                    templist.Add(element.Time);                    
                }
            }
            return templist;
        }
        /*public ArrayList Dump(bool isDriver)
        {
            ArrayList templist = new ArrayList();
            foreach(BaseEntry element in list)
            {
                if (element.IsEntry)
                    templist.Add(element.EntryName + '\t' + element.Time);
                else if (IsEndWithSYS(element.ImagePath)&&isDriver)   //判断是否是driver                
                    templist.Add('\t' + element.EntryName + '\t' + element.Description + '\t' + element.ImagePath + '\t' + element.Time);                
                else if (!IsEndWithSYS(element.ImagePath) && !isDriver)  //判断是否是服务
                    templist.Add('\t' + element.EntryName + '\t' + element.Description + '\t' + element.ImagePath + '\t' + element.Time);
            }
            return templist;
        }*/
        public ArrayList DumpV2(bool isDriver,bool flag)  //dump for services/drivers
        {
            ArrayList templist = new ArrayList();
            foreach (BaseEntry element in list)
            {
                if (element.IsEntry)
                {
                    templist.Add(element.EntryName);
                    templist.Add(element.Time);
                    templist.Add(string.Empty);
                    templist.Add(string.Empty);
                    templist.Add(string.Empty);
                }
                else if (IsEndWithSYS(element.ImagePath) && isDriver)   //判断是否是driver    
                {
                    if (element.Publisher.IndexOf("Microsoft Windows")>=0 &&flag) continue;
                    templist.Add(element.EntryName);
                    templist.Add(element.Description);
                    templist.Add(element.Publisher);
                    templist.Add(element.ImagePath);
                    templist.Add(element.Time);
                }
                else if (!IsEndWithSYS(element.ImagePath) && !isDriver)  //判断是否是服务
                {
                    if ((element.Publisher.IndexOf("Microsoft Windows") >= 0|| element.Publisher.IndexOf("no signature") >= 0 )&& flag) continue;
                    //if (element.Publisher.IndexOf("no signature") >= 0) continue;
                    templist.Add(element.EntryName);
                    templist.Add(element.Description);
                    templist.Add(element.Publisher);
                    templist.Add(element.ImagePath);
                    templist.Add(element.Time);
                }
            }
            return templist;
        }

        private bool IsEndWithSYS(string path)
        {            
            int frag = path.LastIndexOf(".");
            string exterm = path.Substring(frag + 1);
            if (exterm == "sys") return true;
            else return false;
        }
    }
}
