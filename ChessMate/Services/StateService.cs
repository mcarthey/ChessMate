// File: ChessMate/Services/StateService.cs

using ChessMate.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessMate.Services
{
    public class StateService : IStateService, IDisposable
    {
        private readonly IChessBoard _board;

        public string CurrentPlayer { get; private set; } = "White";
        public virtual bool IsCheck { get; set; }
        public virtual bool IsCheckmate { get; set; }
        public Position? EnPassantTarget { get; private set; }

        // Castling flags
        public bool WhiteKingMoved { get; set; }
        public bool BlackKingMoved { get; set; }
        public bool WhiteRookKingSideMoved { get; set; }
        public bool WhiteRookQueenSideMoved { get; set; }
        public bool BlackRookKingSideMoved { get; set; }
        public bool BlackRookQueenSideMoved { get; set; }

        public List<string> MoveLog { get; private set; } = new List<string>();

        public HashSet<Position> WhiteAttacks { get; private set; } = new();
        public HashSet<Position> BlackAttacks { get; private set; } = new();

        /// <summary>
        /// Constructor that injects the chess board and subscribes to board events.
        /// </summary>
        public StateService(IChessBoard board)
        {
            _board = board ?? throw new ArgumentNullException(nameof(board));

            // Subscribe to board events
            _board.OnPieceSet += HandlePieceSet;
            _board.OnPieceRemoved += HandlePieceRemoved;

            // Initial attack map update
            UpdateAttackMaps();
        }

        /// <summary>
        /// Handles the event when a piece is set on the board.
        /// </summary>
        private void HandlePieceSet(ChessPiece piece, Position position)
        {
            UpdateAttackMaps();
        }

        /// <summary>
        /// Handles the event when a piece is removed from the board.
        /// </summary>
        private void HandlePieceRemoved(Position position)
        {
            UpdateAttackMaps();
        }

        /// <summary>
        /// Switches the current player after a move.
        /// </summary>
        public virtual void SwitchPlayer()
        {
            CurrentPlayer = (CurrentPlayer == "White") ? "Black" : "White";
        }

        /// <summary>
        /// Allows setting the current player explicitly (for testing).
        /// </summary>
        /// <param name="player">"White" or "Black"</param>
        public void SetPlayer(string player)
        {
            if (player != "White" && player != "Black")
            {
                throw new ArgumentException("Invalid player. Must be 'White' or 'Black'.");
            }

            CurrentPlayer = player;
        }

        /// <summary>
        /// Sets the en passant target position and ensures it applies only to pawns.
        /// </summary>
        public virtual void SetEnPassantTarget(Position target, ChessPiece piece)
        {
            if (piece is Pawn)
            {
                EnPassantTarget = target;
            }
        }

        /// <summary>
        /// Resets the en passant target when no longer applicable.
        /// </summary>
        public void ResetEnPassantTarget()
        {
            EnPassantTarget = null;
        }

        public void ResetState()
        {
            CurrentPlayer = "White";
            IsCheck = false;
            IsCheckmate = false;
            EnPassantTarget = null;

            // Reset castling flags
            WhiteKingMoved = false;
            BlackKingMoved = false;
            WhiteRookKingSideMoved = false;
            WhiteRookQueenSideMoved = false;
            BlackRookKingSideMoved = false;
            BlackRookQueenSideMoved = false;

            MoveLog.Clear();
            WhiteAttacks.Clear();
            BlackAttacks.Clear();
        }

        /// <summary>
        /// Updates the game state after a move.
        /// </summary>
        public void UpdateGameStateAfterMove(ChessPiece piece, Position from, Position to)
        {
            // Log the move
            MoveLog.Add($"{piece.Color} {piece.GetType().Name} from {from} to {to}");

            // Switch player after updating the attack maps via events
            SwitchPlayer();

            // Update check and checkmate status
            string opponentColor = CurrentPlayer;
            IsCheck = IsKingInCheck(opponentColor);
            IsCheckmate = IsCheck && !HasLegalMoves(opponentColor);
        }

        public virtual void UpdateAttackMaps()
        {
            WhiteAttacks.Clear();
            BlackAttacks.Clear();

            foreach (var piece in _board.GetAllPieces())
            {
                var attackMap = piece.Color == "White" ? WhiteAttacks : BlackAttacks;
                var possibleMoves = GetPossibleMoves(piece);
                attackMap.UnionWith(possibleMoves);
            }
        }

        private List<Position> GetPossibleMoves(ChessPiece piece)
        {
            var moves = new List<Position>();
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    var targetPosition = new Position(r, c);

                    // Skip the piece's own position
                    if (piece.Position.Equals(targetPosition))
                        continue;

                    if (piece.IsValidMove(targetPosition, _board, this))
                    {
                        moves.Add(targetPosition);
                    }
                }
            }
            return moves;
        }

        public virtual bool WouldMoveCauseSelfCheck(ChessPiece piece, Position from, Position to)
        {
            var board = _board;
            var originalPosition = piece.Position;
            var targetPiece = board.GetPieceAt(to);

            // Simulate the move
            board.RemovePieceAt(from);
            board.SetPieceAt(to, piece);
            piece.Position = to;

            // Update attack maps
            UpdateAttackMaps();

            // Check if the player's own king is in check after the move
            bool isInCheck = IsKingInCheck(piece.Color);

            // Undo the move
            board.SetPieceAt(from, piece);
            board.SetPieceAt(to, targetPiece);
            piece.Position = originalPosition;

            // Update attack maps back to original state
            UpdateAttackMaps();

            return isInCheck;
        }

        /// <summary>
        /// Determines if the king of the specified color is in check.
        /// </summary>
        public virtual bool IsKingInCheck(string color)
        {
            var board = _board;
            var kingPosition = board.FindKing(color);

            // Use the attack maps to check if the king is in check
            var opponentAttacks = color == "White" ? BlackAttacks : WhiteAttacks;
            return opponentAttacks.Contains(kingPosition);
        }

        /// <summary>
        /// Determines if the player of the specified color has any legal moves.
        /// </summary>
        public bool HasLegalMoves(string color)
        {
            var board = _board;

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
                        var targetPosition = new Position(row, col);

                        if (!piece.IsValidMove(targetPosition, _board, this))
                            continue;

                        // Simulate move
                        var targetPiece = board.GetPieceAt(targetPosition);
                        board.SetPieceAt(targetPosition, piece);
                        board.RemovePieceAt(originalPosition);
                        piece.Position = targetPosition;

                        // Update attack maps
                        UpdateAttackMaps();

                        bool isInCheck = IsKingInCheck(color);

                        // Undo move
                        board.SetPieceAt(originalPosition, piece);
                        board.SetPieceAt(targetPosition, targetPiece);
                        piece.Position = originalPosition;

                        // Update attack maps back to original state
                        UpdateAttackMaps();

                        if (!isInCheck)
                        {
                            return true; // Found at least one legal move
                        }
                    }
                }
            }

            return false; // No legal moves available
        }

        /// <summary>
        /// Disposes the StateService by unsubscribing from board events.
        /// </summary>
        public void Dispose()
        {
            _board.OnPieceSet -= HandlePieceSet;
            _board.OnPieceRemoved -= HandlePieceRemoved;
        }
    }
}
