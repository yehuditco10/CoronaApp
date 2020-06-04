using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(Configuration)
           .CreateLogger();

            try
            {
               // Log.Information("Getting the motors running...");

                BuildWebHost(args).Run();
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
        public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
             .UseStartup<Startup>()
             .UseConfiguration(Configuration)
             .UseSerilog()
             .Build();
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>()
        //            .UseConfiguration(Configuration)
        //            .UseSerilog();
        //        });
    }
}
