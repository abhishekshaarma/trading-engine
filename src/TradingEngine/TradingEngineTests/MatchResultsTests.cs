using System.Collections.Generic;
using Xunit;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;

namespace TradingEngineTests
{
    public class MatchResultsTests
    {
        [Fact]
        public void MatchResults_Constructor_InitializesEmptyCollections()
        {
            // Act
            var results = new MatchResults();

            // Assert
            Assert.NotNull(results.FilledOrders);
            Assert.NotNull(results.PartialFills);
            Assert.NotNull(results.RemainingOrders);
            Assert.Empty(results.FilledOrders);
            Assert.Empty(results.PartialFills);
            Assert.Empty(results.RemainingOrders);
            Assert.False(results.HasMatches);
        }

        [Fact]
        public void MatchResults_AddFilledOrder_AddsToCollection()
        {
            // Arrange
            var results = new MatchResults();
            var orderRecord = CreateOrderRecord(1, 10, 100, true, "user1", 0);

            // Act
            results.AddFilledOrder(orderRecord);

            // Assert
            Assert.Single(results.FilledOrders);
            Assert.Equal(orderRecord, results.FilledOrders[0]);
            Assert.True(results.HasMatches);
        }

        [Fact]
        public void MatchResults_AddPartialFill_AddsToCollection()
        {
            // Arrange
            var results = new MatchResults();
            var orderRecord = CreateOrderRecord(1, 5, 100, true, "user1", 0);

            // Act
            results.AddPartialFill(orderRecord);

            // Assert
            Assert.Single(results.PartialFills);
            Assert.Equal(orderRecord, results.PartialFills[0]);
            Assert.True(results.HasMatches);
        }

        [Fact]
        public void MatchResults_AddRemainingOrder_AddsToCollection()
        {
            // Arrange
            var results = new MatchResults();
            var orderRecord = CreateOrderRecord(1, 10, 100, true, "user1", 0);

            // Act
            results.AddRemainingOrder(orderRecord);

            // Assert
            Assert.Single(results.RemainingOrders);
            Assert.Equal(orderRecord, results.RemainingOrders[0]);
            Assert.False(results.HasMatches); // Remaining orders don't count as matches
        }

        [Fact]
        public void MatchResults_MultipleOrders_StoredCorrectly()
        {
            // Arrange
            var results = new MatchResults();
            var filledOrder1 = CreateOrderRecord(1, 10, 100, true, "user1", 0);
            var filledOrder2 = CreateOrderRecord(2, 10, 95, false, "user2", 0);
            var partialFill = CreateOrderRecord(3, 5, 100, true, "user3", 0);
            var remainingOrder = CreateOrderRecord(4, 15, 100, true, "user4", 0);

            // Act
            results.AddFilledOrder(filledOrder1);
            results.AddFilledOrder(filledOrder2);
            results.AddPartialFill(partialFill);
            results.AddRemainingOrder(remainingOrder);

            // Assert
            Assert.Equal(2, results.FilledOrders.Count);
            Assert.Equal(1, results.PartialFills.Count);
            Assert.Equal(1, results.RemainingOrders.Count);
            Assert.True(results.HasMatches);
        }

        [Fact]
        public void MatchResults_HasMatches_TrueWhenFilledOrdersExist()
        {
            // Arrange
            var results = new MatchResults();
            var orderRecord = CreateOrderRecord(1, 10, 100, true, "user1", 0);

            // Act
            results.AddFilledOrder(orderRecord);

            // Assert
            Assert.True(results.HasMatches);
        }

        [Fact]
        public void MatchResults_HasMatches_TrueWhenPartialFillsExist()
        {
            // Arrange
            var results = new MatchResults();
            var orderRecord = CreateOrderRecord(1, 5, 100, true, "user1", 0);

            // Act
            results.AddPartialFill(orderRecord);

            // Assert
            Assert.True(results.HasMatches);
        }

        [Fact]
        public void MatchResults_HasMatches_FalseWhenOnlyRemainingOrders()
        {
            // Arrange
            var results = new MatchResults();
            var orderRecord = CreateOrderRecord(1, 10, 100, true, "user1", 0);

            // Act
            results.AddRemainingOrder(orderRecord);

            // Assert
            Assert.False(results.HasMatches);
        }

        [Fact]
        public void MatchResults_Collections_AreMutable()
        {
            // Arrange
            var results = new MatchResults();
            var orderRecord = CreateOrderRecord(1, 10, 100, true, "user1", 0);

            // Act
            results.FilledOrders.Add(orderRecord);
            results.PartialFills.Add(orderRecord);
            results.RemainingOrders.Add(orderRecord);

            // Assert
            Assert.Single(results.FilledOrders);
            Assert.Single(results.PartialFills);
            Assert.Single(results.RemainingOrders);
        }

        [Fact]
        public void MatchResults_OrderRecord_PropertiesAreCorrect()
        {
            // Arrange
            var results = new MatchResults();
            var orderRecord = CreateOrderRecord(1, 10, 100, true, "user1", 5);

            // Act
            results.AddFilledOrder(orderRecord);

            // Assert
            var storedRecord = results.FilledOrders[0];
            Assert.Equal(1, storedRecord.OrderId);
            Assert.Equal(10u, storedRecord.Quantity);
            Assert.Equal(100, storedRecord.Price);
            Assert.True(storedRecord.IsBuySide);
                         Assert.Equal("user1", storedRecord.username);
             Assert.Equal(5u, storedRecord.TheoriticalQueuePosition);
        }

        private static OrderRecord CreateOrderRecord(long orderId, uint quantity, long price, bool isBuySide, string username, int queuePosition)
        {
                         return new TradingEngineServer.Orders.OrderRecord(orderId, quantity, price, isBuySide, username, 1, (uint)queuePosition);
        }
    }
}
