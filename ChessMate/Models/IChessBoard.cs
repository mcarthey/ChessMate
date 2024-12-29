using System.Collections.Generic;

namespace ChessMate.Models
{
    public interface IChessBoard
    {
        void InitializeBoard();

        ChessPiece GetPieceAt(Position position);
        void SetPieceAt(Position position, ChessPiece piece);
        void RemovePieceAt(Position position);

        Position FindKing(string color);

        IEnumerable<ChessPiece> GetAllPieces();
    }
}
