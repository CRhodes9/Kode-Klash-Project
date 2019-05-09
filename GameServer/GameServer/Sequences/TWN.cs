using GameServer.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Sequences
{
    class TWN : Sequence
    {
        public TWN(Player player, string[] command)
        {
            SubSequences = new List<string> { "TWN1", "TWN2", "TWNEAST", "TWNWEST", "TWNCENTER" };
            EnemyList = new List<Enemy> { };
            CurrentEnemy = GetCurrentEnemy(player);

            switch (player.Sequence.Substring(3))
            {
                case "1":
                    Response = "You arrive at the town. Where will you go?" +
                        "\n(E)ast Side (W)est Side (T)own Center";
                    player.SetSequence("TWN2");
                    break;
                case "2":
                    switch (command[1].ToLower())
                    {
                        case "e":
                        case "east":
                            Response = "You enter the east side of town. On your left you see the Blacksmith and the Alchemy Shop." +
                                "\nOn your right you see a dark alleyway that somehow seems suspiciously inviting." +
                                "\n(B)lacksmith (A)lchemy Shop (D)ark Alleyway (L)eave";
                            player.SetSequence("TWNEAST");
                            break;
                        case "w":
                        case "west":
                            Response = "You enter the west side of town. On your left you see the General Store." +
                                "\nOn your right you see the Tavern." +
                                "\n(G)eneral Store (T)avern (L)eave";
                            player.SetSequence("TWNWEST");
                            break;
                        case "t":
                        case "town":
                        case "town center":
                        case "center":
                            Response = "As you approach the center of town, the hustle and bustle of the market carries you through the shop stalls." +
                                "\nEventually you stop in front of a few stalls that catch your interest." +
                                "\n(J)ewelry (M)agic (L)eave";
                            player.SetSequence("TWNCENTER");
                            break;
                    }
                    break;
                case "EAST":
                    switch (command[1].ToLower())
                    {
                        case "b":
                        case "blacksmith":
                            Response = "You walk up to the Blacksmith. As you approach, you hear the clanging of metal on metal, and feel the heat of the forge." +
                                "\nPress Enter to continue...";
                            player.SetSequence("BSM1");
                            break;
                        case "a":
                        case "alchemy":
                        case "alchemy shop":
                            break;
                        case "d":
                        case "dark":
                        case "alley":
                        case "alleyway":
                        case "dark alleyway":
                            break;
                        case "l":
                        case "leave":
                            break;
                    }
                    break;
                case "WEST":
                    switch (command[1].ToLower())
                    {
                        case "g":
                        case "general":
                        case "general store":
                        case "store":
                            break;
                        case "t":
                        case "taven":
                            break;
                        case "l":
                        case "leave":
                            break;
                    }
                    break;
                case "CENTER":
                    switch (command[1].ToLower())
                    {
                        case "j":
                        case "jewelry":
                            break;
                        case "m":
                        case "magic":
                            break;
                        case "l":
                        case "leave":
                            break;
                    }
                    break;
            }
        }
    }
}
