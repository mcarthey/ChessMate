// ChessMate/Models/IChessBoard.cs
namespace ChessMate.Models
{
    public interface IChessBoard
    {
        bool MovePiece((int Row, int Col) from, (int Row, int Col) to);
        ChessPiece[,] ChessPieces { get; set; }
        string Orientation { get; set; }
    }
}
