using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orders
{
     public class Limit
     {
          public Limit(long price)
          {
               Price = price;
          }
          
          public long Price { get; set; }
          public OrderbookEntry Head { get; set; }
          public OrderbookEntry Tail { get; set; }
          public bool isEmpty
          {
               get
               {
                    return Head == null && Tail == null;
               }
          }

           public Side Side
          {
               get
               {
                    if (isEmpty)
                    {
                         return Side.Unknown;
                    }
                    else
                    {
                         return Head.CurrentOrder.IsBuySide ? Side.Bids : Side.Asks;
                    }
               }
          }

          public uint GetLevelOrderCount()
          {
               uint orderCount = 0;
               OrderbookEntry headPointer = Head;
               while (headPointer != null)
               {
                    if (headPointer.CurrentOrder.CurrentQuantity != 0)
                    {
                         orderCount++;
                    }
                    headPointer = headPointer.Next;

               }
               return orderCount; 

          }

          public uint GetLevelTotalQuantity()
          {
               uint totalQuantity = 0;
               OrderbookEntry headPointer = Head;
               while (headPointer != null)
               {
                    totalQuantity += headPointer.CurrentOrder.CurrentQuantity;
                    headPointer = headPointer.Next;
               }
               return totalQuantity;
          }

          public List<TradingEngineServer.Orders.OrderRecord> GetLevelOrderRecord()
          { 
               List<TradingEngineServer.Orders.OrderRecord> records = new List<TradingEngineServer.Orders.OrderRecord>();
               OrderbookEntry headPointer = Head;
               uint theoreticalQueuePosition = 0;
               while (headPointer != null)
               { 
                    var CurrentOrder = headPointer.CurrentOrder;
                    if (CurrentOrder.CurrentQuantity != 0)
                    {
                         records.Add(new TradingEngineServer.Orders.OrderRecord(CurrentOrder.OrderId, CurrentOrder.CurrentQuantity, CurrentOrder.Price, CurrentOrder.IsBuySide, CurrentOrder.Username, int.Parse(CurrentOrder.SecuirityId), theoreticalQueuePosition));
                    }
                    theoreticalQueuePosition++;
                    headPointer = headPointer.Next;
               }

               return records;
          }
     }
}

