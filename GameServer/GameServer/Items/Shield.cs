namespace GameServer.Items
{
    class Shield : Weapon
    {
        public Shield()
        {
            Name = "Shield";
            Description = "A basic shield";
            SellPrice = 1;
            BuyPrice = 5;
            Attack = 2;
            Armor = 5;
        }
    }
}
