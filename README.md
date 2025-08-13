# **Trading Engine**
High-performance trading engine with async logging, order management, and matching engine built on .NET 8 and TPL Dataflow.


## **Architecture**
* **LoggingCS**: Async logging library using `BufferBlock<LogInformation>`
* **OrdersCS**: Order lifecycle management with validation and status tracking
* **OrderbookCS**: Multi-interface orderbook with matching engine
* **TradingEngineServer**: Main server with configurable port (default: 12000)
* **TPL Dataflow**: Non-blocking processing with producer-consumer patterns

## **Configuration**
`TradingEngineServer/appsettings.json`:

```json
{
  "LoggerConfiguration": {
    "LoggerType": "Text",
    "TextLoggerConfiguration": {
      "Directory": "C:\\TradingEngine\\logs",
      "FileName": "TradingEngineServer",
      "FileExtension": ".log"
    }
  },
  "TradingEngineServerConfiguration": {
    "TradingEngineServerSettings": {
      "Port": 12000
    }
  }
}
```

## **Key Features**
* **Async Logging**: Non-blocking using `BufferBlock<LogInformation>`
* **Order Management**: Complete lifecycle with `Order`, `ModifyOrder`, `CancelOrder`
* **Orderbook Interfaces**: Segregated `IMatchingOrderbook`, `IOrderEntryOrderbook`, `IReadOnlyOrderbook`
* **Performance**: ~100k+ logs/second, microsecond timestamp precision
* **Thread Safety**: Lock-based disposal with `CancellationTokenSource`
* **File Rotation**: Daily logs in `{yyyy-MM-dd}/` folders

## **Core Classes**

```csharp
public record OrderRecord(OrderCore OrderCore, DateTime CreationTime, OrderStatus Status);

public class TextLogger : AbstractLogger, ITextLogger, IDisposable
{
    private readonly BufferBlock<LogInformation> _logQueue;
}

public interface IMatchingOrderbook
{
    MatchResults Match(Order order);
}
```

## **Project Structure**

```
TradingEngine/
├── LoggingCS/                    # Async logging library
├── OrdersCS/                     # Order management system
├── OrderbookCS/                  # Orderbook and matching engine
└── TradingEngineServer/          # Main server application
```

## **Performance**
* **Log Processing**: ~100k+ logs/second with TPL Dataflow
* **Memory**: Constant footprint with backpressure handling
* **Threading**: Linear scaling with producer threads
* **File I/O**: `FileStream` with concurrent read access

## **Future**
* Market data distribution
* Risk management integration
* Database logging backend
* Docker support

## **Requirements**
* .NET 8 SDK
* Windows 10/11 (for EventLog, FileSystem features)
* SSD storage recommended

**Built with .NET 8, TPL Dataflow, and modern C# practices**
