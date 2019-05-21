using Newtonsoft.Json;
using System;
using System.IO;
using GameServer.Sequences;
using System.Security.Cryptography;
using System.Text;

namespace GameServer
{
    class CommandHandler
    {
        private static string response;

        public static string Process(string address, string[] command)
        {
            Player player;
            string playerStatsJson;

            //File Tree
            //| GameServer.exe
            //|
            //+---Logs
            //|        YYYY - MM - DD.log
            //|        YYYY - MM - DD.log
            //|
            //\---Players
            //    + ---000
            //    |       currentenemy.json
            //    |       stats.json
            //    |
            //    \---001
            //            currentenemy.json
            //            stats.json

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
                //Convert Json file to Player object
                player = JsonConvert.DeserializeObject<Player>(playerStatsJson);
            }
            else
            {
                //If the PlayerID doesn't exist, set up the player var as a new player with the username and password entered
                player = new Player(command[0], GetMd5Hash(MD5.Create(), command[2]));
            }

            switch (command[1].ToLower())
            {
                //Creating a new player account
                case "newaccount":
                    response = "New Account Success";
                    //Save the generated Player as a new player file
                    SaveJson(command[0], player);
                    break;
                case "login":
                    response = Login(command, player, address);

                    SaveJson(command[0], player);
                    break;
                case "leaderboard":
                    int i = 1;
                    response = "\n";
                    GameServer.leaderboard.TopTen.ForEach(p => response += string.Format("\n{0}. {1}({2}) {3} ({4}exp)", i++, p.Name, p.UserID, p.Level, p.Experience));
                    break;
                //This will 100% break if you try and log in with an account that doesn't exist, I think
                //Just ignore that and it will be fixed by the time you remember it
                default:
                    response = SequenceResponse(player, command);
                    break;
            }
            return response;
        }

        private static string Login(string[] command, Player player, string address)
        {
            string response;

            //Password hash checking
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, command[2]);

                if (VerifyMd5Hash(md5Hash, command[2], player.Password))
                {
                    GameServer.Log("Logged in " + player.Name + "(" + command[1] + ")" + "{" + address + "}");
                    //If the player hasn't played before prompt them to start playing
                    if (player.FirstTime == true)
                    {
                        response = "Success!%%" +
                    "-----------------------------------------------------------------\n" +
                    "|                                                               |\n" +
                    "|                                                               |\n" +
                    "|                   Welcome to GAMENAMEHERE                     |\n" +
                    "|            I don't know what else to put in this box          |\n" +
                    "|                                                               |\n" +
                    "|                                                               |\n" +
                    "|                                                               |\n" +
                    "-----------------------------------------------------------------\n" 
                    + "\nPlease enter your name: ";
                        response += string.Format("%%{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}/{13}%%", player.Name, player.Level, player.Experience, player.Gold, player.MaxHealth, player.CurrentHealth, player.MaxMana, player.CurrentMana, player.Strength, player.Dexterity, player.Constitution, player.Intelligence, player.Wisdom, player.Charisma);
                        player.Items.ForEach(i => response += i.Name + "/");
                        response += "%%";
                        player.Spells.ForEach(s => response += s.Name + "/");
                        player.FirstTime = false;
                    }
                    else
                    {
                        response = "Success!%%" +
                    "-----------------------------------------------------------------\n" +
                    "|                                                               |\n" +
                    "|                                                               |\n" +
                    "|                   Welcome to GAMENAMEHERE                     |\n" +
                    "|            I don't know what else to put in this box          |\n" +
                    "|                                                               |\n" +
                    "|                                                               |\n" +
                    "|                                                               |\n" +
                    "-----------------------------------------------------------------\n";

                        response += player.LastResponse + string.Format("%%{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}/{13}%%", player.Name, player.Level, player.Experience, player.Gold, player.MaxHealth, player.CurrentHealth, player.MaxMana, player.CurrentMana, player.Strength, player.Dexterity, player.Constitution, player.Intelligence, player.Wisdom, player.Charisma);
                        player.Items.ForEach(item => response += item.Name + "/");
                        response += "%%";
                        player.Spells.ForEach(s => response += s.Name + "/");

                    }
                }
                else
                {
                    response = "Failure!";
                }
            }
            return response;
        }

        private static string SequenceResponse(Player player, string[] command)
        {
            try
            {
                //Set the player's Sequence to an instance of the sequence noted in the player file
                //EX: 'FST1' becomes an instance of FST.cs using the number case in Substring(3) to determine section
                Sequence sequence = (Sequence)Activator.CreateInstance(Type.GetType("GameServer.Sequences." + player.Sequence.Substring(0, 3)), player, command);
                //Set the response to be sent to the player to the response from the sequence case
                response = sequence.Response;
                //Set the last command entered and last response sent for re-logging purposes
                player.LastCommand = command;
                player.LastResponse = sequence.Response;
                //Send the player stats/gear/spells to be updated on the client side
                response += string.Format("%%{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}/{13}%%", player.Name, player.Level, player.Experience, player.Gold, player.MaxHealth, player.CurrentHealth, player.MaxMana, player.CurrentMana, player.Strength, player.Dexterity, player.Constitution, player.Intelligence, player.Wisdom, player.Charisma);
                player.Items.ForEach(item => response += item.Name + "/");
                response += "%%";
                player.Spells.ForEach(s => response += s.Name + "/");

                SaveJson(command[0], player);
            }
            catch (ArgumentNullException)
            {
                //Errors with the player's sequence sends the player back to Town and lets them know the error causing sequence, and to alert an admin if the issue persists
                string error = "Error getting sequence! Setting to 'TWN1'. Contact an Admin if this continues. Player ID: " + player.UserID + " Sequence: " + player.Sequence.Substring(0, 3);
                response += error;
                GameServer.Log(error);
                player.SetSequence("TWN1");

                SaveJson(command[0], player);
            }

            return response;
        }

        private static string GetMd5Hash(MD5 md5Hash, string text)
        {
            //Hash the input text of the password
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder sb = new StringBuilder();
            //Convert the byte array to the string version of the hashed text
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
            //Convert the updated Player object back to the Json file
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
