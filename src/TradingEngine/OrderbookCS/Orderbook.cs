using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Instruments;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
     public class Orderbook : IRetrievalOrderbook
     {
          private readonly Secuirity _instrument;
          private readonly Dictionary<long, OrderbookEntry> _orders = new Dictionary<long, OrderbookEntry>();
          //store the ask and the bids
          private readonly SortedList<Limit> _askLimits = new SortedList<Limit>(AskLimitComparer.Comparer);
          
          private readonly SortedList<Limit> _bidLimits = new SortedList<Limit>(BidLimitComparer.Comparer);


          public Orderbook(Secuirity instrument) 
          {
               _instrument = instrument;
          }

          public int Count => _orders.Count;

          public void AddOrder(Order order)
          {
               
          }

          public void ChangeOrder(ModifyOrder order)
          {
               throw new NotImplementedException();
          }

          public bool ContainsOrder(long Order)
          {
               return _orders.ContainsKey(Order);
          }

          public List<OrderbookEntry> GetAskOrders()
          {
               throw new NotImplementedException();
          }

          public List<OrderbookEntry> GetBidOrders()
          {
               throw new NotImplementedException();
          }

          public OrderbookSpread GetSpread()
          {
               throw new NotImplementedException();
          }

          public void RemoveOrder(CancelOrder orderId)
          {
               throw new NotImplementedException();
          }
     }
}
