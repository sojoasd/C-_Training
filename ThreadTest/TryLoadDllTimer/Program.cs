using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryLoadDllTimer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            LoadDll LD = new LoadDll(@"E:\Workspaces\Cs_Training\ThreadTest\dllroot");
            LD.update_dll();

            var arr = LD.OAss.GetExportedTypes();
            string Audi_FN = arr[0].FullName;
            //string Benz_FN = arr[1].FullName;
            //string Jaguar_FN = arr[2].FullName;

            //List<TimerTask> lsTask = new List<TimerTask>();
            TimerTask task = new TimerTask();

            task.TaskName = arr[0].Name;
            task.taskoAss = LD.OAss;
            task.taskFullName = Audi_FN;
            task.CurrentTime = DateTime.Now.AddSeconds(5);
            task.NextTime = DateTime.Now.AddSeconds(10);
            int totalSecond = Convert.ToInt32((task.CurrentTime - DateTime.Now).TotalMilliseconds);

            task.TaskTimer = new Timer(task.DoWork, task, totalSecond, 0);
            task.autoEvent.WaitOne(0, false);

            //lsTask.Add(task);

            Console.ReadLine();
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

        public LoadDll(string filePath)
        {
            this.filepath = filePath;

            //建立 FileWatch 事件
            this.FileWatch = new FileSystemWatcher();
            this.FileWatch.Path = filePath;
            this.FileWatch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.FileWatch.Filter = "*.dll";
            this.FileWatch.EnableRaisingEvents = true;
            this.FileWatch.Changed -= MyFileWatch_Changed;
            this.FileWatch.Changed += MyFileWatch_Changed;
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

    public class TimerTask
    {
        public string TaskName { set; get; }
        public Timer TaskTimer { set; get; }

        public DateTime CurrentTime { set; get; }

        public DateTime NextTime { set; get; }

        public Assembly taskoAss { set; get; }
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
                this.taskoAss = objState.taskoAss;
                this.taskFullName = objState.taskFullName;
                Object obj = taskoAss.CreateInstance(taskFullName);
                MethodInfo method = taskoAss.GetType(taskFullName).GetMethod("ShowBrand");

                ThreadStart threadMain = delegate()
                {
                    method.Invoke(obj, new Object[] { });
                };

                new Thread(threadMain).Start();

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
}