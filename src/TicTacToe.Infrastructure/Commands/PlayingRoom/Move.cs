using System;

namespace TicTacToe.Infrastructure.Commands.PlayingRoom
{
    public class Move : CommandWithResult<TicTacToe.Core.Domain.PlayingRoom>
    {
        public Guid RoomId { get; set; }
        public string[][] GameState { get; set; }
        public string Identity { get; set; }
    }
}