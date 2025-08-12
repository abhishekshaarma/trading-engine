using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace TradingEngineServer.Orders
{
     public class OrderCore : IOrderCore
     {
          public OrderCore(long orderId, string username, string secuirityId)
          {
               OrderId = orderId;
               Username = username;
               SecuirityId = secuirityId;


          }
          public long OrderId { get; private set; }
          public string Username { get; private set; }
          public string SecuirityId { get; private set; }

     }
}