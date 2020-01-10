using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToe.Core.Domain;
using TicTacToe.Infrastructure.DTO;

namespace TicTacToe.Infrastructure.Services
{
    public interface IPlayingRoomService : IService
    {
        Task<List<PlayingRoomDto>> GetAllAsync();
        Task<Guid> CreateAsync();
        Task<PlayingRoom> GetAsync(Guid id);
        Task<PlayingRoom> JoinAsync(Guid id, string connectionId);
        Task<PlayingRoom> LeaveAsync(Guid id, string connectionId);
        Task<PlayingRoom> PerformMoveAsync(Guid id, string[][] gameState, string identity);
    }
}