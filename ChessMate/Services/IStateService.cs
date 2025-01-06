using ChessMate.Models;

public interface IStateService
{
    string CurrentPlayer { get; }
    bool IsCheck { get; set; }
    bool IsCheckmate { get; set; }
    List<string> MoveLog { get; }
    bool WhiteKingMoved { get; set; }
    bool BlackKingMoved { get; set; }
    bool WhiteRookKingSideMoved { get; set; }
    bool WhiteRookQueenSideMoved { get; set; }
    bool BlackRookKingSideMoved { get; set; }
    bool BlackRookQueenSideMoved { get; set; }
    Position? EnPassantTarget { get; }
    IReadOnlyDictionary<Position, List<ChessPiece>> WhiteAttacks { get; }
    IReadOnlyDictionary<Position, List<ChessPiece>> BlackAttacks { get; }
    List<ChessPiece> CapturedPieces { get; }
    List<ChessPiece> GetWhiteAttackers(Position position);
    List<ChessPiece> GetBlackAttackers(Position position);
    List<Position> GetPossibleMoves(ChessPiece piece);
    void SwitchPlayer();
    void SetPlayer(string player);
    void SetEnPassantTarget(Position target, ChessPiece piece);
    void ResetEnPassantTarget();
    void ResetState();
    void UpdateAttackMaps();
    void UpdateGameStateAfterMove(ChessPiece piece, Position from, Position to);
    bool WouldMoveCauseSelfCheck(ChessPiece piece, Position from, Position to);
    void RegisterCapture(ChessPiece capturedPiece);
    bool IsKingInCheck(string color);
    bool HasLegalMoves(string color);
}
