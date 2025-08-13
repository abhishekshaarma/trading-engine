using System;
using Xunit;
using TradingEngineServer.Orders;
using TradingEngineServer.Instruments;

namespace TradingEngineTests
{
    public class OrderbookEntryTests
    {
        [Fact]
        public void OrderbookEntry_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var order = CreateOrder(1, 100, 10, true, "user1");
            var limit = new Limit(100);

            // Act
            var entry = new OrderbookEntry(order, limit);

            // Assert
            Assert.Equal(order, entry.CurrentOrder);
            Assert.Equal(limit, entry.ParentLimit);
            Assert.Null(entry.Next);
            Assert.Null(entry.Previous);
            Assert.True(entry.CreationTime > DateTime.UtcNow.AddSeconds(-1));
        }

        [Fact]
        public void OrderbookEntry_LinkedList_ConnectsCorrectly()
        {
            // Arrange
            var order1 = CreateOrder(1, 100, 10, true, "user1");
            var order2 = CreateOrder(2, 100, 15, true, "user2");
            var limit = new Limit(100);
            var entry1 = new OrderbookEntry(order1, limit);
            var entry2 = new OrderbookEntry(order2, limit);

            // Act
            entry1.Next = entry2;
            entry2.Previous = entry1;

            // Assert
            Assert.Equal(entry2, entry1.Next);
            Assert.Equal(entry1, entry2.Previous);
            Assert.Null(entry1.Previous);
            Assert.Null(entry2.Next);
        }

        [Fact]
        public void OrderbookEntry_LinkedListChain_WorksCorrectly()
        {
            // Arrange
            var order1 = CreateOrder(1, 100, 10, true, "user1");
            var order2 = CreateOrder(2, 100, 15, true, "user2");
            var order3 = CreateOrder(3, 100, 20, true, "user3");
            var limit = new Limit(100);
            var entry1 = new OrderbookEntry(order1, limit);
            var entry2 = new OrderbookEntry(order2, limit);
            var entry3 = new OrderbookEntry(order3, limit);

            // Act
            entry1.Next = entry2;
            entry2.Previous = entry1;
            entry2.Next = entry3;
            entry3.Previous = entry2;

            // Assert
            Assert.Equal(entry2, entry1.Next);
            Assert.Equal(entry3, entry2.Next);
            Assert.Null(entry3.Next);
            
            Assert.Null(entry1.Previous);
            Assert.Equal(entry1, entry2.Previous);
            Assert.Equal(entry2, entry3.Previous);
        }

        [Fact]
        public void OrderbookEntry_CreationTime_IsSetToUtcNow()
        {
            // Arrange
            var beforeCreation = DateTime.UtcNow;
            var order = CreateOrder(1, 100, 10, true, "user1");
            var limit = new Limit(100);

            // Act
            var entry = new OrderbookEntry(order, limit);
            var afterCreation = DateTime.UtcNow;

            // Assert
            Assert.True(entry.CreationTime >= beforeCreation);
            Assert.True(entry.CreationTime <= afterCreation);
        }

        [Fact]
        public void OrderbookEntry_Properties_AreAccessible()
        {
            // Arrange
            var order = CreateOrder(1, 100, 10, true, "user1");
            var limit = new Limit(100);
            var entry = new OrderbookEntry(order, limit);

            // Act & Assert
            Assert.NotNull(entry.CurrentOrder);
            Assert.NotNull(entry.ParentLimit);
            Assert.Equal(1, entry.CurrentOrder.OrderId);
            Assert.Equal(100, entry.ParentLimit.Price);
        }

        [Fact]
        public void OrderbookEntry_LinkedListModification_UpdatesCorrectly()
        {
            // Arrange
            var order1 = CreateOrder(1, 100, 10, true, "user1");
            var order2 = CreateOrder(2, 100, 15, true, "user2");
            var order3 = CreateOrder(3, 100, 20, true, "user3");
            var limit = new Limit(100);
            var entry1 = new OrderbookEntry(order1, limit);
            var entry2 = new OrderbookEntry(order2, limit);
            var entry3 = new OrderbookEntry(order3, limit);

            // Initial setup
            entry1.Next = entry2;
            entry2.Previous = entry1;
            entry2.Next = entry3;
            entry3.Previous = entry2;

            // Act - Remove entry2 from the middle
            entry1.Next = entry3;
            entry3.Previous = entry1;
            entry2.Next = null;
            entry2.Previous = null;

            // Assert
            Assert.Equal(entry3, entry1.Next);
            Assert.Equal(entry1, entry3.Previous);
            Assert.Null(entry2.Next);
            Assert.Null(entry2.Previous);
        }

        private static Order CreateOrder(long orderId, long price, uint quantity, bool isBuySide, string username)
        {
            var orderCore = new OrderCore(orderId, username, "TEST");
            return new Order(orderCore, price, quantity, quantity, isBuySide);
        }
    }
}
