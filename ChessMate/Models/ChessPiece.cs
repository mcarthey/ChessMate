using ChessMate.Services;

namespace ChessMate.Models;

/// <summary>
/// Base class for all chess pieces, handling validation.
/// </summary>
public abstract class ChessPiece
{
    // Core properties
    public string Color { get; set; } // "White" or "Black"
    public (int Row, int Col) Position { get; set; } // Current position on the board
    public string Representation { get; protected set; } // Unicode representation

    /// <summary>
    /// Initializes a new chess piece.
    /// </summary>
    protected ChessPiece(string color, (int Row, int Col) position, string representation)
    {
        Color = color ?? throw new ArgumentNullException(nameof(color));
        Position = position;
        Representation = representation ?? throw new ArgumentNullException(nameof(representation));
    }

    /// <summary>
    /// Validates whether the move is legal based on rules and game context.
    /// Must be overridden in derived classes.
    /// </summary>
    public abstract bool IsValidMove((int Row, int Col) targetPosition, IGameContext context);

    /// <summary>
    /// Handles updates to the piece after a successful move.
    /// Can be overridden in derived classes.
    /// </summary>
    public virtual void OnMoved((int Row, int Col) to, IGameContext context)
    {
        // Default implementation does nothing
        // Derived classes can override to implement specific behavior (e.g., pawn promotion)
    }

    /// <summary>
    /// Handles errors during move validation.
    /// </summary>
    protected virtual void HandleValidationError((int Row, int Col) targetPosition, Exception ex)
    {
        string errorMessage = ex switch
        {
            ArgumentOutOfRangeException => $"Invalid move: {targetPosition} is out of bounds.",
            _ => $"Error validating move for {Representation} at {Position}: {ex.Message}"
        };
        Console.WriteLine(errorMessage);
    }
}
