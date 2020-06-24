using Messages;

using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MDA_Service
{
    class UserCreatedHandler : IHandleMessages<UserCreated>
    {
        static ILog log = LogManager.GetLogger<UserCreatedHandler>();
        public Task Handle(UserCreated message, IMessageHandlerContext context)
        {
            log.Info($"Received UserCreated, UserId = {message.UserId} ...");
            if (message.UserId.Contains("t"))
                throw new Exception("do you see me??????????");
            return Task.CompletedTask;
        }

    }
}
