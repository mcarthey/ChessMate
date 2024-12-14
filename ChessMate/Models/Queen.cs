namespace ChessMate.Models;

public class Queen : ChessPiece
{
    public Queen(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♕" : "♛") { }

    public override bool IsValidMove((int Row, int Col) targetPosition, ChessPiece[,] board)
    {
        int rowDiff = Math.Abs(targetPosition.Row - Position.Row);
        int colDiff = Math.Abs(targetPosition.Col - Position.Col);
        return rowDiff == colDiff || targetPosition.Row == Position.Row || targetPosition.Col == Position.Col;
    }
}