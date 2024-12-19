using ChessMate.Services;

namespace ChessMate.Models;

public class Queen : ChessPiece
{
    public Queen(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♕" : "♛")
    {
        MoveDelegate = (targetPosition, board) => ValidateQueenMove(targetPosition, board);
        OnMoveEffect = (to) =>
        {
            // Queen-specific state updates can be added here if needed
        };
    }

    private bool ValidateQueenMove((int Row, int Col) targetPosition, IChessBoard board)
    {
        int rowDifference = Math.Abs(targetPosition.Row - Position.Row);
        int colDifference = Math.Abs(targetPosition.Col - Position.Col);

        bool isDiagonalMove = rowDifference == colDifference;
        bool isStraightMove = targetPosition.Row == Position.Row || targetPosition.Col == Position.Col;

        if (isDiagonalMove || isStraightMove)
        {
            // Check if the path is clear
            if (!IsPathClear(Position, targetPosition, board))
                return false;

            // Check if the target position is empty or occupied by an opponent's piece
            var targetPiece = board.GetPieceAt(targetPosition);
            return targetPiece == null || targetPiece.Color != Color;
        }

        return false;
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
