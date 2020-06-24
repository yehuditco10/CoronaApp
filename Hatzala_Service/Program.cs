
using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Hatzala_Service
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Samples.Notifications";

            #region logging

            var defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Level(LogLevel.Fatal);

            #endregion

            #region endpointConfig

            var endpointConfiguration = new EndpointConfiguration("Samples.Notifications");
            SubscribeToNotifications.Subscribe(endpointConfiguration);

            #endregion

            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseTransport<LearningTransport>();

            #region customDelayedRetries

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                customizations: delayed =>
                {
                    delayed.TimeIncrease(TimeSpan.FromSeconds(1));
                });

            #endregion

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            var message = new UserCreated
            {
                UserId = "PropertyValue"
            };
            await endpointInstance.SendLocal(message)
                .ConfigureAwait(false);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
