using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient
{
    public class Player
    {
        //Stats
        public string Name { get; set; }
        public int Level { get; set; }
        public ulong Experience { get; set; }
        public int Gold { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxMana { get; set; }
        public int CurrentMana { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

    }
}
