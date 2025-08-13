using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
     public class Limit
     {
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

          public List<OrderRecord> GetLevelOrderRecord()
          { 
               List<OrderRecord> records = new List<OrderRecord>();
               OrderbookEntry headPointer = Head;
               uint theoreticalQueuePosition = 0;
               while (headPointer != null)
               { 
                    var CurrentOrder = headPointer.CurrentOrder;
                    if (CurrentOrder.CurrentQuantity != 0)
                    {
                         records.Add(new OrderRecord(CurrentOrder.OrderId, CurrentOrder.CurrentQuantity, CurrentOrder.Price, CurrentOrder.IsBuySide, CurrentOrder.Username, int.Parse(CurrentOrder.SecuirityId), theoreticalQueuePosition));
                    }
                    theoreticalQueuePosition++;
                    headPointer = headPointer.Next;
               }

               return records;
          }
     }
}

