            // White pieces
            Board[0, 0] = "♖"; // Rook
            Board[0, 1] = "♘"; // Knight
            Board[0, 2] = "♗"; // Bishop
            Board[0, 3] = "♕"; // Queen
            Board[0, 4] = "♔"; // King
            Board[0, 5] = "♗"; // Bishop
            Board[0, 6] = "♘"; // Knight
            Board[0, 7] = "♖"; // Rook

            for (int col = 0; col < 8; col++)
            {
                Board[1, col] = "♙"; // Pawns
            }

            // Black pieces
            Board[7, 0] = "♜"; // Rook
            Board[7, 1] = "♞"; // Knight
            Board[7, 2] = "♝"; // Bishop
            Board[7, 3] = "♛"; // Queen
            Board[7, 4] = "♚"; // King
            Board[7, 5] = "♝"; // Bishop
            Board[7, 6] = "♞"; // Knight
            Board[7, 7] = "♜"; // Rook

            for (int col = 0; col < 8; col++)
            {
                Board[6, col] = "♟"; // Pawns
            }