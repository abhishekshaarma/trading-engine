namespace TradingEngineServer.Orderbook
{
     public interface IReadOnlyOrderbook
     {
          bool ContainsOrder(long Order);
          OrderbookSpread GetSpread();
           int Count { get;  }

     }
}
