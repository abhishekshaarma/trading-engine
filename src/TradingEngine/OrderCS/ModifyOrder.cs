using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
     public class ModifyOrder : IOrderCore
     {
          public ModifyOrder(IOrderCore ordercore, long modifyPrice, uint modifyQuanitity, bool isBuySide)
          {
               Price = modifyPrice;
               Quantity = modifyQuanitity;
               IsBuySide = isBuySide;


               _orderCore = ordercore;
          }
     public long Price { get; private set; }
          public uint Quantity { get; private set; }
          public bool IsBuySide { get; private set; }

          public long OrderId => _orderCore.OrderId;

          public string Username => _orderCore.Username;

          public string SecuirityId => _orderCore.SecuirityId;

          //METHODS
          public CancelOrder ToCancelOrder()
          {
               return new CancelOrder(this);
          }


          //+_+_+_+_+_+//

          private readonly IOrderCore _orderCore;

     }
}
