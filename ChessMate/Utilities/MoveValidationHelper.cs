using ChessMate.Models;

namespace ChessMate.Utilities;

public static class MoveValidationHelper
{
    public static bool IsPathClear((int Row, int Col) from, (int Row, int Col) to, IChessBoard board)
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
