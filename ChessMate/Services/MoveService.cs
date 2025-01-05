// File: ChessMate/Services/MoveService.cs

using ChessMate.Hubs;
using ChessMate.Models;
using Microsoft.AspNetCore.SignalR;
using System;

namespace ChessMate.Services
{
    /// <summary>
    /// Service responsible for handling chess moves.
    /// </summary>
    public class MoveService : IMoveService
    {
        private readonly IChessBoard _board;
        private readonly IStateService _stateService;
        private readonly IMoveValidatorService _moveValidator;
        private readonly IHubContext<ChessHub> _hubContext; // For SignalR

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveService"/> class.
        /// </summary>
        /// <param name="board">The chess board.</param>
        /// <param name="stateService">The game state service.</param>
        /// <param name="moveValidator">The move validator service.</param>
        public MoveService(
            IChessBoard board,
            IStateService stateService,
            IMoveValidatorService moveValidator,
            IHubContext<ChessHub> hubContext) // Injected HubContext
        {
            _board = board ?? throw new ArgumentNullException(nameof(board));
            _stateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
            _moveValidator = moveValidator ?? throw new ArgumentNullException(nameof(moveValidator));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        /// <summary>
        /// Attempts to move a piece from one position to another.
        /// </summary>
        /// <param name="from">The starting position.</param>
        /// <param name="to">The target position.</param>
        /// <returns>True if the move was successful; otherwise, false.</returns>
        public bool TryMove(Position from, Position to)
        {
            var piece = _board.GetPieceAt(from);
            if (piece == null || piece.Color != _stateService.CurrentPlayer)
                return false; // Invalid move: no piece or wrong player's turn.

            // Validate the move using the move validator service
            bool isValidMove;
            try
            {
                isValidMove = _moveValidator.IsValidMove(piece, to, _board, _stateService);
            }
            catch
            {
                return false;
            }

            if (!isValidMove)
                return false;

            // Check if the move would result in self-check
            if (_stateService.WouldMoveCauseSelfCheck(piece, from, to))
                return false;

            // Perform the move
            ExecuteMove(piece, from, to);

            // Update game state
            _stateService.UpdateGameStateAfterMove(piece, from, to);

            // Optional: Broadcast the move via SignalR for multiplayer
            // This will only execute if SignalR is enabled in the configuration
            // You can check the configuration here or manage it externally

            return true;
        }

        /// <summary>
        /// Executes the move on the chess board.
        /// </summary>
        /// <param name="piece">The chess piece to move.</param>
        /// <param name="from">The starting position.</param>
        /// <param name="to">The target position.</param>
        private void ExecuteMove(ChessPiece piece, Position from, Position to)
        {
            var capturedPiece = _board.GetPieceAt(to);

            _board.RemovePieceAt(from);
            _board.SetPieceAt(to, piece);

            // Execute any post-move actions (e.g., pawn promotion) and handle captures
            piece.OnMoved(from, to, _board, _stateService, capturedPiece);

            // If a capture occurred, register it
            if (capturedPiece != null)
            {
                _stateService.RegisterCapture(capturedPiece);
            }
        }

        // Optional: Method to broadcast moves via SignalR
        public async Task BroadcastMoveAsync(Position from, Position to)
        {
            string fromNotation = from.ToChessNotation();
            string toNotation = to.ToChessNotation();
            await _hubContext.Clients.All.SendAsync("ReceiveMove", fromNotation, toNotation);
        }
    }
}
