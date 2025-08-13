using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
     public interface IOrderCore
     {
          public long OrderId { get; }
          public string Username { get; }
          public string SecuirityId { get; }
     }
}
