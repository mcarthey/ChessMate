using ChessMate.Models;
using ChessMate.Utilities;

namespace ChessMate.Services
{
    public class ChessGame
    {
        public ChessBoard Board { get; private set; }

        public ChessGame(string orientation = "White")
        {
            Board = new ChessBoard(orientation);
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

            bool success = Board.MovePiece(from, to);

            if (success)
            {
                Console.WriteLine($"Moved piece from {fromNotation} to {toNotation}");
            }
            else
            {
                Console.WriteLine($"Failed to move piece from {fromNotation} to {toNotation}");
            }

            return success;
        }
    }
}
