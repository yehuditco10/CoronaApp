using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaApp.Services
{
   
    class UserCreatedHandler : IHandleMessages<UserCreated>
    {
        static ILog log = LogManager.GetLogger<UserCreated>();

        public Task Handle(UserCreated message, IMessageHandlerContext context)
        {
            log.Info($"send PlaceOrder, OrderId = {message.UserId}");

            // This is normally where some business logic would occur

            var userCreated = new UserCreated
            {
                UserId = message.UserId
            };
            return context.Publish(userCreated);
        }
    }


}
