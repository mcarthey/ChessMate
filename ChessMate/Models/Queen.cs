namespace ChessMate.Models;

public class Queen : ChessPiece
{
    public Queen(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♕" : "♛") { }

    public override bool IsValidMove((int Row, int Col) targetPosition, ChessBoard chessBoard)
    {
        if (!IsWithinBoardBounds(targetPosition))
            return false;

        int rowDiff = Math.Abs(targetPosition.Row - Position.Row);
        int colDiff = Math.Abs(targetPosition.Col - Position.Col);

        bool isDiagonalMove = rowDiff == colDiff;
        bool isStraightMove = targetPosition.Row == Position.Row || targetPosition.Col == Position.Col;

        if (isDiagonalMove || isStraightMove)
        {
            if (IsPathClear(Position, targetPosition, chessBoard))
            {
                return IsTargetPositionEmpty(targetPosition, chessBoard) ||
                    IsOpponentPieceAtPosition(targetPosition, chessBoard);
            }
        }

        // Invalid move
        return false;
    }
}
