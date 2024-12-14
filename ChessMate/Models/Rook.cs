namespace ChessMate.Models;

public class Rook : ChessPiece
{
    public Rook(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♖" : "♜") { }

    public override bool IsValidMove((int Row, int Col) targetPosition, ChessPiece[,] board)
    {
        // Rook can move any number of spaces horizontally or vertically.
        return (targetPosition.Row == Position.Row || targetPosition.Col == Position.Col);
    }
}