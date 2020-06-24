using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Fines_Service
{
    public class UserTestedHandler :
        IHandleMessages<UserTested>
    {
        static ILog log = LogManager.GetLogger<UserTestedHandler>();

        public Task Handle(UserTested message, IMessageHandlerContext context)
        {
            log.Info($"Received UserTested, IsPositive = {message.IsPositive} ");
            return Task.CompletedTask;
        }
    }
}