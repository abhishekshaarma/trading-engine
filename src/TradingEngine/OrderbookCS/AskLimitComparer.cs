using System;
using System.Collections.Generic;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public class AskLimitComparer : IComparer<Limit>
    {
        public static readonly AskLimitComparer Comparer = new AskLimitComparer();
        
        private AskLimitComparer() { }
        
        public int Compare(Limit x, Limit y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            
            return x.Price.CompareTo(y.Price);
        }
    }
}
