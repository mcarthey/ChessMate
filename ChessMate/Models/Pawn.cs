namespace ChessMate.Models;

public class Pawn : ChessPiece
{
    public bool HasMovedTwoSquares { get; private set; }
    public (int Row, int Col)? EnPassantTarget { get; private set; }

    public Pawn(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♙" : "♟")
    {
        MoveDelegate = (targetPosition, board) => ValidatePawnMove(targetPosition, board);
        OnMoveEffect = (to) =>
        {
            // If the pawn moved two squares, set the flag and en passant target
            HasMovedTwoSquares = Math.Abs(to.Row - Position.Row) == 2;
            if (HasMovedTwoSquares)
            {
                EnPassantTarget = ((Position.Row + to.Row) / 2, Position.Col);
            }
            else
            {
                EnPassantTarget = null;
            }
        };
    }

    private bool ValidatePawnMove((int Row, int Col) targetPosition, IChessBoard board)
    {
        int forwardDirection = (Color == "White") ? -1 : 1;
        int rowDifference = targetPosition.Row - Position.Row;
        int colDifference = targetPosition.Col - Position.Col;

        // Single square forward
        if (colDifference == 0 && rowDifference == forwardDirection)
            return board.GetPieceAt(targetPosition) == null;

        // Double square forward on first move
        int startingRow = (Color == "White") ? 6 : 1;
        if (colDifference == 0 && rowDifference == 2 * forwardDirection && Position.Row == startingRow)
        {
            var middleSquare = (Position.Row + forwardDirection, Position.Col);
            return board.GetPieceAt(targetPosition) == null && board.GetPieceAt(middleSquare) == null;
        }

        // Diagonal capture
        if (Math.Abs(colDifference) == 1 && rowDifference == forwardDirection)
        {
            var targetPiece = board.GetPieceAt(targetPosition);
            return targetPiece != null && targetPiece.Color != Color;
        }

        // En passant capture
        if (Math.Abs(colDifference) == 1 && rowDifference == forwardDirection && EnPassantTarget.HasValue)
        {
            var enPassantPiece = board.GetPieceAt((Position.Row, targetPosition.Col));
            if (enPassantPiece is Pawn && enPassantPiece.Color != Color && enPassantPiece.Position == EnPassantTarget.Value)
            {
                board.RemovePieceAt(enPassantPiece.Position);
                return true;
            }
        }

        return false; // Invalid move
    }
}



