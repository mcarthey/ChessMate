using ChessMate.Models;

namespace ChessMate.Services
{
    public class ChessGame
    {
        public ChessPiece[,] Board { get; private set; } = new ChessPiece[8, 8];

        public ChessGame()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // White pieces
            Board[0, 0] = new Rook("White", (0, 0));
            Board[0, 1] = new Knight("White", (0, 1));
            Board[0, 2] = new Bishop("White", (0, 2));
            Board[0, 3] = new Queen("White", (0, 3));
            Board[0, 4] = new King("White", (0, 4));
            Board[0, 5] = new Bishop("White", (0, 5));
            Board[0, 6] = new Knight("White", (0, 6));
            Board[0, 7] = new Rook("White", (0, 7));
            for (int col = 0; col < 8; col++)
                Board[1, col] = new Pawn("White", (1, col));

            // Black pieces
            Board[7, 0] = new Rook("Black", (7, 0));
            Board[7, 1] = new Knight("Black", (7, 1));
            Board[7, 2] = new Bishop("Black", (7, 2));
            Board[7, 3] = new Queen("Black", (7, 3));
            Board[7, 4] = new King("Black", (7, 4));
            Board[7, 5] = new Bishop("Black", (7, 5));
            Board[7, 6] = new Knight("Black", (7, 6));
            Board[7, 7] = new Rook("Black", (7, 7));
            for (int col = 0; col < 8; col++)
                Board[6, col] = new Pawn("Black", (6, col));
        }

        public string GetPiece(int row, int col)
        {
            var piece = Board[row, col];
            return piece != null ? piece.Representation : ""; // Return the piece's symbol or empty string
        }

        public bool MovePiece((int Row, int Col) from, (int Row, int Col) to)
        {
            var piece = Board[from.Row, from.Col];
            if (piece == null || !piece.IsValidMove(to, Board))
            {
                return false; // Invalid move
            }

            // Perform the move
            Board[to.Row, to.Col] = piece;
            Board[from.Row, from.Col] = null;
            piece.Position = to;
            return true;
        }
    }
}
