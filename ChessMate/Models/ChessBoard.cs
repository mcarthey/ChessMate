namespace ChessMate.Models;

public class ChessBoard : IChessBoard
{
    private ChessPiece[,] _pieces { get; set; } = new ChessPiece[8, 8];

    public void InitializeBoard()
    {
        // Initialize White pieces
        _pieces[7, 0] = new Rook("White", new Position(7, 0));
        _pieces[7, 1] = new Knight("White", new Position(7, 1));
        _pieces[7, 2] = new Bishop("White", new Position(7, 2));
        _pieces[7, 3] = new Queen("White", new Position(7, 3));
        _pieces[7, 4] = new King("White", new Position(7, 4));
        _pieces[7, 5] = new Bishop("White", new Position(7, 5));
        _pieces[7, 6] = new Knight("White", new Position(7, 6));
        _pieces[7, 7] = new Rook("White", new Position(7, 7));

        for (int col = 0; col < 8; col++)
            _pieces[6, col] = new Pawn("White", new Position(6, col));

        // Initialize Black pieces
        _pieces[0, 0] = new Rook("Black", new Position(0, 0));
        _pieces[0, 1] = new Knight("Black", new Position(0, 1));
        _pieces[0, 2] = new Bishop("Black", new Position(0, 2));
        _pieces[0, 3] = new Queen("Black", new Position(0, 3));
        _pieces[0, 4] = new King("Black", new Position(0, 4));
        _pieces[0, 5] = new Bishop("Black", new Position(0, 5));
        _pieces[0, 6] = new Knight("Black", new Position(0, 6));
        _pieces[0, 7] = new Rook("Black", new Position(0, 7));

        for (int col = 0; col < 8; col++)
            _pieces[1, col] = new Pawn("Black", new Position(1, col));

        // Empty squares
        for (int row = 2; row < 6; row++)
        {
            for (int col = 0; col < 8; col++)
                _pieces[row, col] = null;
        }
    }

    public ChessPiece GetPieceAt(Position position)
    {
        if (!IsWithinBoardBounds(position))
            throw new ArgumentOutOfRangeException(nameof(position), "Position is out of bounds.");

        return _pieces[position.Row, position.Col];
    }

    public void SetPieceAt(Position position, ChessPiece piece)
    {
        if (IsWithinBoardBounds(position))
        {
            _pieces[position.Row, position.Col] = piece;
            if (piece != null)
                piece.Position = position;
        }
    }

    public void RemovePieceAt(Position position)
    {
        if (IsWithinBoardBounds(position))
            _pieces[position.Row, position.Col] = null;
    }

    public Position FindKing(string color)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                var piece = _pieces[row, col];
                if (piece is King && piece.Color == color)
                    return new Position(row, col);
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

    public void SetCustomBoard(params (ChessPiece piece, Position position)[] pieces)
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
    private bool IsWithinBoardBounds(Position position)
    {
        return position.Row >= 0 && position.Row < 8 && position.Col >= 0 && position.Col < 8;
    }
}
