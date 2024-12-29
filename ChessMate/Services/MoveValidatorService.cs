using ChessMate.Models;

namespace ChessMate.Services;

public class MoveValidatorService : IMoveValidatorService
{
    public bool IsValidMove(ChessPiece piece, Position to, IGameContext context)
    {
        return piece?.IsValidMove(to, context) ?? false;
    }
}