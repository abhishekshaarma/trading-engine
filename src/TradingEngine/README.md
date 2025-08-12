# Trading Engine Server

High-performance trading engine server with async logging built on .NET 8 and TPL Dataflow.

## Quick Start

```bash
# Build
dotnet build TradingEngineServer.sln

# Run
cd TradingEngineServer
dotnet run
```

## Architecture

- **LoggingCS**: Async logging library using `BufferBlock<LogInformation>`
- **TradingEngineServer**: Main server with configurable port (default: 12000)
- **TPL Dataflow**: Non-blocking log processing with producer-consumer pattern

## Configuration

`TradingEngineServer/appsettings.json`:
```json
{
  "LoggerConfiguration": {
    "LoggerType": "Text",
    "TextLoggerConfiguration": {
      "Directory": "C:\\TradingEngine\\src\\TradingEngine",
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

## Key Features

- **Async Logging**: Non-blocking using `BufferBlock<LogInformation>`
- **File Rotation**: Daily logs in `{Directory}/{yyyy-MM-dd}/` folders
- **Thread Safety**: Lock-based disposal with `CancellationTokenSource`
- **Performance**: ~100k+ logs/second, microsecond timestamp precision
- **Memory Efficient**: Constant footprint regardless of log volume

## Log Format

```
[HH:mm:ss.fffff] [ThreadName: ThreadId] [LogLevel] Message
```

Example: `[18:45:20.15671] [MainThread: 0001] [Information] Server started`

## Core Classes

### TextLogger
```csharp
public class TextLogger : AbstractLogger, ITextLogger, IDisposable
{
    private readonly BufferBlock<LogInformation> _logQueue;
    private readonly CancellationTokenSource _cancellationTokenSource;
}
```

### LogInformation
```csharp
public record LogInformation(
    Loglevel Loglevel, string Module, string Message, 
    DateTime now, int ThreadId, string ThreadName);
```

## Performance

- **Log Processing**: Asynchronous with TPL Dataflow
- **File I/O**: `FileStream` with `FileShare.Read` for concurrent access
- **Memory**: Automatic backpressure handling via `BufferBlock<T>`
- **Threading**: Linear scaling with producer threads

## Development

### Add New Logger Type
1. Implement `ILogger` interface
2. Add to `LoggerType` enum
3. Extend `LoggerConfiguration` class
4. Register with DI container

### Testing
```bash
dotnet test TradingEngineServer.sln
```

## Requirements

- .NET 8 SDK
- Windows 10/11 (for EventLog, FileSystem features)
- SSD storage recommended for log performance

## Project Structure

```
TradingEngine/
├── LoggingCS/                    # Logging library
│   ├── TextLogger.cs            # Async file logging
│   ├── LogInformation.cs        # Immutable log records
│   └── LoggingConfiguration/    # Configuration classes
└── TradingEngineServer/          # Main server app
    ├── Program.cs               # Entry point
    ├── appsettings.json         # Configuration
    └── TradingEngineServer.cs   # Server implementation
```

## Future

- Database logging backend
- Network logging (UDP/TCP)
- Log compression
- Docker support
- Kubernetes manifests

---

**Built with .NET 8, TPL Dataflow, and modern C# practices**
