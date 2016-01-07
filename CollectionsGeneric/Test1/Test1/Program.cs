using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Cars<string> obj = new Cars<string>();
            obj.Adds("Audi");
            obj.Adds("BMW");
            obj.ShowCarName();
        }
    }

    public class Cars<T>
    {
        private T _vars;

        private List<T> _list = new List<T>();

        //public Cars(T input)
        //{
        //    item = input;
        //}

        public void Adds(T input)
        {
            _vars = input;
            this._list.Add(_vars);
        }

        public void ShowCarName()
        {
            foreach (var item in this._list)
            {
                Console.WriteLine("My car is " + item);
            }
        }
    }
}