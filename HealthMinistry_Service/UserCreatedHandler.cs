using Messages;

using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HealthMinistry_Service
{
   public class UserCreatedHandler : IHandleMessages<UserCreated>
    {
        static ILog log = LogManager.GetLogger<UserCreatedHandler>();
        public Task Handle(UserCreated message, IMessageHandlerContext context)
        {
            log.Info($"Received userCreated in userViolation, UserId = {message.UserId} ...");

            var userViolations = new UserViolations
            {
                UserId = message.UserId,
                Violations=1
            };
            return context.Publish(userViolations);
            
        }
    }
}
