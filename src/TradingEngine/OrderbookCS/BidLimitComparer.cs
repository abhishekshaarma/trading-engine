using System;
using System.Collections.Generic;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public class BidLimitComparer : IComparer<Limit>
    {
        public static readonly BidLimitComparer Comparer = new BidLimitComparer();
        
        private BidLimitComparer() { }
        
        public int Compare(Limit x, Limit y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            
            return y.Price.CompareTo(x.Price); // Descending order for bids
        }
    }
}
