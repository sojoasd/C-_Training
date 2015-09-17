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

                        // 這裡的寫法很像 callback function，此寫法很特殊，因為會帶入引數
                        Thread oth = new Thread(() => that.DoWork(method, obj, new Object[] { }));

                        oth.Start();

                        Thread.Sleep(500);
                    }
                }

                Console.WriteLine("====================================================");
            }
        }

        public void DoWork(MethodInfo method, object obj, params object[] parameters)
        {
            Object res = method.Invoke(obj, parameters);
            Console.WriteLine("the Method returned {0}.", res);
        }
    }
}