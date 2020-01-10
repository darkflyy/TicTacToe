using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Core.Domain;

namespace TicTacToe.Infrastructure.Repositories
{
    public class PlayingRoomRepository : IPlayingRoomRepository
    {
        private static List<PlayingRoom> _rooms = new List<PlayingRoom> { };

        public async Task AddAsync(PlayingRoom playingRoom)
        {
            _rooms.Add(playingRoom);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(PlayingRoom playingRoom)
        {
            _rooms.Remove(playingRoom);
            await Task.CompletedTask;
        }

        public async Task EditAsync(PlayingRoom playingRoom)
        {
            _rooms[_rooms.FindIndex(room => room.RoomId == playingRoom.RoomId)] = playingRoom;
            await Task.CompletedTask;
        }

        public async Task<List<PlayingRoom>> GetAllAsync()
            => await Task.FromResult(_rooms);

        public async Task<PlayingRoom> GetAsync(Guid roomId)
            => await Task.FromResult(_rooms.SingleOrDefault(room => room.RoomId == roomId));
    }
}