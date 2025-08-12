using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
     public sealed class RejectCreator
     {
          public static Reject GenerateOrderCoreRejection(IOrderCore rejectedOrder, RejectionReason rejectionReason)
          {
               return new Reject(rejectedOrder, rejectionReason);
          }
     }
}
