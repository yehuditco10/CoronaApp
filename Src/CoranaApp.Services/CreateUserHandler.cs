using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaApp.Services
{
   
    class CreateUserHandler : IHandleMessages<CreateUser>
    {
        static ILog log = LogManager.GetLogger<CreateUserHandler>();

        public Task Handle(CreateUser message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.UserId}");

            // This is normally where some business logic would occur

            var userCreated = new UserCreated
            {
                UserId = message.UserId
            };
            return context.Publish(userCreated);
        }
    }


}
