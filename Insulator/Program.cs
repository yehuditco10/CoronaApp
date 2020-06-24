
using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace Insulator
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Insulator";

            var endpointConfiguration = new EndpointConfiguration("Insulator");

            //persistence
            var connection = @"Data Source = DESKTOP-1HT6NS2; Initial Catalog = Outbox_DB; Integrated Security = True";
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });


            //RabitMQ
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString("host= localhost:5672;username=guest;password=guest");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.EnableOutbox();

            var routing = transport.Routing();
            routing.RouteToEndpoint(
            assembly: typeof(UserCreated).Assembly,
            destination: "MDA_Service");



            var endpointInstance = await Endpoint.Start(endpointConfiguration)
          .ConfigureAwait(false);

          
            await RunLoop(endpointInstance)
                .ConfigureAwait(false);

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        static ILog log = LogManager.GetLogger<Program>();

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                log.Info("Press 'P' to create a new user, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        // Instantiate the command
                        var createUserEvent = new UserCreated
                        {
                            UserId = Guid.NewGuid().ToString()
                        };

                        // Send the command
                        log.Info($"Published a new user, UserId = {createUserEvent.UserId}");
                        await endpointInstance.Publish(createUserEvent)
                            .ConfigureAwait(false);

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }

}
