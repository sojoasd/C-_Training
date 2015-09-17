using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    public class Benz : ICar
    {
        public string IBrand
        {
            get
            {
                return "Benz";
            }
        }

        public string ShowBrand()
        {
            return "The Car's brand is " + this.IBrand;
        }
    }
}