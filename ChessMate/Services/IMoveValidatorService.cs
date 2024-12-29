using ChessMate.Models;

namespace ChessMate.Services;

public interface IMoveValidatorService
{
    bool IsValidMove(ChessPiece piece, Position to, IGameContext context);
}