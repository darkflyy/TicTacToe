using System.Threading.Tasks;
using TicTacToe.Infrastructure.Commands;
using TicTacToe.Infrastructure.Commands.PlayingRoom;
using TicTacToe.Infrastructure.Services;

namespace TicTacToe.Infrastructure.Handlers.PlayingRoom
{
    public class GetAllHandler : ICommandHandler<GetAll>
    {
        private readonly IPlayingRoomService _roomService;

        public GetAllHandler(IPlayingRoomService roomService){
            _roomService = roomService;
        }

        public async Task HandleAsync(GetAll command)
        {
            command.Result = await _roomService.GetAllAsync();
        }
    }
}