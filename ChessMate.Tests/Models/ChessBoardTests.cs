// File: ChessMate.Tests/Models/ChessBoardTests.cs

using ChessMate.Models;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Models;

public class ChessBoardTests : TestHelper
{
    public ChessBoardTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ChessBoard_InitializeBoard_ShouldSetUpPiecesCorrectly()
    {
        // Arrange
        var chessBoard = new ChessBoard();
        chessBoard.InitializeBoard(); // Ensure the board is initialized

        // Act
        var whitePawn = chessBoard.GetPieceAt((6, 0));
        var blackPawn = chessBoard.GetPieceAt((1, 0));
        var whiteRook = chessBoard.GetPieceAt((7, 0));
        var blackKing = chessBoard.GetPieceAt((0, 4));

        // Assert
        Assert.NotNull(whitePawn);
        Assert.NotNull(blackPawn);
        Assert.IsType<Pawn>(whitePawn);
        Assert.IsType<Pawn>(blackPawn);
        Assert.IsType<Rook>(whiteRook);
        Assert.IsType<King>(blackKing);
    }

    [Fact]
    public void ChessBoard_SetPieceAt_ShouldPlacePieceCorrectly()
    {
        // Arrange
        var chessBoard = new ChessBoard();
        var whitePawn = new Pawn("White", (6, 0));
        var position = (4, 4);

        // Act
        chessBoard.SetPieceAt(position, whitePawn);

        // Assert
        var piece = chessBoard.GetPieceAt(position);
        Assert.NotNull(piece);
        Assert.IsType<Pawn>(piece);
        Assert.Equal("White", piece.Color);
    }

    [Fact]
    public void ChessBoard_RemovePieceAt_ShouldRemovePieceCorrectly()
    {
        // Arrange
        var chessBoard = InitializeCustomBoard(
            (new Pawn("White", (6, 0)), (6, 0))
        );

        // Act
        chessBoard.RemovePieceAt((6, 0));

        // Assert
        var piece = chessBoard.GetPieceAt((6, 0));
        Assert.Null(piece);
    }

    [Fact]
    public void ChessBoard_FindKing_ShouldReturnCorrectPosition()
    {
        // Arrange
        var whiteKing = new King("White", (7, 4));
        var blackKing = new King("Black", (0, 4));
        var chessBoard = InitializeCustomBoard(
            (whiteKing, (7, 4)),
            (blackKing, (0, 4))
        );

        // Act
        var whiteKingPosition = chessBoard.FindKing("White");
        var blackKingPosition = chessBoard.FindKing("Black");

        // Assert
        Assert.Equal((7, 4), whiteKingPosition);
        Assert.Equal((0, 4), blackKingPosition);
    }

    [Fact]
    public void ChessBoard_GetAllPieces_ShouldReturnAllPieces()
    {
        // Arrange
        var pieces = new (ChessPiece piece, (int Row, int Col) position)[]
        {
            (new King("White", (7, 4)), (7, 4)),
            (new King("Black", (0, 4)), (0, 4)),
            (new Pawn("White", (6, 0)), (6, 0)),
            (new Pawn("Black", (1, 0)), (1, 0))
        };
        var chessBoard = InitializeCustomBoard(pieces);

        // Act
        var allPieces = chessBoard.GetAllPieces().ToList();

        // Assert
        Assert.Equal(pieces.Length, allPieces.Count);
        Assert.Contains(allPieces, p => p is King && p.Color == "White");
        Assert.Contains(allPieces, p => p is King && p.Color == "Black");
    }

    [Fact]
    public void ChessBoard_SetCustomBoard_ShouldSetUpCustomPiecesCorrectly()
    {
        // Arrange
        var whitePawn = new Pawn("White", (6, 0));
        var blackPawn = new Pawn("Black", (1, 0));
        var whiteKing = new King("White", (7, 4));
        var blackKing = new King("Black", (0, 4));
        var chessBoard = InitializeCustomBoard(
            (whitePawn, (6, 0)),
            (blackPawn, (1, 0)),
            (whiteKing, (7, 4)),
            (blackKing, (0, 4))
        );

        // Act
        var customWhitePawn = chessBoard.GetPieceAt((6, 0));
        var customBlackPawn = chessBoard.GetPieceAt((1, 0));
        var customWhiteKing = chessBoard.GetPieceAt((7, 4));
        var customBlackKing = chessBoard.GetPieceAt((0, 4));

        // Assert
        Assert.NotNull(customWhitePawn);
        Assert.NotNull(customBlackPawn);
        Assert.NotNull(customWhiteKing);
        Assert.NotNull(customBlackKing);
        Assert.IsType<Pawn>(customWhitePawn);
        Assert.IsType<Pawn>(customBlackPawn);
        Assert.IsType<King>(customWhiteKing);
        Assert.IsType<King>(customBlackKing);
    }
}

