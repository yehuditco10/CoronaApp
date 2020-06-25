using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
    public class TestCanceled:ICommand
    {
        public string UserId { get; set; }
    }
}
