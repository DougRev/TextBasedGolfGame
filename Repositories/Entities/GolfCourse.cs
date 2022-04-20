using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GolfCourse
    {
        public GolfCourse()
        {

        }

        public GolfCourse(string courseName, int holes = 0, int totalDist = 0, int parTotal = 0)
        {
            CourseName = courseName;
            Holes = holes;
            TotalDistance = totalDist;
            ParTotal = parTotal;
        }

        public string CourseName { get; set; }
        public int Holes { get; set;}
        public int TotalDistance { get; set; }
        public int ParTotal { get; set;}

    }
}
