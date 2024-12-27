using ChessMate.Models;

namespace ChessMate.Services
{
    public interface IStateService
    {
        string CurrentPlayer { get; }
        bool IsCheck { get; set; }
        bool IsCheckmate { get; set; }
        List<string> MoveLog { get; }

        bool WhiteKingMoved { get; set; }
        bool BlackKingMoved { get; set; }
        bool WhiteRookKingSideMoved { get; set; }
        bool WhiteRookQueenSideMoved { get; set; }
        bool BlackRookKingSideMoved { get; set; }
        bool BlackRookQueenSideMoved { get; set; }

        (int Row, int Col)? EnPassantTarget { get; }

        void SwitchPlayer();
        void SetPlayer(string player);

        void SetEnPassantTarget((int Row, int Col) target, ChessPiece piece);
        void ResetEnPassantTarget();

        void ResetState();
    }
}
