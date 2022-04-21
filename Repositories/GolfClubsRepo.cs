﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GolfClubsRepo
    {
         private readonly List<GolfClub> _clubDbContext = new List<GolfClub>();
         private int _count;
        public bool AddClubToDatabase(GolfClub club)
        {
            if (club == null)
            {
                return false;
            }
            else
            {
                _count++;
                _clubDbContext.Add(club);
                return true;
            }
        }

        public List<GolfCourse> GetClubs()
        {

        }
    }
}
