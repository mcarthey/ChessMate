using ChessMate.Models;

namespace ChessMate.Services
{
    public interface IStateService
    {
        string CurrentPlayer { get; }
        bool IsCheck { get; set; }
        bool IsCheckmate { get; set; }
        void SwitchPlayer();
        bool IsKingInCheck(string color, IChessBoard board);
        bool HasLegalMoves(string color, IChessBoard board);
        List<string> MoveLog { get; }
    }
}
