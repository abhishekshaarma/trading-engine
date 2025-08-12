namespace TradingEngineServer.Logging.LoggingConfiguration
{
     public class LoggerConfiguration
     {
          public LoggerType LoggerType { get; set; } = LoggerType.Text;
          public TextLoggerConfiguration TextLoggingConfig { get; set; } = new TextLoggerConfiguration(); // Changed property name

          public class TextLoggerConfiguration
          {
               public string Directory { get; set; }
               public string FileName { get; set; }
               public string FileExtension { get; set; }
          }
     }
}