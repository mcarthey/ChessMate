using ChessMate.Services;

namespace ChessMate.Models;

public class Rook : ChessPiece
{
    public Rook(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♖" : "♜")
    {
        MoveDelegate = (targetPosition, board) => ValidateRookMove(targetPosition, board);
        OnMoveEffect = (to) =>
        {
            // Rook-specific state updates can be added here if needed
        };
    }

    private bool ValidateRookMove((int Row, int Col) targetPosition, IChessBoard board)
    {
        int rowDifference = targetPosition.Row - Position.Row;
        int colDifference = targetPosition.Col - Position.Col;

        // Rook moves in straight lines (either row or column must be the same)
        if (rowDifference != 0 && colDifference != 0)
            return false;

        // Check if the path is clear
        if (!IsPathClear(Position, targetPosition, board))
            return false;

        // Check if the target position is empty or occupied by an opponent's piece
        var targetPiece = board.GetPieceAt(targetPosition);
        return targetPiece == null || targetPiece.Color != Color;
    }

    private bool IsPathClear((int Row, int Col) from, (int Row, int Col) to, IChessBoard board)
    {
        int rowStep = Math.Sign(to.Row - from.Row);
        int colStep = Math.Sign(to.Col - from.Col);

        int currentRow = from.Row + rowStep;
        int currentCol = from.Col + colStep;

        while ((currentRow, currentCol) != to)
        {
            if (board.GetPieceAt((currentRow, currentCol)) != null)
                return false;

            currentRow += rowStep;
            currentCol += colStep;
        }

        return true;
    }
}
