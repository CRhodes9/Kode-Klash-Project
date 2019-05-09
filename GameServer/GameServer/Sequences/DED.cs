using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Sequences
{
    class DED : Sequence
    {
        public DED(Player player, string[] command)
        {
            Response = "You are dead, " + player.Name + ". You cannot " + command[1] + ". You can't do anything.";
        }
    }
}
