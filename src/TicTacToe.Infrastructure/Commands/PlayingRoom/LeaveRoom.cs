using System;

namespace TicTacToe.Infrastructure.Commands.PlayingRoom
{
    public class LeaveRoom : CommandWithResult<TicTacToe.Core.Domain.PlayingRoom>
    {
        public Guid RoomId { get; set; }
        public string ConnectionId { get; set; }
    }
}