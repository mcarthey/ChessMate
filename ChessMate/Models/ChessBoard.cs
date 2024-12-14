namespace ChessMate.Models;

public class ChessBoard
{
    public ChessPiece[,] Board { get; set; }

    public ChessBoard()
    {
        Board = new ChessPiece[8, 8];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        // Add pawns
        for (int col = 0; col < 8; col++)
        {
            Board[1, col] = new Pawn("Black", (1, col));
            Board[6, col] = new Pawn("White", (6, col));
        }
        // Add other pieces (Rooks, Knights, etc.)
    }

    public bool MovePiece((int Row, int Col) from, (int Row, int Col) to)
    {
        var piece = Board[from.Row, from.Col];
        if (piece != null && piece.IsValidMove(to, Board))
        {
            Board[to.Row, to.Col] = piece;
            Board[from.Row, from.Col] = null;
            piece.Position = to;
            return true;
        }
        return false;
    }
}