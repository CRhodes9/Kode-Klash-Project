using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameClient
{
    public class Client
    {
        private static string userId;
        public static string response = "";
        public static bool loginStatus = false;
        private static AutoResetEvent sendDone = new AutoResetEvent(false);
        private static AutoResetEvent receiveDone = new AutoResetEvent(false);

        public static void Main()
        {
            StartClient();
        }

        private static void StartClient()
        {
retry:
            try
            {
                IPHostEntry ipHostInfo;
                IPAddress ipAddress;
                IPEndPoint remoteEP;
                Socket client;

                Console.WriteLine("Please Log In");
                userId = Console.ReadLine() + " ";
                ipHostInfo = Dns.GetHostEntry("127.0.0.1");
                ipAddress = ipHostInfo.AddressList[0];
                remoteEP = new IPEndPoint(ipAddress, 1900);
                client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                
                Send(client, userId + "LOGIN");
                Receive(client);
                receiveDone.WaitOne(1000);

                if (response.StartsWith("Success!"))
                {
                    loginStatus = true;
                    Console.Clear();
                    Console.WriteLine(response.Substring(9));
                    Send(client, "");
                    ContinueClient(ipHostInfo, ipAddress, remoteEP, userId);
                }
                else
                {
                    Console.WriteLine(response);
                    goto retry;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ContinueClient(IPHostEntry ipHostInfo, IPAddress ipAddress, IPEndPoint remoteEP, String userId)
        {
            while (loginStatus == true)
            {
                String input = Console.ReadLine();
                Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);

                if (input != null)
                {
                    switch (input.ToLower())
                    {
                        case "logout":
                            loginStatus = false;
                            client.Close();
                            break;
                        case "clear":
                        case "clr":
                            Console.Clear();
                            break;
                        default:
                            Send(client, userId + input);
                            sendDone.WaitOne(500);
                            Receive(client);
                            receiveDone.WaitOne(500);
                            Console.WriteLine(response);
                            break;
                    }
                }
            }
        }

        public class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 256;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
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
                        response = state.sb.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Send(Socket client, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
