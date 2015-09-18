using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    public class AudiCar : ICar
    {
        public virtual string IBrand
        {
            get
            {
                return "Audi";
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