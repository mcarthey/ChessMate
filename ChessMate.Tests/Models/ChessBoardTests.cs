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
        var whitePawn = chessBoard.ChessPieces[6, 0];
        var blackPawn = chessBoard.ChessPieces[1, 0];
        var whiteRook = chessBoard.ChessPieces[7, 0];
        var blackKing = chessBoard.ChessPieces[0, 4];

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
    public void ChessBoard_MovePiece_ShouldMovePieceCorrectly()
    {
        // Arrange
        var chessBoard = new ChessBoard();
        var from = (6, 0); // a2
        var to = (5, 0); // a3

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Debugging output
        CustomOutput.WriteLine("Test: ChessBoard_MovePiece_ShouldMovePieceCorrectly");
        CustomOutput.WriteLine($"From Position: {ChessNotationUtility.ToChessNotation(from)}");
        CustomOutput.WriteLine($"To Position: {ChessNotationUtility.ToChessNotation(to)}");
        CustomOutput.WriteLine($"Move Success: {moveSuccess}");
        CustomOutput.Flush();

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
        var to = (5, 1); // b3 (invalid move for a pawn)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Debugging output
        CustomOutput.WriteLine("Test: ChessBoard_MovePiece_ShouldRejectInvalidMove");
        CustomOutput.WriteLine($"From Position: {ChessNotationUtility.ToChessNotation(from)}");
        CustomOutput.WriteLine($"To Position: {ChessNotationUtility.ToChessNotation(to)}");
        CustomOutput.WriteLine($"Move Success: {moveSuccess}");
        CustomOutput.Flush();

        // Assert
        Assert.False(moveSuccess);
        Assert.IsType<Pawn>(chessBoard.ChessPieces[6, 0]);
        Assert.Null(chessBoard.ChessPieces[5, 1]);
    }
}
