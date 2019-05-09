using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Enemies;

namespace GameServer
{
    public class Spell
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public string Description { get; set; }

        public Spell()
        {
            Name = "Default Spell";
            Cost = 0;
            Description = "A default spell that you shouldn't normally be able to see.";
        }

        public string Cast(Player player, Enemy currentEnemy)
        {
            return "Poof!";
        }
    }
}
