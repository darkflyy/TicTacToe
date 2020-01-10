using System.Threading.Tasks;
using TicTacToe.Infrastructure.Commands;
using TicTacToe.Infrastructure.Commands.PlayingRoom;
using TicTacToe.Infrastructure.Services;

namespace TicTacToe.Infrastructure.Handlers.PlayingRoom
{
    public class MoveHandler : ICommandHandler<Move>
    {
        private readonly IPlayingRoomService _roomService;

        public MoveHandler(IPlayingRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task HandleAsync(Move command)
        {
            command.Result = await _roomService.PerformMoveAsync(command.RoomId, command.GameState, command.Identity);
        }
    }
}