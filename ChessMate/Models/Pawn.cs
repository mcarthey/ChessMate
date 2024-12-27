using ChessMate.Services;

namespace ChessMate.Models;

public class Pawn : ChessPiece
{
    public Pawn(string color, (int Row, int Col) position)
        : base(
            color,
            position,
            color == "White" ? "♙" : "♟" // Unicode representation
        )
    { }

    /// <summary>
    /// Validates the pawn's movement based on the given context.
    /// </summary>
    public override bool IsValidMove((int Row, int Col) targetPosition, IGameContext context)
    {
        var board = context.Board;
        var state = context.State;

        int forwardDirection = (Color == "White") ? -1 : 1;
        int rowDifference = targetPosition.Row - Position.Row;
        int colDifference = targetPosition.Col - Position.Col;

        // 1. Single square forward
        if (colDifference == 0 && rowDifference == forwardDirection)
        {
            return board.GetPieceAt(targetPosition) == null; // Must be empty
        }

        // 2. Double square forward (only from starting position)
        int startingRow = (Color == "White") ? 6 : 1;
        if (colDifference == 0 && rowDifference == 2 * forwardDirection && Position.Row == startingRow)
        {
            var middleSquare = (Position.Row + forwardDirection, Position.Col);
            return board.GetPieceAt(targetPosition) == null && board.GetPieceAt(middleSquare) == null;
        }

        // 3. Diagonal capture
        if (Math.Abs(colDifference) == 1 && rowDifference == forwardDirection)
        {
            var targetPiece = board.GetPieceAt(targetPosition);
            if (targetPiece != null && targetPiece.Color != Color)
            {
                return true; // Capture
            }

            // 4. En passant capture
            var enPassantTarget = state.EnPassantTarget;
            if (enPassantTarget.HasValue && enPassantTarget.Value == targetPosition)
            {
                var adjacentPiecePosition = (Position.Row, targetPosition.Col);
                var adjacentPiece = board.GetPieceAt(adjacentPiecePosition);
                if (adjacentPiece is Pawn && adjacentPiece.Color != Color)
                {
                    return true; // Valid en passant
                }
            }
        }

        return false; // Invalid move
    }

    /// <summary>
    /// Handles updates to the state after a successful move.
    /// </summary>
    public override void OnMoved((int Row, int Col) to, IGameContext context)
    {
        var state = context.State;
        var board = context.Board;

        // En passant eligibility
        if (Math.Abs(to.Row - Position.Row) == 2) // Moved two squares
        {
            var enPassantTarget = ((Position.Row + to.Row) / 2, to.Col);
            state.SetEnPassantTarget(enPassantTarget, this);
        }
        else
        {
            state.ResetEnPassantTarget();
        }

        // Promotion check
        int promotionRow = (Color == "White") ? 0 : 7;
        if (to.Row == promotionRow)
        {
            // Promote pawn to queen (default)
            var queen = new Queen(Color, to);
            board.SetPieceAt(to, queen);
        }
        else
        {
            // Update position
            Position = to;
        }

        // Call base method to switch player and update game state
        base.OnMoved(to, context);
    }
}
