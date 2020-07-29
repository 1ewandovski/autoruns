using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WpfApp1
{
    class Services : BaseFunctions  //能查到services 和drivers
    {
        private const string RegEntry = "System\\CurrentControlSet\\Services";

        public Services()
        {
            LoadServices();
        }
        private void LoadServices()
        {
            AddEntryList(true, RegEntry, string.Empty, string.Empty, string.Empty, DateTime.Now);
            RegistryKey Key = Registry.LocalMachine;
            RegistryKey SubKey = Key.OpenSubKey(RegEntry, false);
            string[] ChildKeys = SubKey.GetSubKeyNames();
            foreach (string keyname in ChildKeys)
            {
                string ServiceEntryPara = RegEntry + @"\" + keyname + @"\Parameters";  //service下的parameter子键
                string ServiceEntryname = RegEntry + @"\" + keyname;   //service
                RegistryKey serviceparakey = Key.OpenSubKey(ServiceEntryPara, false);
                RegistryKey servicekey = Key.OpenSubKey(ServiceEntryname, false);
                if (servicekey == null) continue;
                string ImagePath = string.Empty;
                if (serviceparakey == null)  //parameter打开失败
                {
                    
                    object temp = servicekey.GetValue("ImagePath");
                    if (temp == null) continue;
                    //string temp = servicekey.GetValue("ImagePath").ToString();
                    //if (temp == string.Empty) continue;
                    ImagePath = GetValueContentAsPath(temp.ToString());
                    ImagePath = GetPathInSystem(ImagePath);  //获取系统盘下路径
                    if (ImagePath == string.Empty) continue;
                }
                else  //parameter打开成功
                {
                    object temp = serviceparakey.GetValue("serviceDLL");
                    //string temp = serviceparakey.GetValue("serviceDLL").ToString();
                    if (temp == null) //parameter下无有用信息,通常是sys
                    {                    
                        object o = servicekey.GetValue("ImagePath");
                        if (o == null) continue;
                        ImagePath = GetValueContentAsPath(o.ToString());
                        //ImagePath = GetPathInSystem(ImagePath);  //获取系统盘下路径
                        ImagePath = GetSysPath(ImagePath);
                        if (ImagePath == string.Empty) continue;
                    }
                    else  //parameter下有有用信息
                    {
                        object o = serviceparakey.GetValue("serviceDLL");
                        //string temp = serviceparakey.GetValue("serviceDLL").ToString();
                        if (o == null) continue;
                        ImagePath = GetValueContentAsPath(o.ToString());
                        if (ImagePath == string.Empty) continue;
                    }
                }
                object ob = servicekey.GetValue("DisplayName");
                string disp = ob == null ? "" : ob.ToString();
                if (disp.StartsWith("@"))   //at 开头的都是dll
                    disp = MUI.LoadMuiStringValue(servicekey, "DisplayName");
                if (disp == string.Empty) disp = keyname;
                disp += ":";

                ob = servicekey.GetValue("Description");
                string desc = ob == null ? "" : ob.ToString();
                if (desc.StartsWith("@"))
                    desc = MUI.LoadMuiStringValue(servicekey, "Discription");
                if (desc == string.Empty)
                    desc = GetDescription(ImagePath);
                //AddEntryList(false, GetShortCutValue(ImagePath), GetDescription(ImagePath), GetPublisher(ImagePath), ImagePath, GetFileTime(ImagePath));
                AddEntryList(false, keyname, disp + desc, GetPublisher(ImagePath), ImagePath, GetFileTime(ImagePath));
            }
            
        }
        /*private void LoadServices()
        {
            AddEntryList(true, RegEntry, string.Empty, string.Empty,string.Empty, DateTime.Now);
            RegistryKey Key = Registry.LocalMachine;
            RegistryKey SubKey = Key.OpenSubKey(RegEntry, false);
            string[] ChildKeys = SubKey.GetSubKeyNames();
            foreach (string keyname in ChildKeys)
            {
                string ServiceEntryPara =RegEntry+@"\"+ keyname + @"\Parameters";  //service下的parameter子键
                string ServiceEntryname = RegEntry + @"\" + keyname;   //service
                RegistryKey serviceparakey = Key.OpenSubKey(ServiceEntryPara, false);
                RegistryKey servicekey = Key.OpenSubKey(ServiceEntryname, false);
                string ImagePath = string.Empty;
                if (serviceparakey == null)  //parameter打开失败
                {
                    if (servicekey == null) continue;
                    object temp = servicekey.GetValue("ImagePath");
                    if (temp == null) continue;
                    //string temp = servicekey.GetValue("ImagePath").ToString();
                    //if (temp == string.Empty) continue;
                    ImagePath = GetValueContentAsPath(temp.ToString());
                    ImagePath = GetPathInSystem(ImagePath);  //获取系统盘下路径
                    if (ImagePath == string.Empty) continue;
                }
                else  //parameter打开成功
                {
                    object temp = serviceparakey.GetValue("serviceDLL");
                    //string temp = serviceparakey.GetValue("serviceDLL").ToString();
                    if (temp == null) continue;
                    ImagePath = GetValueContentAsPath(temp.ToString());
                    if (ImagePath == string.Empty) continue;
                    
                }
                object o = servicekey.GetValue("DisplayName");
                string disp = o == null ? "" : o.ToString();
                if (disp.StartsWith("@"))   //at 开头的都是dll
                    disp = MUI.LoadMuiStringValue(servicekey, "DisplayName");
                if (disp == string.Empty) disp = keyname;
                disp += ":";

                o = servicekey.GetValue("Description");
                string desc = o == null ? "" : o.ToString();
                if (desc.StartsWith("@"))
                    desc = MUI.LoadMuiStringValue(servicekey, "Discription");
                if (desc == string.Empty)
                    desc = GetDescription(ImagePath);

                AddEntryList(false, keyname, disp+desc, GetPublisher(ImagePath), ImagePath, GetFileTime(ImagePath));
            }
        }*/
    }
}
