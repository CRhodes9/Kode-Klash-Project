using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GameServer.Sequences;

namespace GameServer
{
    class CommandHandler
    {
        private static string response = "\n";

        public static string Process(string address, string[] command)
        {
            Player player;
            string playerStatsJson = "";
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + command[0];
            try
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
            catch (DirectoryNotFoundException)
            {
                Server.Log("Player Doesn't Exist! (" + command[0] + "). Creating a new file.");
                player = new Player(command[0]);

                Directory.CreateDirectory(path);
                File.Create(path + "\\stats.json").Close();
                SaveJson(command[0], player);
            }


            if (command[1] == "LOGIN")
            {
                response = Login(command, player, address);
                player.SetSequence("TWN1");
            }
            else
            {
                try
                {
                    Sequence sequence = (Sequence)Activator.CreateInstance(Type.GetType("GameServer.Sequences." + player.Sequence.Substring(0, 3)), player, command);
                    response = sequence.Response;
                }
                catch (ArgumentNullException)
                {
                    string error = "Error getting sequence! Setting to 'TWN1'. Contact an Admin if this continues. Player ID: " + player.UserID + " Sequence: " + player.Sequence.Substring(0, 3);
                    response += error;
                    Server.Log(error);
                    player.SetSequence("TWN1");
                }
            }

            SaveJson(command[0], player);
            return response;
        }

        private static string Login(string[] command, Player player, string address)
        {
            string response;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + command[0] + "\\stats.json"))
            {
                Server.Log("Logged in " + player.Name + "(" + command[0] + ")" + "{" + address + "}");
                if (player.FirstTime == true)
                {
                    response = "Success!" + 
@"-----------------------------------------------------------------
|                                                               |
|                                                               |
|                   Welcome to GAMENAMEHERE                     |
|            I don't know what else to put in this box          |
|                                                               |
|                                                               |
|                                                               |
-----------------------------------------------------------------" + "\n\nPlease enter your name: ";
                }
                else
                {
                    response = "Success!" + 
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
                        Sequence sequence = (Sequence)Activator.CreateInstance(Type.GetType("GameServer.Sequences." + player.Sequence.Substring(0, 3)), player, command);
                        response += sequence.Response;
                    }
                    catch (ArgumentNullException )
                    {
                        string error = "Error getting sequence! Setting to 'TWN1'. Contact an Admin if this continues. Player ID: " + player.UserID + " Sequence: " + player.Sequence.Substring(0, 3);
                        response += error;
                        Server.Log(error);
                        player.SetSequence("TWN1");
                    };
                }
            }
            else
            {
                response = "Failed Login!";
            }
            return response;
        }

        private static void SaveJson(string userId, Player player)
        {
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + userId + "\\stats.json", false);
            sw.Write(JsonConvert.SerializeObject(player));
            sw.Close();
        }
    }
}
