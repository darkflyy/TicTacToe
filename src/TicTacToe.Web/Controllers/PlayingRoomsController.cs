using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicTacToe.Infrastructure.Commands;
using TicTacToe.Infrastructure.Commands.PlayingRoom;
using TicTacToe.Web.Hubs;

namespace TicTacToe.Web.Controllers
{
    [ApiController]
    public class PlayingRoomsController : ApiControllerBase
    {
        private readonly ILogger<PlayingRoomsController> _logger;
        private readonly PlayingRoomListHub _roomListHub;
        private readonly PlayingRoomHub _roomHub;

        public PlayingRoomsController(ILogger<PlayingRoomsController> logger, ICommandDispatcher commandDispatcher, PlayingRoomHub roomHub, PlayingRoomListHub roomListHub) : base(commandDispatcher)
        {
            _logger = logger;
            _roomHub = roomHub;
            _roomListHub = roomListHub;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var command = new GetAll();
            await DispatchAsync(command);
            return Ok(command.Result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create()
        {
            var command = new CreateRoom();
            await DispatchAsync(command);
            await _roomListHub.SendToAllAsync("created", new { roomId = command.Result });
            return Ok(new { roomId = command.Result });
        }
    }
}