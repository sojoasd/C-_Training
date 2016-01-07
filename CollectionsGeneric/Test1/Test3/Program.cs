using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Cars<Audi> oAudi = new Cars<Audi>();
            Audi car1 = oAudi.SetCarName();
            Console.WriteLine(car1.CarName);

            Console.WriteLine("=================================================");

            Cars<Jaguar> oJaguar = new Cars<Jaguar>();
            Jaguar car2 = oJaguar.SetCarName();
            Console.WriteLine(car2.CarName);

            Console.ReadLine();
        }
    }

    public class Cars<T>
    {
        public Cars()
        {
        }

        public T SetCarName()
        {
            Type type = typeof(T);
            Object instance = Activator.CreateInstance(type);
            string Name = string.Format("{0} is my car !", instance.GetType().Name);
            instance.GetType().GetProperty("CarName").SetValue(instance, Name, null);
            return (T)Convert.ChangeType(instance, typeof(T)); ;
        }
    }

    public class Audi
    {
        public string CarName { get; set; }
    }

    public class Jaguar
    {
        public string CarName { get; set; }
    }
}