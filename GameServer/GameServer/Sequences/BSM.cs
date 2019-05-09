using System;
using System.Collections.Generic;
using GameServer.Enemies;
using GameServer.Items;

namespace GameServer.Sequences
{
    class BSM : Sequence
    {
        public BSM(Player player, string[] command)
        {
            SubSequences = new List<string> { "BSM1", "BSM2", "BSMBUY", "BSMSELL" };
            EnemyList = new List<Enemy> { };

            List<Item> shopList = new List<Item> { new Shortsword() };
            switch (player.Sequence.Substring(3))
            {
                case "1":
                    Response = "You walk into the blacksmith's shop." +
                        "\nAs you enter, a booming voice rings out. \"Welcome! What can I do for you?\"" +
                        "\n(B)uy (S)ell (T)alk (E)xit";
                    player.SetSequence("BSM2");
                    break;
                case "2":
                    switch (command[1].ToLower())
                    {
                        case "b":
                        case "buy":
                            Response = "\"Well here's what I've got for you today!\"";
                            shopList.ForEach(i => Response += "\n" + i.Name + "(" + i.BuyPrice + "g)");
                            player.SetSequence("BSMBUY");
                            break;
                        case "s":
                        case "sell":
                            Response = "\"What have you got for me today?\"";
                            foreach (Item i in player.Items)
                            {
                                if (i.SellPrice > 0)
                                {
                                    Response += "\n" + i.Name + "(" + i.SellPrice + "g)";
                                }
                            }
                            player.SetSequence("BSMSELL");
                            break;
                        case "t":
                        case "talk":
                            Response = "\"Ha.\"" +
                                "\n(B)uy (S)ell (T)alk (E)xit";
                            player.SetSequence("BSM2");
                            break;
                    }
                    break;
                case "BUY":
                    if (shopList.Exists(i => i.Name == command[1]))
                    {
                        Item item = shopList.Find(i => i.Name.ToLower() == command[1].ToLower());
                        if (player.Gold < item.BuyPrice)
                        {
                            Response = "\"Sorry, you don't have enough for that!\"" +
                                "\n(B)uy (S)ell (T)alk (E)xit";
                        }
                        else
                        {
                            Response = "\"Pleasure doing business with you!\"" +
                                "\nYou bought " + item.Name + " for " + item.BuyPrice + "g" +
                                "\n(B)uy (S)ell (T)alk (E)xit";
                            player.Gold -= item.BuyPrice;
                            player.Items.Add(item);
                        }
                    }
                    else
                    {
                        Response = "\"Sorry, I don't have that!\"";
                    }

                    player.SetSequence("BSM1");
                    break;
                case "SELL":
                    if (player.Items.Contains(new Item() { Name = command[1] }))
                    {
                        Item item = player.Items.Find(i => i.Name.ToLower() == command[1].ToLower());

                        if (item.SellPrice != 0)
                        {
                            Response = "\"Pleasure doing business with you!\"" +
                                "\nYou sold " + item.Name + " for " + item.SellPrice + "g" +
                                "\n(B)uy (S)ell (T)alk (E)xit";
                            player.Gold += item.SellPrice;
                            player.Items.Remove(item);
                        }
                    }
                    else
                    {
                        Response = "\"You're trying to sell me WHAT?\"";
                    }

                    player.SetSequence("BSM1");
                    break;
            }
        }
    }
}
