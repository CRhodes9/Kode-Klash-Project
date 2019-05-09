using GameServer.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Spells
{
    class Firebolt : Spell
    {
        public Firebolt()
        {
            Name = "Firebolt";
            Cost = 10;
            Description = "A bolt of fire that is shot at the target.";
        }

        public void Cast(ref Player player, ref Enemy enemy)
        {

        }
    }
}
