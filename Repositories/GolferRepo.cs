using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GolferRepo
    {
        private readonly List<Player> _golferDbContext = new List<Player>();
        private int _count;

        public bool AddGolferToDatabase(Player player)
        {
            if (player is null)
            {
                return false;
            }
            else
            {
                _count++;
                _golferDbContext.Add(player);
                return true;
            }
        }

        public List<Player> GetGolfers()
        {
            return _golferDbContext;
        }

        public Player GetGolferByName(string name)
        {
            foreach (var player in _golferDbContext)
            {
                if (player.Name == name)
                {
                    return player;
                }
            }
            return null;
        }

        

        public bool RemoveGolfer(string name)
        {
            foreach (var player in _golferDbContext)
            {
                if (player.Name == name)
                {
                    _golferDbContext.Remove(player);
                    return true;
                }
            } return false;
        }


    }
}
