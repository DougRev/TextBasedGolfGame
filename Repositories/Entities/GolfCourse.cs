using Repositories.Entities;
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

        public GolfCourse(string courseName, List<Hole> holeList, int totalDist = 0, int parTotal = 0)
        {
            CourseName = courseName;
            List<Hole> holesList = new List<Hole>();
            TotalDistance = totalDist;
            ParTotal = parTotal;
        }

        public string CourseName { get; set; }
        public List<Hole> HoleList { get; set; } = new List<Hole>();
        public int TotalDistance { get; set; }
        public int ParTotal { get; set;}

    }
}
