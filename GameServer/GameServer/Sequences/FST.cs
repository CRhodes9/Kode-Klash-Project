using GameServer.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Sequences
{
    class FST : Sequence
    {
        public FST(Player player, string[] command)
        {
            SubSequences = new List<string> { "FST1", "FST2", "FSTCMBTSLIME", "FST4", "FST5", "FST6", "FST7", "FST8", "FST9", "FST10" };
            EnemyList = new List<Enemy> { new Slime() };
            CurrentEnemy = GetCurrentEnemy(player);

            switch (player.Sequence.Substring(3))
            {
                case "1":
                    Response = "You awake in a small clearing in a forest. " +
                        "\nYou have no idea where you are, and no belongings on you. " +
                        "\nYou find a Shortsword on the ground next to you. You pick it up." +
                        "\nYou got a Shortsword!" +
                        "\n\nIn front of you are two paths, behind you is what appears to be a hollowed out tree stump." +
                        "\nWhere will you go?" +
                        "\n(L)eft Path (R)ight Path (T)ree Stump";
                    player.Items.Add(new Items.Shortsword());
                    player.SetSequence("FST2");
                    break;
                case "2":
                    switch (command[1].ToLower())
                    {
                        case "l":
                        case "left":
                            Response = "";
                            break;
                        case "r":
                        case "right":
                            Response = "";
                            break;
                        case "t":
                        case "tree":
                        case "stump":
                            SetCurrentEnemy(player, EnemyList[0]);
                            Response = "You turn around and begin walking towards the stump. " +
                                "\nBefore you have time to react, a Slime pops out from inside it!" +
                                "\n\nYou are fighting " + CurrentEnemy.Name + " (" + CurrentEnemy.CurrentHealth + "hp)" +
                                "\n\nTip: If you know any spells, you can enter the spell name to automatically cast it!" +
                                "\n(F)ight || (G)uard || (S)pell || (R)un || (H)elp";
                            player.SetSequence("FSTCMBTSLIME");
                            break;
                        default:
                            Response = "(L)eft Path (R)ight Path (T)ree Stump";
                            break;
                    }
                    break;
                case "CMBTSLIME":
                    Enemy currentEnemy = CurrentEnemy;
                    string combatResponse = Combat(command, ref player, currentEnemy);
                    Response = combatResponse;

                    if (combatResponse.StartsWith("You won! ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Response += combatResponse;
                        player.SetSequence("FST4");
                    }
                    else if (combatResponse.StartsWith("You escaped ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Response += combatResponse;
                        player.SetSequence("FST4");
                    }
                    else
                    {
                        SetCurrentEnemy(player, currentEnemy);
                    }
                    break;
                case "4":
                    Response = "In front of you are two paths." +
                            "\nWhere will you go?" +
                            "\n(L)eft Path (R)ight Path";
                    break;
                case "5":
                    break;
                case "6":
                    break;
                case "7":
                    break;
                case "8":
                    break;
                case "9":
                    break;
            }
        }
    }
}
