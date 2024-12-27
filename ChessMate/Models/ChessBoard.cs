namespace ChessMate.Models;

public class ChessBoard : IChessBoard
{
    private ChessPiece[,] _pieces { get; set; } = new ChessPiece[8, 8];

    public void InitializeBoard()
    {
        // Initialize White pieces
        _pieces[7, 0] = new Rook("White", (7, 0));
        _pieces[7, 1] = new Knight("White", (7, 1));
        _pieces[7, 2] = new Bishop("White", (7, 2));
        _pieces[7, 3] = new Queen("White", (7, 3));
        _pieces[7, 4] = new King("White", (7, 4));
        _pieces[7, 5] = new Bishop("White", (7, 5));
        _pieces[7, 6] = new Knight("White", (7, 6));
        _pieces[7, 7] = new Rook("White", (7, 7));
        for (int col = 0; col < 8; col++)
            _pieces[6, col] = new Pawn("White", (6, col));

        // Initialize Black pieces
        _pieces[0, 0] = new Rook("Black", (0, 0));
        _pieces[0, 1] = new Knight("Black", (0, 1));
        _pieces[0, 2] = new Bishop("Black", (0, 2));
        _pieces[0, 3] = new Queen("Black", (0, 3));
        _pieces[0, 4] = new King("Black", (0, 4));
        _pieces[0, 5] = new Bishop("Black", (0, 5));
        _pieces[0, 6] = new Knight("Black", (0, 6));
        _pieces[0, 7] = new Rook("Black", (0, 7));
        for (int col = 0; col < 8; col++)
            _pieces[1, col] = new Pawn("Black", (1, col));

        // Empty squares
        for (int row = 2; row < 6; row++)
        {
            for (int col = 0; col < 8; col++)
                _pieces[row, col] = null;
        }
    }

    public ChessPiece GetPieceAt((int Row, int Col) position)
    {
        if (!IsWithinBoardBounds(position))
            throw new ArgumentOutOfRangeException(nameof(position), "Position is out of bounds.");

        return _pieces[position.Row, position.Col];
    }

    public void SetPieceAt((int Row, int Col) position, ChessPiece piece)
    {
        if (IsWithinBoardBounds(position))
            _pieces[position.Row, position.Col] = piece;
    }

    public void RemovePieceAt((int Row, int Col) position)
    {
        if (IsWithinBoardBounds(position))
            _pieces[position.Row, position.Col] = null;
    }

    public (int Row, int Col) FindKing(string color)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                var piece = _pieces[row, col];
                if (piece is King && piece.Color == color)
                    return (row, col);
            }
        }
        throw new InvalidOperationException($"No {color} king found!");
    }

    public IEnumerable<ChessPiece> GetAllPieces()
    {
        foreach (var piece in _pieces)
        {
            if (piece != null)
                yield return piece;
        }
    }

    public void SetCustomBoard(params (ChessPiece piece, (int Row, int Col) position)[] pieces)
    {
        _pieces = new ChessPiece[8, 8]; // Clear the board

        foreach (var (piece, position) in pieces)
        {
            SetPieceAt(position, piece);
        }
    }

    /// <summary>
    /// Checks if a given position is within the valid boundaries of the chessboard.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns>True if the position is within the board, false otherwise.</returns>
    private bool IsWithinBoardBounds((int Row, int Col) position)
    {
        return position.Row >= 0 && position.Row < 8 && position.Col >= 0 && position.Col < 8;
    }
}
