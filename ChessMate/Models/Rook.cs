namespace ChessMate.Models;

public class Rook : ChessPiece
{
    public Rook(string color, (int Row, int Col) position)
        : base(color, position, color == "White" ? "♖" : "♜") { }

    public override bool IsValidMove((int Row, int Col) targetPosition, ChessBoard chessBoard)
    {
        if (!IsWithinBoardBounds(targetPosition))
            return false;

        if (targetPosition.Row == Position.Row || targetPosition.Col == Position.Col)
        {
            if (IsPathClear(Position, targetPosition, chessBoard))
            {
                return IsTargetPositionEmpty(targetPosition, chessBoard) ||
                    IsOpponentPieceAtPosition(targetPosition, chessBoard);
            }
        }

        return false;
    }

}
