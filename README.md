# ChessMate

ChessMate is a C# project implementing the core mechanics of a chess game with a graphical user interface built using Blazor. It enforces standard chess rules and is designed with extensibility in mind, making it a great foundation for advanced chess features. Future plans include adding an AI mentor to assist players with move suggestions and strategy insights.

---

## Features

- **Core Game Mechanics**: Implements the chessboard and standard rules for piece movement.
- **Interactive GUI**: Built using Blazor, providing a visually appealing and responsive interface for playing chess.
- **Object-Oriented Design**: Uses abstract and derived classes for chess pieces, enabling clear and modular logic.
- **Validation of Moves**: Enforces movement rules for pawns, rooks, bishops, knights, queens, and kings, including edge cases.
- **Test Coverage**: Extensive unit tests using xUnit to validate the functionality of the chess game mechanics.
- **Future-Proof**: Designed to support additional features like AI mentoring and advanced gameplay modes.

---

## Implementation Details

### Project Structure

- **`ChessMate.Models`**:
  - Contains the abstract `ChessPiece` class and its derived classes (`Pawn`, `Rook`, `Knight`, `Bishop`, `Queen`, and `King`).
  - Each class defines the movement logic for its respective chess piece in the `IsValidMove` method.

- **`ChessMate.Services`**:
  - Houses the `ChessGame` class, which represents the chessboard and game state.
  - Includes the `MovePiece` method to facilitate piece movement and enforce rules.

- **`ChessMate.Pages`**:
  - Contains `ChessBoard.razor`, which implements the GUI using Blazor.
  - Enables users to interact with the chessboard by clicking on pieces and selecting their target positions.
  - Highlights selected squares and displays the game's current state.

- **Unit Tests**:
  - Implemented using xUnit.
  - `ChessGameTests` includes tests for initializing the board, validating legal and illegal moves, and ensuring special rules like two-square pawn moves on the first turn are respected.

---

### Key Components

#### GUI (Blazor)
- **ChessBoard.razor**:
  - Renders the 8x8 chessboard dynamically using a table structure.
  - Displays Unicode chess piece symbols (`♙`, `♖`, etc.) for visual clarity.
  - Handles user interactions like selecting a piece and moving it to a valid position.

#### ChessPiece (Abstract)
Defines the base functionality for all chess pieces, including:
- `string Representation`: A Unicode symbol representing the piece.
- `string Color`: The color of the piece (`"White"` or `"Black"`).
- `virtual bool IsValidMove(...)`: Abstract method implemented by derived classes to enforce piece-specific movement rules.

#### Derived Piece Classes
Each piece (e.g., `Pawn`, `Rook`, `Queen`) overrides `IsValidMove` to handle its specific movement logic.

#### ChessGame
- **Properties**:
  - `ChessPiece[,] Board`: An 8x8 array representing the chessboard and its pieces.
- **Methods**:
  - `void InitializeBoard()`: Sets up the chessboard for a new game with all pieces in their starting positions.
  - `bool MovePiece((int Row, int Col) from, (int Row, int Col) to)`: Validates and performs a move if it's legal.

#### Unit Tests
Key test cases include:
- **Board Initialization**:
  - Verifies all pieces are correctly placed at the start of the game.
- **Valid Moves**:
  - Confirms legal moves for all pieces, including two-square pawn moves on the first turn.
- **Invalid Moves**:
  - Ensures invalid moves (e.g., a pawn moving backward) are correctly rejected.

---

## Future Enhancements

- **AI Mentor**:
  - Provide real-time move suggestions and strategic advice to players.
  - Help users learn and improve their chess skills through analysis and guidance.

- **Advanced Gameplay Features**:
  - Add support for special moves like castling, en passant, and pawn promotion.
  - Implement check and checkmate detection.
  - Introduce turn-based play for multiplayer or against the AI.

- **Improved GUI**:
  - Add drag-and-drop functionality for piece movement.
  - Highlight valid moves for the selected piece.
  - Display move history and current game status (e.g., check, checkmate).

---

## Technologies Used

- **C#**: Core language for the project.
- **Blazor**: Framework for the GUI, enabling dynamic and responsive interactions.
- **xUnit**: Unit testing framework to ensure reliability and correctness of game mechanics.

---

Feel free to reach out or contribute ideas to expand ChessMate further!