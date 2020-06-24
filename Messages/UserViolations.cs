using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
   public class UserViolations:IEvent
    {
        public string UserId { get; set; }
        public int Violations { get; set; }
    }
}
