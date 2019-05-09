using System;
using System.Collections.Generic;
using GameServer.Enemies;
using GameServer.Items;

namespace GameServer.Sequences
{
    class EXM : Sequence
    {
        public EXM(Player player, string[] command)
        {
            //List each subsequence that exists in the sequence the player will go through
            //Each subsequence should start with the 3 letter identifier of the Sequence
            SubSequences = new List<string> { "EXM1", "EXM2", "EXM3", "EXMCMBTSLIME", "EXMCHOICE", "EXMEND" };
            //List each enemy the area will contain
            EnemyList = new List<Enemy> { new Slime() };
            //Set the player's current enemy to whatever the player was fighting last
            //Always include this, regardless of whether the player will encounter something, just to be safe
            CurrentEnemy = GetCurrentEnemy(player);

            switch (player.Sequence.Substring(3))
            {
                case "1":
                    Response = "Give a brief description of the starting area." +
                        "\nThen prompt the player to do something." +
                        "\nMaybe choose a path to walk down or talk to a specific person.";
                    //Set the player's sequence to the next in line
                    player.SetSequence("EXM2");
                    break;
                case "2":
                    //Switch through each possible command the player could input from the last section
                    switch (command[1].ToLower())
                    {
                        case "option1":
                            Response = "This response shows when the player's last choice was 'option1'" +
                                "\nDon't forget to prompt the player for their next choice in this response!";
                            //Set the subsequence to wherever 'option1' leads
                            player.SetSequence("EXM3");
                            break;
                        case "option2":
                            Response = "This response shows when the player's last choice was 'option2'";
                            //Set the enemy the player will be fighting
                            CurrentEnemy = EnemyList[0];

                            Response += "\n\nYou are fighting " + CurrentEnemy.Name + " (" + CurrentEnemy.CurrentHealth + "hp)" +
                                     "\n(F)ight || (G)uard || (S)pell || (R)un || (H)elp";
                            player.SetSequence("EXMCMBTSLIME");
                            break;
                        default:
                            Response = "This response shows when the player's command wasn't in the choices." +
                                "\nGenerally a good response is just 'Invalid Command!'";
                            //Don't set the sequence to something else, it will automatically return them to this one
                            break;
                    }
                    break;
                case "3":
                    Response = "Maybe you want to send them back a section in a subsequence.";
                    player.SetSequence("EXM2");
                    break;
                case "CMBTSLIME":
                    //Set a currentEnemy variable to the player's Current Enemy
                    Enemy currentEnemy = CurrentEnemy;
                    //Create a string response where Combat() returns what happened in the turn of combat
                    string combatResponse = Combat(command, ref player, currentEnemy);
                    //Send the player the results of the round of combat
                    Response = combatResponse;
                    //Check to see if the player won, then advance them to the next subsequence after
                    if (combatResponse.StartsWith("\nYou won! ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Response += "Inform the player of after-fight results." +
                            "\nDon't forget to prompt them for the next choice after the fight is finished!";
                        player.SetSequence("EXMCHOICE");
                    }
                    else if (combatResponse.StartsWith("You escaped ", StringComparison.InvariantCultureIgnoreCase))
                    {

                    }
                    else
                    {
                        //Set the Current Enemy to the current enemy the player is in combat with
                        SetCurrentEnemy(player, currentEnemy);
                    }
                    //Do not change the sequence if the fight isn't over, it will automatically return them here each turn
                    break;
                case "CHOICE":
                    //Maybe you want to give the player items based on their choices
                    switch (command[1].ToLower())
                    {
                        case "sword":
                            Response = "Inform the player of the item they received" +
                                "\nPrompt them to finish up after they get the item.";
                            //Add the item to their Items list
                            player.Items.Add(new Sword());
                            player.SetSequence("EXMEND");
                            break;
                        case "shield":
                            Response = "Inform the player of the item they received" +
                                "\nPrompt them to finish up after they get the item.";
                            //Add the item to their Items list
                            player.Items.Add(new Shield());
                            player.SetSequence("EXMEND");
                            break;
                        case "bow":
                            Response = "Inform the player of the item they received" +
                                "\nPrompt them to finish up after they get the item.";
                            //Add the item to their Items list
                            player.Items.Add(new Bow());
                            player.SetSequence("EXMEND");
                            break;
                        case "spear":
                            Response = "Inform the player of the item they received" +
                                "\nPrompt them to finish up after they get the item.";
                            //Add the item to their Items list
                            player.Items.Add(new Spear());
                            player.SetSequence("EXMEND");
                            break;
                        default:
                            Response = "Invalid command!";
                            break;
                    }
                    break;
                case "END":
                    Response = "Then finish things up here with the story of the section." +
                        "\nSend them on to the next area afterwards, or back to town if there's no more to be done.";
                    player.SetSequence("TWN");
                    break;
            }
        }
    }
}
