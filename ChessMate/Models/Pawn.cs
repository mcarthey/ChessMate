namespace ChessMate.Models;

public class Pawn : ChessPiece
{
    public Pawn(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♙" : "♟") { }

    public override bool IsValidMove((int Row, int Col) targetPosition, ChessPiece[,] board)
    {
        int direction = Color == "White" ? -1 : 1;

        // Moving forward
        if (targetPosition.Col == Position.Col &&
            targetPosition.Row == Position.Row + direction &&
            board[targetPosition.Row, targetPosition.Col] == null)
        {
            return true;
        }

        // Add logic for capturing diagonally, first move, etc.
        return false;
    }
}