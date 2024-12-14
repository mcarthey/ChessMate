using ChessMate.Models;
using ChessMate.Services;
using Xunit;
using Xunit.Abstractions;
using System.IO;
using System.Text;

namespace ChessMate.Tests;

public class ChessGameTests
{
    private readonly CustomTestOutputHelper _customOutput;

    public ChessGameTests(ITestOutputHelper output)
    {
        _customOutput = new CustomTestOutputHelper(output);
    }

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

        // Flush buffered output to test runner
        _customOutput.Flush();

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
        var target = (2, 0); // One square forward

        // Act
        bool moveSuccess = chessGame.MovePiece(start, target);

        // Debugging output
        _customOutput.WriteLine("Test: MovePiece_ShouldMovePawnCorrectly");
        _customOutput.WriteLine($"Move Success: {moveSuccess}");
        _customOutput.WriteLine("Board after move:");
        PrintBoard(chessGame);

        // Flush buffered output to test runner
        _customOutput.Flush();

        // Assert
        Assert.True(moveSuccess, "The move should be successful.");
        Assert.Null(chessGame.Board[start.Item1, start.Item2]);
        Assert.IsType<Pawn>(chessGame.Board[target.Item1, target.Item2]);
    }

    [Fact]
    public void MovePiece_ShouldAllowTwoSquaresOnFirstMove()
    {
        // Arrange
        var chessGame = new ChessGame();
        var start = (6, 0); // Black Pawn at A7
        var target = (4, 0); // A5 (two squares forward)

        // Act
        bool moveSuccess = chessGame.MovePiece(start, target);

        // Debugging output
        _customOutput.WriteLine("Test: MovePiece_ShouldAllowTwoSquaresOnFirstMove");
        _customOutput.WriteLine($"Attempted move from {start} to {target}");
        _customOutput.WriteLine($"Move Success: {moveSuccess}");
        _customOutput.WriteLine("Board after move:");
        PrintBoard(chessGame);

        // Flush buffered output to test runner
        _customOutput.Flush();

        // Assert
        Assert.True(moveSuccess, "The Pawn should be able to move two squares forward on its first move.");
        Assert.Null(chessGame.Board[start.Item1, start.Item2]);
        Assert.IsType<Pawn>(chessGame.Board[target.Item1, target.Item2]);
    }

    [Fact]
    public void MovePiece_ShouldRejectTwoSquaresAfterFirstMove()
    {
        // Arrange
        var chessGame = new ChessGame();
        var start = (6, 0); // Black Pawn at A7
        var firstMoveTarget = (5, 0); // A6 (one square forward)
        var invalidTarget = (3, 0); // A5 (two squares forward, invalid after the first move)

        // Act
        bool firstMoveSuccess = chessGame.MovePiece(start, firstMoveTarget);
        bool moveSuccess = chessGame.MovePiece(firstMoveTarget, invalidTarget);

        // Debugging output
        _customOutput.WriteLine("Test: MovePiece_ShouldRejectTwoSquaresAfterFirstMove");
        _customOutput.WriteLine($"First move success: {firstMoveSuccess}");
        _customOutput.WriteLine($"Attempted move from {firstMoveTarget} to {invalidTarget}");
        _customOutput.WriteLine($"Move Success: {moveSuccess}");
        _customOutput.WriteLine("Board after attempted move:");
        PrintBoard(chessGame);

        // Flush buffered output to test runner
        _customOutput.Flush();

        // Assert
        Assert.True(firstMoveSuccess, "The first move should have been successful.");
        Assert.False(moveSuccess, "The two-square move should have been rejected after the first move.");
        Assert.IsType<Pawn>(chessGame.Board[firstMoveTarget.Item1, firstMoveTarget.Item2]); // Ensure Pawn is still in original position
        Assert.Null(chessGame.Board[invalidTarget.Item1, invalidTarget.Item2]); // Ensure target square is still empty
    }


    private void PrintBoard(ChessGame chessGame)
    {
        _customOutput.WriteLine("  A  B  C  D  E  F  G  H");
        _customOutput.WriteLine(" +-----------------------+");
        for (int row = 0; row < 8; row++)
        {
            var rowBuilder = new StringBuilder();
            rowBuilder.Append(8 - row).Append("|");
            for (int col = 0; col < 8; col++)
            {
                var piece = chessGame.Board[row, col];
                rowBuilder.Append((piece?.Representation ?? ".").PadRight(3));
            }
            rowBuilder.Append("|");
            _customOutput.WriteLine(rowBuilder.ToString());
        }
        _customOutput.WriteLine(" +-----------------------+");
    }

    [Fact]
    public void Pawn_ShouldRejectTwoSquaresAfterFirstMove()
    {
        // Arrange
        var pawn = new Pawn("White", (2, 0)); // Place Pawn one square forward
        var board = new ChessPiece[8, 8];
        board[2, 0] = pawn;

        // Act
        bool moveSuccess = pawn.IsValidMove((4, 0), board); // Attempt two-square move

        // Debugging output
        _customOutput.WriteLine("Test: Pawn_ShouldRejectTwoSquaresAfterFirstMove");
        _customOutput.WriteLine($"Attempted move from {pawn.Position} to (4, 0)");
        _customOutput.WriteLine($"Move Success: {moveSuccess}");

        // Flush buffered output to test runner
        _customOutput.Flush();

        // Assert
        Assert.False(moveSuccess, "The Pawn should not be able to move two squares after the first move.");
    }
}

