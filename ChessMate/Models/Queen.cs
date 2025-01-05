using ChessMate.Services;
using ChessMate.Utilities;

namespace ChessMate.Models;

public class Queen : ChessPiece
{
    public Queen(string color, Position position)
        : base(
            color,
            position,
            color == "White" ? "♕" : "♛" // Unicode representation
        )
    { }

    /// <summary>
    /// Validates the queen's movement based on the given context.
    /// </summary>
    public override bool IsValidMove(Position targetPosition, IChessBoard board, IStateService state)
    {
        int rowDiff = Math.Abs(targetPosition.Row - Position.Row);
        int colDiff = Math.Abs(targetPosition.Col - Position.Col);

        bool isDiagonalMove = rowDiff == colDiff;
        bool isStraightMove = Position.Row == targetPosition.Row || Position.Col == targetPosition.Col;

        if (!(isDiagonalMove || isStraightMove))
            return false;

        // Check if the path is clear
        if (!MoveValidationHelper.IsPathClear(Position, targetPosition, board))
            return false;

        // Check if the target square is empty or occupied by an opponent's piece
        var targetPiece = board.GetPieceAt(targetPosition);
        return targetPiece == null || targetPiece.Color != Color;
    }

    // Optional: Override OnMoved if queen has specific post-move behavior
    public override void OnMoved(Position from, Position to, IChessBoard board, IStateService state, ChessPiece capturedPiece = null)
    {
        base.OnMoved(from, to, board, state);
        // Add any bishop-specific logic here if needed
    }

}
