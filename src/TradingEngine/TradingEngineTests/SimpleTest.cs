using System;
using Xunit;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;
using TradingEngineServer.Instruments;

namespace TradingEngineTests
{
    public class SimpleTest
    {
        [Fact]
        public void BasicOrderbookTest()
        {
            // Arrange
            var instrument = new Secuirity("TEST", "Test Instrument", 100, 2);
            var orderbook = new Orderbook(instrument);
            
            // Act & Assert
            Console.WriteLine($"[BasicOrderbookTest] Instrument={instrument.Symbol}, OrderbookCount={orderbook.Count}");
            Assert.NotNull(orderbook);
            Assert.Equal(0, orderbook.Count);
        }
        
        [Fact]
        public void BasicLimitTest()
        {
            // Arrange
            var limit = new Limit(100);
            
            // Act & Assert
            Console.WriteLine($"[BasicLimitTest] Price={limit.Price}, isEmpty={limit.isEmpty}");
            Assert.NotNull(limit);
            Assert.Equal(100, limit.Price);
            Assert.True(limit.isEmpty);
        }
        
        [Fact]
        public void BasicOrderbookEntryTest()
        {
            // Arrange
            var orderCore = new OrderCore(1, "user1", "TEST");
            var order = new Order(orderCore, 100, 10, 10, true);
            var limit = new Limit(100);
            var entry = new OrderbookEntry(order, limit);
            
            // Act & Assert
            Console.WriteLine($"[BasicOrderbookEntryTest] OrderId={orderCore.OrderId}, Price={order.Price}, Qty={order.CurrentQuantity}");
            Assert.NotNull(entry);
            Assert.Equal(order, entry.CurrentOrder);
            Assert.Equal(limit, entry.ParentLimit);
        }
    }
}
