using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicLoadDll
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //透過 MyClass 類別先初始化一個 FileWatch 事件
            MyClass myClass = new MyClass(@"E:\Workspaces\Cs_Training\ThreadTest\dllroot");
            //使 Assembly 做 load 或 reload，並建立 oAss 物件
            myClass.MyMethod();

            while (true)
            {
                Thread.Sleep(1000);

                // 取出每一個類別
                foreach (var type in myClass.OAss.GetExportedTypes())
                {
                    // 不包含 Interface 的類型，因為類別才有實作
                    if (!type.IsInterface)
                    {
                        // 建立指定的類別之物件，這裡的 FullName = "Cars.XXX"，包含整個類別的 namespace.class
                        Object obj = myClass.OAss.CreateInstance(type.FullName);

                        // 取得指定類別內的 function，這裡是 ShowBrand
                        MethodInfo method = myClass.OAss.GetType(type.FullName).GetMethod("ShowBrand");

                        // 用 delegate，但其實這裡等於把內容寫成子函式
                        ThreadStart threadMain = delegate()
                        {
                            // 直接用 Invoke 執行 function 取得回傳值
                            method.Invoke(obj, new Object[] { });
                        };

                        // 這裡才是真正執行 Thread 運作
                        new Thread(threadMain).Start();

                        Thread.Sleep(1000);
                    }
                }

                Console.WriteLine("====================================================");
                Thread.Sleep(1000);
            }
        }
    }

    public class MyClass
    {
        private string filepath;

        private byte[] readAllBytes { get; set; }

        private Assembly _oAss;

        public Assembly OAss
        {
            get { return _oAss; }
            set { _oAss = value; }
        }

        private FileSystemWatcher _fileWatch;

        public FileSystemWatcher MyFileWatch
        {
            get { return _fileWatch; }
            set { _fileWatch = value; }
        }

        public MyClass(string filePath)
        {
            this.filepath = filePath;

            //建立 FileWatch 事件
            this.MyFileWatch = new FileSystemWatcher();
            this.MyFileWatch.Path = filePath;
            this.MyFileWatch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.MyFileWatch.Filter = "*.dll";
            this.MyFileWatch.EnableRaisingEvents = true;
            this.MyFileWatch.Changed -= MyFileWatch_Changed;
            this.MyFileWatch.Changed += MyFileWatch_Changed;
        }

        public void MyMethod()
        {
            this.readAllBytes = File.ReadAllBytes(this.filepath + "\\Cars.dll");
            this._oAss = Assembly.Load(this.readAllBytes);
        }

        public void MyFileWatch_Changed(object sender, FileSystemEventArgs e)
        {
            this.MyMethod();
        }
    }
}