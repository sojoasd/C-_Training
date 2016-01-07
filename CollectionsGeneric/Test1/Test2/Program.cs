using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Program p = new Program();
            var ls = p.GetObject<MyClass>();
            var ls1 = p.GetObject<MyClass1>();
            foreach (var item in ls)
            {
                Console.WriteLine(item.Value);
            }
            Console.WriteLine("-----------------------------------");
            foreach (var item in ls1)
            {
                Console.WriteLine(item.Value);
            }
            Console.ReadLine();
        }

        public List<T> GetObject<T>() where T : class
        {
            Type type = typeof(T);
            List<T> returnObj = new List<T>();
            for (int i = 0; i < 5; i++)
            {
                Object instance = Activator.CreateInstance(type);
                instance.GetType().GetProperty("Value").SetValue(instance, i, null);
                returnObj.Add(instance as T);
            }
            return returnObj;
        }
    }

    public class MyClass
    {
        public int Value { set; get; }
    }

    public class MyClass1
    {
        public int Value { set; get; }
    }
}