using ChessMate.Models;

namespace ChessMate.Services;

public class StateService : IStateService
{
    public string CurrentPlayer { get; private set; } = "White";
    public bool IsCheck { get; set; }
    public bool IsCheckmate { get; set; }

    // Castling flags
    public bool WhiteKingMoved { get; set; }
    public bool BlackKingMoved { get; set; }
    public bool WhiteRookKingSideMoved { get; set; }
    public bool WhiteRookQueenSideMoved { get; set; }
    public bool BlackRookKingSideMoved { get; set; }
    public bool BlackRookQueenSideMoved { get; set; }

    public List<string> MoveLog { get; private set; } = new List<string>();

    public void SwitchPlayer()
    {
        CurrentPlayer = (CurrentPlayer == "White") ? "Black" : "White";
    }

    public bool IsKingInCheck(string color, IChessBoard board)
    {
        var kingPosition = board.FindKing(color);
        return board.GetAllPieces()
            .Where(p => p.Color != color)
            .Any(opponent => opponent.IsValidMove(kingPosition, board));
    }

    public bool HasLegalMoves(string color, IChessBoard board)
    {
        return board.GetAllPieces().Cast<ChessPiece?>()
            .Where(p => p != null && p.Color == color)
            .Any(piece =>
                Enumerable.Range(0, 8)
                    .SelectMany(r => Enumerable.Range(0, 8).Select(c => (r, c)))
                    .Any(target => piece!.IsValidMove(target, board)));
    }
}
