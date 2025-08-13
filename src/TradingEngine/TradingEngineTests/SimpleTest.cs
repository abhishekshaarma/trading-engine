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
            Assert.NotNull(orderbook);
            Assert.Equal(0, orderbook.Count);
        }
        
        [Fact]
        public void BasicLimitTest()
        {
            // Arrange
            var limit = new Limit(100);
            
            // Act & Assert
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
            Assert.NotNull(entry);
            Assert.Equal(order, entry.CurrentOrder);
            Assert.Equal(limit, entry.ParentLimit);
        }
    }
}
