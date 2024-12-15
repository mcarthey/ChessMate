using ChessMate.Utilities;

namespace ChessMate.Services
{
    public class ChessService
    {
        public ChessGame Game { get; private set; }

        public ChessService()
        {
            Game = new ChessGame();
        }

        public bool MovePiece(string fromNotation, string toNotation)
        {
            if (!ChessNotationUtility.IsValidChessNotation(fromNotation) ||
                !ChessNotationUtility.IsValidChessNotation(toNotation))
            {
                Console.WriteLine($"Invalid chess notation: from {fromNotation} to {toNotation}");
                return false;
            }

            bool success = Game.MovePiece(fromNotation, toNotation);

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
