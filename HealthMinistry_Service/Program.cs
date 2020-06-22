﻿using NServiceBus;
using System;
using System.Threading.Tasks;

namespace HealthMinistry_Service
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "HealthMinistryService";

            var endpointConfiguration = new EndpointConfiguration("HealthMinistryService");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
