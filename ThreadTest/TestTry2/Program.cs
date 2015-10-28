using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestTry2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TT tt = new TT();
            TimerCallback timerDelegate = new TimerCallback(tt.test);

            tt.TaskTimer = new Timer(timerDelegate, tt.autoEvent, 2000, 0);

            tt.autoEvent.WaitOne(2000, false);

            //Thread.Sleep(100000);
            Console.ReadLine();
        }
    }

    public class TT
    {
        public Timer TaskTimer { set; get; }

        public AutoResetEvent autoEvent { set; get; }

        public TT()
        {
            this.autoEvent = new AutoResetEvent(false);
        }

        public void test(object state)
        {
            this.autoEvent = (AutoResetEvent)state;
            Console.WriteLine("mytest");
            //this.autoEvent.Set();
            this.ChangeTime();
            this.autoEvent.Set();
        }

        private void ChangeTime()
        {
            this.TaskTimer.Change(2000, 0);
        }
    }
}