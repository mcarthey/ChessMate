using ChessMate.Services;

namespace ChessMate.Models;

public class Knight : ChessPiece
{
    public Knight(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♘" : "♞")
    {
        MoveDelegate = (targetPosition, board) => ValidateKnightMove(targetPosition, board);
        OnMoveEffect = (to) =>
        {
            // Knight-specific state updates can be added here if needed
        };
    }

    private bool ValidateKnightMove((int Row, int Col) targetPosition, IChessBoard board)
    {
        int rowDifference = Math.Abs(targetPosition.Row - Position.Row);
        int colDifference = Math.Abs(targetPosition.Col - Position.Col);

        // Knight moves in an L-shape (2 squares in one direction and 1 square in the other)
        if ((rowDifference == 2 && colDifference == 1) || (rowDifference == 1 && colDifference == 2))
        {
            // Knights can jump over other pieces, so we only need to check the target position
            var targetPiece = board.GetPieceAt(targetPosition);
            return targetPiece == null || targetPiece.Color != Color;
        }

        return false;
    }
}
