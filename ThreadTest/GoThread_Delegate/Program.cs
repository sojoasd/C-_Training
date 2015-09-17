using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoThread_Delegate
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Program that = new Program();

            //取得 dll 檔案存放的路徑
            Assembly oAss = Assembly.LoadFile(@"E:\Workspaces\Cs_Training\ThreadTest\dllroot\Cars.dll");

            while (true)
            {
                Thread.Sleep(500);

                // 取出每一個類別
                foreach (var type in oAss.GetExportedTypes())
                {
                    // 不包含 Interface 的類型，因為類別才有實作
                    if (!type.IsInterface)
                    {
                        // 建立指定的類別之物件，這裡的 FullName = "Cars.XXX"，包含整個類別的 namespace
                        Object obj = oAss.CreateInstance(type.FullName);

                        // 取得指定類別內的 function，這裡是 ShowBrand
                        MethodInfo method = oAss.GetType(type.FullName).GetMethod("ShowBrand");

                        // 用 delegate，但其實這裡等於把內容寫成子函式
                        ThreadStart threadMain = delegate()
                        {
                            // 直接用 Invoke 執行 function 取得回傳值
                            Object res = method.Invoke(obj, new Object[] { });
                            Console.WriteLine("the Method returned {0}.", res);
                        };

                        // 這裡才是真正執行 Thread 運作
                        new Thread(threadMain).Start();

                        Thread.Sleep(500);
                    }
                }

                Console.WriteLine("====================================================");
            }
        }
    }
}