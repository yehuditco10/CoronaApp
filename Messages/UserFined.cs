using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
    public class UserFined : ICommand
    {
        public string UserId { get; set; }
        public int Sum { get; set; }
    }
}
