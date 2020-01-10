namespace TicTacToe.Core.Domain
{
    public class Player
    {
        public string Identity { get; set; }
        public string ConnectionId { get; set; }

        public Player()
        {
            this.Identity = "";
            this.ConnectionId = "";
        }

        public Player(string identity, string connectionId)
        {
            this.Identity = identity;
            this.ConnectionId = connectionId;
        }
    }
}