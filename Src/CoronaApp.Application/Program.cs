using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Messages;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Hosting;
using NServiceBus.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;

namespace CoronaApp.Api
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
       .Build();
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(Configuration)
           .CreateLogger();

            try
            {
                // Log.Information("Getting the motors running...");

                //BuildWebHost(args).Run();
                CreateHostBuilder(args)
                      .Build()
                      .Run();
            }
            catch (Exception ex)
            {
                // Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                //  Log.CloseAndFlush();
            }
            //Log.Logger = new LoggerConfiguration()
            //  .WriteTo
            //  .MSSqlServer(
            //   connectionString: "Server=localhost;Database=LogDb;Integrated Security=SSPI;",
            //   sinkOptions: new SinkOptions { TableName = "LogEvents" })
            //  .CreateLogger();

            //CreateHostBuilder(args)
            //      .Build()
            //      .Run();

            //Serilog.Debugging.SelfLog.Enable(msg =>
            //{
            //    Debug.Print(msg);
            //    Debugger.Break();
            //    Console.WriteLine(msg);
            //});



        }

        //public static IWebHost BuildWebHost(string[] args) =>
        // WebHost.CreateDefaultBuilder(args)

        //     .UseNServiceBus(hostBuilderContext =>
        //     {
        //         var endpointConfiguration = new EndpointConfiguration("createUser");

        //         var transport = endpointConfiguration.UseTransport<LearningTransport>();

        //         var routing = transport.Routing();
        //         routing.RouteToEndpoint(typeof(CreateUser), "HealthMinistryService");

        //         var endpointInstance = await Endpoint.Start(endpointConfiguration)
        //               .ConfigureAwait(false);
        //         var recoverability = endpointConfiguration.Recoverability();
        //         recoverability.Delayed(
        //             delayed =>
        //             {
        //                 delayed.NumberOfRetries(2);
        //                 delayed.TimeIncrease(TimeSpan.FromMinutes(5));
        //             });
        //         recoverability.Immediate(
        //             immediate =>
        //             {
        //                 immediate.NumberOfRetries(3);
        //             });
        //         //persistence
        //         var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        //         var connection = @"Data Source = DESKTOP-1HT6NS2; Initial Catalog = Corona_DB; Integrated Security = True";
        //         var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        //         var subscriptions = persistence.SubscriptionSettings();
        //         subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        //         persistence.SqlDialect<SqlDialect.MsSqlServer>();
        //         persistence.ConnectionBuilder(
        //             connectionBuilder: () =>
        //             {
        //                 return new SqlConnection(connection);
        //             });

        //         return endpointConfiguration;
        //     })
        //     .UseStartup<Startup>()
        //     .UseConfiguration(Configuration)
        //     .UseSerilog()
        //     .Build();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            //.UseNServiceBus(async  hostBuilderContext =>
            // {
            //     var endpointConfiguration = new EndpointConfiguration("createUser");

            //     var connection = @"Data Source = DESKTOP-1HT6NS2; Initial Catalog = Outbox; Integrated Security = True";
            //     var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            //     var subscriptions = persistence.SubscriptionSettings();
            //     subscriptions.CacheFor(TimeSpan.FromMinutes(1));
            //     persistence.SqlDialect<SqlDialect.MsSqlServer>();
            //     persistence.ConnectionBuilder(
            //         connectionBuilder: () =>
            //         {
            //             return new SqlConnection(connection);
            //         });


            //     //RabitMQ
            //     var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            //     transport.UseConventionalRoutingTopology();
            //     transport.ConnectionString("host= localhost:5672;username=guest;password=guest");
            //     endpointConfiguration.EnableInstallers();
            //     endpointConfiguration.EnableOutbox();

            //     var routing = transport.Routing();
            //     routing.RouteToEndpoint(
            //     assembly: typeof(CreateUser).Assembly,
            //     destination: "HealthMinistry_Service");



            //     var endpointInstance =await Endpoint.Start(endpointConfiguration)
            //           .ConfigureAwait(false);

            //     var command = new CreateUser
            //     {
            //         UserId = Guid.NewGuid().ToString()
            //     };

            //     // Send the command
            //     log.Info($"Sending PlaceOrder command, OrderId = {command.UserId}");
            //   await endpointInstance.Send(command)
            //        .ConfigureAwait(false);
            //     //RunLoop(endpointInstance)
            //     //    .ConfigureAwait(false);

            //     //endpointInstance.Stop()
            //     //   .ConfigureAwait(false);
            //     // return endpointConfiguration;
            //     return Task.CompletedTask;
            // })
            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                      .UseConfiguration(Configuration)
                      .UseSerilog();

                });


     
    }
}