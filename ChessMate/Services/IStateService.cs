// File: ChessMate/Services/IStateService.cs

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

        HashSet<Position> WhiteAttacks { get; }
        HashSet<Position> BlackAttacks { get; }

        void SwitchPlayer();
        void SetPlayer(string player);

        void SetEnPassantTarget(Position target, ChessPiece piece);
        void ResetEnPassantTarget();

        void ResetState();

        void UpdateAttackMaps(IGameContext context);
        void UpdateGameStateAfterMove(ChessPiece piece, Position from, Position to, IGameContext context);

        bool WouldMoveCauseSelfCheck(ChessPiece piece, Position from, Position to, IGameContext context);
        bool IsKingInCheck(string color, IGameContext context);
        bool HasLegalMoves(string color, IGameContext context);
    }
}
