using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GolfTools
    {
        public GolfTools(string driver, string threeWood, string fiveIron)
        {
            Driver = driver;
            ThreeWood = threeWood;
            FiveIron = fiveIron;
        }

        public string Driver { get; set; }
        public string ThreeWood { get; set; }
        public string FiveIron { get; set; }
    }

    
}
