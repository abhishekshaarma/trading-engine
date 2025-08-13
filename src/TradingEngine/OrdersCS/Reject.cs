using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
     public class Reject : IOrderCore
     {
          public Reject(IOrderCore rejectedOrder, RejectionReason reason) 
          {
               RejectionReason = reason; 

               _orderCore = rejectedOrder;
          }
          private readonly IOrderCore _orderCore;
          public RejectionReason RejectionReason { get; private set; }
          public long OrderId => _orderCore.OrderId;
           
          public string Username => _orderCore.Username;

          public string SecuirityId => _orderCore.SecuirityId;
     }
}
