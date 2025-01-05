using ChessMate.Services;
using ChessMate.Utilities;

namespace ChessMate.Models;

public class Bishop : ChessPiece
{
    public Bishop(string color, Position position)
        : base(
            color,
            position,
            color == "White" ? "♗" : "♝" // Unicode representation
        )
    { }

    /// <summary>
    /// Validates the bishop's movement based on the given context.
    /// </summary>
    public override bool IsValidMove(Position targetPosition, IChessBoard board, IStateService stateService)
    {
        // 1. Validate diagonal movement
        int rowDifference = Math.Abs(targetPosition.Row - Position.Row);
        int colDifference = Math.Abs(targetPosition.Col - Position.Col);
        if (rowDifference != colDifference)
            return false; // Bishops move diagonally

        // 2. Ensure the path is clear
        if (!MoveValidationHelper.IsPathClear(Position, targetPosition, board))
            return false; // Path must be clear

        // 3. Ensure target square is valid (empty or opponent's piece)
        var targetPiece = board.GetPieceAt(targetPosition);
        return targetPiece == null || targetPiece.Color != Color;
    }

    // Optional: Override OnMoved if bishop has specific post-move behavior
    public override void OnMoved(Position to, IChessBoard board, IStateService stateService)
    {
        base.OnMoved(to, board, stateService);
        // Add any bishop-specific logic here if needed
    }
}
