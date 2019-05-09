using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Weapon : Item
    {
        public int Attack { get; set; }

        public Weapon()
        {
            Name = "Nothing";
            Description = "";
            SellPrice = 0;
            BuyPrice = 0;
            Attack = 0;
        }
    }
}
