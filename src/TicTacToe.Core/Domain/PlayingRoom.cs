using System;
using System.Collections.Generic;

namespace TicTacToe.Core.Domain
{
    public class PlayingRoom
    {
        public Guid RoomId { get; set; }
        public int NumberOfPlayers { get; set; }
        public string[][] GameState { get; set; }
        public string FirstMove { get; set; }
        public string Turn { get; set; }
        public int XScore { get; set; }
        public int OScore { get; set; }
        public string MoveState { get; set; }
        public List<Player> Players { get; set; }

        public PlayingRoom()
        {
            this.RoomId = Guid.NewGuid();
            this.NumberOfPlayers = 0;
            this.GameState = new string[][]{
                new[]{"●","●","●"},
                new[]{"●","●","●"},
                new[]{"●","●","●"},
            };
            this.FirstMove = "X";
            this.Turn = "X";
            this.XScore = 0;
            this.OScore = 0;
            this.MoveState = "waiting";
            this.Players = new List<Player>();
        }

        public PlayingRoom(Guid roomId, int numberOfPlayers, string[][] gameState, string firstMove, string turn, int xScore, int oScore, string moveState, List<Player> players)
        {
            this.RoomId = roomId;
            this.NumberOfPlayers = numberOfPlayers;
            this.GameState = gameState;
            this.FirstMove = firstMove;
            this.Turn = turn;
            this.XScore = xScore;
            this.OScore = oScore;
            this.MoveState = moveState;
            this.Players = players;
        }
    }
}