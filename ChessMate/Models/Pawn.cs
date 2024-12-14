namespace ChessMate.Models;

public class Pawn : ChessPiece
{
    public Pawn(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♙" : "♟") { }

    public override bool IsValidMove((int Row, int Col) targetPosition, ChessPiece[,] board)
    {
        int direction = Color == "White" ? 1 : -1; // White moves down, Black moves up
        int rowDiff = targetPosition.Row - Position.Row;
        int colDiff = targetPosition.Col - Position.Col;

        // Move forward one square
        if (colDiff == 0 && rowDiff == direction && board[targetPosition.Row, targetPosition.Col] == null)
        {
            return true; // Valid single forward move
        }

        // First move: allow two squares forward
        if (colDiff == 0 && rowDiff == 2 * direction && Position.Row == (Color == "White" ? 1 : 6) &&
            board[Position.Row + direction, Position.Col] == null &&
            board[targetPosition.Row, targetPosition.Col] == null)
        {
            return true; // Valid two-square move on the first move
        }

        // Invalid move
        return false;
    }

}