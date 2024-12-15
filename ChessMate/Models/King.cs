namespace ChessMate.Models;

public class King : ChessPiece
{
    public King(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♔" : "♚") { }

    public override bool IsValidMove((int Row, int Col) targetPosition, ChessBoard chessBoard)
    {
        if (!IsWithinBoardBounds(targetPosition))
            return false;

        int rowDiff = Math.Abs(targetPosition.Row - Position.Row);
        int colDiff = Math.Abs(targetPosition.Col - Position.Col);

        // King moves one square in any direction
        if (rowDiff <= 1 && colDiff <= 1)
        {
            return IsTargetPositionEmpty(targetPosition, chessBoard) ||
                IsOpponentPieceAtPosition(targetPosition, chessBoard);
        }

        return false;
    }

}
