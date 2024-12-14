namespace ChessMate.Services
{
    public class ChessService
    {
        public ChessGame Game { get; private set; }

        public ChessService()
        {
            Game = new ChessGame();
        }

        public bool MovePiece(int startRow, int startCol, int endRow, int endCol)
        {
            // Call the game logic to move the piece
            bool success = Game.MovePiece(startRow, startCol, endRow, endCol);
            if (success)
            {
                Console.WriteLine($"Moved piece from ({startRow},{startCol}) to ({endRow},{endCol})");
            }
            else
            {
                Console.WriteLine($"Failed to move piece from ({startRow},{startCol}) to ({endRow},{endCol})");
            }
            return success;
        }

        // Optionally, you can still use MakeMove(string start, string end) for notation-based moves.
        public void MakeMove(string start, string end)
        {
            // If you have algebraic notation, convert start/end to row/col and call MovePiece.
            Console.WriteLine($"Move from {start} to {end}");
        }
    }
}
