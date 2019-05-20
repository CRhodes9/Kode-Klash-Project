namespace GameServer.Items
{
    class Spear : Weapon
    {
        public Spear()
        {
            Name = "Spear";
            Description = "A basic spear";
            SellPrice = 1;
            BuyPrice = 5;
            Attack = 5;
            Armor = 0;
        }
    }
}
