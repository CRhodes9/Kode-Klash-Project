using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Items
{
    class Shortsword : Weapon
    {
        public Shortsword()
        {
            Name = "Shortsword";
            Description = "A basic shortsword";
            SellPrice = 1;
            BuyPrice = 5;
            Attack = 5;
        }
    }
}
