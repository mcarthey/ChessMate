namespace ChessMate.Models;

public class Pawn : ChessPiece
{
    public Pawn(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♙" : "♟") { }

    public override bool IsValidMove((int Row, int Col) targetPosition, ChessBoard chessBoard)
    {
        // Check if target position is within board boundaries
        if (!IsWithinBoardBounds(targetPosition))
            return false;

        // Determine forward direction based on color and board orientation
        int forwardDirection = (Color == chessBoard.Orientation) ? -1 : 1;

        int rowDifference = targetPosition.Row - Position.Row;
        int colDifference = targetPosition.Col - Position.Col;

        // Single square forward move
        if (colDifference == 0 && rowDifference == forwardDirection)
        {
            if (IsTargetPositionEmpty(targetPosition, chessBoard))
                return true;
        }

        // Double square forward move on first move
        int startingRow = (Color == "White")
            ? (chessBoard.Orientation == "White" ? 6 : 1)
            : (chessBoard.Orientation == "White" ? 1 : 6);

        if (colDifference == 0 && rowDifference == 2 * forwardDirection && Position.Row == startingRow)
        {
            if (IsPathClear(Position, targetPosition, chessBoard) &&
                IsTargetPositionEmpty(targetPosition, chessBoard))
            {
                return true;
            }
        }

        // Diagonal capture move
        if (Math.Abs(colDifference) == 1 && rowDifference == forwardDirection)
        {
            if (IsOpponentPieceAtPosition(targetPosition, chessBoard))
                return true;
        }

        // Move is invalid
        return false;
    }
}
