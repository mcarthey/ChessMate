using ChessMate.Models;
using ChessMate.Services;

namespace ChessMate.Tests;

public class ChessGameTests
{
    [Fact]
    public void InitializeBoard_ShouldSetUpPiecesCorrectly()
    {
        // Arrange
        var chessGame = new ChessGame();

        // Act
        var whitePawn = chessGame.Board[1, 0];
        var blackPawn = chessGame.Board[6, 0];
        var whiteRook = chessGame.Board[0, 0];
        var blackKing = chessGame.Board[7, 4];

        // Assert
        Assert.NotNull(whitePawn);
        Assert.NotNull(blackPawn);
        Assert.IsType<Pawn>(whitePawn);
        Assert.IsType<Pawn>(blackPawn);
        Assert.IsType<Rook>(whiteRook);
        Assert.IsType<King>(blackKing);
    }

    [Fact]
    public void MovePiece_ShouldMovePawnCorrectly()
    {
        // Arrange
        var chessGame = new ChessGame();
        var start = (1, 0); // White Pawn at (1,0)
        var target = (2, 0);

        // Act
        bool moveSuccess = chessGame.MovePiece(start, target);

        // Assert
        Assert.True(moveSuccess);
        Assert.Null(chessGame.Board[start.Item1, start.Item2]);
        Assert.IsType<Pawn>(chessGame.Board[target.Item1, target.Item2]);
    }

    [Fact]
    public void MovePiece_ShouldRejectInvalidMove()
    {
        // Arrange
        var chessGame = new ChessGame();
        var start = (1, 0); // White Pawn at (1,0)
        var invalidTarget = (3, 0); // Trying to jump two squares, which is not valid for a Pawn

        // Act
        bool moveSuccess = chessGame.MovePiece(start, invalidTarget);

        // Assert
        Assert.False(moveSuccess);
        Assert.IsType<Pawn>(chessGame.Board[start.Item1, start.Item2]);
        Assert.Null(chessGame.Board[invalidTarget.Item1, invalidTarget.Item2]);
    }
}