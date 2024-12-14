namespace ChessMate.Services
{
    public class ChessGame
    {
        private string[,] board = new string[8, 8];

        public ChessGame()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // White pieces
            board[0, 0] = "♖"; // Rook
            board[0, 1] = "♘"; // Knight
            board[0, 2] = "♗"; // Bishop
            board[0, 3] = "♕"; // Queen
            board[0, 4] = "♔"; // King
            board[0, 5] = "♗"; // Bishop
            board[0, 6] = "♘"; // Knight
            board[0, 7] = "♖"; // Rook

            for (int col = 0; col < 8; col++)
            {
                board[1, col] = "♙"; // Pawns
            }

            // Black pieces
            board[7, 0] = "♜"; // Rook
            board[7, 1] = "♞"; // Knight
            board[7, 2] = "♝"; // Bishop
            board[7, 3] = "♛"; // Queen
            board[7, 4] = "♚"; // King
            board[7, 5] = "♝"; // Bishop
            board[7, 6] = "♞"; // Knight
            board[7, 7] = "♜"; // Rook

            for (int col = 0; col < 8; col++)
            {
                board[6, col] = "♟"; // Pawns
            }

            // Empty squares
            for (int row = 2; row < 6; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    board[row, col] = "";
                }
            }
        }

        public string GetPiece(int row, int col)
        {
            return board[row, col];
        }

        public bool MovePiece(int startRow, int startCol, int endRow, int endCol)
        {
            // Basic validation: ensure there's a piece at the start location
            string piece = board[startRow, startCol];
            if (string.IsNullOrEmpty(piece))
            {
                return false;
            }

            // Move the piece to the new location
            board[endRow, endCol] = piece;
            board[startRow, startCol] = ""; // Clear the starting square
            return true;
        }
    }
}
