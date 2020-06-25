using NServiceBus;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Fines_Service
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Fines";

            var endpointConfiguration = new EndpointConfiguration("Fines");

            endpointConfiguration.EnableOutbox();
            var connection = @"Data Source = ILBHARTMANLT; Initial Catalog = Outbox_DB; Integrated Security = True";
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString("host= localhost:5672;username=guest;password=guest");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.AuditSagaStateChanges(
    serviceControlQueue: "Particular.Coronaservicecontrol");


            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}