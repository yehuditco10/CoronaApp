//using Messages;

//using NServiceBus;
//using NServiceBus.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace MDA_Service
//{
//   public class UserCreatedHandler : IHandleMessages<UserCreated>
//    {
//        static ILog log = LogManager.GetLogger<UserCreatedHandler>();
//        public Task Handle(UserCreated message, IMessageHandlerContext context)
//        {
//            log.Info($"Received userTested, UserId = {message.UserId} ...");

//            var userTested = new UserTested
//            {
//                UserId = message.UserId,
//                IsPositive = true
//            };
//            return context.Publish(userTested);
            
//        }
//    }
//}
