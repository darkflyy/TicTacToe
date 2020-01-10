using System.Threading.Tasks;
using TicTacToe.Infrastructure.Commands;
using TicTacToe.Infrastructure.Commands.PlayingRoom;
using TicTacToe.Infrastructure.Services;

namespace TicTacToe.Infrastructure.Handlers.PlayingRoom
{
    public class CreateRoomHandler : ICommandHandler<CreateRoom>
    {
        private readonly IPlayingRoomService _roomService;

        public CreateRoomHandler(IPlayingRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task HandleAsync(CreateRoom command)
        {
            command.Result = await _roomService.CreateAsync();
        }
    }
}