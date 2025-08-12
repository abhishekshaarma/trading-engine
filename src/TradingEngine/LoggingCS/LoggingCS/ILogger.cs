using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Logging
{
     public interface ILogger
     {
          // modeule refers to the class where the error or log persists
          void Debug(string module, string message);


          void Debug(string module, Exception exeption);
          void Information(string module, string message);
          void Information(string module, Exception exception);

          void Warning(string module, string message);
          void Warning(string module, Exception exception);
          void Error(string module, string message);
          void Error(string module, Exception exception);
          
     }
}
