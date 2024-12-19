using ChessMate.Services;

namespace ChessMate.Models;

public class King : ChessPiece
{
    public King(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♔" : "♚")
    {
        MoveDelegate = (targetPosition, board) => ValidateKingMove(targetPosition, board);
        OnMoveEffect = (to) =>
        {
            // King-specific state updates can be added here if needed
        };
    }

    private bool ValidateKingMove((int Row, int Col) targetPosition, IChessBoard board)
    {
        int rowDifference = Math.Abs(targetPosition.Row - Position.Row);
        int colDifference = Math.Abs(targetPosition.Col - Position.Col);

        // King moves one square in any direction
        if (rowDifference <= 1 && colDifference <= 1)
        {
            // Check if the target position is empty or occupied by an opponent's piece
            var targetPiece = board.GetPieceAt(targetPosition);
            return targetPiece == null || targetPiece.Color != Color;
        }

        return false;
    }
}
