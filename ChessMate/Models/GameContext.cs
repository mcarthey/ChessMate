using ChessMate.Services;

namespace ChessMate.Models
{
    public class GameContext : IGameContext
    {
        public IChessBoard Board { get; }
        public IStateService State { get; }

        public GameContext(IChessBoard board, IStateService state)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            State = state ?? throw new ArgumentNullException(nameof(state));
        }

        /// <summary>
        /// Resets the game to its initial state.
        /// </summary>
        public void ResetGame()
        {
            Board.InitializeBoard();
            State.ResetState();
        }
    }

    public interface IGameContext
    {
        IChessBoard Board { get; }
        IStateService State { get; }
        void ResetGame();
    }
}
