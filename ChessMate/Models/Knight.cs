using ChessMate.Services;

namespace ChessMate.Models;

public class Knight : ChessPiece
{
    public Knight(string color, Position position)
        : base(
            color,
            position,
            color == "White" ? "♘" : "♞" // Unicode representation
        )
    { }

    /// <summary>
    /// Validates the knight's movement based on the given context.
    /// </summary>
    public override bool IsValidMove(Position targetPosition, IGameContext context)
    {
        int rowDiff = Math.Abs(targetPosition.Row - Position.Row);
        int colDiff = Math.Abs(targetPosition.Col - Position.Col);

        // Knight moves in an L-shape: 2 by 1 or 1 by 2
        bool isValidLShape = (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);

        if (!isValidLShape)
            return false;

        var board = context.Board;
        var targetPiece = board.GetPieceAt(targetPosition);

        // Knights can jump over pieces, so we only need to check the target square
        return targetPiece == null || targetPiece.Color != Color;
    }

    // Optional: Override OnMoved if knight has specific post-move behavior
    public override void OnMoved(Position to, IGameContext context)
    {
        base.OnMoved(to, context);
        // Add any knight-specific logic here if needed
    }
}
