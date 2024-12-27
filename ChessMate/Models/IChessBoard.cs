using System.Collections.Generic;

namespace ChessMate.Models
{
    public interface IChessBoard
    {
        void InitializeBoard();

        ChessPiece GetPieceAt((int Row, int Col) position);
        void SetPieceAt((int Row, int Col) position, ChessPiece piece);
        void RemovePieceAt((int Row, int Col) position);

        (int Row, int Col) FindKing(string color);

        IEnumerable<ChessPiece> GetAllPieces();
    }
}
