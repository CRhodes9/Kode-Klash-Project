namespace GameServer
{
	//
    public class Armor : Item
    {
        public int Defense { get; set; }
    }
    //Armor equipment slots
    public class Helmet : Armor
    {
        public Helmet()
        {
            Name = "Nothing";
            Description = "";
            SellPrice = 0;
            BuyPrice = 0;
            Defense = 0;
        }
    }
    public class Chest : Armor
    {
        public Chest()
        {
            Name = "Nothing";
            Description = "";
            SellPrice = 0;
            BuyPrice = 0;
            Defense = 0;
        }
    }
    public class Legs : Armor
    {
        public Legs()
        {
            Name = "Nothing";
            Description = "";
            SellPrice = 0;
            BuyPrice = 0;
            Defense = 0;
        }
    }
    public class Boots : Armor
    {
        public Boots()
        {
            Name = "Nothing";
            Description = "";
            SellPrice = 0;
            BuyPrice = 0;
            Defense = 0;
        }
    }
}
