using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GolfClub
    {
        public GolfClub(int driver = 225, int threeWood = 200, int fiveIron = 185)
        {
           int Driver = driver;
           int ThreeWood = threeWood;
           int FiveIron = fiveIron;
        }

        public string Driver { get; set; }
        public string ThreeWood { get; set; }
        public string FiveIron { get; set; }
    }

    
}
