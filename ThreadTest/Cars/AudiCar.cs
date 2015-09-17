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

        public string ShowBrand()
        {
            return "The Car's brand is " + this.IBrand;
        }
    }
}