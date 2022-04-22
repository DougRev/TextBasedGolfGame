using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GolfClub
    {
        public GolfClub()
        {

        }

        public GolfClub(string name, int distance)
        {
            Name = name;
            Distance = distance;
        }

        public string Name { get; set; }
        public int Distance { get; set; }
    }
}
