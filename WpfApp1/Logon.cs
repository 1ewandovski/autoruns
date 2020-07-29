using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;


namespace WpfApp1
{
    class Logon: BaseFunctions
    {
        private string[] RegEntry =
        {
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Userinit",
            "HKLM\\System\\CurrentControlSet\\Control\\Terminal Server\\Wds\\rdpwd\\StartupPrograms",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\AppSetup",
            "HKLM\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Startup",
            "HKCU\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Logon",
            "HKLM\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Logon",
            "HKCU\\Environment\\UserInitMprLogonScript",
            "HKLM\\Environment\\UserInitMprLogonScript",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Userinit",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\VmApplet",
            "HKLM\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Shutdown",
            "HKCU\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Logoff",
            "HKLM\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Logoff",
            "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Startup",
            "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Startup",
            "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Logon",
            "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Logon",
            "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Logoff",
            "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Logoff",
            "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Shutdown",
            "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Shutdown",
            "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\\Shell",
            "HKCU\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Shell",
            "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\\Shell",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Shell",
            "HKLM\\SYSTEM\\CurrentControlSet\\Control\\SafeBoot\\AlternateShell",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Taskman",
            "HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\AlternateShells\\AvailableShells",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\Runonce",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\RunonceEx",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server\\WinStations\\RDP-Tcp\\InitialProgram",
            "HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKCU\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKCU\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnceEx",
            "HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKCU\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKCU\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows\\Load",
            "HKCU\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows\\Run",
            "HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\Run",
            "HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\Run",
            "HKLM\\SOFTWARE\\Microsoft\\Active Setup\\Installed Components",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Active Setup\\Installed Components",
            "HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows\\IconServiceLib",
            "HKCU\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\Runonce",
            "HKCU\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\RunonceEx",
            "HKCU\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKLM\\SOFTWARE\\Microsoft\\Windows CE Services\\AutoStartOnConnect",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows CE Services\\AutoStartOnConnect",
            "HKLM\\SOFTWARE\\Microsoft\\Windows CE Services\\AutoStartOnDisconnect",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows CE Services\\AutoStartOnDisconnect",
        };
        public Logon()  //构造函数
        {
            LoadRegTable();
        }
        private void LoadRegTable() 
        {
            foreach ( string entry in RegEntry)
            {
                if(!ReadRegEntry(entry))
                    AddEntryList(true, entry, string.Empty, string.Empty, string.Empty, DateTime.Now,true); //键写入list
            }                
        }
        private bool ReadRegEntry(string RegPath)  //读一个键
        {
            int Frag1 = RegPath.IndexOf('\\');
            string RootKey = RegPath.Substring(0, Frag1); //根键
            string ChildPath = RegPath.Substring(Frag1 + 1); 
            RegistryKey Key;
            switch (RootKey)
            {
                case "HKLM":
                    Key = Registry.LocalMachine;
                    break;
                case "HKCU":
                    Key = Registry.CurrentUser;
                    break;
                default:
                    return false;
            }
            

            RegistryKey SubKey = Key.OpenSubKey(ChildPath, false);  //只读方式打开键
            //string Value = string.Empty;
            if (SubKey == null)
            {
                int Frag2 = ChildPath.LastIndexOf('\\');
                string ChildRegPath = ChildPath.Substring(0, Frag2);
                string RegValue = ChildPath.Substring(Frag2 + 1); 
                SubKey = Key.OpenSubKey(ChildRegPath, false);
                if (SubKey == null) return false;
                if (SubKey.GetValue(RegValue) == null) return false;
                string Value = SubKey.GetValue(RegValue).ToString();
                if (Value == string.Empty) return false;
                string ImagePath = GetValueContentAsPath(Value);
                if (ImagePath.IndexOf('.') == -1)
                    ImagePath += ".exe";
                //if (!System.IO.File.Exists(ImagePath))
                //    ImagePath = GetFilePathUnderSystemPath(ImagePath);
                if (ImagePath == "") return false;
                //string ValueContent = GetValueContentAsPath(Value);
                //要将Value写进返回的list
                AddEntryList(true, RegPath, string.Empty, string.Empty, string.Empty, DateTime.Now); //键写入list
                string shortcutValue = GetShortCutValue(Value);
                AddEntryList(false,shortcutValue,GetDescription(ImagePath), GetPublisher(ImagePath), ImagePath,GetFileTime(ImagePath));   //结果写入list
                return true;
            }
            else
            {
                int num = 0;                
                string[] Values = SubKey.GetValueNames();
                foreach (string valuename in Values)
                {
                    string value = SubKey.GetValue(valuename).ToString();
                    //要将value写进返回的list
                    string ImagePath = GetValueContentAsPath(value);
                    if (ImagePath.IndexOf('.') == -1)
                        ImagePath += ".exe";
                    if (ImagePath == "") continue;
                    if (num++==0) AddEntryList(true, RegPath, string.Empty, string.Empty, string.Empty, DateTime.Now); //键写入list
                    string shortcutvalue = GetShortCutValue(value);
                    AddEntryList(false,shortcutvalue, GetDescription(ImagePath), GetPublisher(ImagePath), ImagePath, GetFileTime(ImagePath));  //结果写入list
                }
                return true;
            }
        }

    }
}
