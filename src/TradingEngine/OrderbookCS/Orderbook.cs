using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using TradingEngineServer.Instruments;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
     public class Orderbook : IRetrievalOrderbook
     {
          private readonly Secuirity _instrument;
          private readonly Dictionary<long, OrderbookEntry> _orders = new Dictionary<long, OrderbookEntry>();
          //store the ask and the bids
          private readonly SortedSet<Limit> _askLimits = new SortedSet<Limit>(AskLimitComparer.Comparer);

          private readonly SortedSet<Limit> _bidLimits = new SortedSet<Limit>(BidLimitComparer.Comparer);


          public Orderbook(Secuirity instrument)
          {
               _instrument = instrument;
          }

          public int Count => _orders.Count;

          public void AddOrder(Order order)
          {
               var baseLimit = new Limit(order.Price);
               AddOrder(order, baseLimit, order.IsBuySide ? _bidLimits : _askLimits, _orders);
          }
          private static void AddOrder(Order order, Limit baseLimit, SortedSet<Limit> limits, Dictionary<long, OrderbookEntry> orders)
          {
               if (limits.TryGetValue(baseLimit, out Limit limit))
               {
                    OrderbookEntry orderbookEntry = new OrderbookEntry(order, baseLimit);
                    if (limit.Head == null)
                    {
                         limit.Head = orderbookEntry;
                         limit.Tail = orderbookEntry;
                    }
                    else
                    {
                         OrderbookEntry tailPointer = limit.Tail;
                         tailPointer.Next = orderbookEntry;
                         orderbookEntry.Previous = tailPointer;

                         limit.Tail = orderbookEntry;
                    }
                    orders.Add(order.OrderId, orderbookEntry);

               }
               else
               {
                    limits.Add(baseLimit);
                    OrderbookEntry orderbookEntry = new OrderbookEntry(order, baseLimit);
                    baseLimit.Head = orderbookEntry;
                    baseLimit.Tail = orderbookEntry;
                    orders.Add(order.OrderId, orderbookEntry);
               }
          }

          public void ChangeOrder(ModifyOrder modifyOrder)
          {
               if (_orders.TryGetValue(modifyOrder.OrderId, out OrderbookEntry obe))
               {
                    RemoveOrder(modifyOrder.ToCancelOrder());
                    AddOrder(modifyOrder.ToNewOrder(), obe.ParentLimit, modifyOrder.IsBuySide ? _bidLimits : _askLimits, _orders);

               }
          }

          public bool ContainsOrder(long Order)
          {
               return _orders.ContainsKey(Order);
          }

          public List<OrderbookEntry> GetAskOrders()
          {
               List<OrderbookEntry> orderbookEntries = new List<OrderbookEntry>();

               foreach(var limit in _askLimits)
               {
                    if (limit.isEmpty)
                         continue;
                    else
                    {
                         OrderbookEntry headPointer = limit.Head;
                         while (headPointer != null)
                         {
                              orderbookEntries.Add(headPointer);
                              headPointer = headPointer.Next;
                         }
                    }
               }
               return orderbookEntries;
          }

          public List<OrderbookEntry> GetBidOrders()
          {
               List<OrderbookEntry> orderbookEntries = new List<OrderbookEntry>();
               foreach (var limit in _bidLimits)
               {
                    if (limit.isEmpty)
                         continue;
                    else
                    {
                         OrderbookEntry headPointer = limit.Head;
                         while (headPointer != null)
                         {
                              orderbookEntries.Add(headPointer);
                              headPointer = headPointer.Next;
                         }
                    }
               }
               return orderbookEntries;
          }

          public OrderbookSpread GetSpread()
          {
               long? bestAsk = null, bestBid = null;
               if (_askLimits.Any() && _askLimits.Min.isEmpty)
                    bestAsk = _askLimits.Min.Price;
               if (_bidLimits.Any() && _bidLimits.Max.isEmpty)
                    bestBid = _bidLimits.Max.Price;
               return new OrderbookSpread(bestBid, bestAsk);
          }

          public void RemoveOrder(CancelOrder cancelOrder)
          {
               if (_orders.TryGetValue(cancelOrder.OrderId, out var obe))
               {
                    RemoveOrder(cancelOrder.OrderId, obe, _orders);
               }
          }
          private static void RemoveOrder(long orderId, OrderbookEntry obe, Dictionary<long, OrderbookEntry> internalBook)
          {
               {
                    // dealing with the orderbook entry inside the linked list 
                    if (obe.Previous != null && obe.Next != null)
                    {
                         obe.Next.Previous = obe.Previous;
                         obe.Previous.Next = obe.Next;
                    }
                    else if (obe.Previous != null)
                    {
                         obe.Previous.Next = null;
                    }
                    else if (obe.Next != null)
                    { 
                         obe.Next.Previous = null;
                    }
               }
               // now deal with the limit level 
               if (obe.ParentLimit.Head == obe && obe.ParentLimit.Tail == obe)
               { 
                    obe.ParentLimit.Head = null;
                    obe.ParentLimit.Tail = null;  
               }
               else if (obe.ParentLimit.Head == obe)
               {
                    obe.ParentLimit.Head = obe.Next;
                    //obe.Next.Previous = null;
               }
               else if (obe.ParentLimit.Tail == obe)
               {
                    obe.ParentLimit.Tail = obe.Previous;
                    //obe.Previous.Next = null;
               }
               internalBook.Remove(orderId);
          }
     }
}
