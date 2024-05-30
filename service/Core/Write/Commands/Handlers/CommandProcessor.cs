using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CqrsReadWriteSample.Core.Write.Commands.Handlers;

public class CommandProcessor
{
    public async Task ExecuteCommand<TCommand>(TCommand command) where TCommand : class
    {
        var handler = typeof(ICommandHandler<>);
        var handlerType = handler.MakeGenericType(command.GetType());

        var concreteTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && t.GetInterfaces().Contains(handlerType))
            .ToArray();

        if (!concreteTypes.Any()) return;

        foreach (var type in concreteTypes)
        {
            var concreteHandler = Activator.CreateInstance(type) as ICommandHandler<TCommand>;
            await concreteHandler?.Handle(command)!;
        }
    }
}