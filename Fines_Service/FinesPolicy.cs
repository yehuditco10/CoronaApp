using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fines_Service
{
    public class FinesPolicy : Saga<FinesPolicyData>,
         IAmStartedByMessages<UserTested>,
         IAmStartedByMessages<UserViolations>
    {
        static ILog log = LogManager.GetLogger<FinesPolicy>();

        public Task Handle(UserTested message, IMessageHandlerContext context)
        {

            log.Info($"UserTested message received.");
            Data.IsUserTested = message.IsPositive;
            //log.Info($"Received UserTested, IsPositive = {message.IsPositive} ");
            return Task.CompletedTask;
        }
        public Task Handle(UserViolations message, IMessageHandlerContext context)
        {
            log.Info($"UserViolations message received.");
            Data.IsUserViolations = message.Violations > 0 ? true : false;
            //log.Info($"Received UserViolations, Number of violations = {message.Violations} ");
            return Task.CompletedTask;
        }
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<FinesPolicyData> mapper)
        {
            mapper.ConfigureMapping<UserTested>(message => message.UserId)
                .ToSaga(sagaData => sagaData.UserId);
            mapper.ConfigureMapping<UserViolations>(message => message.UserId)
                .ToSaga(sagaData => sagaData.UserId);
        }
        //private async Task ProcessFine(IMessageHandlerContext context)
        //{
        //    if (Data.IsUserTested && Data.IsUserTested)
        //    {
        //        await context.SendLocal(new UserFined() { 
        //        UserId=Data.UserId,
        //        Sum=500
        //       });
        //        MarkAsComplete();
        //    }
        //}
    }
}
