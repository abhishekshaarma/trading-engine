using System;
using System.Linq;
using Xunit;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;
using TradingEngineServer.Instruments;

namespace TradingEngineTests
{
    public class OrderbookMatchingTests
    {
        private readonly Secuirity _testInstrument;
        private readonly Orderbook _orderbook;

        public OrderbookMatchingTests()
        {
            _testInstrument = new Secuirity("TEST", "Test Instrument", 100, 2);
            _orderbook = new Orderbook(_testInstrument);
        }

        [Fact]
        public void Match_NoOrders_ReturnsEmptyResults()
        {
            // Act
            var results = _orderbook.Match();

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results.FilledOrders);
            Assert.Empty(results.PartialFills);
            Assert.Empty(results.RemainingOrders);
            Assert.False(results.HasMatches);
        }

        [Fact]
        public void Match_SingleBidOrder_NoMatching()
        {
            // Arrange
            var bidOrder = CreateOrder(1, 100, 10, true, "user1");

            // Act
            _orderbook.AddOrder(bidOrder);
            var results = _orderbook.Match();

            // Assert
            Assert.False(results.HasMatches);
            Assert.Equal(1, _orderbook.Count);
            Assert.Equal(1, _orderbook.GetBidOrders().Count);
            Assert.Empty(_orderbook.GetAskOrders());
        }

        [Fact]
        public void Match_SingleAskOrder_NoMatching()
        {
            // Arrange
            var askOrder = CreateOrder(1, 100, 10, false, "user1");

            // Act
            _orderbook.AddOrder(askOrder);
            var results = _orderbook.Match();

            // Assert
            Assert.False(results.HasMatches);
            Assert.Equal(1, _orderbook.Count);
            Assert.Equal(1, _orderbook.GetAskOrders().Count);
            Assert.Empty(_orderbook.GetBidOrders());
        }

        [Fact]
        public void Match_CrossingOrders_MatchesCorrectly()
        {
            // Arrange
            var bidOrder = CreateOrder(1, 100, 10, true, "user1");
            var askOrder = CreateOrder(2, 95, 10, false, "user2");

            // Act
            _orderbook.AddOrder(bidOrder);
            _orderbook.AddOrder(askOrder);
            var results = _orderbook.Match();

            // Assert
            Assert.True(results.HasMatches);
            Assert.Equal(2, results.FilledOrders.Count);
            Assert.Equal(0, _orderbook.Count); // Both orders should be fully filled
            Assert.Empty(_orderbook.GetBidOrders());
            Assert.Empty(_orderbook.GetAskOrders());
        }

        [Fact]
        public void Match_PriceTimePriority_Respected()
        {
            // Arrange
            var bidOrder1 = CreateOrder(1, 100, 5, true, "user1");
            var bidOrder2 = CreateOrder(2, 100, 5, true, "user2");
            var askOrder = CreateOrder(3, 95, 10, false, "user3");

            // Act
            _orderbook.AddOrder(bidOrder1);
            _orderbook.AddOrder(bidOrder2);
            _orderbook.AddOrder(askOrder);
            var results = _orderbook.Match();

            // Assert
            Assert.True(results.HasMatches);
            Assert.Equal(2, results.FilledOrders.Count);
            
            // First bid should be fully filled, second should remain
            Assert.Equal(1, _orderbook.Count);
            Assert.Equal(1, _orderbook.GetBidOrders().Count);
            Assert.Empty(_orderbook.GetAskOrders());
            
            var remainingBid = _orderbook.GetBidOrders().First();
            Assert.Equal(2, remainingBid.CurrentOrder.OrderId);
            Assert.Equal(5, (double)remainingBid.CurrentOrder.CurrentQuantity);
        }

        [Fact]
        public void Match_PartialFill_HandledCorrectly()
        {
            // Arrange
            var bidOrder = CreateOrder(1, 100, 15, true, "user1");
            var askOrder = CreateOrder(2, 95, 10, false, "user2");

            // Act
            _orderbook.AddOrder(bidOrder);
            _orderbook.AddOrder(askOrder);
            var results = _orderbook.Match();

            // Assert
            Assert.True(results.HasMatches);
            Assert.Equal(2, results.FilledOrders.Count);
            Assert.Equal(1, _orderbook.Count); // Bid order should remain with partial quantity
            
            var remainingBid = _orderbook.GetBidOrders().First();
            Assert.Equal(1, remainingBid.CurrentOrder.OrderId);
            Assert.Equal(5, (double)remainingBid.CurrentOrder.CurrentQuantity); // 15 - 10 = 5
        }

        [Fact]
        public void Match_MultiplePriceLevels_MatchesCorrectly()
        {
            // Arrange
            var bidOrder1 = CreateOrder(1, 100, 10, true, "user1");
            var bidOrder2 = CreateOrder(2, 99, 10, true, "user2");
            var askOrder1 = CreateOrder(3, 98, 5, false, "user3");
            var askOrder2 = CreateOrder(4, 97, 5, false, "user4");

            // Act
            _orderbook.AddOrder(bidOrder1);
            _orderbook.AddOrder(bidOrder2);
            _orderbook.AddOrder(askOrder1);
            _orderbook.AddOrder(askOrder2);
            var results = _orderbook.Match();

            // Assert
            Assert.True(results.HasMatches);
            Assert.Equal(4, results.FilledOrders.Count);
            Assert.Equal(0, _orderbook.Count); // All orders should be fully filled
        }

        [Fact]
        public void Match_NonCrossingOrders_NoMatching()
        {
            // Arrange
            var bidOrder = CreateOrder(1, 95, 10, true, "user1");
            var askOrder = CreateOrder(2, 100, 10, false, "user2");

            // Act
            _orderbook.AddOrder(bidOrder);
            _orderbook.AddOrder(askOrder);
            var results = _orderbook.Match();

            // Assert
            Assert.False(results.HasMatches);
            Assert.Equal(2, _orderbook.Count);
            Assert.Equal(1, _orderbook.GetBidOrders().Count);
            Assert.Equal(1, _orderbook.GetAskOrders().Count);
        }

        [Fact]
        public void Match_EmptyOrderbook_ReturnsEmptyResults()
        {
            // Act
            var results = _orderbook.Match();

            // Assert
            Assert.NotNull(results);
            Assert.False(results.HasMatches);
            Assert.Empty(results.FilledOrders);
        }

        [Fact]
        public void Match_OrderModification_UpdatesCorrectly()
        {
            // Arrange
            var originalOrder = CreateOrder(1, 100, 10, true, "user1");
            _orderbook.AddOrder(originalOrder);
            
            var orderCore = new OrderCore(1, "user1", "TEST");
            var modifyOrder = new ModifyOrder(orderCore, 95, 15, true);

            // Act
            _orderbook.ChangeOrder(modifyOrder);
            var results = _orderbook.Match();

            // Assert
            Assert.Equal(1, _orderbook.Count);
            var updatedOrder = _orderbook.GetBidOrders().First();
            Assert.Equal(95, updatedOrder.CurrentOrder.Price);
            Assert.Equal(15, (double)updatedOrder.CurrentOrder.CurrentQuantity);
        }

        [Fact]
        public void Match_OrderCancellation_RemovesCorrectly()
        {
            // Arrange
            var order = CreateOrder(1, 100, 10, true, "user1");
            _orderbook.AddOrder(order);
            
            var orderCore = new OrderCore(1, "user1", "TEST");
            var cancelOrder = new CancelOrder(orderCore);

            // Act
            _orderbook.RemoveOrder(cancelOrder);
            var results = _orderbook.Match();

            // Assert
            Assert.Equal(0, _orderbook.Count);
            Assert.Empty(_orderbook.GetBidOrders());
            Assert.Empty(_orderbook.GetAskOrders());
        }

        [Fact]
        public void Match_SpreadCalculation_Correct()
        {
            // Arrange
            var bidOrder = CreateOrder(1, 95, 10, true, "user1");
            var askOrder = CreateOrder(2, 100, 10, false, "user2");

            // Act
            _orderbook.AddOrder(bidOrder);
            _orderbook.AddOrder(askOrder);
            var spread = _orderbook.GetSpread();

            // Assert
            Assert.Equal(95, spread.Bid);
            Assert.Equal(100, spread.Ask);
        }

        private static Order CreateOrder(long orderId, long price, uint quantity, bool isBuySide, string username)
        {
            var orderCore = new OrderCore(orderId, username, "TEST");
            return new Order(orderCore, price, quantity, quantity, isBuySide);
        }
    }
}
