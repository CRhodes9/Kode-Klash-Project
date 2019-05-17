using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GameClient
{
    public partial class ClientForm : Form
    {
        public Player player;

        public static IPHostEntry ipHostInfo;
        public static IPAddress ipAddress;
        public static IPEndPoint remoteEP;
        public static Socket client;

        public static string username;
        public static string[] response;
        public static bool loginStatus = false;

        private static AutoResetEvent sendDone = new AutoResetEvent(false);
        private static AutoResetEvent receiveDone = new AutoResetEvent(false);

        public ClientForm()
        {
            InitializeComponent();

            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();

            string[] playerStats = response[2].Split('/');

            player = new Player()
            {
                Name = playerStats[0],
                Level = int.Parse(playerStats[1]),
                Experience = ulong.Parse(playerStats[2]),
                Gold = int.Parse(playerStats[3]),
                MaxHealth = int.Parse(playerStats[4]),
                CurrentHealth = int.Parse(playerStats[5]),
                MaxMana = int.Parse(playerStats[6]),
                CurrentMana = int.Parse(playerStats[7]),
                Strength = int.Parse(playerStats[8]),
                Dexterity = int.Parse(playerStats[9]),
                Constitution = int.Parse(playerStats[10]),
                Intelligence = int.Parse(playerStats[11]),
                Wisdom = int.Parse(playerStats[12]),
                Charisma = int.Parse(playerStats[13])
            };

            outputRichTextBox.AppendText(response[1]);
        }
        private void UpdatePlayerInfo()
        {
            TabPage playerTab = tabControl1.TabPages[0];

            string[] playerStats = response[1].Split('/');
            string[] playerItems = response[2].Split('/');
            string[] playerSpells = response[3].Split('/');

            player.Name = playerStats[0];
            player.Level = int.Parse(playerStats[1]);
            player.Experience = ulong.Parse(playerStats[2]);
            player.Gold = int.Parse(playerStats[3]);
            player.MaxHealth = int.Parse(playerStats[4]);
            player.CurrentHealth = int.Parse(playerStats[5]);
            player.MaxMana = int.Parse(playerStats[6]);
            player.CurrentMana = int.Parse(playerStats[7]);
            player.Strength = int.Parse(playerStats[8]);
            player.Dexterity = int.Parse(playerStats[9]);
            player.Constitution = int.Parse(playerStats[10]);
            player.Intelligence = int.Parse(playerStats[11]);
            player.Wisdom = int.Parse(playerStats[12]);
            player.Charisma = int.Parse(playerStats[13]);

            UpdatePlayerTab(player);
            playerTab.Name = username;
            string items = "";
            foreach (string item in playerItems)
            {
                items += item + "\n";
            }
            playerItemsTabRichTextBox.Text = items;
            string spells = "";
            foreach (string spell in playerSpells)
            {
                spells += spell + "\n";
            }
            playerSpellsTabRichTextBox.Text = spells;
        }
        private void UpdatePlayerTab(Player player)
        {
            playerTabPlayerInfoRichTextBox.Text = string.Format(
                "Name: {0}" +
                "\nLevel: {1}" +
                "\nExperience: {2}" +
                "\nGold: {3}g" +
                "\nStrength: {4}" +
                "\nDexterity: {5}" +
                "\nConstitution: {6}" +
                "\nIntelligence {7}" +
                "\nWisdom: {8}" +
                "\nCharisma: {9}",
                player.Name, player.Level, player.Experience, player.Gold, player.Strength, player.Dexterity, player.Constitution, player.Intelligence, player.Wisdom, player.Charisma);
        }

        #region Connection
        private void SendCommand(string command)
        {
            ipHostInfo = Dns.GetHostEntry("127.0.0.1");
            ipAddress = ipHostInfo.AddressList[0];
            remoteEP = new IPEndPoint(ipAddress, 1900);
            client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);

            Send(client, username + "%%" + command);
            sendDone.WaitOne(500);
            Receive(client);
            receiveDone.WaitOne(500);

            if (response.Length > 2)
            {
                UpdatePlayerInfo();
            }

            outputRichTextBox.AppendText("\n\n" + response[0]);
        }

        public class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 256;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
            }
            catch (Exception e)
            {
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString().Split(new string[] { "%%" }, StringSplitOptions.None);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void Send(Socket client, string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string command = inputTextBox.Text;
            inputTextBox.Clear();
            SendCommand(command);
        }
        #endregion
    }
}
