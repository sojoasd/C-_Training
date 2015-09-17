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

        public string ShowBrand()
        {
            return "The Car's brand is " + this.IBrand;
        }
    }
}