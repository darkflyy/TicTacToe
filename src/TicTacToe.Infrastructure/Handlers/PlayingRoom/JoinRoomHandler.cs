using System.Threading.Tasks;
using TicTacToe.Infrastructure.Commands;
using TicTacToe.Infrastructure.Commands.PlayingRoom;
using TicTacToe.Infrastructure.Services;

namespace TicTacToe.Infrastructure.Handlers.PlayingRoom
{
    public class JoinRoomHandler : ICommandHandler<JoinRoom>
    {
        private readonly IPlayingRoomService _roomSerive;

        public JoinRoomHandler(IPlayingRoomService roomService)
        {
            _roomSerive = roomService;
        }

        public async Task HandleAsync(JoinRoom command)
        {
            command.Result = await _roomSerive.JoinAsync(command.RoomId, command.ConnectionId);
        }
    }
}