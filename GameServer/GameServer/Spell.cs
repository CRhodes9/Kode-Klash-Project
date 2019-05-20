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
        //Perform magics based off the spell effects
        public string Cast(Player player, Enemy currentEnemy)
        {
            return "Poof!";
        }
    }
}
