using ChessMate.Services;
using ChessMate.Utilities;

namespace ChessMate.Models;

public class Rook : ChessPiece
{
    public Rook(string color, Position position)
        : base(
            color,
            position,
            color == "White" ? "♖" : "♜" // Unicode representation
        )
    { }

    /// <summary>
    /// Validates the rook's movement based on the given context.
    /// </summary>
    public override bool IsValidMove(Position targetPosition, IGameContext context)
    {
        var board = context.Board;

        // Rook moves in straight lines along rows or columns
        bool sameRow = Position.Row == targetPosition.Row;
        bool sameCol = Position.Col == targetPosition.Col;

        if (!(sameRow || sameCol))
            return false;

        // Check if the path is clear
        if (!MoveValidationHelper.IsPathClear(Position, targetPosition, board))
            return false;

        // Check if the target square is empty or occupied by an opponent's piece
        var targetPiece = board.GetPieceAt(targetPosition);
        return targetPiece == null || targetPiece.Color != Color;
    }

    // Optional: Override OnMoved if rook has specific post-move behavior
    public override void OnMoved(Position to, IGameContext context)
    {
        base.OnMoved(to, context);
        // Add any rook-specific logic here if needed (e.g., updating castling rights)
    }
}
