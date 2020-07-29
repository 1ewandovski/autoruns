using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskScheduler;
using System.Xml;

namespace WpfApp1
{
    class Tasks : BaseFunctions
    {
        public Tasks()
        {
            LoadTasks();
        }
        public void LoadTasks()
        {
            foreach (IRegisteredTask task in GetAllTasks())
            {
                string ImagePath = GetImagePathFromXML(task.Xml);
                
                if (ImagePath == string.Empty) continue;
                AddEntryList(false, task.Name, GetDescription(ImagePath), GetPublisher(ImagePath),ImagePath, task.LastRunTime);
            }
        }
        public static IRegisteredTaskCollection GetAllTasks()
        {
            TaskSchedulerClass ts = new TaskSchedulerClass();
            ts.Connect(null, null, null, null);
            ITaskFolder folder = ts.GetFolder("\\");
            IRegisteredTaskCollection task_exists = folder.GetTasks(1);
            return task_exists;

        }
        public string GetImagePathFromXML(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if ("Actions" == node.Name.ToString())
                {
                    foreach (XmlNode inode in node.ChildNodes)
                    {
                        string imagepath = inode.InnerText;
                        imagepath = GetValueContentAsPath(imagepath);
                        imagepath = GetPathInSystem(imagepath);
                        if (!imagepath.Contains(".exe")) continue;
                        return imagepath;
                    }
                }
            }
            return string.Empty;
        }
    }
}
