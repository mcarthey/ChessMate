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
        var whitePawn = chessBoard.GetPieceAt(new Position("a2"));
        var blackPawn = chessBoard.GetPieceAt(new Position("a7"));
        var whiteRook = chessBoard.GetPieceAt(new Position("a1"));
        var blackKing = chessBoard.GetPieceAt(new Position("e8"));

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
        var whitePawn = new Pawn("White", new Position("a2"));
        var position = new Position("e4");

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
            (new Pawn("White", new Position("a2")), new Position("a2"))
        );

        // Act
        chessBoard.RemovePieceAt(new Position("a2"));

        // Assert
        var piece = chessBoard.GetPieceAt(new Position("a2"));
        Assert.Null(piece);
    }

    [Fact]
    public void ChessBoard_FindKing_ShouldReturnCorrectPosition()
    {
        // Arrange
        var whiteKing = new King("White", new Position("e1"));
        var blackKing = new King("Black", new Position("e8"));
        var chessBoard = InitializeCustomBoard(
            (whiteKing, new Position("e1")),
            (blackKing, new Position("e8"))
        );

        // Act
        var whiteKingPosition = chessBoard.FindKing("White");
        var blackKingPosition = chessBoard.FindKing("Black");

        // Assert
        Assert.Equal(new Position("e1"), whiteKingPosition);
        Assert.Equal(new Position("e8"), blackKingPosition);
    }

    [Fact]
    public void ChessBoard_GetAllPieces_ShouldReturnAllPieces()
    {
        // Arrange
        var pieces = new (ChessPiece piece, Position position)[]
        {
            (new King("White", new Position("e1")), new Position("e1")),
            (new King("Black", new Position("e8")), new Position("e8")),
            (new Pawn("White", new Position("a2")), new Position("a2")),
            (new Pawn("Black", new Position("a7")), new Position("a7"))
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
        var whitePawn = new Pawn("White", new Position("a2"));
        var blackPawn = new Pawn("Black", new Position("a7"));
        var whiteKing = new King("White", new Position("e1"));
        var blackKing = new King("Black", new Position("e8"));
        var chessBoard = InitializeCustomBoard(
            (whitePawn, new Position("a2")),
            (blackPawn, new Position("a7")),
            (whiteKing, new Position("e1")),
            (blackKing, new Position("e8"))
        );

        // Act
        var customWhitePawn = chessBoard.GetPieceAt(new Position("a2"));
        var customBlackPawn = chessBoard.GetPieceAt(new Position("a7"));
        var customWhiteKing = chessBoard.GetPieceAt(new Position("e1"));
        var customBlackKing = chessBoard.GetPieceAt(new Position("e8"));

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



