using Golf_Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Repo
{
    public class GolferRepository
    {
        private readonly List<Player> _playerDbContext = new List<Player>();
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
                _playerDbContext.Add(player);
                return true;
            }
        }

        public List<Player> GetGolfers()
        {
            return _playerDbContext;
        }

        public bool RemoveGolfer(string name)
        {
            foreach (var player in _playerDbContext)
            {
                if (player.Name == name)
                {
                    _playerDbContext.Remove(player);
                    return true;
                }
            }
            return false;
        }
    }
}
