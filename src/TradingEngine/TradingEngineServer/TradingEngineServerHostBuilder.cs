using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;
using TradingEngineServer.Logging.LoggingConfiguration;

namespace TradingEngineServer.Core
{
     public sealed class TradingEngineServerHostBuilder
     {
          public static IHost BuildTradingEngineServer() => Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
          {
               //configure services here
               services.AddOptions();

               services.Configure<TradingEngineServerConfiguration>(context.Configuration.GetSection(nameof(TradingEngineServerConfiguration)));
               //services.Configure<LoggerConfiguration>(context.Configuration.GetSection(nameof(LoggerConfiguration)));
               services.Configure<Logging.LoggingConfiguration.LoggerConfiguration>(context.Configuration.GetSection(nameof(LoggerConfiguration)));
               //register singleton service for TradingEngineServer
               services.AddSingleton<ITradingEngineServer, TradingEngineServer>();

               // add the hosted service -> add the type
               services.AddHostedService<TradingEngineServer>();
               services.AddSingleton<ITextLogger, TextLogger>();

          }).Build();
          
      
          
     }
}
