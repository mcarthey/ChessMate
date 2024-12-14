namespace ChessMate.Models;

public class Knight : ChessPiece
{
    public Knight(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♘" : "♞") { }

    public override bool IsValidMove((int Row, int Col) targetPosition, ChessPiece[,] board)
    {
        int rowDiff = Math.Abs(targetPosition.Row - Position.Row);
        int colDiff = Math.Abs(targetPosition.Col - Position.Col);
        return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
    }
}