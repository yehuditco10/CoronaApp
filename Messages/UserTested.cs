using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
   public class UserTested:IEvent
    {
        public string UserId { get; set; }
        public bool IsPositive { get; set; }
    }
}
