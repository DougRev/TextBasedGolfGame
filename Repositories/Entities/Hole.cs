using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Hole
    {
        public Hole()
        {

        }

        public Hole(int number, int par, int distance)
        {
            Number = number;
            Par = par;
            Distance = distance;
        }

        public int Number { get; set; }
        public int Par { get; set; }
        public int Distance { get; set; }
    }
}
