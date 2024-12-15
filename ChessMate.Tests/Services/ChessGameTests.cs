using ChessMate.Models;
using ChessMate.Services;
using ChessMate.Utilities;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services;

public class ChessGameTests : TestHelper
{
    public ChessGameTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ChessGame_InitializeBoard_ShouldSetUpPiecesCorrectly()
    {
        // Arrange
        var mockBoard = new Mock<IChessBoard>();
        var chessPieces = new ChessPiece[8, 8];
        chessPieces[6, 0] = new Pawn("White", (6, 0));
        chessPieces[1, 0] = new Pawn("Black", (1, 0));
        chessPieces[7, 0] = new Rook("White", (7, 0));
        chessPieces[0, 4] = new King("Black", (0, 4));

        mockBoard.SetupGet(b => b.ChessPieces).Returns(chessPieces);
        var chessGame = new ChessGame(mockBoard.Object);

        // Act
        var whitePawn = chessGame.Board.ChessPieces[6, 0];
        var blackPawn = chessGame.Board.ChessPieces[1, 0];
        var whiteRook = chessGame.Board.ChessPieces[7, 0];
        var blackKing = chessGame.Board.ChessPieces[0, 4];

        // Debugging output
        PrintBoard(chessGame.Board);
        CustomOutput.Flush();

        // Assert
        Assert.NotNull(whitePawn);
        Assert.NotNull(blackPawn);
        Assert.IsType<Pawn>(whitePawn);
        Assert.IsType<Pawn>(blackPawn);
        Assert.IsType<Rook>(whiteRook);
        Assert.IsType<King>(blackKing);
    }

    [Fact]
    public void ChessGame_MovePiece_ShouldCallMovePieceOnBoard()
    {
        // Arrange
        var mockBoard = new Mock<IChessBoard>();
        var chessGame = new ChessGame(mockBoard.Object);
        string start = "a2";
        string target = "a3";
        var from = ChessNotationUtility.FromChessNotation(start);
        var to = ChessNotationUtility.FromChessNotation(target);

        mockBoard.Setup(b => b.MovePiece(from, to)).Returns(true);

        // Act
        bool moveSuccess = chessGame.MovePiece(start, target);

        // Assert
        Assert.True(moveSuccess);
        mockBoard.Verify(b => b.MovePiece(from, to), Times.Once);
    }

    [Fact]
    public void ChessGame_MovePiece_ShouldReturnFalseForInvalidNotation()
    {
        // Arrange
        var mockBoard = new Mock<IChessBoard>();
        var chessGame = new ChessGame(mockBoard.Object);

        // Act
        bool moveSuccess = chessGame.MovePiece("invalid", "a3");

        // Assert
        Assert.False(moveSuccess);
    }

    [Fact]
    public void ChessGame_MovePiece_ShouldReturnFalseForInvalidMove()
    {
        // Arrange
        var mockBoard = new Mock<IChessBoard>();
        var chessGame = new ChessGame(mockBoard.Object);
        string start = "a2";
        string target = "a3";
        var from = ChessNotationUtility.FromChessNotation(start);
        var to = ChessNotationUtility.FromChessNotation(target);

        mockBoard.Setup(b => b.MovePiece(from, to)).Returns(false);

        // Act
        bool moveSuccess = chessGame.MovePiece(start, target);

        // Assert
        Assert.False(moveSuccess);
        mockBoard.Verify(b => b.MovePiece(from, to), Times.Once);
    }
}

