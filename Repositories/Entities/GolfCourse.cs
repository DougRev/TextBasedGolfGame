﻿using Repositories.Entities;
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

        public GolfCourse(int id, string courseName, List<Hole> holeList, int totalDist = 0, int parTotal = 0)
        {
            Id = id;
            CourseName = courseName;
            HoleList = holeList;
            TotalDistance = totalDist;
            ParTotal = parTotal;
        }
        public int Id { get; set; }
        public string CourseName { get; set; }
        public List<Hole> HoleList { get; set; } = new List<Hole>();
        public int TotalDistance { get; set; }
        public int ParTotal { get; set;}

    }
}
