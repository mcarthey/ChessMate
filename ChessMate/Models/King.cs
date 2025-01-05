namespace ChessMate.Models
{
    /// <summary>
    /// Represents the King piece in the chess game.
    /// </summary>
    public class King : ChessPiece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="King"/> class.
        /// </summary>
        /// <param name="color">The color of the King ("White" or "Black").</param>
        /// <param name="position">The current position of the King on the board.</param>
        public King(string color, Position position)
            : base(color, position, color == "White" ? "♔" : "♚")
        {
        }

        /// <summary>
        /// Determines whether the move to the target position is valid.
        /// Ensures the King does not move into a square that is under attack.
        /// </summary>
        /// <param name="targetPosition">The position the King intends to move to.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <param name="stateService">The current state service handling game states and attacks.</param>
        /// <returns><c>true</c> if the move is valid; otherwise, <c>false</c>.</returns>
        public override bool IsValidMove(Position targetPosition, IChessBoard board, IStateService stateService)
        {
            // Calculate the difference in rows and columns
            int rowDiff = Math.Abs(targetPosition.Row - this.Position.Row);
            int colDiff = Math.Abs(targetPosition.Col - this.Position.Col);

            // Standard King move: one square in any direction
            bool isStandardMove = (rowDiff <= 1) && (colDiff <= 1);

            // Castling logic can be added here if implemented

            if (!isStandardMove)
            {
                return false;
            }

            // Ensure the target square is either empty or occupied by an opponent's piece
            ChessPiece? targetPiece = board.GetPieceAt(targetPosition);
            if (targetPiece != null && targetPiece.Color == this.Color)
            {
                return false; // Cannot capture own piece
            }

            // Determine opposing color
            string opposingColor = this.Color == "White" ? "Black" : "White";

            // Check if the target square is under attack by opposing pieces
            bool isUnderAttack = opposingColor == "White"
                ? stateService.GetWhiteAttackers(targetPosition).Any()
                : stateService.GetBlackAttackers(targetPosition).Any();

            if (isUnderAttack)
            {
                return false; // Cannot move into check
            }

            // Additional checks such as moving into check indirectly can be implemented here

            return true;
        }

        /// <summary>
        /// Handles updates to the King's state after a successful move.
        /// </summary>
        /// <param name="from">The original position of the King.</param>
        /// <param name="to">The new position of the King.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <param name="stateService">The current state service handling game states and attacks.</param>
        /// <param name="capturedPiece">The piece captured during the move, if any.</param>
        public override void OnMoved(Position from, Position to, IChessBoard board, IStateService stateService, ChessPiece? capturedPiece = null)
        {
            // Since the King has moved, castling rights are lost
            if (this.Color == "White")
            {
                stateService.WhiteKingMoved = true;
            }
            else
            {
                stateService.BlackKingMoved = true;
            }

            // Additional post-move logic (e.g., handling castling) can be added here
        }

        /// <summary>
        /// Handles any validation errors that occur during move validation.
        /// </summary>
        /// <param name="targetPosition">The position that caused the validation error.</param>
        /// <param name="ex">The exception that was thrown.</param>
        public override void HandleValidationError(Position targetPosition, Exception ex)
        {
            // Log the error or implement additional error handling as needed
            Console.WriteLine($"Validation Error for King moving to {targetPosition}: {ex.Message}");
        }
    }
}
