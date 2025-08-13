
namespace TradingEngineServer.Instruments
{
     public class Secuirity
     {
          public Secuirity(string symbol, string name, long price, int precision)
          {
               Symbol = symbol;
               Name = name;
               Price = price;
               Precision = precision;
          }
          
          public string Symbol { get; set; }
          public string Name { get; set; }
          public long Price { get; set; }
          public int Precision { get; set; }
     }
}
