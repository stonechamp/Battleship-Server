//-----------------------------------------------------------
//File:   Prog5.cs
//Desc:   Battleship Server by Stone Champion
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Battleship.Model
{
    class Server
    {
        public string serverLock = "LOCK";
        const int Port = 6500;
        private Action<string> logMessageAction;
        public Dictionary<string, Game> Games = new Dictionary<string, Game>();
        
        //Starts the server on a thread.
        public void StartServer(Action<string> logMessageAction)
        {
            //game = new Game(10, "demo");
            TcpListener listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine("Listening on port " + Port);
            this.logMessageAction = logMessageAction;
            while (true)
            {
                Console.WriteLine("Waiting for client to connect.");
                TcpClient tcpClient = listener.AcceptTcpClient();
                
                Task.Run(()=>HandleClient(tcpClient));
                
            }
        }

   
        //Handles the incoming client.
        private void HandleClient(TcpClient tcpClient)
        {
            string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
            Console.WriteLine("Received connection request from " + clientEndPoint);

            try
            {
                using (NetworkStream networkStream = tcpClient.GetStream())
                {
                    StreamReader reader = new StreamReader(networkStream);
                    StreamWriter writer = new StreamWriter(networkStream);

                    writer.WriteLine("Welcome to Battleship. What game do you wish to join?");
                    writer.Flush();

                    string gamename = reader.ReadLine();
                    Game game;
                    if (Games.ContainsKey(gamename))
                    {
                        game = Games[gamename];
                    }else
                    {
                        game = new Game(10, gamename);
                        Games.Add(gamename, game);
                    }
                    
                    
                    
                    game.SendGameState(writer, game);
                    
                    
                    

                    logMessageAction("Player " + clientEndPoint + " has joined the game: " + game.Name);

                    string requestStr = reader.ReadLine();
                    while (requestStr != null)
                    {


                        string responseStr;
                        lock (serverLock)
                        {
                            var requestMsg = RequestMessage.Deserialize(requestStr);
                            ResponseMessage responseMsg = requestMsg.Execute();
                            responseStr = responseMsg.Serialize(game, writer, logMessageAction, clientEndPoint);
                        }
                        
                        

                        
                        writer.WriteLine(responseStr);
                        writer.Flush();
                        requestStr = reader.ReadLine();


                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Network error: " + e.Message);
            }

            tcpClient.Close();

            // Client closed connection
            Console.WriteLine("Client closed connection.");
        }


    }
}
