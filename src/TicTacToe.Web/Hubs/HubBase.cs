using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TicTacToe.Infrastructure.Commands;

namespace TicTacToe.Web.Hubs
{
    public abstract class HubBase : Hub
    {
        private readonly ICommandDispatcher CommandDispatcher;

        protected HubBase(ICommandDispatcher commandDispatcher)
        {
            CommandDispatcher = commandDispatcher;
        }

        protected async Task DispatchAsync<T>(T command) where T : ICommand
        {
            await CommandDispatcher.DispatchAsync(command);
        }
    }

}