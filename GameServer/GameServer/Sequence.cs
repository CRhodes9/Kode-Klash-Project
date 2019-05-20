using GameServer.Enemies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameServer.Sequences
{
    public class Sequence
    {
        //The Substring case of the Sequence the player is on
        //EX: FST(1) FST(2) FST(3)
        public List<string> SubSequences { get; set; }
        //The response to be sent to the player based on their choices in the SubSequence
        public string Response { get; set; }
        //The list of all enemies that can appear in the Sequence
        public List<Enemy> EnemyList { get; set; }
        //The current enemy the player is engaged with in the Sequence
        public Enemy CurrentEnemy { get; set; }

        public Sequence()
        {
            SubSequences = new List<string>();
            Response = "";
            EnemyList = new List<Enemy>();
        }

        //Reads the current enemy file from the speicfic player
        public Enemy GetCurrentEnemy(Player player)
        {
            Enemy currentEnemy;
            string currentEnemyJson;
            try
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + player.UserID + "\\currentenemy.json");

                currentEnemyJson = sr.ReadLine();

                while (sr.ReadLine() != null)
                {
                    currentEnemyJson += sr.ReadLine();
                }
                sr.Close();

                currentEnemy = JsonConvert.DeserializeObject<Enemy>(currentEnemyJson);
            }
            catch (FileNotFoundException)
            {
                GameServer.Log("Enemy File Doesn't Exist! (" + player.UserID + ")");
                currentEnemy = new Enemy();
                File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + player.UserID + "\\currentenemy.json").Close();
                SetCurrentEnemy(player, currentEnemy);
            }

            return currentEnemy;
        }
        //Sets the player's current enemy to the Enemy they should be fighting, writing it to the currentenemy file
        public void SetCurrentEnemy(Player player, Enemy enemy)
        {
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + player.UserID + "\\currentenemy.json", false);
            sw.Write(JsonConvert.SerializeObject(enemy));
            sw.Close();
        }

        public string Combat(string[] command, ref Player player, Enemy currentEnemy)
        {
            Random ranGen = new Random();
            string response = "";
            //If the player's health is 0 or lower, they are dead
            if (player.CurrentHealth <= 0)
            {
                response = "You died!" +
                    "\n\nYou are dead!";
                player.SetSequence("DED");

                return response;
            }
            //Likewise if the enemy's health is 0 or lower, they are also dead
            //Yes, this results in the player dying before the enemy if they both reach 0 at the same time
            //Yes, this is intended
            else if (currentEnemy.CurrentHealth <= 0)
            {
                //Grab a random item from the enemy's list of dropped items
                Item droppedItem = currentEnemy.DroppedItemsPool[ranGen.Next(currentEnemy.DroppedItemsPool.Count)];
                //Get the xp the enemy drops
                ulong droppedXP = currentEnemy.DroppedExperience;
                response = "You won! You defeated the " + currentEnemy.Name + " !" +
                    "\nYou gained " + droppedXP + " experience and " + droppedItem.Name + "!" +
                    "\nPress Enter to continue...";
                //Reward the player with the spoils of war
                player.AddExperience(currentEnemy.DroppedExperience);
                player.Items.Add(droppedItem);

                return response;
            }
            else
            {
                //Determine the damage both combatants will do
                int playerHit = player.Attack(currentEnemy);
                int enemyHit = currentEnemy.Attack(currentEnemy);

                switch (command[1].ToLower())
                {
                    //If the player decides to fight
                    case "f":
                    case "fight":
                        response = "You attack the " + currentEnemy.Name + " for " + playerHit + " damage!";
                        //Damage the enemy by the hit amount
                        currentEnemy.Hurt(playerHit);
                        //Check to see if that hit defeated the enemy
                        if (currentEnemy.CurrentHealth <= 0)
                        {
                            Item droppedItem = currentEnemy.DroppedItemsPool[ranGen.Next(currentEnemy.DroppedItemsPool.Count)];
                            ulong droppedXP = currentEnemy.DroppedExperience;
                            response += "\nYou won! You defeated the " + currentEnemy.Name + "!" +
                                "\nYou gained " + droppedXP + " experience and " + droppedItem.Name + "!" +
                                "\nPress Enter to continue...";

                            player.AddExperience(currentEnemy.DroppedExperience);
                            player.Items.Add(droppedItem);
                        }
                        //If the enemy is still alive, attack back
                        else
                        {
                            response += "\nThe " + currentEnemy.Name + " attacks you for " + enemyHit + " damage!";
                            //Hit the player for the enemy's hit amount
                            player.Hurt(enemyHit);
                            //Check to see if the player is defeated from that
                            if (player.CurrentHealth <= 0)
                            {
                                response += "\nYou died!" +
                                    "\n\nYou are dead!";
                                player.SetSequence("DED");
                            }
                            //Prompt the player to continue the fight
                            else
                            {
                                response += "\n\nYou are fighting " + currentEnemy.Name + " (" + currentEnemy.CurrentHealth + "hp)" +
                                    "\n(F)ight || (G)uard || (S)pell || (R)un || (H)elp";
                            }
                        }
                        break;
                    //If the player decides to guard
                    case "g":
                    case "guard":
                        //Lower the enemy hit amount by the player's Armor value
                        enemyHit -= player.Armor;
                        response = "You guard against the " + currentEnemy.Name + "'s attacks!" +
                            "\nThe " + currentEnemy.Name + " attacks you for " + enemyHit + " damage!";
                        player.Hurt(enemyHit - player.Armor);
                        if (player.CurrentHealth <= 0)
                        {
                            response += "\nYou died!" +
                                "\n\nYou are dead!";
                            player.SetSequence("DED");
                        }
                        else
                        {
                            response += "\n\nYou are fighting " + currentEnemy.Name + " (" + currentEnemy.CurrentHealth + "hp)" +
                                "\n(F)ight || (G)uard || (S)pell || (R)un || (H)elp";
                        }
                        break;
                    //If the player decides to cast a spell, list off the spells they can cast
                    case "s":
                    case "spell":
                        response = "List of Spells: ";
                        player.Spells.ForEach(spell => response += spell.Name + " ");

                        response += "\n\nYou are fighting " + currentEnemy.Name + " (" + currentEnemy.CurrentHealth + "hp)" +
                            "\n(F)ight || (G)uard || (S)pell || (R)un || (H)elp";
                        break;
                    //If the player decides to run from the fight
                    case "r":
                    case "run":
                        //If the player is 30% more Charismatic than the enemy, they manage to run
                        if (player.Charisma > currentEnemy.Charisma * 1.3)
                        {
                            response = "You escaped from the " + currentEnemy.Name + "!";

                        }
                        else
                        {
                            //If the escape failed, the enemy gets a free hit in
                            response = "You failed to escape!" +
                                "\nThe " + currentEnemy.Name + " attacks you for " + enemyHit + " damage!";
                            player.Hurt(enemyHit - player.Armor);
                            if (player.CurrentHealth <= 0)
                            {
                                response += "\nYou died!" +
                                    "\n\nYou are dead!";
                                player.SetSequence("DED");
                            }
                            else
                            {
                                response += "\n\nYou are fighting " + currentEnemy.Name + " (" + currentEnemy.CurrentHealth + "hp)" +
                                    "\n(F)ight || (G)uard || (S)pell || (R)un || (H)elp";
                            }
                        }
                        break;
                    //TODO: Make Help
                    case "h":
                    case "help":
                        response = "TODO: Make Help";
                        break;
                    default:
                        //Check if the player has any spells
                        if (player.Spells.Count > 0)
                        {
                            //Check the command to see if the player typed in a spell they know
                            Spell spellCast = player.Spells.Where(spell => spell.Name.ToLower() == command[1].ToLower()).First();
                            //If it does, cast it and process the effects
                            if (spellCast != null)
                            {
                                response = spellCast.Cast(player, currentEnemy);
                                //Combat continues
                                if (currentEnemy.CurrentHealth <= 0)
                                {
                                    Item droppedItem = currentEnemy.DroppedItemsPool[ranGen.Next(currentEnemy.DroppedItemsPool.Count)];
                                    ulong droppedXP = currentEnemy.DroppedExperience;
                                    response += "\nYou won! You defeated the " + currentEnemy.Name + "!" +
                                        "\nYou gained " + droppedXP + " experience and " + droppedItem.Name + "!" +
                                        "\nPress Enter to continue...";

                                    player.AddExperience(currentEnemy.DroppedExperience);
                                    player.Items.Add(droppedItem);
                                }
                                else
                                {
                                    response += "\nThe " + currentEnemy.Name + " attacks you for " + enemyHit + " damage!";
                                    player.Hurt(enemyHit - player.Armor);
                                    if (player.CurrentHealth <= 0)
                                    {
                                        response += "\nYou died!" +
                                            "\n\nYou are dead!";
                                        player.SetSequence("DED");
                                    }
                                    else
                                    {
                                        response += "\n\nYou are fighting " + currentEnemy.Name + " (" + currentEnemy.CurrentHealth + "hp)" +
                                            "\n(F)ight || (G)uard || (S)pell || (R)un || (H)elp";
                                    }
                                }

                            }
                            else
                            {
                                response = "Invalid Command!";
                            }
                        }
                        else
                        {
                            response = "Invalid Command!";
                        }
                        break;
                }

                return response;
            }
        }
    }
}