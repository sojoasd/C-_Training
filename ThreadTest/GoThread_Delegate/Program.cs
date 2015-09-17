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

                        ThreadStart threadMain = delegate()
                        {
                            Object res = method.Invoke(obj, new Object[] { });
                            Console.WriteLine("the Method returned {0}.", res);
                        };
                        new Thread(threadMain).Start();
                        Thread.Sleep(500);
                    }
                }

                Console.WriteLine("====================================================");
            }
        }
    }
}