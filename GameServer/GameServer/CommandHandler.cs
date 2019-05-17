using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GameServer.Sequences;
using System.Security.Cryptography;
using System.Text;

namespace GameServer
{
    class CommandHandler
    {
        private static string response = "\n";

        public static string Process(string address, string[] command)
        {
            Player player;
            string playerStatsJson;
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + command[0];
            if (Directory.Exists(path))
            {
                StreamReader sr = new StreamReader(path + "\\stats.json");

                playerStatsJson = sr.ReadLine();

                while (sr.ReadLine() != null)
                {
                    playerStatsJson += sr.ReadLine();
                }
                sr.Close();

                player = JsonConvert.DeserializeObject<Player>(playerStatsJson);
            }
            else
            {
                player = new Player(command[0], GetMd5Hash(MD5.Create(), command[2]));
            }

            switch (command[1].ToLower())
            {
                case "newaccount":
                    response = "New Account Success";

                    SaveJson(command[0], player);
                    break;
                case "login":
                    response = Login(command, player, address);

                    SaveJson(command[0], player);
                    break;
                case "leaderboard":
                    int i = 1;
                    GameServer.leaderboard.TopTen.ForEach(p => response += string.Format("\n{0}. {1}({2}) {3} ({4}exp)", i++, p.Name, p.UserID, p.Level, p.Experience));
                    break;
                default:
                    try
                    {
                        Sequence sequence = (Sequence)Activator.CreateInstance(Type.GetType("GameServer.Sequences." + player.Sequence.Substring(0, 3)), player, command);
                        response = sequence.Response;
                        player.LastCommand = command;
                        player.LastResponse = sequence.Response;
                        response += string.Format("%%{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}/{13}%%", player.Name, player.Level, player.Experience, player.Gold, player.MaxHealth, player.CurrentHealth, player.MaxMana, player.CurrentMana, player.Strength, player.Dexterity, player.Constitution, player.Intelligence, player.Wisdom, player.Charisma);
                        player.Items.ForEach(item => response += item.Name + "/");
                        response += "%%";
                        player.Spells.ForEach(s => response += s.Name + "/");

                        SaveJson(command[0], player);
                    }
                    catch (ArgumentNullException)
                    {
                        string error = "Error getting sequence! Setting to 'TWN1'. Contact an Admin if this continues. Player ID: " + player.UserID + " Sequence: " + player.Sequence.Substring(0, 3);
                        response += error;
                        GameServer.Log(error);
                        player.SetSequence("TWN1");

                        SaveJson(command[0], player);
                    }
                    break;
            }
            return response;
        }

        private static string Login(string[] command, Player player, string address)
        {
            string response;
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, command[2]);

                if (VerifyMd5Hash(md5Hash, command[2], player.Password))
                {
                    GameServer.Log("Logged in " + player.Name + "(" + command[1] + ")" + "{" + address + "}");
                    if (player.FirstTime == true)
                    {
                        response = "Success!%%" +
@"-----------------------------------------------------------------
|                                                               |
|                                                               |
|                   Welcome to GAMENAMEHERE                     |
|            I don't know what else to put in this box          |
|                                                               |
|                                                               |
|                                                               |
-----------------------------------------------------------------" + "\n\nPlease enter your name: ";
                        response += string.Format("%%{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}/{13}%%", player.Name, player.Level, player.Experience, player.Gold, player.MaxHealth, player.CurrentHealth, player.MaxMana, player.CurrentMana, player.Strength, player.Dexterity, player.Constitution, player.Intelligence, player.Wisdom, player.Charisma);
                        player.Items.ForEach(i => response += i.Name + "/");
                        response += "%%";
                        player.Spells.ForEach(s => response += s.Name + "/");
                    }
                    else
                    {
                        response = "Success!%%" +
@"-----------------------------------------------------------------
|                                                               |
|                                                               |
|                   Welcome back to GAMENAMEHERE                |
|            I don't know what else to put in this box          |
|                                                               |
|                                                               |
|                                                               |
-----------------------------------------------------------------" + "\n\n";


                        try
                        {
                            response += player.LastResponse;
                            Sequence sequence = (Sequence)Activator.CreateInstance(Type.GetType("GameServer.Sequences." + player.Sequence.Substring(0, 3)), player, command);
                            player.LastCommand = command;
                            player.LastResponse = sequence.Response;
                            response += sequence.Response;
                            response += string.Format("%%{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}/{13}%%", player.Name, player.Level, player.Experience, player.Gold, player.MaxHealth, player.CurrentHealth, player.MaxMana, player.CurrentMana, player.Strength, player.Dexterity, player.Constitution, player.Intelligence, player.Wisdom, player.Charisma);
                            player.Items.ForEach(i => response += i.Name + "/");
                            response += "%%";
                            player.Spells.ForEach(s => response += s.Name + "/");
                        }
                        catch (ArgumentNullException)
                        {
                            string error = "Error getting sequence! Setting to 'TWN1'. Contact an Admin if this continues. Player ID: " + player.UserID + " Sequence: " + player.Sequence.Substring(0, 3);
                            response += error;
                            GameServer.Log(error);
                            player.SetSequence("TWN1");
                        };
                    }
                }
                else
                {
                    response = "Failure!";
                }
            }
            return response;
        }

        private static string GetMd5Hash(MD5 md5Hash, string text)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private static bool VerifyMd5Hash(MD5 md5Hash, string text, string hash)
        {
            string hashOfText = GetMd5Hash(md5Hash, text);

            if (hashOfText == hash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static void SaveJson(string userId, Player player)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + userId + "\\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            StreamWriter sw = new StreamWriter(path + "stats.json", false);
            sw.Write(JsonConvert.SerializeObject(player));
            sw.Close();
        }
    }
}
