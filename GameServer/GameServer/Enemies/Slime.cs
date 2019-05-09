using GameServer.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Enemies
{
    class Slime : Enemy
    {
        public Slime()
        {
            Name = "Slime";
            Level = 1;
            MaxHealth = 20;
            CurrentHealth = 20;
            Strength = 5;
            Dexterity = 2;
            Constitution = 10;
            Intelligence = 1;
            Wisdom = 1;
            Charisma = 1;

            DroppedExperience = 10;
            DroppedItemsPool = new List<Item> { new Slimeball() };
        }
    }
}
