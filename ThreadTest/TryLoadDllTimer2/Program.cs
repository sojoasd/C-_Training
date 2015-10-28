using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryLoadDllTimer2
{
    public class Program
    {
        private static void Main(string[] args)
        {
            List<TimerTask> lsTask = new List<TimerTask>();

            TimerTask task = new TimerTask();
            Type[] arr = task.OAss.GetExportedTypes();

            task.TaskName = arr[0].Name;
            task.taskFullName = arr[0].FullName;
            task.CurrentTime = DateTime.Now.AddSeconds(5);
            task.NextTime = DateTime.Now.AddSeconds(10);
            lsTask.Add(task);

            task = new TimerTask();
            task.TaskName = arr[1].Name;
            task.taskFullName = arr[1].FullName;
            task.CurrentTime = DateTime.Now.AddSeconds(10);
            task.NextTime = DateTime.Now.AddSeconds(20);
            lsTask.Add(task);

            task = new TimerTask();
            task.TaskName = arr[2].Name;
            task.taskFullName = arr[2].FullName;
            task.CurrentTime = DateTime.Now.AddSeconds(15);
            task.NextTime = DateTime.Now.AddSeconds(30);
            lsTask.Add(task);

            foreach (var item in lsTask)
            {
                TimerTask objTask = item as TimerTask;
                int totalSecond = Convert.ToInt32((objTask.CurrentTime - DateTime.Now).TotalMilliseconds);
                objTask.TaskTimer = new Timer(objTask.DoWork, objTask, totalSecond, 0);
                objTask.autoEvent.WaitOne(0, false);
            }

            Console.ReadLine();
        }
    }

    public class TimerTask : LoadDll
    {
        public string TaskName { set; get; }
        public Timer TaskTimer { set; get; }

        public DateTime CurrentTime { set; get; }

        public DateTime NextTime { set; get; }

        public string taskFullName { set; get; }

        public AutoResetEvent autoEvent { set; get; }

        public TimerTask()
        {
            this.autoEvent = new AutoResetEvent(false);
        }

        public void DoWork(object state)
        {
            TimerTask objState = state as TimerTask;
            if (objState == null)
            {
                Console.WriteLine("obj is null");
            }
            else
            {
                this.taskFullName = objState.taskFullName;
                Object obj = OAss.CreateInstance(this.taskFullName);
                obj.GetType().InvokeMember("ShowBrand", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance, null, obj, null);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(this.TaskName + " be executed at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                this.autoEvent.Set();
            }

            this.ChangeTime();
        }

        private void ChangeTime()
        {
            int totalSecond = Convert.ToInt32((this.NextTime - this.CurrentTime).TotalMilliseconds);
            this.TaskTimer.Change(totalSecond, 0);
        }

        public void Stop()
        {
            Console.WriteLine(this.TaskName);
            this.TaskTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }
    }

    public class LoadDll
    {
        private string filepath { get; set; }

        private byte[] readAllBytes { get; set; }

        private Assembly _oAss;

        public Assembly OAss
        {
            get { return _oAss; }
            set { _oAss = value; }
        }

        private FileSystemWatcher _fileWatch;

        public FileSystemWatcher FileWatch
        {
            get { return _fileWatch; }
            set { _fileWatch = value; }
        }

        public LoadDll()
        {
            this.filepath = @"E:\Workspaces\Cs_Training\ThreadTest\dllroot";

            //建立 FileWatch 事件
            this.FileWatch = new FileSystemWatcher();
            this.FileWatch.Path = this.filepath;
            this.FileWatch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.FileWatch.Filter = "*.dll";
            this.FileWatch.EnableRaisingEvents = true;
            this.FileWatch.Changed -= MyFileWatch_Changed;
            this.FileWatch.Changed += MyFileWatch_Changed;
            this.update_dll();
        }

        public void update_dll()
        {
            this.readAllBytes = File.ReadAllBytes(this.filepath + "\\Cars.dll");
            this._oAss = Assembly.Load(this.readAllBytes);
        }

        public void MyFileWatch_Changed(object sender, FileSystemEventArgs e)
        {
            //Console.WriteLine("changed");
            this.update_dll();
        }
    }
}