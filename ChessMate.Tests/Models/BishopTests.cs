using ChessMate.Models;
using ChessMate.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Models;

public class BishopTests : TestHelper
{
    public BishopTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldAllowDiagonalMove()
    {
        // Arrange
        var bishop = new Bishop("White", (7, 2));
        var chessBoard = InitializeCustomBoard((bishop, (7, 2)));
        var targetPosition = (5, 4); // Move to (5, 4)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Bishop_IsValidMove_ShouldAllowDiagonalMove");
        CustomOutput.WriteLine($"Bishop Position: {ChessNotationUtility.ToChessNotation(bishop.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The bishop should be able to move diagonally.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectNonDiagonalMove()
    {
        // Arrange
        var bishop = new Bishop("White", (7, 2));
        var chessBoard = InitializeCustomBoard((bishop, (7, 2)));
        var targetPosition = (6, 2); // Move to (6, 2)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Bishop_IsValidMove_ShouldRejectNonDiagonalMove");
        CustomOutput.WriteLine($"Bishop Position: {ChessNotationUtility.ToChessNotation(bishop.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The bishop should not be able to move non-diagonally.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldAllowCapture()
    {
        // Arrange
        var bishop = new Bishop("White", (7, 2));
        var blackPawn = new Pawn("Black", (5, 4));
        var chessBoard = InitializeCustomBoard((bishop, (7, 2)), (blackPawn, (5, 4)));
        var targetPosition = (5, 4); // Move to (5, 4)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Bishop_IsValidMove_ShouldAllowCapture");
        CustomOutput.WriteLine($"Bishop Position: {ChessNotationUtility.ToChessNotation(bishop.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The bishop should be able to capture an opponent's piece.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectMoveToOccupiedSquare()
    {
        // Arrange
        var bishop = new Bishop("White", (7, 2));
        var whitePawn = new Pawn("White", (5, 4));
        var chessBoard = InitializeCustomBoard((bishop, (7, 2)), (whitePawn, (5, 4)));
        var targetPosition = (5, 4); // Move to (5, 4)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Bishop_IsValidMove_ShouldRejectMoveToOccupiedSquare");
        CustomOutput.WriteLine($"Bishop Position: {ChessNotationUtility.ToChessNotation(bishop.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The bishop should not be able to move to a square occupied by a piece of the same color.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectMoveOutOfBounds()
    {
        // Arrange
        var bishop = new Bishop("White", (7, 2));
        var chessBoard = InitializeCustomBoard((bishop, (7, 2)));
        var targetPosition = (8, 3); // Move to (8, 3) - out of bounds

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Bishop_IsValidMove_ShouldRejectMoveOutOfBounds");
        CustomOutput.WriteLine($"Bishop Position: {ChessNotationUtility.ToChessNotation(bishop.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The bishop should not be able to move out of bounds.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectMoveIfPathIsNotClear()
    {
        // Arrange
        var bishop = new Bishop("White", (7, 2));
        var whitePawn = new Pawn("White", (6, 3));
        var chessBoard = InitializeCustomBoard((bishop, (7, 2)), (whitePawn, (6, 3)));
        var targetPosition = (5, 4); // Move to (5, 4)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Bishop_IsValidMove_ShouldRejectMoveIfPathIsNotClear");
        CustomOutput.WriteLine($"Bishop Position: {ChessNotationUtility.ToChessNotation(bishop.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The bishop should not be able to move if the path is not clear.");
    }
}





