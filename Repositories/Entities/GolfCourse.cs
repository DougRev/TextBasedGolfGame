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

        public GolfCourse(int id, string courseName, List<Hole> holeList)
        {
            Id = id;
            CourseName = courseName;
            HoleList = holeList;
        }
        public int Id { get; set; }
        public string CourseName { get; set; }
        public List<Hole> HoleList { get; set; } = new List<Hole>();
        public int TotalDistance
        {
            get
            {
                int totalDist = 0;
                foreach (var hole in HoleList)
                {
                    totalDist += hole.Distance;
                }
                return totalDist;
            }
        }
        public int TotalStrokes
        {
            get
            {
                int totalStrokes = 0;
                foreach (var hole in HoleList)
                {
                    totalStrokes += hole.Strokes;
                }
                return totalStrokes;
            }
        }
        public int ParTotal
        {
            get
            {
                int total = 0;
                foreach (var hole in HoleList)
                {
                    total += hole.Par;
                }
                return total;
            }
        }
        public int ScoreCard
        {
            get
            {
                int currentScore = 0;
                int currentPar = 0;
                int calcScore = 0;
                foreach (var hole in HoleList)
                {
                    if (hole.Strokes == 0)
                    {
                        HoleList.Skip(1);
                    }
                    else
                    {
                        currentScore += hole.Strokes;
                        currentPar += hole.Par;
                        calcScore = currentScore - currentPar;

                    }

                }
                return calcScore;
            }
        }

    }
}
