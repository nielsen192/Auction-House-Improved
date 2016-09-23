using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionClient
{
    class ClientLogic
    {
        
        //static IPAddress serverAddress = IPAddress.Parse("10.140.79.102");
        
        
        private NetworkStream clientStream;
        
        public void Run()
        {
            // Connecting to server
            Console.WriteLine("Hello, please enter the server ip: ");
            try
            {
                string ipAddress = Console.ReadLine();
                IPAddress serverAddress = IPAddress.Parse(ipAddress);
                int serverPort = 8000;
                TcpClient tcpClient = new TcpClient(serverAddress.ToString(), serverPort);
                clientStream = tcpClient.GetStream();

                Thread readThread = new Thread(Read);
                Thread writeThread = new Thread(WriteBid);

                // Running threads for read and write methods
                readThread.Start();
                writeThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Please make sure that you have entered the correct ip address and that it is in the correct format.");
            }
            

            

            
        }

        public void Read()
        {
            // Receiving data setup
            StreamReader reader = new StreamReader(clientStream);
            while (true)
            {
                
                string data = reader.ReadLine();
                Console.WriteLine(data);

            }
        }

        public void WriteBid()
        {
            // Writing data setup
            StreamWriter writer = new StreamWriter(clientStream);
            while (true)
            {
                
                writer.WriteLine(Console.ReadLine());
                writer.Flush();

            }
        }
    }
}
