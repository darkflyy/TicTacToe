using AutoMapper;
using TicTacToe.Core.Domain;
using TicTacToe.Infrastructure.DTO;

namespace TicTacToe.Infrastructure.Automapper
{
    public static class AutomapperConfig
    {
        public static IMapper Initialize() =>
        new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PlayingRoom, PlayingRoomDto>();
        }).CreateMapper();
    }
}