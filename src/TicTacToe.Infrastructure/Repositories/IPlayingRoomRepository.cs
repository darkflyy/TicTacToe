using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToe.Core.Domain;

namespace TicTacToe.Infrastructure.Repositories
{
    public interface IPlayingRoomRepository : IRepository
    {
        Task AddAsync(PlayingRoom playingRoom);
        Task<PlayingRoom> GetAsync(Guid roomId);
        Task<List<PlayingRoom>> GetAllAsync();
        Task EditAsync(PlayingRoom playingRoom);
        Task DeleteAsync(PlayingRoom playingRoom);
    }
}