using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoThread
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Program that = new Program();

            Assembly oAss = Assembly.LoadFile(@"E:\Workspaces\Cs_Training\ThreadTest\dllroot\Cars.dll");

            while (true)
            {
                Thread.Sleep(500);

                foreach (var type in oAss.GetExportedTypes())
                {
                    if (!type.IsInterface)
                    {
                        Object obj = oAss.CreateInstance(type.FullName);

                        MethodInfo method = oAss.GetType(type.FullName).GetMethod("ShowBrand");

                        // 這裡的寫法很 delegate 或 callback，此寫法很特殊，因為會帶入引數
                        Thread oth = new Thread(() => that.DoWork(method, obj, new Object[] { }));

                        oth.Start(); // 其中一個 Thread 開始執行
                        //oth.Join(); // 當下的 Thread 跑完才會往下執行
                        Thread.Sleep(500); // 之後的 Thread 先去睡覺等等再執行，效果有點類似 Join
                    }
                }

                Console.WriteLine("====================================================");
            }
        }

        public void DoWork(MethodInfo method, object obj, params object[] parameters)
        {
            Object res = method.Invoke(obj, parameters);
        }
    }
}