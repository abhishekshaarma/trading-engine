using Xunit;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;

namespace TradingEngineTests
{
    public class ComparerTests
    {
        [Fact]
        public void AskLimitComparer_SortsAscending()
        {
            // Arrange
            var comparer = AskLimitComparer.Comparer;
            var limit1 = new Limit(100);
            var limit2 = new Limit(95);
            var limit3 = new Limit(105);

            // Act & Assert
            Assert.Equal(1, comparer.Compare(limit1, limit2)); // 100 > 95
            Assert.Equal(-1, comparer.Compare(limit2, limit1)); // 95 < 100
            Assert.Equal(0, comparer.Compare(limit1, limit1)); // 100 == 100
            Assert.Equal(-1, comparer.Compare(limit2, limit3)); // 95 < 105
            Assert.Equal(1, comparer.Compare(limit3, limit2)); // 105 > 95
        }

        [Fact]
        public void AskLimitComparer_NullHandling()
        {
            // Arrange
            var comparer = AskLimitComparer.Comparer;
            var limit = new Limit(100);

            // Act & Assert
            Assert.Equal(-1, comparer.Compare(null, limit));
            Assert.Equal(1, comparer.Compare(limit, null));
            Assert.Equal(0, comparer.Compare(null, null));
        }

        [Fact]
        public void BidLimitComparer_SortsDescending()
        {
            // Arrange
            var comparer = BidLimitComparer.Comparer;
            var limit1 = new Limit(100);
            var limit2 = new Limit(95);
            var limit3 = new Limit(105);

            // Act & Assert
            Assert.Equal(-1, comparer.Compare(limit1, limit2)); // 100 < 95 (descending)
            Assert.Equal(1, comparer.Compare(limit2, limit1)); // 95 > 100 (descending)
            Assert.Equal(0, comparer.Compare(limit1, limit1)); // 100 == 100
            Assert.Equal(1, comparer.Compare(limit2, limit3)); // 95 > 105 (descending)
            Assert.Equal(-1, comparer.Compare(limit3, limit2)); // 105 < 95 (descending)
        }

        [Fact]
        public void BidLimitComparer_NullHandling()
        {
            // Arrange
            var comparer = BidLimitComparer.Comparer;
            var limit = new Limit(100);

            // Act & Assert
            Assert.Equal(-1, comparer.Compare(null, limit));
            Assert.Equal(1, comparer.Compare(limit, null));
            Assert.Equal(0, comparer.Compare(null, null));
        }

        [Fact]
        public void AskLimitComparer_SortedSet_OrdersCorrectly()
        {
            // Arrange
            var sortedSet = new SortedSet<Limit>(AskLimitComparer.Comparer);
            var limit1 = new Limit(100);
            var limit2 = new Limit(95);
            var limit3 = new Limit(105);

            // Act
            sortedSet.Add(limit1);
            sortedSet.Add(limit2);
            sortedSet.Add(limit3);

            // Assert
            var limits = sortedSet.ToArray();
            Assert.Equal(95, limits[0].Price); // Lowest first
            Assert.Equal(100, limits[1].Price);
            Assert.Equal(105, limits[2].Price); // Highest last
        }

        [Fact]
        public void BidLimitComparer_SortedSet_OrdersCorrectly()
        {
            // Arrange
            var sortedSet = new SortedSet<Limit>(BidLimitComparer.Comparer);
            var limit1 = new Limit(100);
            var limit2 = new Limit(95);
            var limit3 = new Limit(105);

            // Act
            sortedSet.Add(limit1);
            sortedSet.Add(limit2);
            sortedSet.Add(limit3);

            // Assert
            var limits = sortedSet.ToArray();
            Assert.Equal(105, limits[0].Price); // Highest first
            Assert.Equal(100, limits[1].Price);
            Assert.Equal(95, limits[2].Price); // Lowest last
        }

        [Fact]
        public void Comparers_SingletonPattern()
        {
            // Act & Assert
            var askComparer1 = AskLimitComparer.Comparer;
            var askComparer2 = AskLimitComparer.Comparer;
            var bidComparer1 = BidLimitComparer.Comparer;
            var bidComparer2 = BidLimitComparer.Comparer;

            Assert.Same(askComparer1, askComparer2);
            Assert.Same(bidComparer1, bidComparer2);
            Assert.NotSame(askComparer1, bidComparer1);
        }
    }
}
