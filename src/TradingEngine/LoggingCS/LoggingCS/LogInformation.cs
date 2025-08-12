using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Logging;

namespace TradingEngineServer.Logging
{
        public record LogInformation(Loglevel Loglevel, string Module, string Message, DateTime now, int ThreadId, string ThreadName);


     
}
namespace System.Runtime.CompilerServices
{ 
     internal static class IsExternalInit
     {
          // This class is used to enable init-only properties in C# 9 and later.
          // It is empty but must be defined to allow the compiler to recognize init properties.
     }
}