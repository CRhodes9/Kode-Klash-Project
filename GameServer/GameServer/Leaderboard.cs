using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            playerList.OrderByDescending(p => p.Level).ThenBy(p => p.Experience);

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
