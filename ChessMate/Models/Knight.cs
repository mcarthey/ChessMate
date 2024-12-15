namespace ChessMate.Models;

public class Knight : ChessPiece
{
    public Knight(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♘" : "♞") { }

    public override bool IsValidMove((int Row, int Col) targetPosition, ChessBoard chessBoard)
    {
        if (!IsWithinBoardBounds(targetPosition))
            return false;

        int rowDiff = Math.Abs(targetPosition.Row - Position.Row);
        int colDiff = Math.Abs(targetPosition.Col - Position.Col);

        // Moves in an L-shape
        if ((rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2))
        {
            // Knights can jump over other pieces, so we only need to check the target position
            return IsTargetPositionEmpty(targetPosition, chessBoard) ||
                IsOpponentPieceAtPosition(targetPosition, chessBoard);
        }

        // Invalid move
        return false;
    }
}
