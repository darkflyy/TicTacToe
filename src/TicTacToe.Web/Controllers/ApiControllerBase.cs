using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Infrastructure.Commands;

namespace TicTacToe.Web.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private readonly ICommandDispatcher CommandDispatcher;

        protected ApiControllerBase(ICommandDispatcher commandDispatcher)
        {
            CommandDispatcher = commandDispatcher;
        }

        protected async Task DispatchAsync<T>(T command) where T : ICommand
        {
            await CommandDispatcher.DispatchAsync(command);
        }
    }
}