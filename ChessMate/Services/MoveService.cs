// File: ChessMate/Services/MoveService.cs

using ChessMate.Models;

namespace ChessMate.Services
{
    public class MoveService : IMoveService
    {
        private readonly IGameContext _context;
        private readonly IMoveValidatorService _moveValidator;

        public MoveService(
            IGameContext context,
            IMoveValidatorService moveValidator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _moveValidator = moveValidator ?? throw new ArgumentNullException(nameof(moveValidator));
        }

        public bool TryMove(Position from, Position to)
        {
            var board = _context.Board;
            var state = _context.State;

            var piece = board.GetPieceAt(from);
            if (piece == null || piece.Color != state.CurrentPlayer)
                return false; // Invalid move: no piece or wrong player's turn.

            // Validate the move using the piece's method
            bool isValidMove;
            try
            {
                isValidMove = _moveValidator.IsValidMove(piece, to, _context);
            }
            catch
            {
                return false;
            }

            if (!isValidMove)
                return false;

            // Check if the move would result in self-check
            if (state.WouldMoveCauseSelfCheck(piece, from, to, _context))
                return false;

            // Perform the move
            ExecuteMove(piece, from, to);

            // Update game state
            state.UpdateGameStateAfterMove(piece, from, to, _context);

            return true;
        }

        private void ExecuteMove(ChessPiece piece, Position from, Position to)
        {
            var board = _context.Board;

            board.RemovePieceAt(from);
            board.SetPieceAt(to, piece);
            piece.Position = to;

            // Execute any post-move actions (e.g., pawn promotion)
            piece.OnMoved(to, _context);
        }
    }
}





