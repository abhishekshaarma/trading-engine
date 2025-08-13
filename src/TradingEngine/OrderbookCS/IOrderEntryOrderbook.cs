using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
     public interface IOrderEntryOrderbook :IReadOnlyOrderbook
     {
          void AddOrder(Order order);
          void ChangeOrder(ModifyOrder order);
          void RemoveOrder(CancelOrder orderId);
     }
}
