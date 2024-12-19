using ChessMate.Models;

namespace ChessMate.Services
{
    public interface IGameEngine
    {
        IChessBoard Board { get; }
        IStateService State { get; }
        bool ProcessMove(string fromNotation, string toNotation);
        string GetCurrentPlayer();
    }
}
