using ChessMate.Models;

namespace ChessMate.Utilities;

public static class MoveValidationHelper
{
    public static bool IsPathClear(Position from, Position to, IChessBoard board)
    {
        int rowStep = Math.Sign(to.Row - from.Row);
        int colStep = Math.Sign(to.Col - from.Col);

        int currentRow = from.Row + rowStep;
        int currentCol = from.Col + colStep;

        while (new Position(currentRow, currentCol) != to)
        {
            var currentPosition = new Position(currentRow, currentCol);
            if (board.GetPieceAt(currentPosition) != null)
                return false;

            currentRow += rowStep;
            currentCol += colStep;
        }

        return true;
    }
}
