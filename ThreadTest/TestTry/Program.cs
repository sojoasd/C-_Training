using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestTry
{
    public class Program
    {
        public Program()
        {
        }

        private static void Main(string[] args)
        {
            //TimerTask tt = new TimerTask();

            ////while (true)
            ////{
            //tt.Timer_Tick(new TimeSpan(11, 25, 10), "first task");
            //tt.Timer_Tick(new TimeSpan(11, 25, 20), "sencond task");
            //tt.Timer_Tick(new TimeSpan(11, 25, 30), "third task");
            //}
            Console.WriteLine("Start");

            List<TimeObject> lsTask = new List<TimeObject>();

            TimeObject obj = new TimeObject();
            obj.TaskName = "1st Task";
            obj.CurrentTime = DateTime.Now.AddSeconds(10);
            obj.NextTime = DateTime.Now.AddSeconds(20);
            //int totalSecond = int.Parse((obj.CurrentTime - DateTime.Now).TotalMilliseconds.ToString());
            int totalSecond = Convert.ToInt32((obj.CurrentTime - DateTime.Now).TotalMilliseconds);
            obj.TaskTimer = new Timer(obj.DoWork, obj.TaskName, totalSecond, 0);
            lsTask.Add(obj);

            //obj = new TimeObject();
            //obj.TaskName = "2nd Task";
            //obj.CurrentTime = DateTime.Now.AddSeconds(20).Ticks;
            //obj.NextTime = DateTime.Now.AddSeconds(30).Ticks;
            //obj.TaskTimer = new Timer(obj.DoWork, obj.TaskName, (obj.CurrentTime - DateTime.Now.Ticks) / 10000, 0);
            //lsTask.Add(obj);

            //obj = new TimeObject();
            //obj.TaskName = "3rd Task";
            //obj.TaskTimer = new Timer(obj.DoWork, obj, 1000, 0);
            //lsTask.Add(obj);

            //Thread.Sleep(30000);
            //Console.WriteLine("Do Stop");
            //foreach (var item in lsTask)
            //{
            //    item.Stop();
            //}
            //Console.WriteLine("Do Stop End");
            Console.ReadLine();
        }
    }

    public class TimeObject
    {
        public string TaskName { set; get; }
        public Timer TaskTimer { set; get; }

        public DateTime CurrentTime { set; get; }

        public DateTime NextTime { set; get; }

        public void DoWork(object state)
        {
            Console.WriteLine(state);
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

    public class TimerTask
    {
        private Timer timer;

        public void Timer_Tick(TimeSpan sptime, string msg)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = sptime - current.TimeOfDay;
            if (timeToGo < TimeSpan.Zero)
            {
                return;
            }
            //this.timer = new Timer(DoWork(msg), null, timeToGo, Timeout.InfiniteTimeSpan);
            this.timer = new Timer(DoWork, msg, timeToGo, Timeout.InfiniteTimeSpan);
        }

        private void DoWork(object state)
        {
            Console.WriteLine(state);
        }
    }
}