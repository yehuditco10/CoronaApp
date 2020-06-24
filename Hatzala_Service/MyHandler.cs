using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

public class MyHandler :
    IHandleMessages<UserCreated>
{
    public Task Handle(UserCreated message, IMessageHandlerContext context)
    {
        throw new Exception("The exception message");
    }
}