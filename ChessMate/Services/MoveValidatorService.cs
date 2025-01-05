// File: ChessMate/Services/MoveValidatorService.cs

using ChessMate.Models;

namespace ChessMate.Services;

/// <summary>
/// Service responsible for validating chess moves.
/// </summary>
public class MoveValidatorService : IMoveValidatorService
{
    /// <summary>
    /// Determines if a move is valid for a given chess piece.
    /// </summary>
    /// <param name="piece">The chess piece to move.</param>
    /// <param name="to">The target position.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <param name="stateService">The current game state.</param>
    /// <returns>True if the move is valid; otherwise, false.</returns>
    public bool IsValidMove(ChessPiece piece, Position to, IChessBoard board, IStateService stateService)
    {
        return piece?.IsValidMove(to, board, stateService) ?? false;
    }
}
