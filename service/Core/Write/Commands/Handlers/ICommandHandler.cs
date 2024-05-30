using System.Threading.Tasks;

namespace CqrsReadWriteSample.Core.Write.Commands.Handlers;

public interface ICommandHandler<TCommand> where TCommand : class
{
    Task Handle(TCommand command);
}