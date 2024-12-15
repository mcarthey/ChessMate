namespace ChessMate.Models;

public abstract class ChessPiece
{
    public string Color { get; set; } // "White" or "Black"
    public (int Row, int Col) Position { get; set; } // Current position on the board
    public string Representation { get; protected set; } // Unicode for the piece

    protected ChessPiece(string color, (int Row, int Col) position, string representation)
    {
        Color = color;
        Position = position;
        Representation = representation;
    }

    // Abstract method for movement rules
    public abstract bool IsValidMove((int Row, int Col) targetPosition, ChessBoard chessBoard);

    // Common method to check if the target position is within board boundaries
    protected bool IsWithinBoardBounds((int Row, int Col) position)
    {
        return position.Row >= 0 && position.Row < 8 && position.Col >= 0 && position.Col < 8;
    }

    // Common method to check if the target position is empty
    protected bool IsTargetPositionEmpty((int Row, int Col) position, ChessBoard chessBoard)
    {
        return chessBoard.ChessPieces[position.Row, position.Col] == null;
    }

    // Common method to check if the target position has an opponent's piece
    protected bool IsOpponentPieceAtPosition((int Row, int Col) position, ChessBoard chessBoard)
    {
        ChessPiece targetPiece = chessBoard.ChessPieces[position.Row, position.Col];
        return targetPiece != null && targetPiece.Color != this.Color;
    }

    // Common method to check if the path between two positions is clear (for linear movement)
    protected bool IsPathClear((int Row, int Col) start, (int Row, int Col) end, ChessBoard chessBoard)
    {
        int rowDirection = Math.Sign(end.Row - start.Row);
        int colDirection = Math.Sign(end.Col - start.Col);

        int currentRow = start.Row + rowDirection;
        int currentCol = start.Col + colDirection;

        while (currentRow != end.Row || currentCol != end.Col)
        {
            if (chessBoard.ChessPieces[currentRow, currentCol] != null)
                return false; // Path is blocked

            currentRow += rowDirection;
            currentCol += colDirection;
        }

        return true; // Path is clear
    }
}
