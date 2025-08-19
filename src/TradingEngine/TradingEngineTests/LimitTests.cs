using System;
using System.Linq;
using Xunit;
using TradingEngineServer.Orders;
using TradingEngineServer.Instruments;

namespace TradingEngineTests
{
    public class LimitTests
    {
        [Fact]
        public void Limit_EmptyLimit_IsEmptyTrue()
        {
            // Arrange
            var limit = new Limit(100);

            // Assert
            Console.WriteLine($"[Limit_EmptyLimit_IsEmptyTrue] Price={limit.Price}, isEmpty={limit.isEmpty}, Head={(limit.Head == null ? "null" : "not-null")}, Tail={(limit.Tail == null ? "null" : "not-null")} ");
            Assert.True(limit.isEmpty);
            Assert.Null(limit.Head);
            Assert.Null(limit.Tail);
            Assert.Equal(100, limit.Price);
        }

        [Fact]
        public void Limit_WithSingleOrder_IsEmptyFalse()
        {
            // Arrange
            var limit = new Limit(100);
            var order = CreateOrder(1, 100, 10, true, "user1");
            var entry = new OrderbookEntry(order, limit);

            // Act
            limit.Head = entry;
            limit.Tail = entry;

            // Assert
            Console.WriteLine($"[Limit_WithSingleOrder_IsEmptyFalse] Side={limit.Side}, OrderCount={limit.GetLevelOrderCount()}, TotalQty={limit.GetLevelTotalQuantity()} ");
            Assert.False(limit.isEmpty);
            Assert.Equal(Side.Bids, limit.Side);
            Assert.Equal(1, (double)limit.GetLevelOrderCount());
            Assert.Equal(10u, limit.GetLevelTotalQuantity());
        }

        [Fact]
        public void Limit_WithMultipleOrders_CountsCorrectly()
        {
            // Arrange
            var limit = new Limit(100);
            var order1 = CreateOrder(1, 100, 10, true, "user1");
            var order2 = CreateOrder(2, 100, 15, true, "user2");
            var entry1 = new OrderbookEntry(order1, limit);
            var entry2 = new OrderbookEntry(order2, limit);

            // Act
            limit.Head = entry1;
            limit.Tail = entry2;
            entry1.Next = entry2;
            entry2.Previous = entry1;

            // Assert
            Console.WriteLine($"[Limit_WithMultipleOrders_CountsCorrectly] Side={limit.Side}, OrderCount={limit.GetLevelOrderCount()}, TotalQty={limit.GetLevelTotalQuantity()} ");
            Assert.False(limit.isEmpty);
            Assert.Equal(2, (double)limit.GetLevelOrderCount());
            Assert.Equal(25u, limit.GetLevelTotalQuantity());
            Assert.Equal(Side.Bids, limit.Side);
        }

        [Fact]
        public void Limit_GetLevelOrderRecord_ReturnsCorrectRecords()
        {
            // Arrange
            var limit = new Limit(100);
            var order1 = CreateOrder(1, 100, 10, true, "user1");
            var order2 = CreateOrder(2, 100, 15, true, "user2");
            var entry1 = new OrderbookEntry(order1, limit);
            var entry2 = new OrderbookEntry(order2, limit);

            limit.Head = entry1;
            limit.Tail = entry2;
            entry1.Next = entry2;
            entry2.Previous = entry1;

            // Act
            var records = limit.GetLevelOrderRecord();

            // Assert
            Console.WriteLine($"[Limit_GetLevelOrderRecord_ReturnsCorrectRecords] Count={records.Count}, Records=[{string.Join(", ", records.Select(r => $"(Id={r.OrderId},Qty={r.Quantity},Pos={r.TheoriticalQueuePosition})"))}] ");
            Assert.Equal(2, records.Count);
            Assert.Equal(1, records[0].OrderId);
            Assert.Equal(10u, records[0].Quantity);
            Assert.Equal(0u, records[0].TheoriticalQueuePosition);
            Assert.Equal(2, records[1].OrderId);
            Assert.Equal(15u, records[1].Quantity);
            Assert.Equal(1u, records[1].TheoriticalQueuePosition);
        }

        [Fact]
        public void Limit_WithZeroQuantityOrder_ExcludedFromCounts()
        {
            // Arrange
            var limit = new Limit(100);
            var order1 = CreateOrder(1, 100, 10, true, "user1");
            var order2 = CreateOrder(2, 100, 0, true, "user2"); // Zero quantity
            var entry1 = new OrderbookEntry(order1, limit);
            var entry2 = new OrderbookEntry(order2, limit);

            limit.Head = entry1;
            limit.Tail = entry2;
            entry1.Next = entry2;
            entry2.Previous = entry1;

            // Act
            var orderCount = limit.GetLevelOrderCount();
            var totalQuantity = limit.GetLevelTotalQuantity();
            var records = limit.GetLevelOrderRecord();

            // Assert
            Console.WriteLine($"[Limit_WithZeroQuantityOrder_ExcludedFromCounts] OrderCount={orderCount}, TotalQty={totalQuantity}, RecordsCount={records.Count} ");
            Assert.Equal(1, (double)orderCount); // Only non-zero quantity orders
            Assert.Equal(10u, totalQuantity);
               Assert.Equal(1, records.Count);
            Assert.Equal(1, records[0].OrderId);
        }

        [Fact]
        public void Limit_AskSide_ReturnsCorrectSide()
        {
            // Arrange
            var limit = new Limit(100);
            var order = CreateOrder(1, 100, 10, false, "user1"); // Ask order
            var entry = new OrderbookEntry(order, limit);

            // Act
            limit.Head = entry;
            limit.Tail = entry;

            // Assert
            Console.WriteLine($"[Limit_AskSide_ReturnsCorrectSide] Side={limit.Side} ");
            Assert.Equal(Side.Asks, limit.Side);
        }

        [Fact]
        public void Limit_UnknownSide_WhenEmpty()
        {
            // Arrange
            var limit = new Limit(100);

            // Assert
            Console.WriteLine($"[Limit_UnknownSide_WhenEmpty] Side={limit.Side} ");
            Assert.Equal(Side.Unknown, limit.Side);
        }

        [Fact]
        public void Limit_PriceProperty_ReturnsCorrectValue()
        {
            // Arrange
            var limit = new Limit(150);

            // Assert
            Console.WriteLine($"[Limit_PriceProperty_ReturnsCorrectValue] Price={limit.Price} ");
            Assert.Equal(150, limit.Price);
        }

        private static Order CreateOrder(long orderId, long price, uint quantity, bool isBuySide, string username)
        {
            var orderCore = new OrderCore(orderId, username, "TEST");
            return new Order(orderCore, price, quantity, quantity, isBuySide);
        }
    }
}
