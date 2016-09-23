using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionServer
{
    internal class Auction
    {
        public delegate void broadcastDelegate(string message);
        public event broadcastDelegate broadcastEvent;

        private List<Item> itemList = new List<Item>
        {
            new Item() {name = "Swedish Penis Pump", minPrice = 69.69m, endPrice = 69.69m, winner = "No one"},
            new Item() { name = "Burned Down PC", minPrice = 5608.67m, endPrice = 5608.67m, winner = "No one"},
            new Item() { name = "My Tangy Socks", minPrice = 2000000.00m, endPrice = 2000000.00m, winner = "No one"},
            new Item() { name = "A 'fun' day with the princess of Sweden", minPrice = 2.09m, endPrice = 2.09m, winner = "No one"}
        };

        private bool auctionRunning;
        private readonly object gavelLock = new object();
        private readonly object itemLock = new object();
        public Item currentItem;
        private int gavel;

        public void RunAuction()
        {
            // Same actions for all items
            foreach (Item item in itemList)
            {
                
                currentItem = item;
                auctionRunning = true;
                ResetGavel();
                // Sending broadcast for starting auction
                broadcastEvent?.Invoke("Starting auction for: " + currentItem.name + " Starting price: " +
                currentItem.minPrice);

                while (auctionRunning)
                {
                    
                    

                    // Locks gavel resource
                    lock (gavelLock)
                    {
                        Thread.Sleep(1000);
                        gavel--;

                        if (gavel == 10)
                        {
                            if (broadcastEvent != null)
                            {
                                broadcastEvent("Gavel: Going once!");
                                Console.WriteLine("Gavel: Going once!");
                            }
                        }
                        else if (gavel == 5)
                        {
                            if (broadcastEvent != null)
                            {
                                broadcastEvent("Gavel: Going twice!");
                                Console.WriteLine("Gavel: Going twice!");
                            }
                        }
                        else if (gavel == 2)
                        {
                            if (broadcastEvent != null)
                            {
                                broadcastEvent("Gavel: Sold!");
                                Console.WriteLine("Gavel: Sold!");
                                broadcastEvent(currentItem.winner);
                                auctionRunning = false;
                                
                            }
                        }
                    }
                }
                broadcastEvent?.Invoke(currentItem.winner + " won\r\n" + currentItem.name + " : " + currentItem.endPrice);
                // Broadcast the end of the auction
                broadcastEvent?.Invoke("The auction is over - Thank you for participating");
            }
        }

        private void ResetGavel()
        {
            lock (gavelLock)
            {
                gavel = 20;
            }
        }

        public string Bid(string id, decimal amount)
        {
            lock (itemLock)
            {
                // Check if bid is higher than current
                if (currentItem.endPrice <= amount)
                {
                    
                    broadcastEvent(id + " bids " + amount);
                    // Setting the new info
                    currentItem.endPrice = amount;
                    currentItem.winner = id;
                    // Reset gavel
                    ResetGavel();
                }
                else
                {
                    return "Too low";
                }
            }
            return "You bid : " + amount;
        }
    }
}
