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
        IAmStartedByMessages<UserCreated>,
        IHandleMessages<TestCanceled>,
        IHandleTimeouts<MDARemorseIsOver>
    {
        static ILog log = LogManager.GetLogger<MDARemorsePolicy>();

        public async Task Handle(UserCreated message, IMessageHandlerContext context)
        {
            log.Info($"Received UserTested, UserId = {message.UserId}");

            Data.UserId = message.UserId;

            log.Info($"Starting cool down period for user # {Data.UserId}.");
            await RequestTimeout(context, TimeSpan.FromSeconds(20), new MDARemorseIsOver());
        }

        public Task Handle(TestCanceled message, IMessageHandlerContext context)
        {
            log.Info($"Order #{message.UserId} was cancelled.");

            //TODO: Possibly publish an OrderCancelled event?

            MarkAsComplete();

            return Task.CompletedTask;
        }

        public async Task Timeout(MDARemorseIsOver state, IMessageHandlerContext context)
        {
            log.Info($"Cooling down period for order #{Data.UserId} has elapsed.");

            var userTested = new UserTested
            {
                UserId = Data.UserId,
                IsPositive = false
            };

            await context.Publish(userTested);

            MarkAsComplete();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MDARemorseState> mapper)
        {
            //mapper.ConfigureMapping<UserTested>(p => p.UserId).ToSaga(s => s.UserId);
            mapper.ConfigureMapping<UserCreated>(p => p.UserId).ToSaga(s => s.UserId);
            mapper.ConfigureMapping<TestCanceled>(p => p.UserId).ToSaga(s => s.UserId);
        }
    }
    public class MDARemorseIsOver
    { }
    public class MDARemorseState : ContainSagaData
    {
        public string UserId { get; set; }
    }
}
