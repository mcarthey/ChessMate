namespace ChessMate.Models;

public abstract class ChessPiece
{
    public string Color { get; set; } // "White" or "Black"
    public (int Row, int Col) Position { get; set; } // Current position on the board
    public string Representation { get; protected set; } // Unicode for the piece

    protected ChessPiece(string color, (int Row, int Col) position, string representation)
    {
        Color = color;
        Position = position;
        Representation = representation;
    }

    // Abstract method for movement rules
    public abstract bool IsValidMove((int Row, int Col) targetPosition, ChessPiece[,] board);
}