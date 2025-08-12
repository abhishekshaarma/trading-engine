using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
     internal class CancelOrder :IOrderCore
     {
          public CancelOrder(IOrderCore orderCore)
          {
               _orderCore = orderCore;    
          }
          private readonly IOrderCore _orderCore;

          public long OrderId => _orderCore.OrderId;

          public string Username => _orderCore.Username;

          public string SecuirityId => _orderCore.SecuirityId;
     }
     }
