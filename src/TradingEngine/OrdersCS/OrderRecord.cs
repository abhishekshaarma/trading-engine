using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
     public record OrderRecord(long OrderId, uint Quantity, long Price, bool IsBuySide,string username, int SecuirityId, uint TheoriticalQueuePosition);
    
}
namespace System.Runtime.CompilerServices
{
     // This is needed to allow the use of 'required' properties in records
     // in .NET Standard 2.0, which does not support 'required' properties.
     // It can be removed if targeting .NET 5.0 or later.
     internal static class IsExternalInit { }
}