namespace ChessMate.Services
{
    public class ChessService
    {
        public ChessGame Game { get; private set; }

        public ChessService()
        {
            Game = new ChessGame();
        }

        public bool MovePiece((int Row, int Col) from, (int Row, int Col) to)
        {
            bool success = Game.MovePiece(from, to);
            if (success)
            {
                Console.WriteLine($"Moved piece from {from} to {to}");
            }
            else
            {
                Console.WriteLine($"Failed to move piece from {from} to {to}");
            }
            return success;
        }
    }
}
