namespace ChessMate.Models;

public class ChessBoard : IChessBoard
{
    public ChessPiece[,] ChessPieces { get; set; }
    public string Orientation { get; set; } // "White" or "Black"
    public string CurrentPlayer { get; private set; } = "White";
    public string InvalidMoveReason { get; set; } = string.Empty;

    // Game state tracking
    public bool IsCheck { get; private set; }
    public bool IsCheckmate { get; private set; }
    public (int Row, int Col)? EnPassantTarget { get; private set; } // Track en passant target
    private bool whiteKingMoved = false, blackKingMoved = false;
    private bool whiteRookKingSideMoved = false, whiteRookQueenSideMoved = false;
    private bool blackRookKingSideMoved = false, blackRookQueenSideMoved = false;

    public ChessBoard(string orientation = "White")
    {
        ChessPieces = new ChessPiece[8, 8];
        Orientation = orientation;
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        // Initialize white pieces
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

        // Initialize black pieces
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
            for (int col = 0; col < 8; col++)
                ChessPieces[row, col] = null;
    }

    public bool MovePiece((int Row, int Col) from, (int Row, int Col) to)
    {
        InvalidMoveReason = string.Empty; // Reset the reason for each move

        var piece = ChessPieces[from.Row, from.Col];
        if (piece == null)
        {
            InvalidMoveReason = "No piece at the starting position.";
            return false;
        }

        if (piece.Color != CurrentPlayer)
        {
            InvalidMoveReason = "It's not your turn.";
            return false;
        }

        // En passant handling
        if (piece is Pawn && IsEnPassantMove(from, to))
        {
            PerformEnPassant(from, to);
            return true;
        }

        // Perform the move temporarily
        var targetPiece = ChessPieces[to.Row, to.Col];
        ChessPieces[to.Row, to.Col] = piece;
        ChessPieces[from.Row, from.Col] = null;
        piece.Position = to;

        bool moveValid = !IsKingInCheck(CurrentPlayer);

        if (!moveValid)
        {
            // Revert invalid move
            ChessPieces[from.Row, from.Col] = piece;
            ChessPieces[to.Row, to.Col] = targetPiece;
            piece.Position = from;
            InvalidMoveReason = "Move would put or leave the king in check.";
            return false;
        }

        UpdateGameState(piece, from, to);
        return true;
    }

    private void UpdateGameState(ChessPiece piece, (int Row, int Col) from, (int Row, int Col) to)
    {
        // Track en passant target: Only for pawns moving two squares forward
        if (piece is Pawn && Math.Abs(from.Row - to.Row) == 2)
        {
            // The en passant target is the square the pawn "jumped over"
            EnPassantTarget = ((from.Row + to.Row) / 2, from.Col);
        }
        else
        {
            // Clear en passant target after any non-pawn two-square move
            EnPassantTarget = null;
        }

        // Update castling flags: Mark king or rooks as having moved
        if (piece is King)
        {
            if (piece.Color == "White") whiteKingMoved = true;
            else blackKingMoved = true;
        }
        else if (piece is Rook)
        {
            if (piece.Color == "White")
            {
                if (from == (7, 0)) whiteRookQueenSideMoved = true;
                if (from == (7, 7)) whiteRookKingSideMoved = true;
            }
            else if (piece.Color == "Black")
            {
                if (from == (0, 0)) blackRookQueenSideMoved = true;
                if (from == (0, 7)) blackRookKingSideMoved = true;
            }
        }

        // Update check and checkmate states for the opponent
        var opponentColor = piece.Color == "White" ? "Black" : "White";
        IsCheck = IsKingInCheck(opponentColor);
        IsCheckmate = IsCheck && !HasLegalMoves(opponentColor);

        // Switch the current player
        CurrentPlayer = opponentColor;
    }

    private bool IsEnPassantMove((int Row, int Col) from, (int Row, int Col) to)
    {
        return EnPassantTarget.HasValue && to == EnPassantTarget.Value;
    }

    private void PerformEnPassant((int Row, int Col) from, (int Row, int Col) to)
    {
        ChessPieces[to.Row, to.Col] = ChessPieces[from.Row, from.Col];
        ChessPieces[from.Row, from.Col] = null;
        ChessPieces[to.Row + (CurrentPlayer == "White" ? 1 : -1), to.Col] = null;
    }

    public bool IsKingInCheck(string color)
    {
        var kingPosition = FindKing(color);
        return ChessPieces.Cast<ChessPiece?>()
                          .Where(p => p != null && p.Color != color)
                          .Any(opponent => opponent!.IsValidMove(kingPosition, this));
    }

    private (int Row, int Col) FindKing(string color)
    {
        for (int row = 0; row < 8; row++)
            for (int col = 0; col < 8; col++)
                if (ChessPieces[row, col] is King king && king.Color == color)
                    return (row, col);

        throw new InvalidOperationException("King not found!");
    }

    private bool HasLegalMoves(string color)
    {
        return ChessPieces.Cast<ChessPiece?>()
                          .Where(p => p != null && p.Color == color)
                          .Any(piece =>
                              Enumerable.Range(0, 8)
                                        .SelectMany(r => Enumerable.Range(0, 8).Select(c => (r, c)))
                                        .Any(target => piece!.IsValidMove(target, this)));
    }
}


