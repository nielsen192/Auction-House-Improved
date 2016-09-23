using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace AuctionServer
{
    internal class ClientHandler
    {
        // Creating socket for the client
        Socket _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Auction _auction;

        public ClientHandler(Socket client, Auction auction)
        {
            this._client = client;
            this._auction = auction;

            // Connect to the broadcast
            _auction.broadcastEvent += _auction_broadcastEvent;
        }

        private void _auction_broadcastEvent(string message)
        {
            // Set up streams for I/O
            NetworkStream stream = new NetworkStream(_client);
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            // Send message
            writer.WriteLine(message);
        }
        // Method for running client
        public void RunClient()
        {
            bool done = false;
            NetworkStream NS = new NetworkStream(_client);
            StreamReader SR = new StreamReader(NS);
            StreamWriter SW = new StreamWriter(NS);
            SW.AutoFlush = true;

            IPAddress ClientIP = IPAddress.Parse(((IPEndPoint) _client.RemoteEndPoint).Address.ToString());
            Console.WriteLine("A Client connected");
            SW.WriteLine(
                "Welcome!\r\n Write 'exit' to close the connection, and 'bid' plus a natural number to bid on an item.");
            SW.WriteLine("Hello.. This is what is currently going: " + _auction.currentItem.name + ". The lowest price is: " + _auction.currentItem.minPrice + " And this is the current price: " + _auction.currentItem.endPrice);


            while (!done)
            {
                try
                {
                    // Receive "command" from client
                    string[] commands = SR.ReadLine().Split(' ');

                    // Client commands
                    switch (commands[0])
                    {
                        case "exit":
                            // Disconnect from broadcast
                            _auction.broadcastEvent -= _auction_broadcastEvent;
                            SW.WriteLine("Goodbye...");

                            SW.Close();
                            SR.Close();
                            NS.Close();
                            _client.Close();
                            
                            done = true;
                            break;
                        case "bid":
                            // Makes a bid
                            string bidString = _auction.Bid(ClientIP.ToString(), decimal.Parse(commands[1]));

                            SW.WriteLine(bidString);
                            break;
                        default:
                            SW.WriteLine("What the **** are you doing?!");
                            SW.WriteLine("Try again");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception thrown by ClientHandler. {0}", ex);
                    break;
                }
                

            }

        }
    }
}
