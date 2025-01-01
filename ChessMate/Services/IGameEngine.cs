using ChessMate.Models;

namespace ChessMate.Services
{
    public interface IGameEngine
    {
        IGameContext Context { get; }
        IMoveService Move { get; }
        IChessBoard Board { get; }
        IStateService State { get; }

        bool ProcessMove(Position from, Position to);

        string GetCurrentPlayer();
    }
}
