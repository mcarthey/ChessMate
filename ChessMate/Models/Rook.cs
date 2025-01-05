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
    public override bool IsValidMove(Position targetPosition, IChessBoard board, IStateService state)
    {
        // Rook moves in straight lines along rows or columns
        bool sameRow = Position.Row == targetPosition.Row;
        bool sameCol = Position.Col == targetPosition.Col;

        if (!(sameRow || sameCol))
            return false;

        // Check if the path is clear
        if (!MoveValidationHelper.IsPathClear(Position, targetPosition, board)) {
            Console.WriteLine($"The position {Position} is not clear");
            return false;
        }

        // Check if the target square is empty or occupied by an opponent's piece
        var targetPiece = board.GetPieceAt(targetPosition);
        return targetPiece == null || targetPiece.Color != Color;
    }

    // Optional: Override OnMoved if rook has specific post-move behavior
    public override void OnMoved(Position from, Position to, IChessBoard board, IStateService state, ChessPiece capturedPiece = null)
    {
        base.OnMoved(from, to, board, state);
        // Add any bishop-specific logic here if needed
    }

}
