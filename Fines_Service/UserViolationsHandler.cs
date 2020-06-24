using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Fines_Service
{
    public class UserViolationsHandler :
        IHandleMessages<UserViolations>
    {
        static ILog log = LogManager.GetLogger<UserViolationsHandler>();

        public Task Handle(UserViolations message, IMessageHandlerContext context)
        {
            log.Info($"Received UserViolations, Number of violations = {message.Violations} ");
            return Task.CompletedTask;
        }
    }
}