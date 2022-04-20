using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class Player
    {
        public Player()
        {

        }

        public Player(string name, int strength = 0, int accuracy = 0, int stamina = 0, int handicap = 0)
        {
            Name = name;
            Strength = strength;
            Accuracy = accuracy;
            Stamina = stamina;
            Handicap = handicap;
        }
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Accuracy { get; set; }
        public int Stamina { get; set; } 
        public int Handicap { get; set; }
       


    }
}
