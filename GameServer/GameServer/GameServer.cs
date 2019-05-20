using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GameServer
{
    public class GameServer
    {
        public static Leaderboard leaderboard = new Leaderboard();

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static bool stopped = false;

        public static void Main()
        {
            //Update the Leaderboard when the server starts up and create a new Thread to Update the Leaderboard daily.
            leaderboard.Refresh();

            Thread thread = new Thread(LeaderboardUpdate);
            //Begin listening for new connections
            StartListening();
        }

        public static void LeaderboardUpdate()
        {
            //Thread sleep while not midnight
            while (DateTime.Today.TimeOfDay.Ticks != 0)
            {
                Thread.Sleep(1);
            }
            leaderboard.Refresh();
        }

        public static void Log(string mes)
        {
            //|Logs
            //\    YYYY - MM - DD.log
            StreamWriter sw = new StreamWriter(string.Format("{0}\\Logs\\{1}-{2}-{3}.log", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), true);
            sw.Write("[" + DateTime.Now.TimeOfDay + "] " + mes + "\n");
            sw.Close();
            Console.WriteLine(mes);
        }

        public static void StartListening()
        {
            //Get the connection info for hosting the server
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[1];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1900);
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
 
            try
            {
                //Begin listening on the socket
                listener.Bind(localEndPoint);
                listener.Listen(100);
                Log("Game Server started!");
                while (stopped == false)
                {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Log(e.ToString());
            }

            Log("Stopped Server!");
            Console.Read();
        }

        //Microsoft example used for client/server connection, understood enough to use it, but not comment it :)
        public class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 1024;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject
            {
                workSocket = handler
            };
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            try
            {
                string[] content;

                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    content = state.sb.ToString().Split(new string[] { "%%" }, StringSplitOptions.None);
                    if (content[1] == "logout")
                    {
                        handler.Close();
                    }
                    else
                    {
                        Send(handler, CommandHandler.Process(handler.RemoteEndPoint.ToString(), content));
                    }

                }
            }
            catch (SocketException)
            {
            }
        }

        private static void Send(Socket handler, string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;

                handler.Shutdown(SocketShutdown.Send);
                handler.Close();

            }
            catch (Exception e)
            {
                Log(e.ToString());
            }
        }
    }
}
