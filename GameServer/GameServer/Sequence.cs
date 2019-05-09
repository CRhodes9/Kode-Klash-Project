using GameServer.Enemies;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Sequences
{
    public class Sequence
    {
        public List<string> SubSequences { get; set; }
        public string Response { get; set; }
        public List<Enemy> EnemyList { get; set; }
        public Enemy CurrentEnemy { get; set; }

        public Sequence()
        {
            SubSequences = new List<string>();
            Response = "";
            EnemyList = new List<Enemy>();
        }

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
                Server.Log("Enemy File Doesn't Exist! (" + player.UserID + ")");
                currentEnemy = new Enemy();
                File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + player.UserID + "\\currentenemy.json").Close();
                SetCurrentEnemy(player, currentEnemy);
            }

            return currentEnemy;
        }

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

            if (player.CurrentHealth <= 0)
            {
                response = "You died!" +
                    "\n\nYou are dead!";
                player.SetSequence("DED");

                return response;
            }
            else if (currentEnemy.CurrentHealth <= 0)
            {
                Item droppedItem = currentEnemy.DroppedItemsPool[ranGen.Next(currentEnemy.DroppedItemsPool.Count)];
                ulong droppedXP = currentEnemy.DroppedExperience;
                response = "You won! You defeated the " + currentEnemy.Name + " !" +
                    "\nYou gained " + droppedXP + " experience and " + droppedItem.Name + "!" +
                    "\nPress Enter to continue...";

                player.AddExperience(currentEnemy.DroppedExperience);
                player.Items.Add(droppedItem);

                return response;
            }
            else
            {
                int playerHit = player.Attack(currentEnemy);
                int enemyHit = currentEnemy.Attack(currentEnemy);

                switch (command[1].ToLower())
                {
                    case "f":
                    case "fight":
                        response = "You attack the " + currentEnemy.Name + " for " + playerHit + " damage!";
                        currentEnemy.Hurt(playerHit);
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
                            player.Hurt(enemyHit);
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
                    case "g":
                    case "guard":
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
                    case "s":
                    case "spell":
                        response = "List of Spells: ";
                        player.Spells.ForEach(spell => response += spell.Name + " ");

                        response += "\n\nYou are fighting " + currentEnemy.Name + " (" + currentEnemy.CurrentHealth + "hp)" +
                            "\n(F)ight || (G)uard || (S)pell || (R)un || (H)elp";
                        break;
                    case "r":
                    case "run":
                        if (player.Dexterity > currentEnemy.Dexterity * 1.3)
                        {
                            response = "You escaped from the " + currentEnemy.Name + "!";

                        }
                        else
                        {
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
                    case "h":
                    case "help":
                        response = "TODO: Make Help";
                        break;
                    default:
                        if (player.Spells.Count > 0)
                        {
                            Spell spellCast = player.Spells.Where(spell => spell.Name.ToLower() == command[1].ToLower()).First();

                            if (spellCast != null)
                            {
                                response = spellCast.Cast(player, currentEnemy);

                                response += "\n\nYou are fighting " + currentEnemy.Name + " (" + currentEnemy.CurrentHealth + "hp)" +
                                         "\n(F)ight || (G)uard || (S)pell || (R)un || (H)elp";

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