using ChessMate.Models;
using ChessMate.Utilities;
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

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        var whitePawn = chessBoard.GetPieceAt((6, 0));
        var blackPawn = chessBoard.GetPieceAt((1, 0));
        var whiteRook = chessBoard.GetPieceAt((7, 0));
        var blackKing = chessBoard.GetPieceAt((0, 4));

        // Debugging output
        CustomOutput.WriteLine("Test: ChessBoard_InitializeBoard_ShouldSetUpPiecesCorrectly");
        CustomOutput.WriteLine($"White Pawn Position: {ChessNotationUtility.ToChessNotation((6, 0))}");
        CustomOutput.WriteLine($"Black Pawn Position: {ChessNotationUtility.ToChessNotation((1, 0))}");
        CustomOutput.WriteLine($"White Rook Position: {ChessNotationUtility.ToChessNotation((7, 0))}");
        CustomOutput.WriteLine($"Black King Position: {ChessNotationUtility.ToChessNotation((0, 4))}");
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

        // Debugging output
        CustomOutput.WriteLine("Test: ChessBoard_SetPieceAt_ShouldPlacePieceCorrectly");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_RemovePieceAt_ShouldRemovePieceCorrectly()
    {
        // Arrange
        var chessBoard = new ChessBoard();
        var position = (6, 0);

        // Act
        chessBoard.RemovePieceAt(position);

        // Assert
        var piece = chessBoard.GetPieceAt(position);
        Assert.Null(piece);

        // Debugging output
        CustomOutput.WriteLine("Test: ChessBoard_RemovePieceAt_ShouldRemovePieceCorrectly");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_FindKing_ShouldReturnCorrectPosition()
    {
        // Arrange
        var chessBoard = new ChessBoard();

        // Act
        var whiteKingPosition = chessBoard.FindKing("White");
        var blackKingPosition = chessBoard.FindKing("Black");

        // Assert
        Assert.Equal((7, 4), whiteKingPosition);
        Assert.Equal((0, 4), blackKingPosition);

        // Debugging output
        CustomOutput.WriteLine("Test: ChessBoard_FindKing_ShouldReturnCorrectPosition");
        CustomOutput.WriteLine($"White King Position: {ChessNotationUtility.ToChessNotation(whiteKingPosition)}");
        CustomOutput.WriteLine($"Black King Position: {ChessNotationUtility.ToChessNotation(blackKingPosition)}");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_GetAllPieces_ShouldReturnAllPieces()
    {
        // Arrange
        var chessBoard = new ChessBoard();

        // Act
        var allPieces = chessBoard.GetAllPieces().ToList();

        // Assert
        Assert.Equal(32, allPieces.Count);
        Assert.Contains(allPieces, p => p is King && p.Color == "White");
        Assert.Contains(allPieces, p => p is King && p.Color == "Black");

        // Debugging output
        CustomOutput.WriteLine("Test: ChessBoard_GetAllPieces_ShouldReturnAllPieces");
        CustomOutput.WriteLine($"Total Pieces: {allPieces.Count}");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
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

        // Debugging output
        CustomOutput.WriteLine("Test: ChessBoard_SetCustomBoard_ShouldSetUpCustomPiecesCorrectly");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }
}




