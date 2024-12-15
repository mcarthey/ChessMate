using ChessMate.Models;
using ChessMate.Services;
using Xunit;
using Xunit.Abstractions;
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

        // Debugging output
        PrintBoard(chessGame);
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
        string start = "a2";
        string target = "a3";

        // Act
        bool moveSuccess = chessGame.MovePiece(start, target);

        // Debugging output
        _customOutput.WriteLine("Test: MovePiece_ShouldMovePawnCorrectly");
        _customOutput.WriteLine($"Move Success: {moveSuccess}");
        _customOutput.WriteLine("Board after move:");
        PrintBoard(chessGame);
        _customOutput.Flush();

        // Assert
        Assert.True(moveSuccess, "The move should be successful.");
        Assert.Null(chessGame.Board[6, 0]); // Row 6, Col 0 (a2)
        Assert.IsType<Pawn>(chessGame.Board[5, 0]); // Row 5, Col 0 (a3)
    }

    [Fact]
    public void MovePiece_ShouldAllowTwoSquaresOnFirstMove()
    {
        // Arrange
        var chessGame = new ChessGame();
        string start = "a7";
        string target = "a5";

        // Act
        bool moveSuccess = chessGame.MovePiece(start, target);

        // Debugging output
        _customOutput.WriteLine("Test: MovePiece_ShouldAllowTwoSquaresOnFirstMove");
        _customOutput.WriteLine($"Attempted move from {start} to {target}");
        _customOutput.WriteLine($"Move Success: {moveSuccess}");
        PrintBoard(chessGame);
        _customOutput.Flush();

        // Assert
        Assert.True(moveSuccess, "The Pawn should be able to move two squares forward on its first move.");
        Assert.Null(chessGame.Board[6, 0]); // Row 6, Col 0 (a7)
        Assert.IsType<Pawn>(chessGame.Board[4, 0]); // Row 4, Col 0 (a5)
    }

    [Fact]
    public void MovePiece_ShouldRejectTwoSquaresAfterFirstMove()
    {
        // Arrange
        var chessGame = new ChessGame();
        string firstMove = "a7";
        string firstTarget = "a6";
        string invalidTarget = "a4";

        // Act
        bool firstMoveSuccess = chessGame.MovePiece(firstMove, firstTarget);
        bool moveSuccess = chessGame.MovePiece(firstTarget, invalidTarget);

        // Debugging output
        _customOutput.WriteLine("Test: MovePiece_ShouldRejectTwoSquaresAfterFirstMove");
        _customOutput.WriteLine($"First move success: {firstMoveSuccess}");
        _customOutput.WriteLine($"Attempted move from {firstTarget} to {invalidTarget}");
        _customOutput.WriteLine($"Move Success: {moveSuccess}");
        PrintBoard(chessGame);
        _customOutput.Flush();

        // Assert
        Assert.True(firstMoveSuccess, "The first move should have been successful.");
        Assert.False(moveSuccess, "The two-square move should have been rejected after the first move.");
        Assert.IsType<Pawn>(chessGame.Board[5, 0]); // Row 5, Col 0 (a6)
        Assert.Null(chessGame.Board[3, 0]); // Row 3, Col 0 (a4)
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
        var pawn = new Pawn("White", (6, 0)); // Place Pawn at a7
        var board = new ChessPiece[8, 8];
        board[6, 0] = pawn;

        // Act
        bool firstMoveSuccess = pawn.IsValidMove((4, 0), board); // Valid two-square move
        pawn.Position = (5, 0); // Move pawn to a6
        bool moveSuccess = pawn.IsValidMove((3, 0), board); // Invalid two-square move after first

        // Debugging output
        _customOutput.WriteLine("Test: Pawn_ShouldRejectTwoSquaresAfterFirstMove");
        _customOutput.WriteLine($"First move success: {firstMoveSuccess}");
        _customOutput.WriteLine($"Second move success: {moveSuccess}");
        _customOutput.Flush();

        // Assert
        Assert.True(firstMoveSuccess, "The Pawn should be able to move two squares forward on its first move.");
        Assert.False(moveSuccess, "The Pawn should not be able to move two squares after the first move.");
    }
}
