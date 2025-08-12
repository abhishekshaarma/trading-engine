using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;

namespace TradingEngineServer.Core
{
     sealed class TradingEngineServer : BackgroundService, ITradingEngineServer
      {   
          private readonly  ITextLogger _logger;
          private readonly IOptions<TradingEngineServerConfiguration> _config; // dependency injection for configuration options
          public TradingEngineServer( IOptions<TradingEngineServerConfiguration> config, ITextLogger textLogger) // dependency injection for logger |||| dependency injection for 
          {    
               
               _logger = textLogger ?? throw new ArgumentNullException(nameof(textLogger)); // throw if logger is null 
               _config = config ?? throw new ArgumentNullException(nameof(config)); // throw if config is null (?? is the null coelision operator)
               /*
                 if (config.Value is null)
               throw new ArgumentNullException(nameof(config));

               _tradingEngineServerConfig = config.Value;

                */
          }

          public Task Run(CancellationToken token) => ExecuteAsync(token); // call the ExecuteAsync method with the cancellation token () making it public
          
       
          protected override Task ExecuteAsync(CancellationToken stoppingToken)
          {

               _logger.Information(nameof(TradingEngineServer), "Starting Trading Engine");

               while (!stoppingToken.IsCancellationRequested)
               { 
                    
               }
               _logger.Information(nameof(TradingEngineServer),"Trading Engine Server is stopping..."); // log the stopping message
               //throw new NotImplementedException();
               return Task.CompletedTask;
          }
     }
}
