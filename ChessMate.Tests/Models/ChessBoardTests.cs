using ChessMate.Models;
using Xunit;

namespace ChessMate.Tests.Models;

public class ChessBoardTests
{
    [Fact]
    public void ChessBoard_InitializeBoard_ShouldSetUpPiecesCorrectly()
    {
        // Arrange
        var chessBoard = new ChessBoard();

        // Act
        var whitePawn = chessBoard.ChessPieces[6, 0];
        var blackPawn = chessBoard.ChessPieces[1, 0];
        var whiteRook = chessBoard.ChessPieces[7, 0];
        var blackKing = chessBoard.ChessPieces[0, 4];

        // Assert
        Assert.NotNull(whitePawn);
        Assert.NotNull(blackPawn);
        Assert.IsType<Pawn>(whitePawn);
        Assert.IsType<Pawn>(blackPawn);
        Assert.IsType<Rook>(whiteRook);
        Assert.IsType<King>(blackKing);
    }

    [Fact]
    public void ChessBoard_MovePiece_ShouldMovePieceCorrectly()
    {
        // Arrange
        var chessBoard = new ChessBoard();
        var from = (6, 0); // a2
        var to = (5, 0); // a3

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Assert
        Assert.True(moveSuccess);
        Assert.Null(chessBoard.ChessPieces[6, 0]);
        Assert.IsType<Pawn>(chessBoard.ChessPieces[5, 0]);
    }

    [Fact]
    public void ChessBoard_MovePiece_ShouldRejectInvalidMove()
    {
        // Arrange
        var chessBoard = new ChessBoard();
        var from = (6, 0); // a2
        var to = (4, 0); // a4

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Assert
        Assert.False(moveSuccess);
        Assert.IsType<Pawn>(chessBoard.ChessPieces[6, 0]);
        Assert.Null(chessBoard.ChessPieces[4, 0]);
    }
}
