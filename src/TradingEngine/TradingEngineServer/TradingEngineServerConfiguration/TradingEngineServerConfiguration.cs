using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Core.Configuration
{
     class TradingEngineServerConfiguration
     {
         public TradingEngineServerSettings Settings { get; set; } = new TradingEngineServerSettings(); // default settings
     }
     class TradingEngineServerSettings
     { 
          public int Port { get; set; } = 5000; 
     }
}
