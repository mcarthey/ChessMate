using ChessMate.Services;
using ChessMate.Utilities;

namespace ChessMate.Models;

public class King : ChessPiece
{
    public King(string color, (int Row, int Col) position)
        : base(
            color,
            position,
            color == "White" ? "♔" : "♚" // Unicode representation
        )
    { }

    /// <summary>
    /// Validates the king's movement based on the given context.
    /// </summary>
    public override bool IsValidMove((int Row, int Col) targetPosition, IGameContext context)
    {
        var board = context.Board;
        var state = context.State;

        int rowDiff = Math.Abs(targetPosition.Row - Position.Row);
        int colDiff = Math.Abs(targetPosition.Col - Position.Col);

        // Normal king movement: one square in any direction
        bool isOneSquareMove = rowDiff <= 1 && colDiff <= 1 && (rowDiff + colDiff) > 0;

        if (isOneSquareMove)
        {
            var targetPiece = board.GetPieceAt(targetPosition);
            return targetPiece == null || targetPiece.Color != Color;
        }

        // Castling logic (simplified)
        if (rowDiff == 0 && colDiff == 2)
        {
            // Implement castling rules here if necessary
            // Check if the king or rook has moved, path is clear, not in check, etc.
            // For now, we'll return false to indicate castling is not implemented
            return false;
        }

        return false; // Invalid move
    }

    // Optional: Override OnMoved if king has specific post-move behavior
    public override void OnMoved((int Row, int Col) to, IGameContext context)
    {
        base.OnMoved(to, context);
        // Add any king-specific logic here if needed (e.g., updating castling rights)
    }

    /// <summary>
    /// Handles errors during move validation specific to the king.
    /// </summary>
    protected override void HandleValidationError((int Row, int Col) targetPosition, Exception ex)
    {
        // Additional error handling if needed
        base.HandleValidationError(targetPosition, ex);
    }
}
