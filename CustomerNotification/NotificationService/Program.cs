using GreenPipes;
using MassTransit;
using Messaging.InterfacesConstants.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Consumers;
using System;
using System.Threading.Tasks;

namespace NotificationService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreatHostBuilder(args).Build();
            await host.RunAsync();
        }

        private static IHostBuilder CreatHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                

                .ConfigureServices((hostContext, services) =>
                {
                    
                    services.AddMassTransit(c =>
                    {
                        c.AddConsumer<OrderProcessedEventConsumer>();
                    });
                    services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host("localhost", "/", h => { });
                        cfg.ReceiveEndpoint(RabbitMqMassTransitConstants.NotificationServiceQueue, e =>
                        {
                            e.PrefetchCount = 16;
                            e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                            e.Consumer<OrderProcessedEventConsumer>(provider);
                        });
                        //cfg.ConfigureEndpoints(provider);
                    }));
                    services.AddSingleton<IHostedService, BusService>();

                });
            return hostBuilder;
        }
    }
}
