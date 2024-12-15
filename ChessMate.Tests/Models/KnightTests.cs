using ChessMate.Models;
using ChessMate.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Models;

public class KnightTests : TestHelper
{
    public KnightTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Knight_IsValidMove_ShouldAllowLShapedMove()
    {
        // Arrange
        var knight = new Knight("White", (7, 1));
        var chessBoard = InitializeCustomBoard((knight, (7, 1)));
        var targetPosition = (5, 2); // Move to (5, 2)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = knight.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Knight_IsValidMove_ShouldAllowLShapedMove");
        CustomOutput.WriteLine($"Knight Position: {ChessNotationUtility.ToChessNotation(knight.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The knight should be able to move in an L-shape.");
    }

    [Fact]
    public void Knight_IsValidMove_ShouldRejectInvalidMove()
    {
        // Arrange
        var knight = new Knight("White", (7, 1));
        var chessBoard = InitializeCustomBoard((knight, (7, 1)));
        var targetPosition = (6, 1); // Move to (6, 1)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = knight.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Knight_IsValidMove_ShouldRejectInvalidMove");
        CustomOutput.WriteLine($"Knight Position: {ChessNotationUtility.ToChessNotation(knight.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The knight should not be able to move in a non-L-shape.");
    }

    [Fact]
    public void Knight_IsValidMove_ShouldAllowCapture()
    {
        // Arrange
        var knight = new Knight("White", (7, 1));
        var blackPawn = new Pawn("Black", (5, 2));
        var chessBoard = InitializeCustomBoard((knight, (7, 1)), (blackPawn, (5, 2)));
        var targetPosition = (5, 2); // Move to (5, 2)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = knight.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Knight_IsValidMove_ShouldAllowCapture");
        CustomOutput.WriteLine($"Knight Position: {ChessNotationUtility.ToChessNotation(knight.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The knight should be able to capture an opponent's piece.");
    }

    [Fact]
    public void Knight_IsValidMove_ShouldRejectMoveToOccupiedSquare()
    {
        // Arrange
        var knight = new Knight("White", (7, 1));
        var whitePawn = new Pawn("White", (5, 2));
        var chessBoard = InitializeCustomBoard((knight, (7, 1)), (whitePawn, (5, 2)));
        var targetPosition = (5, 2); // Move to (5, 2)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = knight.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Knight_IsValidMove_ShouldRejectMoveToOccupiedSquare");
        CustomOutput.WriteLine($"Knight Position: {ChessNotationUtility.ToChessNotation(knight.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The knight should not be able to move to a square occupied by a piece of the same color.");
    }

    [Fact]
    public void Knight_IsValidMove_ShouldRejectMoveOutOfBounds()
    {
        // Arrange
        var knight = new Knight("White", (7, 1));
        var chessBoard = InitializeCustomBoard((knight, (7, 1)));
        var targetPosition = (8, 3); // Move to (8, 3) - out of bounds

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = knight.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Knight_IsValidMove_ShouldRejectMoveOutOfBounds");
        CustomOutput.WriteLine($"Knight Position: {ChessNotationUtility.ToChessNotation(knight.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The knight should not be able to move out of bounds.");
    }
}




