using ChessMate.Models;
using ChessMate.Utilities;

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
            {
                Board[1, col] = new Pawn("White", (1, col));
            }

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
            {
                Board[6, col] = new Pawn("Black", (6, col));
            }

            // Empty squares
            for (int row = 2; row < 6; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Board[row, col] = null; // Ensure empty squares are explicitly set to null
                }
            }
        }

        public bool MovePiece(string fromNotation, string toNotation)
        {
            if (!ChessNotationUtility.IsValidChessNotation(fromNotation) ||
                !ChessNotationUtility.IsValidChessNotation(toNotation))
            {
                Console.WriteLine("Invalid chess notation provided.");
                return false;
            }

            var from = ChessNotationUtility.FromChessNotation(fromNotation);
            var to = ChessNotationUtility.FromChessNotation(toNotation);

            var piece = Board[from.Row, from.Col];
            if (piece == null)
            {
                Console.WriteLine($"No piece at position {fromNotation}");
                return false;
            }

            bool isValid = piece.IsValidMove(to, Board);
            Console.WriteLine($"{piece?.Representation} move from {fromNotation} to {toNotation}: {(isValid ? "Valid" : "Invalid")}");

            if (!isValid)
                return false;

            Board[to.Row, to.Col] = piece;
            Board[from.Row, from.Col] = null;
            piece.Position = to;
            return true;
        }




        [System.Diagnostics.Conditional("DEBUG")]
        public void PrintBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var piece = Board[row, col];
                    Console.Write(piece?.Representation ?? ".");
                }
                Console.WriteLine(); // Move to the next row
            }
            Console.WriteLine(); // Add a blank line for spacing
        }

    }
}
