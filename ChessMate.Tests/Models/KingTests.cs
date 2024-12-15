using ChessMate.Models;
using ChessMate.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Models;

public class KingTests : TestHelper
{
    public KingTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void King_IsValidMove_ShouldAllowSingleSquareMove()
    {
        // Arrange
        var king = new King("White", (7, 4));
        var chessBoard = InitializeCustomBoard((king, (7, 4)));
        var targetPosition = (6, 4); // Move to (6, 4)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = king.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: King_IsValidMove_ShouldAllowSingleSquareMove");
        CustomOutput.WriteLine($"King Position: {ChessNotationUtility.ToChessNotation(king.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The king should be able to move one square forward.");
    }

    [Fact]
    public void King_IsValidMove_ShouldAllowDiagonalMove()
    {
        // Arrange
        var king = new King("White", (7, 4));
        var chessBoard = InitializeCustomBoard((king, (7, 4)));
        var targetPosition = (6, 5); // Move to (6, 5)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = king.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: King_IsValidMove_ShouldAllowDiagonalMove");
        CustomOutput.WriteLine($"King Position: {ChessNotationUtility.ToChessNotation(king.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The king should be able to move one square diagonally.");
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectInvalidMove()
    {
        // Arrange
        var king = new King("White", (7, 4));
        var chessBoard = InitializeCustomBoard((king, (7, 4)));
        var targetPosition = (5, 4); // Move to (5, 4)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = king.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: King_IsValidMove_ShouldRejectInvalidMove");
        CustomOutput.WriteLine($"King Position: {ChessNotationUtility.ToChessNotation(king.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The king should not be able to move two squares forward.");
    }

    [Fact]
    public void King_IsValidMove_ShouldAllowCapture()
    {
        // Arrange
        var king = new King("White", (7, 4));
        var blackPawn = new Pawn("Black", (6, 4));
        var chessBoard = InitializeCustomBoard((king, (7, 4)), (blackPawn, (6, 4)));
        var targetPosition = (6, 4); // Move to (6, 4)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = king.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: King_IsValidMove_ShouldAllowCapture");
        CustomOutput.WriteLine($"King Position: {ChessNotationUtility.ToChessNotation(king.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The king should be able to capture an opponent's piece.");
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectMoveToOccupiedSquare()
    {
        // Arrange
        var king = new King("White", (7, 4));
        var whitePawn = new Pawn("White", (6, 4));
        var chessBoard = InitializeCustomBoard((king, (7, 4)), (whitePawn, (6, 4)));
        var targetPosition = (6, 4); // Move to (6, 4)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = king.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: King_IsValidMove_ShouldRejectMoveToOccupiedSquare");
        CustomOutput.WriteLine($"King Position: {ChessNotationUtility.ToChessNotation(king.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The king should not be able to move to a square occupied by a piece of the same color.");
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectMoveOutOfBounds()
    {
        // Arrange
        var king = new King("White", (7, 4));
        var chessBoard = InitializeCustomBoard((king, (7, 4)));
        var targetPosition = (8, 4); // Move to (8, 4) - out of bounds

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = king.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: King_IsValidMove_ShouldRejectMoveOutOfBounds");
        CustomOutput.WriteLine($"King Position: {ChessNotationUtility.ToChessNotation(king.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The king should not be able to move out of bounds.");
    }
}


