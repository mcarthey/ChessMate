using ChessMate.Models;

namespace ChessMate.Services;

public class StateService : IStateService
{
    public string CurrentPlayer { get; private set; } = "White";
    public bool IsCheck { get; set; }
    public bool IsCheckmate { get; set; }
    public (int Row, int Col)? EnPassantTarget { get; private set; } // Track en passant target

    // Castling flags
    public bool WhiteKingMoved { get; set; }
    public bool BlackKingMoved { get; set; }
    public bool WhiteRookKingSideMoved { get; set; }
    public bool WhiteRookQueenSideMoved { get; set; }
    public bool BlackRookKingSideMoved { get; set; }
    public bool BlackRookQueenSideMoved { get; set; }

    public List<string> MoveLog { get; private set; } = new List<string>();

    /// <summary>
    /// Switches the current player after a move.
    /// </summary>
    public void SwitchPlayer()
    {
        CurrentPlayer = (CurrentPlayer == "White") ? "Black" : "White";
    }

    /// <summary>
    /// Allows setting the current player explicitly (for testing).
    /// </summary>
    /// <param name="player">"White" or "Black"</param>
    public void SetPlayer(string player)
    {
        if (player != "White" && player != "Black")
        {
            throw new ArgumentException("Invalid player. Must be 'White' or 'Black'.");
        }

        CurrentPlayer = player;
    }

    // Removed IsKingInCheck and HasLegalMoves methods

    /// <summary>
    /// Sets the en passant target position and ensures it applies only to pawns.
    /// </summary>
    public void SetEnPassantTarget((int Row, int Col) target, ChessPiece piece)
    {
        if (piece is Pawn)
        {
            EnPassantTarget = target;
        }
    }

    /// <summary>
    /// Resets the en passant target when no longer applicable.
    /// </summary>
    public void ResetEnPassantTarget()
    {
        EnPassantTarget = null;
    }

    public void ResetState()
    {
        CurrentPlayer = "White";
        IsCheck = false;
        IsCheckmate = false;
        EnPassantTarget = null;

        // Reset castling flags
        WhiteKingMoved = false;
        BlackKingMoved = false;
        WhiteRookKingSideMoved = false;
        WhiteRookQueenSideMoved = false;
        BlackRookKingSideMoved = false;
        BlackRookQueenSideMoved = false;

        MoveLog.Clear();
    }
}
