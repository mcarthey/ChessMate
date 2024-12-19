using ChessMate.Models;
using ChessMate.Services;

public class MoveService : IMoveService
{
    private readonly IChessBoard _board;
    private readonly IStateService _state;

    public MoveService(IChessBoard board, IStateService state)
    {
        _board = board;
        _state = state;
    }

    public bool TryMove((int Row, int Col) from, (int Row, int Col) to)
    {
        var piece = _board.GetPieceAt(from);
        if (piece == null || piece.Color != _state.CurrentPlayer)
            return false;

        // Validate move using the piece's delegate
        if (!piece.IsValidMove(to, _board))
            return false;

        // Execute the move
        _board.SetPieceAt(to, piece);
        _board.RemovePieceAt(from);
        piece.Position = to;

        // Invoke the piece's OnMoveEffect delegate
        piece.OnMoveEffect?.Invoke(to);

        // Finalize the move by updating global state
        FinalizeMove(piece, from, to);

        return true;
    }

    private void FinalizeMove(ChessPiece piece, (int Row, int Col) from, (int Row, int Col) to)
    {
        // Switch players
        _state.SwitchPlayer();

        // Check for check and checkmate
        var opponentColor = _state.CurrentPlayer;
        _state.IsCheck = _state.IsKingInCheck(opponentColor, _board);
        _state.IsCheckmate = _state.IsCheck && !_state.HasLegalMoves(opponentColor, _board);
    }
}
