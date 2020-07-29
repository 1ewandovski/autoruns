using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WpfApp1
{
    class Internet_Explorer : BaseFunctions
    {
        private string[] RegEntry =
        {
            "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects",
            "HKLM\\Software\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects",
            "HKCU\\Software\\Microsoft\\Internet Explorer\\UrlSearchHooks",
            "HKLM\\Software\\Microsoft\\Internet Explorer\\Toolbar",
            "HKLM\\Software\\Wow6432Node\\Microsoft\\Internet Explorer\\Toolbar",
            "HKCU\\Software\\Microsoft\\Internet Explorer\\Explorer Bars",
            "HKLM\\Software\\Microsoft\\Internet Explorer\\Explorer Bars",
            "HKLM\\Software\\Wow6432Node\\Microsoft\\Internet Explorer\\Explorer Bars",
            "HKCU\\Software\\Wow6432Node\\Microsoft\\Internet Explorer\\Explorer Bars",
            "HKCU\\Software\\Microsoft\\Internet Explorer\\Extensions",
            "HKLM\\Software\\Microsoft\\Internet Explorer\\Extensions",
            "HKCU\\Software\\Wow6432Node\\Microsoft\\Internet Explorer\\Extensions",
            "HKLM\\Software\\Wow6432Node\\Microsoft\\Internet Explorer\\Extensions",
        };
        private string ActualEntry = "SOFTWARE\\Classes\\CLSID\\";
        public Internet_Explorer()  //构造函数
        {
            LoadRegTable();
        }
        private void LoadRegTable()
        {
            foreach (string entry in RegEntry)
            {
                if (!ReadRegEntry(entry))
                    AddEntryList(true, entry, string.Empty, string.Empty, string.Empty, DateTime.Now,true);  //键写入list
            }                
        }
        private bool ReadRegEntry(string RegPath)  //读一个键
        {
            int Frag1 = RegPath.IndexOf('\\');
            string RootKey = RegPath.Substring(0, Frag1); //根键
            string ChildPath = RegPath.Substring(Frag1 + 1);
            RegistryKey Key;
            //bool flag;
            switch (RootKey)
            {
                case "HKLM":
                    {
                        Key = Registry.LocalMachine;
                        //flag = true;
                        break;
                    }
                case "HKCU":
                    {
                        Key = Registry.CurrentUser;
                        //flag = false;
                        break;
                    }
                default:
                    return false;
            }
            

            RegistryKey SubKey = Key.OpenSubKey(ChildPath, false);  //只读方式打开键
            //string Value = string.Empty;
            if (SubKey == null)
            {
                return false;
            }
            else
            {
                //string[] Values = SubKey.GetKeyNames();
                string[] keynames = SubKey.GetSubKeyNames();
                int num = 0;
                foreach (string keyname in keynames)
                {
                    //string DllRegEntry = ActualEntry + keyname + "\\InprocServer32";
                    //string DllRegEntry = ActualEntry + keyname;
                    //CheckCLSID(keyname);
                    string DllRegEntry = ActualEntry + CheckCLSID(keyname);
                    //AddEntryList(false, keyname, string.Empty, string.Empty, DateTime.Now);
                    if (num++==0)
                        AddEntryList(true, RegPath, string.Empty, string.Empty, string.Empty, DateTime.Now);
                    GetCLISD(DllRegEntry);                    
                }
                return true;
            }
        }
        private void GetCLISD(string entryname) //必须要考虑CLISD长度,目前还没有考虑，并且entryname还只是dll的名字
        {
            RegistryKey Key= Registry.LocalMachine;
            //if (isHKLM) Key = Registry.LocalMachine;
            //else Key = Registry.CurrentUser;
            string keyname = entryname + "\\InprocServer32";
            RegistryKey SubKey=Key.OpenSubKey(keyname,false);
            RegistryKey SubEntryKey = Key.OpenSubKey(entryname, false);
            if (SubKey == null) return ;
            else
            {
                //foreach (string value in SubKey.GetValueNames())
                //{
                    object s = SubEntryKey.GetValue("");
                    object o = SubKey.GetValue("");
                //string Value = SubKey.GetValue("InprocServer32").ToString();
                //if (Value == string.Empty) return;
                    if (s == null) return ;
                    if (o == null) return ;
                    //string ImagePath = o.ToString();
                    string ImagePath = GetValueContentAsPath(o.ToString());
                    //ImagePath = GetPathInSystem(ImagePath);
                    if (ImagePath == string.Empty) return ;
                //string disp = MUI.LoadMuiStringValue(SubKey, "");
                //AddEntryList(false, GetShortCutValue(ImagePath), GetDescription(ImagePath), ImagePath, GetFileTime(ImagePath));
                AddEntryList(false, s.ToString(), GetDescription(ImagePath), GetPublisher(ImagePath),ImagePath, GetFileTime(ImagePath));
                //}
            }
        }
        private string CheckCLSID(string clsid)   //检测CLSID的位数，如果大于16字节，提示恶意信息的警告
        {
            string temp = clsid.Substring(0,37);
            //AddEntryList(false, (temp + @"}"), string.Empty, string.Empty, DateTime.Now);
            if ((temp + @"}") == clsid) return (temp + @"}");
            else {
                AddEntryList(false, "ALARM!!!next line might be MALICIOUS DLL", string.Empty, string.Empty,string.Empty, DateTime.Now);
                return (temp + @"}");
            }
            //return temp;
        }
    }
}
