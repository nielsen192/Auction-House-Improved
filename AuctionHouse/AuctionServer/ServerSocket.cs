using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace AuctionServer
{
	class ServerSocket
	{
		public static Auction _auction = new Auction();

        // Showing the network cards and network details in the servers console
		private static void ShowServerNetworkConfig(){  
		Console.ForegroundColor = ConsoleColor.Yellow;
		NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
		foreach(NetworkInterface adapter in adapters){
			Console.WriteLine(adapter.Description);
			Console.WriteLine("\tAdapter Name: " + adapter.Name);
			Console.WriteLine("\tMAC Address: " + adapter.GetPhysicalAddress());
			IPInterfaceProperties ip_properties = adapter.GetIPProperties();
			UnicastIPAddressInformationCollection addresses = ip_properties.UnicastAddresses;
			foreach(UnicastIPAddressInformation address in addresses){
				Console.WriteLine("\tIP Address: " + address.Address);
			}
		}
		Console.ForegroundColor = ConsoleColor.White;
	}
		public ServerSocket()
		{
			
			ShowServerNetworkConfig();
            // Starting the servers listener and auction threads
			TcpListener Server = new TcpListener(IPAddress.Any, 8000);
			Server.Start();

			Thread auctionThread = new Thread(_auction.RunAuction);
			auctionThread.Start();

			while (true)
			{
				Socket ClientSocket = Server.AcceptSocket();
				ClientHandler CH = new ClientHandler(ClientSocket, _auction);
				Thread Tr = new Thread(CH.RunClient);
				Tr.Start();
			}
		}
	}
}
