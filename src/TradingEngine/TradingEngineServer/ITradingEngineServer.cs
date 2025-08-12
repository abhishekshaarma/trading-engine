using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using TradingEngineServer.Core;
namespace TradingEngineServer.Core
{
     interface ITradingEngineServer
     {
          Task Run(CancellationToken token);

     }
}