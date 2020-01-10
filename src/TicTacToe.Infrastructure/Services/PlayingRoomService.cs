using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TicTacToe.Core.Domain;
using TicTacToe.Infrastructure.DTO;
using TicTacToe.Infrastructure.Repositories;

namespace TicTacToe.Infrastructure.Services
{
    public class PlayingRoomService : IPlayingRoomService
    {
        private readonly IPlayingRoomRepository _playingRoomRepository;
        private readonly IMapper _mapper;

        public PlayingRoomService(IPlayingRoomRepository playingRoomRepository, IMapper mapper)
        {
            _playingRoomRepository = playingRoomRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync()
        {
            var room = new PlayingRoom();
            await _playingRoomRepository.AddAsync(room);
            return room.RoomId;
        }

        public async Task<PlayingRoom> GetAsync(Guid id)
            => await _playingRoomRepository.GetAsync(id);

        public async Task<List<PlayingRoomDto>> GetAllAsync()
        {
            var rooms = await _playingRoomRepository.GetAllAsync();
            return _mapper.Map<List<PlayingRoomDto>>(rooms);
        }

        public async Task<PlayingRoom> JoinAsync(Guid id, string connectionId)
        {
            var room = await _playingRoomRepository.GetAsync(id);
            if (room.Players.Count < 2)
            {
                if (room.Players.Count == 0)
                {
                    room.Players.Add(new Player("X", connectionId));
                    room.NumberOfPlayers += 1;
                }
                else
                {
                    if (room.Players[0].Identity == "X")
                    {
                        room.Players.Add(new Player("O", connectionId));
                    }
                    else
                    {
                        room.Players.Add(new Player("X", connectionId));
                    }
                    room.NumberOfPlayers += 1;
                    room.MoveState = "continue";
                }
            }
            await _playingRoomRepository.EditAsync(room);
            return room;
        }

        public async Task<PlayingRoom> LeaveAsync(Guid id, string connectionId)
        {
            var room = await _playingRoomRepository.GetAsync(id);
            if (room.Players.Exists(x => x.ConnectionId == connectionId))
            {
                var player = room.Players.Find(x => x.ConnectionId == connectionId);
                room.Players.Remove(player);
                room.NumberOfPlayers -= 1;
            }

            if (room.NumberOfPlayers == 0)
            {
                await _playingRoomRepository.DeleteAsync(room);
                room.MoveState = "expired";
            }
            else
            {
                room.MoveState = "waiting";
                await _playingRoomRepository.EditAsync(room);
            }

            return room;
        }

        public async Task<PlayingRoom> PerformMoveAsync(Guid id, string[][] gameState, string identity)
        {
            var room = await _playingRoomRepository.GetAsync(id);
            var status = WinCheck(gameState);
            room.MoveState = status;
            if (status == "win")
            {
                if (identity == "X")
                {
                    room.XScore += 1;
                }
                else
                {
                    room.OScore += 1;
                }
                room.GameState = _cleanBoard;
                if (room.FirstMove == "X")
                {
                    room.FirstMove = "O";
                    room.Turn = "O";
                }
                else
                {
                    room.FirstMove = "X";
                    room.Turn = "X";
                }
            }
            else if (status == "tie")
            {
                room.GameState = _cleanBoard;
                if (room.FirstMove == "X")
                {
                    room.FirstMove = "O";
                    room.Turn = "O";
                }
                else
                {
                    room.FirstMove = "X";
                    room.Turn = "X";
                }
            }
            else
            {
                room.GameState = gameState;
                if (identity == "X")
                {
                    room.Turn = "O";
                }
                else
                {
                    room.Turn = "X";
                }
            }

            await _playingRoomRepository.EditAsync(room);
            return room;
        }

        private string WinCheck(string[][] gameState)
        {
            // "win" / "tie" / "continue"
            if (HorizontalCheck(gameState) || VerticalCheck(gameState) || CrossCheck(gameState))
            {
                return "win";
            }
            else if (TieCheck(gameState))
            {
                return "tie";
            }
            else
            {
                return "continue";
            }
        }

        private bool HorizontalCheck(string[][] gameState)
        {
            if (gameState[0][0] != "●")
            {
                if (gameState[0][0] == gameState[1][0] && gameState[1][0] == gameState[2][0])
                    return true;
            }
            if (gameState[0][1] != "●")
            {
                if (gameState[0][1] == gameState[1][1] && gameState[1][1] == gameState[2][1])
                    return true;
            }
            if (gameState[0][2] != "●")
            {
                if (gameState[0][2] == gameState[1][2] && gameState[1][2] == gameState[2][2])
                    return true;
            }

            return false;
        }

        private bool VerticalCheck(string[][] gameState)
        {
            if (gameState[0][0] != "●")
            {
                if (gameState[0][0] == gameState[0][1] && gameState[0][1] == gameState[0][2])
                    return true;
            }
            if (gameState[1][0] != "●")
            {
                if (gameState[1][0] == gameState[1][1] && gameState[1][1] == gameState[1][2])
                    return true;
            }
            if (gameState[2][0] != "●")
            {
                if (gameState[2][0] == gameState[2][1] && gameState[2][1] == gameState[2][2])
                    return true;
            }

            return false;
        }

        private bool CrossCheck(string[][] gameState)
        {
            if (gameState[0][0] != "●")
            {
                if (gameState[0][0] == gameState[1][1] && gameState[1][1] == gameState[2][2])
                    return true;
            }
            if (gameState[0][2] != "●")
            {
                if (gameState[0][2] == gameState[1][1] && gameState[1][1] == gameState[2][0])
                    return true;
            }

            return false;
        }

        private bool TieCheck(string[][] gameState)
        {
            int count = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameState[i][j] == "●")
                    {
                        count++;
                    }
                }
            }

            if (count == 0)
            {
                return true;
            }

            return false;
        }

        private static string[][] _cleanBoard = new string[][] {
            new[]{"●","●","●"},
            new[]{"●","●","●"},
            new[]{"●","●","●"},
        };
    }
}