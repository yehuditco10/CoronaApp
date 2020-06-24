using Messages;
using NServiceBus;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MDA_Service
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "MDA_Service";

            var endpointConfiguration = new EndpointConfiguration("MDA_Service");
            endpointConfiguration.EnableOutbox();
            //string connection = "Data Source = ILBHARTMANLT; Initial Catalog = Corona_DB; Integrated Security = True";
            string connection = ConfigurationManager.AppSettings["Outbox_DBConnection"];
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

            //var routing = transport.Routing();
            //routing.RouteToEndpoint(
            //assembly: typeof(OrderPlaced).Assembly,
            //destination: "Billing");

            //assemblyScaner
            //var scanner = endpointConfiguration.AssemblyScanner();
            //scanner.ExcludeAssemblies("Sales");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
               .ConfigureAwait(false);
            // Instantiate the command
          

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
