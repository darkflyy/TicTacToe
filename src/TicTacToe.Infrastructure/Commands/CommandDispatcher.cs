using System;
using System.Threading.Tasks;
using Autofac;

namespace TicTacToe.Infrastructure.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _componentContext;
        public CommandDispatcher(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }
        public async Task DispatchAsync<T>(T command) where T : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), $"Komenda '{typeof(T).Name}' nie może być pusta.");
            }

            var handler = _componentContext.Resolve<ICommandHandler<T>>();
            await handler.HandleAsync(command);
        }
    }
}