using ChessMate.Models;

namespace ChessMate.Services;

public interface IGameStateEvaluator
{
    bool IsKingInCheck(string color, IGameContext context);
    bool HasLegalMoves(string color, IGameContext context);
    bool WouldMoveCauseSelfCheck(ChessPiece piece, Position from, Position to, IGameContext context);
}