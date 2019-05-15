using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GameClient
{
    public partial class LoginForm : Form
    {
        public static string[] response;
        private static AutoResetEvent sendDone = new AutoResetEvent(false);
        private static AutoResetEvent receiveDone = new AutoResetEvent(false);
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            StartClient();
        }

        private void StartClient()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry("127.0.0.1");
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 1900);
                Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne(500);

                string username = usernameTextBox.Text;
                string password = passwordTextBox.Text;

                Send(client, usernameTextBox.Text + "%%" + "LOGIN%%" + password);
                sendDone.WaitOne(500);
                Receive(client);
                receiveDone.WaitOne(500);

                if (response[0] == "Success!")
                {
                    ClientForm.username = username;
                    ClientForm.response = response;
                    Send(client, username);
                    Close();
                }
                else if (response[0] == "Failure!")
                {

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 1024;
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
                    response = state.sb.ToString().Split(new string[] { "%%" }, StringSplitOptions.None);
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
            }
        }

        private void ForgotPasswordLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            forgotPasswordLinkLabel.LinkVisited = true;

            System.Diagnostics.Process.Start("websiteurl");
        }
    }
}
