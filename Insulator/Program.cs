
using Messages;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence;
using System;
using System.Configuration;
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
            var connection = ConfigurationManager.AppSettings["Outbox_DBConnection"];
            var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
            persistence.ConnectionString(connection);
            //var subscriptions = persistence.SubscriptionSettings();
            //subscriptions.CacheFor(TimeSpan.FromMinutes(1));
            //persistence.SqlDialect<SqlDialect.MsSqlServer>();
            //persistence.ConnectionBuilder(
            //    connectionBuilder: () =>
            //    {
            //        return new SqlConnection(connection);
            //    });


            //RabitMQ
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString("host= localhost:5672;username=guest;password=guest");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.EnableOutbox();
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            var routing = transport.Routing();
            //routing.RouteToEndpoint(
            //assembly: typeof(UserCreated).Assembly,
            //destination: "HealthMinistry");
            routing.RouteToEndpoint(typeof(TestCanceled), "MDA_Service");


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
            var lastUser = string.Empty;

            while (true)
            {
                log.Info("Press 'P' to place a new user, 'C' to cancel last user, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        // Instantiate the command
                        var createUserEvent = new UserCreated
                        {
                            //  UserId = Guid.NewGuid().ToString()
                            UserId = "123"

                        };

                        // Send the command
                        log.Info($"Published a new user, UserId = {createUserEvent.UserId}");
                        await endpointInstance.Publish(createUserEvent)
                            .ConfigureAwait(false);


                        lastUser = createUserEvent.UserId; // Store order identifier to cancel if needed.
                        break;

                    case ConsoleKey.C:
                        var cancelEvent = new TestCanceled
                        {
                            UserId = lastUser
                        };
                        await endpointInstance.Send(cancelEvent)
                            .ConfigureAwait(false);
                        log.Info($"Sent a correlated message to {cancelEvent.UserId}");
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
