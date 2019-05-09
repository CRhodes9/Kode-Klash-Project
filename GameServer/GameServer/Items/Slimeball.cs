using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Items
{
    class Slimeball : Item
    {
        public Slimeball()
        {
            Name = "Slimeball";
            Description = "A ball of slime.";
            SellPrice = 5;
            BuyPrice = 10;
        }
    }
}
