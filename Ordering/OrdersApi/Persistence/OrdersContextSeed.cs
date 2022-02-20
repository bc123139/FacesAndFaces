using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersApi.Persistence
{
    public class OrdersContextSeed
    {
        public static async Task SeedAsync(OrdersContext orderContext, ILogger<OrdersContextSeed> logger)
        {
            logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrdersContext).Name);
            
        }

        
    }
}
