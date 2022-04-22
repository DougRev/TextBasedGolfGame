using System;
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

        public List<GolfClub> GetClubs()
        {
            return _clubDbContext;
        }

        public GolfClub GetClubByType(string type)
        {
            foreach (var club in _clubDbContext)
            {
                if (club.Type == type)
                {
                    return club;
                }
            }
            return null;
        }
    }
}
