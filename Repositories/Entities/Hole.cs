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

        public Hole(int number, int par, int distance, GolfCourse golfCourse)
        {
            Number = number;
            Par = par;
            Distance = distance;
            GolfCourse = golfCourse;
        }
        public int Number { get; set; }
        public int Par { get; set; }
        public int Distance { get; set; }
        public GolfCourse GolfCourse { get; set; }
        public int DistanceRemaining { get; set; }
    }
}
