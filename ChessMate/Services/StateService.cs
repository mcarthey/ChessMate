using ChessMate.Models;

namespace ChessMate.Services;

public class StateService : IStateService
{
    public string CurrentPlayer { get; private set; } = "White";
    public bool IsCheck { get; set; }
    public bool IsCheckmate { get; set; }
    public Position? EnPassantTarget { get; private set; } // Track en passant target

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

    /// <summary>
    /// Sets the en passant target position and ensures it applies only to pawns.
    /// </summary>
    public void SetEnPassantTarget(Position target, ChessPiece piece)
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

    /// <summary>
    /// Updates the game state after a move.
    /// </summary>
    public void UpdateGameStateAfterMove(ChessPiece piece, Position from, Position to, IGameContext context, IGameStateEvaluator gameStateEvaluator)
    {
        // Log the move
        MoveLog.Add($"{piece.Color} {piece.GetType().Name} from {from} to {to}");

        // Switch player after the move is completed
        SwitchPlayer();

        // Update check and checkmate status
        string opponentColor = CurrentPlayer;
        IsCheck = gameStateEvaluator.IsKingInCheck(opponentColor, context);
        IsCheckmate = IsCheck && !gameStateEvaluator.HasLegalMoves(opponentColor, context);
    }
}

