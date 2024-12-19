namespace ChessMate.Models;

public class ChessBoard : IChessBoard
{
    public ChessPiece[,] ChessPieces { get; private set; } = new ChessPiece[8, 8];

    public ChessBoard()
    {
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        // Initialize White pieces
        ChessPieces[7, 0] = new Rook("White", (7, 0));
        ChessPieces[7, 1] = new Knight("White", (7, 1));
        ChessPieces[7, 2] = new Bishop("White", (7, 2));
        ChessPieces[7, 3] = new Queen("White", (7, 3));
        ChessPieces[7, 4] = new King("White", (7, 4));
        ChessPieces[7, 5] = new Bishop("White", (7, 5));
        ChessPieces[7, 6] = new Knight("White", (7, 6));
        ChessPieces[7, 7] = new Rook("White", (7, 7));
        for (int col = 0; col < 8; col++)
            ChessPieces[6, col] = new Pawn("White", (6, col));

        // Initialize Black pieces
        ChessPieces[0, 0] = new Rook("Black", (0, 0));
        ChessPieces[0, 1] = new Knight("Black", (0, 1));
        ChessPieces[0, 2] = new Bishop("Black", (0, 2));
        ChessPieces[0, 3] = new Queen("Black", (0, 3));
        ChessPieces[0, 4] = new King("Black", (0, 4));
        ChessPieces[0, 5] = new Bishop("Black", (0, 5));
        ChessPieces[0, 6] = new Knight("Black", (0, 6));
        ChessPieces[0, 7] = new Rook("Black", (0, 7));
        for (int col = 0; col < 8; col++)
            ChessPieces[1, col] = new Pawn("Black", (1, col));

        // Empty squares
        for (int row = 2; row < 6; row++)
        {
            for (int col = 0; col < 8; col++)
                ChessPieces[row, col] = null;
        }
    }

    public ChessPiece GetPieceAt((int Row, int Col) position)
    {
        return ChessPieces[position.Row, position.Col];
    }

    public void SetPieceAt((int Row, int Col) position, ChessPiece piece)
    {
        ChessPieces[position.Row, position.Col] = piece;
    }

    public void RemovePieceAt((int Row, int Col) position)
    {
        ChessPieces[position.Row, position.Col] = null;
    }

    public (int Row, int Col) FindKing(string color)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                var piece = ChessPieces[row, col];
                if (piece is King && piece.Color == color)
                    return (row, col);
            }
        }
        throw new InvalidOperationException($"No {color} king found!");
    }

    public IEnumerable<ChessPiece> GetAllPieces()
    {
        foreach (var piece in ChessPieces)
        {
            if (piece != null)
                yield return piece;
        }
    }

    public void SetCustomBoard(params (ChessPiece piece, (int Row, int Col) position)[] pieces)
    {
        ChessPieces = new ChessPiece[8, 8]; // Clear the board

        foreach (var (piece, position) in pieces)
        {
            SetPieceAt(position, piece);
        }
    }
}

