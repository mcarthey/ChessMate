using ChessMate.Models;

namespace ChessMate.Services
{
    public class MoveService : IMoveService
    {
        private readonly IGameContext _context;

        public MoveService(IGameContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool TryMove((int Row, int Col) from, (int Row, int Col) to)
        {
            var board = _context.Board;
            var state = _context.State;

            var piece = board.GetPieceAt(from);
            if (piece == null || piece.Color != state.CurrentPlayer)
                return false; // Invalid move: no piece or wrong player's turn.

            // Validate the move using the piece's method
            if (!piece.IsValidMove(to, _context))
                return false;

            // Simulate the move to check if it results in check
            var targetPiece = board.GetPieceAt(to);

            // Save the original positions
            var originalFrom = piece.Position;
            var originalToPiece = targetPiece;

            // Perform the move
            board.SetPieceAt(to, piece);
            board.RemovePieceAt(from);
            piece.Position = to;

            // Check if the player's own king is in check after the move
            bool isInCheck = IsKingInCheck(state.CurrentPlayer, board);

            if (isInCheck)
            {
                // Undo the move
                board.SetPieceAt(from, piece);
                board.SetPieceAt(to, targetPiece);
                piece.Position = originalFrom;
                return false; // Move leaves king in check, invalid
            }

            // Execute any post-move actions (e.g., pawn promotion)
            piece.OnMoved(to, _context);

            // Update game state
            // Switch player after the move is completed
            state.SwitchPlayer();

            // Check for check or checkmate against the opponent
            string opponentColor = state.CurrentPlayer;
            state.IsCheck = IsKingInCheck(opponentColor, board);
            state.IsCheckmate = state.IsCheck && !HasLegalMoves(opponentColor, board);

            // Optionally, log the move
            state.MoveLog.Add($"{piece.Color} {piece.GetType().Name} from {from} to {to}");

            return true;
        }

        /// <summary>
        /// Determines if the king of the specified color is in check.
        /// </summary>
        private bool IsKingInCheck(string color, IChessBoard board)
        {
            var kingPosition = board.FindKing(color);
            if (kingPosition == (-1, -1))
            {
                // King not found—this should not happen in a standard game
                return false;
            }

            // Check if any opponent piece can attack the king's position
            return board.GetAllPieces()
                .Where(p => p.Color != color)
                .Any(opponent => opponent.IsValidMove(kingPosition, _context));
        }

        /// <summary>
        /// Determines if the player of the specified color has any legal moves.
        /// </summary>
        private bool HasLegalMoves(string color, IChessBoard board)
        {
            // Get all pieces for the given color
            var pieces = board.GetAllPieces().Where(p => p.Color == color);

            foreach (var piece in pieces)
            {
                var originalPosition = piece.Position;

                // Iterate through all possible target positions
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        var targetPosition = (row, col);

                        if (!piece.IsValidMove(targetPosition, _context))
                            continue;

                        // Simulate move
                        var targetPiece = board.GetPieceAt(targetPosition);
                        board.SetPieceAt(targetPosition, piece);
                        board.RemovePieceAt(originalPosition);
                        piece.Position = targetPosition;

                        bool isInCheck = IsKingInCheck(color, board);

                        // Undo move
                        board.SetPieceAt(originalPosition, piece);
                        board.SetPieceAt(targetPosition, targetPiece);
                        piece.Position = originalPosition;

                        if (!isInCheck)
                        {
                            return true; // Found at least one legal move
                        }
                    }
                }
            }

            return false; // No legal moves available
        }
    }
}
