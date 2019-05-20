namespace GameServer
{
    public class Player : Creature
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public ulong Experience { get; set; }
        public int StatPoints { get; set; }
        public string Sequence { get; set; }
        public string LastResponse { get; set; }
        public string[] LastCommand { get; set; }
        public bool FirstTime { get; set; }

        public Player(string ID, string password)
        {
            UserID = ID;
            Password = password;
            Experience = 0;
            StatPoints = 0;
            Sequence = "NEW1";
            FirstTime = true;
        }

        public void AddExperience(ulong amount)
        {
            Experience += amount;
        }
        public void SetSequence(string sequence)
        {
            Sequence = sequence;
        }
    }
}