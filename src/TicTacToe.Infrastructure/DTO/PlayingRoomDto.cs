using System;

namespace TicTacToe.Infrastructure.DTO
{
    public class PlayingRoomDto
    {
        public Guid RoomId { get; set; }
        public int NumberOfPlayers { get; set; }
    }
}