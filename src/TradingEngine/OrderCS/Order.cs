using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
     public class Order : IOrderCore
     {
          public Order (IOrderCore orderCore, long price, uint initialQuantity, uint currentQuantity, bool isBuySide)
          {
               _orderCore = orderCore;
               Price = price;
               InitialQuantity = initialQuantity;
               CurrentQuantity = currentQuantity;
               IsBuySide = isBuySide;

               // not really necessary because usignned int is always negatice 
               if (initialQuantity < 0)
                    throw new ArgumentOutOfRangeException(nameof(initialQuantity), "Quantity must be greater than zero.");
               if (currentQuantity < 0)
                    throw new ArgumentOutOfRangeException(nameof(currentQuantity), "Quantity must be greater than zero.");

          }

          //Methods
          public void increaseQuantity(uint quantitydelta)
          {
              
               CurrentQuantity += quantitydelta;
          }
          public void decreaseQuantity(uint quantitydelta)
          { 
               if(quantitydelta < CurrentQuantity)
                    throw new InvalidOperationException("You do not have enough quantity");
               CurrentQuantity -= quantitydelta;
          }
          
          
          

          // Fields 
          private readonly IOrderCore _orderCore;

          public long Price { get; private set; }
          public uint InitialQuantity { get; private set; }
          public uint CurrentQuantity { get; private set; }
          public bool IsBuySide { get; private set; }
          public long OrderId => _orderCore.OrderId;
          public string Username => _orderCore.Username;
          public string SecuirityId => _orderCore.SecuirityId;

     }
}
