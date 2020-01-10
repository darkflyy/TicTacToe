using System.Threading.Tasks;
using TicTacToe.Infrastructure.Commands;
using TicTacToe.Infrastructure.Commands.PlayingRoom;
using TicTacToe.Infrastructure.Services;

namespace TicTacToe.Infrastructure.Handlers.PlayingRoom
{
    public class LeaveRoomHandler : ICommandHandler<LeaveRoom>
    {
        private readonly IPlayingRoomService _roomService;

        public LeaveRoomHandler(IPlayingRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task HandleAsync(LeaveRoom command)
        {
            command.Result = await _roomService.LeaveAsync(command.RoomId, command.ConnectionId);
        }
    }
}