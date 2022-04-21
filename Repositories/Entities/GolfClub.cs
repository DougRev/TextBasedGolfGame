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

        public GolfClub(string type, int distance)
        {
            Type = type;
            Distance = distance;
        }

        public string Type { get; set; }
        public int Distance { get; set; }
    }
}
