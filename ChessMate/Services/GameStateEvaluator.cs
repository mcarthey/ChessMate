using ChessMate.Models;

namespace ChessMate.Services;

public class GameStateEvaluator : IGameStateEvaluator
{
    public bool WouldMoveCauseSelfCheck(ChessPiece piece, Position from, Position to, IGameContext context)
    {
        var board = context.Board;
        var originalPosition = piece.Position;
        var targetPiece = board.GetPieceAt(to);

        // Simulate the move
        board.RemovePieceAt(from);
        board.SetPieceAt(to, piece);
        piece.Position = to;

        // Check if the player's own king is in check after the move
        bool isInCheck = IsKingInCheck(piece.Color, context);

        // Undo the move
        board.SetPieceAt(from, piece);
        board.SetPieceAt(to, targetPiece);
        piece.Position = originalPosition;

        return isInCheck;
    }

    /// <summary>
    /// Determines if the king of the specified color is in check.
    /// </summary>
    public bool IsKingInCheck(string color, IGameContext context)
    {
        var board = context.Board;
        var kingPosition = board.FindKing(color);

        // No need to check for null since FindKing will throw an exception if the king isn't found

        return board.GetAllPieces()
            .Where(p => p.Color != color)
            .Any(opponent => opponent.IsValidMove(kingPosition, context));
    }

    /// <summary>
    /// Determines if the player of the specified color has any legal moves.
    /// </summary>
    public bool HasLegalMoves(string color, IGameContext context)
    {
        var board = context.Board;

        // Get all pieces for the given color
        var pieces = board.GetAllPieces().Where(p => p.Color == color);

        foreach (var piece in pieces)
        {
            var originalPosition = piece.Position;

            // Iterate through all possible target positions
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var targetPosition = new Position(row, col);

                    if (!piece.IsValidMove(targetPosition, context))
                        continue;

                    // Simulate move
                    var targetPiece = board.GetPieceAt(targetPosition);
                    board.SetPieceAt(targetPosition, piece);
                    board.RemovePieceAt(originalPosition);
                    piece.Position = targetPosition;

                    bool isInCheck = IsKingInCheck(color, context);

                    // Undo move
                    board.SetPieceAt(originalPosition, piece);
                    board.SetPieceAt(targetPosition, targetPiece);
                    piece.Position = originalPosition;

                    if (!isInCheck)
                    {
                        return true; // Found at least one legal move
                    }
                }
            }
        }

        return false; // No legal moves available
    }
}
