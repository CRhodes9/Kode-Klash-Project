using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Enemies
{
    public class Enemy : Creature
    {
        public ulong DroppedExperience { get; set; }
        public List<Item> DroppedItemsPool { get; set; }

        public Enemy()
        {
            DroppedExperience = 0;
            DroppedItemsPool = new List<Item>();
        }
    }
}
