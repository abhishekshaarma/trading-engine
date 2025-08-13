using System;
using System.Collections.Generic;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public class MatchResults
    {
        public List<OrderRecord> FilledOrders { get; set; } = new List<OrderRecord>();
        public List<OrderRecord> PartialFills { get; set; } = new List<OrderRecord>();
        public List<OrderRecord> RemainingOrders { get; set; } = new List<OrderRecord>();
        
        public bool HasMatches => FilledOrders.Count > 0 || PartialFills.Count > 0;
        
        public void AddFilledOrder(OrderRecord order)
        {
            FilledOrders.Add(order);
        }
        
        public void AddPartialFill(OrderRecord order)
        {
            PartialFills.Add(order);
        }
        
        public void AddRemainingOrder(OrderRecord order)
        {
            RemainingOrders.Add(order);
        }
    }
}
