namespace GameServer.Items
{
    class Bow : Weapon
    {
        public Bow()
        {
            Name = "Bow";
            Description = "A basic bow";
            SellPrice = 1;
            BuyPrice = 5;
            Attack = 5;
            Armor = 0;
        }
    }
}
