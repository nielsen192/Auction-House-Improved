using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer
{
    class Item
    {
        public string name { get; set; }
        public decimal minPrice { get; set; }
        public string winner { get; set; }
        public decimal endPrice { get; set; }

    }
}
