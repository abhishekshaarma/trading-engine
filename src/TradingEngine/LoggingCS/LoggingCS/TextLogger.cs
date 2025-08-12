using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Options;
using TradingEngineServer.Logging;
using TradingEngineServer.Logging.LoggingConfiguration;

namespace TradingEngineServer.Logging
{
     public class TextLogger : AbstractLogger, ITextLogger, IDisposable
     {
          private readonly LoggerConfiguration _loggingConfiguration;
          private readonly BufferBlock<LogInformation> _logQueue = new BufferBlock<LogInformation>();
          private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
          private bool _disposed = false;
          private readonly object _lock = new object();

          public TextLogger(IOptions<LoggerConfiguration> loggingConfiguration)
          {
               _loggingConfiguration = loggingConfiguration.Value;

               if (_loggingConfiguration.LoggerType != LoggerType.Text)
                    throw new InvalidOperationException($"{nameof(TextLogger)} does not match Logger type if {_loggingConfiguration.LoggerType}");

               var now = DateTime.Now;
               string logDirectory = Path.Combine(_loggingConfiguration.TextLoggingConfig.Directory, $"{now:yyyy-MM-dd}");
               Directory.CreateDirectory(logDirectory); // Ensure the directory exists

               string uniqueLogName = $"{_loggingConfiguration.TextLoggingConfig.FileName}-{now:yyyy-MM-dd-HH-mm-ss-fffff}";
               string baseLogName = uniqueLogName + _loggingConfiguration.TextLoggingConfig.FileExtension;

               string filepath = Path.Combine(logDirectory, baseLogName);

               // Start the logging task asynchronously, ignore the returned Task to not block constructor
               _ = Task.Run(() => LogAsync(filepath, _logQueue, _cancellationTokenSource.Token));
          }

          private static async Task LogAsync(string filepath, BufferBlock<LogInformation> logQueue, CancellationToken token)
          {
               using var fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.Read);
               using var sw = new StreamWriter(fs) { AutoFlush = true, };

               try
               {
                    while (!token.IsCancellationRequested)
                    {
                         var logItem = await logQueue.ReceiveAsync(token).ConfigureAwait(false);
                         string formattedMessage = FormatLogItem(logItem);
                         await sw.WriteLineAsync(formattedMessage).ConfigureAwait(false);
                         await sw.FlushAsync().ConfigureAwait(false);
                    }
               }
               catch (OperationCanceledException)
               {
                    // Expected when token is cancelled
               }
          }

          private static string FormatLogItem(LogInformation logItem)
          {
               return $"[{logItem.now:HH--mm--ss.fffff}] [{logItem.ThreadName ?? "NoName"}: {logItem.ThreadId:0000}]" +
                      $" [{logItem.Loglevel}] {logItem.Message}";
          }

          protected override void Log(Loglevel logLevel, string module, string message)
          {
               _logQueue.Post(new LogInformation(
                   logLevel,
                   module,
                   message,
                   DateTime.Now,
                   Thread.CurrentThread.ManagedThreadId,
                   Thread.CurrentThread.Name));
          }

          ~TextLogger() // Finalizer to ensure resources are cleaned up if Dispose is not called
          {
               Dispose(false);
          }

          public void Dispose()
          {
               Dispose(true);
               GC.SuppressFinalize(this);
          }

          protected virtual void Dispose(bool disposing)
          {
               lock (_lock)
               {
                    if (_disposed)
                         return;

                    _disposed = true;
               }

               if (disposing)
               {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource.Dispose();
               }
          }
     }
}