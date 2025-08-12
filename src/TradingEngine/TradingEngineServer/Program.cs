using System;
using System.Formats.Asn1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using TradingEngineServer.Core;


// using will make sure the engine is disposed of properly when the application exits
using var engine = TradingEngineServerHostBuilder.BuildTradingEngineServer();

//get back the IService Provider for the engine 
TradingEngineServerServiceProvider.ServiceProvider = engine.Services;
{ 
     using var scope = TradingEngineServerServiceProvider.ServiceProvider.CreateScope();
     await engine.RunAsync().ConfigureAwait(false);
     // or you can use the scope.ServiceProvider to get the services you need


}
