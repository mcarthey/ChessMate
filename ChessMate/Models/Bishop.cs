namespace ChessMate.Models;

public class Bishop : ChessPiece
{
    public Bishop(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♗" : "♝") { }

    public override bool IsValidMove((int Row, int Col) targetPosition, ChessBoard chessBoard)
    {
        if (!IsWithinBoardBounds(targetPosition))
            return false;

        int rowDiff = Math.Abs(targetPosition.Row - Position.Row);
        int colDiff = Math.Abs(targetPosition.Col - Position.Col);

        // Check for diagonal movement
        if (rowDiff == colDiff)
        {
            if (IsPathClear(Position, targetPosition, chessBoard))
            {
                return IsTargetPositionEmpty(targetPosition, chessBoard) ||
                    IsOpponentPieceAtPosition(targetPosition, chessBoard);
            }
        }

        return false;
    }

}
