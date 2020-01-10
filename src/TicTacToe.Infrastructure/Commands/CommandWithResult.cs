namespace TicTacToe.Infrastructure.Commands
{
    public class CommandWithResult<T> : ICommand
    {
        public T Result { get; set; }
    }
}