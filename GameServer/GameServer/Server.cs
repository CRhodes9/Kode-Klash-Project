using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GameServer
{
    public class Server
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static bool stopped = false;

        public static void Main()
        {
            StartListening();
        }

        public static void Log(string mes)
        {
            StreamWriter sw = new StreamWriter(String.Format("{0}\\Logs\\{1}-{2}-{3}.log", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), true);
            sw.Write("[" + DateTime.Now.TimeOfDay + "] " + mes + "\n");
            sw.Close();
            Console.WriteLine(mes);
        }

        public static void StartListening()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry("127.0.0.1");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1900);
            
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
 
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

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

            Log("Stopped Server..");
            Console.Read();
        }

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
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    content = state.sb.ToString().Split();

                    Send(handler, CommandHandler.Process(handler.RemoteEndPoint.ToString(), content));

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
