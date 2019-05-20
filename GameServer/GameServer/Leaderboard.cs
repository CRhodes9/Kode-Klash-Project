using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameServer
{
    public class Leaderboard
    {
        public List<Player> TopTen { get; set; }

        public void Refresh()
        {
            List<Player> playerList = new List<Player>();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Players\\";
            string[] playerFiles = Directory.GetFiles(path);
            //Go through each player and creates a Player object out of their Json file
            foreach (string fileName in playerFiles)
            {
                Player player;
                string playerStatsJson;

                try
                {
                    StreamReader sr = new StreamReader(path + fileName + "\\stats.json");

                    playerStatsJson = sr.ReadLine();

                    while (sr.ReadLine() != null)
                    {
                        playerStatsJson += sr.ReadLine();
                    }
                    sr.Close();

                    player = JsonConvert.DeserializeObject<Player>(playerStatsJson);

                    playerList.Add(player);
                }
                catch (DirectoryNotFoundException)
                {
                }
            }
            //Order the players by Highest Level to Lowest Level, then Highest Experience to Lowest Experience
            playerList.OrderByDescending(p => p.Level).ThenBy(p => p.Experience);
            //Create a Top 10, or as many as exist if less than 10, List of the highest Leveled players
            if (playerList.Count >= 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    TopTen.Add(playerList[i]);
                }
            }
            else
            {
                TopTen = playerList;
            }
        }
    }
}
