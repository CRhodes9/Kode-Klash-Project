namespace GameServer
{
    public class Weapon : Item
    {
        public int Attack { get; set; }
        public int Armor { get; set; }

        public Weapon()
        {
            Name = "Nothing";
            Description = "";
            SellPrice = 0;
            BuyPrice = 0;
            Attack = 0;
            Armor = 0;
        }
    }
}
