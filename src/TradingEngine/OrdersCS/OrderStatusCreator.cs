using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
     internal class OrderStatusCreator
     {
          public static CancelOrderStatus GenerateCancelOrderStatus(CancelOrder cancelOrder)
          {
               return new CancelOrderStatus();
          }
          public static NewOrderStatus GenerateNewOrderStatus(Order order)
          {
               return new NewOrderStatus();
          }
          public static  ModifyOrderStatus GenerateModifyOrderStatus (ModifyOrderStatus modifyOrder) 
          {
               return new ModifyOrderStatus();
          }


     }
}
