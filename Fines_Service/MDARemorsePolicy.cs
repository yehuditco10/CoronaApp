using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fines_Service
{
    class MDARemorsePolicy : Saga<MDARemorseState>,
        IAmStartedByMessages<UserTested>
    {
        static ILog log = LogManager.GetLogger<MDARemorsePolicy>();

        public async Task Handle(UserTested message, IMessageHandlerContext context)
        {
            log.Info($"Received UserTested, UserId = {message.UserId}");

            Data.UserId = message.UserId;

            log.Info($"Starting cool down period for order #{Data.UserId}.");
            await RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MDARemorseState> mapper)
        {
            mapper.ConfigureMapping<UserTested>(p => p.UserId).ToSaga(s => s.UserId);
        }
    }

    public class MDARemorseState : ContainSagaData
    {
        public string UserId { get; set; }
    }
}
