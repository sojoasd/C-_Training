using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    public class Jaguar : ICar
    {
        public string IBrand
        {
            get
            {
                return "Jaguar";
            }
        }

        public void ShowBrand()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(this.IBrand + ":" + i);
            }
        }
    }
}