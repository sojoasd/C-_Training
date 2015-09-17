using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSample
{
    public class Program
    {
        private static void Main(string[] args)
        {
            SimpleThread a = new SimpleThread("A");
            SimpleThread b = new SimpleThread("B");

            // 若要帶入引數，則寫法會不同，可以參考 GoThread
            Thread athread = new Thread(a.run);
            Thread bthread = new Thread(b.run);

            athread.Start();
            Thread.Sleep(500);
            bthread.Start();

            Console.ReadLine();
        }
    }

    public class SimpleThread
    {
        private String pName;

        public SimpleThread(string name)
        {
            this.pName = name;
        }

        public void run()
        {
            for (int i = 0; i < 10; i++)
            {
                String line = this.pName + ":" + i;
                Console.WriteLine(line);
                // Thread.Sleep(10);
            }
        }
    }
}