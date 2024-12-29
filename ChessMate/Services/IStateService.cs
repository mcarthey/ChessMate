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

        Position? EnPassantTarget { get; }

        void SwitchPlayer();
        void SetPlayer(string player);

        void SetEnPassantTarget(Position target, ChessPiece piece);
        void ResetEnPassantTarget();

        void ResetState();

        void UpdateGameStateAfterMove(ChessPiece piece, Position from, Position to, IGameContext context, IGameStateEvaluator gameStateEvaluator);
    }
}
