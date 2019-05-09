using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Sequences
{
    public class NEW : Sequence
    {
        public NEW(Player player, string[] command)
        {
            SubSequences = new List<string> { "NEW1", "NEW2", "NEW3", "NEW4", "NEW5", "NEW6", "NEW7", "NEW8", "NEW9", "NEW10" };
            Random ranGen = new Random();
            List<int> statValues = new List<int>();
            switch (player.Sequence.Substring(3))
            {
                case "1":
                    player.Name = command[1];
                    Response = "Your name is " + player.Name + ". Is that right? [(Y)es/(N)o]";
                    player.SetSequence("NEW2");
                    break;
                case "2":
                    if (command[1].ToLower() == "yes" || command[1].ToLower() == "y")
                    {
                        int totalStatPoints = ranGen.Next(15, 31);
                        player.StatPoints = totalStatPoints;
                        Response = "Nice to meet you, " + player.Name + "! Now, how about your stats?\nThere's Strength, Constitution, Dexterity, Intelligence, Wisdom, and Charisma." +
                                   "\nYou have a total of " + player.StatPoints + " points to spend." +
                                   "\nLet's start with Strength, an easy one. How strong do you want to be?\nYou have " + player.StatPoints + " points left. \nStrength: ";
                        player.SetSequence("NEW3");
                    }
                    else if (command[1].ToLower() == "no" || command[1].ToLower() == "n")
                    {
                        int totalStatPoints = ranGen.Next(15, 31);
                        player.StatPoints = totalStatPoints;
                        Response = "Well it is now! Nice to meet you, " + player.Name + " Now, how about your stats?\nThere's Strength, Constitution, Dexterity, Intelligence, Wisdom, and Charisma." +
                                   "\nYou have a total of " + player.StatPoints + " points to spend." +
                                   "\nLet's start with Strength, an easy one. How strong do you want to be?\nYou have " + player.StatPoints + " points left. \nStrength: ";
                        player.SetSequence("NEW3");
                    }
                    else
                    {
                        Response = "Is that right? [(Y)es/(N)o]";
                    }
                    break;
                case "3":
                    if (int.TryParse(command[1], out int str) == true)
                    {
                        if (str <= player.StatPoints)
                        {
                            player.ModifyStrength(str);
                            player.StatPoints -= str;
                            Response = "Very good. Now, let's move on to Constitution, shall we? This is your overall \nwellness and how healthy you are.\nYou have " + player.StatPoints + " points left. \nConstitution: ";
                            player.SetSequence("NEW4");
                        }
                        else
                        {
                            Response = "You don't have enough points!";
                        }
                    }
                    else
                    {
                        Response = "Error! That's not a number.";
                    }
                    break;
                case "4":
                    if (int.TryParse(command[1], out int con) == true)
                    {
                        if (con <= player.StatPoints)
                        {
                            player.ModifyConstitution(con);
                            player.MaxHealth = (player.Constitution * 10);
                            player.CurrentHealth = player.MaxHealth;
                            player.StatPoints -= con;
                            Response = "As healthy as a goat! Next up, it's time for your Dexterity. This is how \nagile and flexible you are.\nYou have " + player.StatPoints + " points left. \nDexterity: ";
                            player.SetSequence("NEW5");
                        }
                        else
                        {
                            Response = "You don't have enough points!";
                        }
                    }
                    else
                    {
                        Response = "Error! That's not a number.";
                    }
                    break;
                case "5":
                    if (int.TryParse(command[1], out int dex) == true)
                    {
                        if (dex <= player.StatPoints)
                        {
                            player.ModifyDexterity(dex);
                            player.StatPoints -= dex;
                            Response = "An interesting choice. Next up is Intelligence, which is how smart you are.\n A fairly simple one, but worthwile nonetheless.\nYou have " + player.StatPoints + " points left. \nIntelligence: ";
                            player.SetSequence("NEW6");
                        }
                        else
                        {
                            Response = "You don't have enough points!";
                        }
                    }
                    else
                    {
                        Response = "Error! That's not a number.";
                    }
                    break;
                case "6":
                    if (int.TryParse(command[1], out int intel) == true)
                    {
                        if (intel <= player.StatPoints)
                        {
                            player.ModifyIntelligence(intel);
                            player.StatPoints -= intel;
                            Response = "A smart choice, haha. Get it? Smart? Oh, well. Anyways, next up is Wisdom,\n which is how wise you are. Pretty self-explanittory, don't you think?" +
                                       "\nYou have " + player.StatPoints + " points left. \nWisdom: ";
                            player.SetSequence("NEW7");
                        }
                        else
                        {
                            Response = "You don't have enough points!";
                        }
                    }
                    else
                    {
                        Response = "Error! That's not a number.";
                    }
                    break;
                case "7":
                    if (int.TryParse(command[1], out int wis) == true)
                    {
                        if (wis <= player.StatPoints)
                        {
                            player.ModifyWisdom(wis);
                            player.MaxMana = (player.Wisdom * 10);
                            player.CurrentMana = player.MaxMana;
                            player.StatPoints -= wis;
                            Response = "A wise choi-.. Okay, fine. Last but not least, Charisma. How well you can\n perform and act, as well as socialize with others.\n In case you were wondering, I put all my points in this." +
                                       "\nYou have " + player.StatPoints + " points left. \nCharisma: ";
                            player.SetSequence("NEW8");
                        }
                        else
                        {
                            Response = "You don't have enough points!";
                        }
                    }
                    else
                    {
                        Response = "Error! That's not a number.";
                    }
                    break;
                case "8":
                    if (int.TryParse(command[1], out int cha) == true)
                    {
                        if (cha <= player.StatPoints)
                        {
                            player.ModifyCharisma(cha);
                            player.StatPoints -= cha;

                            if (player.StatPoints > 0)
                            {
                                Response = "You still have " + player.StatPoints + " points left, are you sure you want to continue? [(Y)es/(N)o]";
                            }
                            else
                            {
                                Response = String.Format("Your stats are: Strength: {0}\n Dexterity: {1}\n Constitution: {2}\n Intelligence: {3}\n Wisdom: {4}\n Charisma: {5}\n\nAre these okay? [(Y)es/(N)o]",
                                    player.Strength, player.Dexterity, player.Constitution, player.Intelligence, player.Wisdom, player.Charisma);
                            }
                            player.SetSequence("NEW9");
                        }
                        else
                        {
                            Response = "You don't have enough points!";
                        }
                    }
                    else
                    {
                        Response = "Error! That's not a number.";
                    }
                    break;
                case "9":
                    if (command[1].ToLower() == "yes" || command[1].ToLower() == "y")
                    {
                        Response = "Very well, then. Now, on to your brand new adventure in this wonderful world!" +
                            "\nPress Enter to continue...";
                        player.FirstTime = false;
                        player.SetSequence("FST1");
                    }
                    else if (command[1].ToLower() == "no" || command[1].ToLower() == "n")
                    {
                        Response = "Then I'll give you another shot at it...";
                        player.SetSequence("NEW2");
                    }
                    else
                    {
                        Response = "[(Y)es/(N)o]";
                    }
                    break;
            }
        }
    }
}
