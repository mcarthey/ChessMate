namespace ChessMate.Models;

public abstract class ChessPiece
{
    public string Color { get; set; } // "White" or "Black"
    public (int Row, int Col) Position { get; set; } // Current position on the board
    public string Representation { get; protected set; } // Unicode representation
    public Func<(int Row, int Col), IChessBoard, bool> MoveDelegate { get; protected set; } // Delegate for movement logic
    public Action<(int Row, int Col)> OnMoveEffect { get; set; } // Delegate for state updates

    protected ChessPiece(string color, (int Row, int Col) position, string representation)
    {
        Color = color;
        Position = position;
        Representation = representation;
    }

    public bool IsValidMove((int Row, int Col) targetPosition, IChessBoard board)
    {
        return MoveDelegate(targetPosition, board);
    }

    // Called after a move is completed
    public virtual void OnMove() { }
}
