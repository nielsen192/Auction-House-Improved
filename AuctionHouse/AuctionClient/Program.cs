using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AuctionClient
{
    class Program
    {

        static void Main(string[] args)
        {
            ClientLogic clientLogic = new ClientLogic();
            clientLogic.Run();

        }

        
    }
}
