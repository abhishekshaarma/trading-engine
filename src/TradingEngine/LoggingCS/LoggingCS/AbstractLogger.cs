using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Logging;

namespace TradingEngineServer.Logging
{
     public abstract class AbstractLogger : ILogger
     {
          protected AbstractLogger()
          {
               // Initialize any resources if needed

          }


          public void Debug(string module, string message) => Log(Loglevel.Debug, module, message);
          
          public void Debug(string module, Exception exception) => Log(Loglevel.Debug, module, $"{exception}");

          public void Information(string module, string message) => Log(Loglevel.Information, module, message);
          
          public void Information(string module, Exception exception) => Log(Loglevel.Information, module, $"{exception}");

          public void Warning(string module, string message) => Log(Loglevel.Warning, module, message);


          public void Warning(string module, Exception exception) => Log(Loglevel.Warning, module, $"{exception}");

          public void Error(string module, string message) => Log(Loglevel.Error, module, message);


          public void Error(string module, Exception exception) => Log(Loglevel.Error, module, $"{exception}");



          protected abstract void Log(Loglevel logLevel, string module, string message);




     }
}
