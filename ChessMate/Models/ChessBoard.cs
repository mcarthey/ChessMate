namespace ChessMate.Models;

public class ChessBoard
{
    public ChessPiece[,] ChessPieces { get; set; }
    public string Orientation { get; set; } // "White" or "Black"

    public ChessBoard(string orientation = "White")
    {
        ChessPieces = new ChessPiece[8, 8];
        Orientation = orientation;
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        // White pieces at the bottom (rows 7 and 6)
        ChessPieces[7, 0] = new Rook("White", (7, 0));
        ChessPieces[7, 1] = new Knight("White", (7, 1));
        ChessPieces[7, 2] = new Bishop("White", (7, 2));
        ChessPieces[7, 3] = new Queen("White", (7, 3));
        ChessPieces[7, 4] = new King("White", (7, 4));
        ChessPieces[7, 5] = new Bishop("White", (7, 5));
        ChessPieces[7, 6] = new Knight("White", (7, 6));
        ChessPieces[7, 7] = new Rook("White", (7, 7));
        for (int col = 0; col < 8; col++)
        {
            ChessPieces[6, col] = new Pawn("White", (6, col));
        }

        // Black pieces at the top (rows 0 and 1)
        ChessPieces[0, 0] = new Rook("Black", (0, 0));
        ChessPieces[0, 1] = new Knight("Black", (0, 1));
        ChessPieces[0, 2] = new Bishop("Black", (0, 2));
        ChessPieces[0, 3] = new Queen("Black", (0, 3));
        ChessPieces[0, 4] = new King("Black", (0, 4));
        ChessPieces[0, 5] = new Bishop("Black", (0, 5));
        ChessPieces[0, 6] = new Knight("Black", (0, 6));
        ChessPieces[0, 7] = new Rook("Black", (0, 7));
        for (int col = 0; col < 8; col++)
        {
            ChessPieces[1, col] = new Pawn("Black", (1, col));
        }

        // Empty squares
        for (int row = 2; row < 6; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                ChessPieces[row, col] = null; // Ensure empty squares are explicitly set to null
            }
        }
    }

    public bool MovePiece((int Row, int Col) from, (int Row, int Col) to)
    {
        var piece = ChessPieces[from.Row, from.Col];
        if (piece == null)
        {
            Console.WriteLine($"No piece at position ({from.Row}, {from.Col})");
            return false;
        }

        if (piece.IsValidMove(to, this))
        {
            ChessPieces[to.Row, to.Col] = piece;
            ChessPieces[from.Row, from.Col] = null;
            piece.Position = to;
            return true;
        }
        return false;
    }
}
