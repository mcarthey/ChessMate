using ChessMate.Services;

namespace ChessMate.Models;

public class Bishop : ChessPiece
{
    public Bishop(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♗" : "♝")
    {
        MoveDelegate = (targetPosition, board) => ValidateBishopMove(targetPosition, board);
        OnMoveEffect = (to) =>
        {
            // Bishop-specific state updates can be added here if needed
        };
    }

    private bool ValidateBishopMove((int Row, int Col) targetPosition, IChessBoard board)
    {
        int rowDifference = Math.Abs(targetPosition.Row - Position.Row);
        int colDifference = Math.Abs(targetPosition.Col - Position.Col);

        // Bishop moves diagonally (row difference must equal column difference)
        if (rowDifference != colDifference)
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
