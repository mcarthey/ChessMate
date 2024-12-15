using ChessMate.Models;
using ChessMate.Services;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Models;

public class ChessGamePawnTests : TestHelper
{
    public ChessGamePawnTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ChessGame_InitializeBoard_ShouldSetUpPiecesCorrectly()
    {
        // Arrange
        var chessGame = new ChessGame();

        // Act
        var whitePawn = chessGame.Board.ChessPieces[6, 0];
        var blackPawn = chessGame.Board.ChessPieces[1, 0];
        var whiteRook = chessGame.Board.ChessPieces[7, 0];
        var blackKing = chessGame.Board.ChessPieces[0, 4];

        // Debugging output
        PrintBoard(chessGame);
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
    public void ChessGame_MovePiece_ShouldMovePawnCorrectly()
    {
        // Arrange
        var chessGame = new ChessGame();
        string start = "a2"; // Corresponds to (6, 0) for white
        string target = "a3"; // Corresponds to (5, 0)

        // Act
        bool moveSuccess = chessGame.MovePiece(start, target);

        // Debugging output
        CustomOutput.WriteLine("Test: MovePiece_ShouldMovePawnCorrectly");
        CustomOutput.WriteLine($"Move Success: {moveSuccess}");
        CustomOutput.WriteLine("ChessPieces after move:");
        PrintBoard(chessGame);
        CustomOutput.Flush();

        // Assert
        Assert.True(moveSuccess, "The move should be successful.");
        Assert.Null(chessGame.Board.ChessPieces[6, 0]); // Row 6, Col 0 (a2)
        Assert.IsType<Pawn>(chessGame.Board.ChessPieces[5, 0]); // Row 5, Col 0 (a3)
    }

    [Fact]
    public void ChessGame_MovePiece_PawnShouldAllowTwoSquaresOnFirstMove()
    {
        // Arrange
        var chessGame = new ChessGame();
        string start = "a2"; // (6, 0)
        string target = "a4"; // (4, 0)

        // Act
        bool moveSuccess = chessGame.MovePiece(start, target);

        // Debugging output
        CustomOutput.WriteLine("Test: MovePiece_PawnShouldAllowTwoSquaresOnFirstMove");
        CustomOutput.WriteLine($"Attempted move from {start} to {target}");
        CustomOutput.WriteLine($"Move Success: {moveSuccess}");
        PrintBoard(chessGame);
        CustomOutput.Flush();

        // Assert
        Assert.True(moveSuccess, "The Pawn should be able to move two squares forward on its first move.");
        Assert.Null(chessGame.Board.ChessPieces[6, 0]); // Row 6, Col 0 (a2)
        Assert.IsType<Pawn>(chessGame.Board.ChessPieces[4, 0]); // Row 4, Col 0 (a4)
    }

    [Fact]
    public void ChessGame_MovePiece_PawnShouldRejectTwoSquaresAfterFirstMove()
    {
        // Arrange
        var chessGame = new ChessGame();
        string firstMove = "a2"; // (6, 0)
        string firstTarget = "a3"; // (5, 0)
        string invalidTarget = "a5"; // (3, 0)

        // Act
        bool firstMoveSuccess = chessGame.MovePiece(firstMove, firstTarget);
        bool moveSuccess = chessGame.MovePiece(firstTarget, invalidTarget);

        // Debugging output
        CustomOutput.WriteLine("Test: MovePiece_PawnShouldRejectTwoSquaresAfterFirstMove");
        CustomOutput.WriteLine($"First move success: {firstMoveSuccess}");
        CustomOutput.WriteLine($"Attempted move from {firstTarget} to {invalidTarget}");
        CustomOutput.WriteLine($"Move Success: {moveSuccess}");
        PrintBoard(chessGame);
        CustomOutput.Flush();

        // Assert
        Assert.True(firstMoveSuccess, "The first move should have been successful.");
        Assert.False(moveSuccess, "The two-square move should have been rejected after the first move.");
        Assert.IsType<Pawn>(chessGame.Board.ChessPieces[5, 0]); // Row 5, Col 0 (a3)
        Assert.Null(chessGame.Board.ChessPieces[3, 0]); // Row 3, Col 0 (a5)
    }

    [Fact]
    public void ChessGame_IsValidMove_PawnShouldRejectTwoSquaresAfterFirstMove()
    {
        // Arrange
        var chessBoard = new ChessBoard();
        var pawn = chessBoard.ChessPieces[6, 0] as Pawn; // White pawn at (6, 0)
        var targetFirstMove = (4, 0); // Move to (4, 0) - two squares forward
        var targetSecondMove = (2, 0); // Attempt to move two squares again

        // Act
        bool firstMoveSuccess = pawn.IsValidMove(targetFirstMove, chessBoard);
        if (firstMoveSuccess)
        {
            chessBoard.MovePiece(pawn.Position, targetFirstMove);
            pawn = chessBoard.ChessPieces[4, 0] as Pawn;
        }
        bool secondMoveSuccess = pawn.IsValidMove(targetSecondMove, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Pawn_ShouldRejectTwoSquaresAfterFirstMove");
        CustomOutput.WriteLine($"First move success: {firstMoveSuccess}");
        CustomOutput.WriteLine($"Second move success: {secondMoveSuccess}");
        CustomOutput.Flush();

        // Assert
        Assert.True(firstMoveSuccess, "The Pawn should be able to move two squares forward on its first move.");
        Assert.False(secondMoveSuccess, "The Pawn should not be able to move two squares after the first move.");
    }
}
