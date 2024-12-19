using ChessMate.Models;
using ChessMate.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Models;

public class QueenTests : TestHelper
{
    public QueenTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Queen_IsValidMove_ShouldAllowDiagonalMove()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var chessBoard = InitializeCustomBoard((queen, (7, 3)));
        var targetPosition = (5, 5); // Move to (5, 5)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = queen.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Queen_IsValidMove_ShouldAllowDiagonalMove");
        CustomOutput.WriteLine($"Queen Position: {ChessNotationUtility.ToChessNotation(queen.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The queen should be able to move diagonally.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldAllowStraightMove()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var chessBoard = InitializeCustomBoard((queen, (7, 3)));
        var targetPosition = (7, 5); // Move to (7, 5)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = queen.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Queen_IsValidMove_ShouldAllowStraightMove");
        CustomOutput.WriteLine($"Queen Position: {ChessNotationUtility.ToChessNotation(queen.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The queen should be able to move straight.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldRejectInvalidMove()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var chessBoard = InitializeCustomBoard((queen, (7, 3)));
        var targetPosition = (6, 5); // Move to (6, 5)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = queen.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Queen_IsValidMove_ShouldRejectInvalidMove");
        CustomOutput.WriteLine($"Queen Position: {ChessNotationUtility.ToChessNotation(queen.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The queen should not be able to move in an invalid pattern.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldAllowCapture()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var blackPawn = new Pawn("Black", (5, 5));
        var chessBoard = InitializeCustomBoard((queen, (7, 3)), (blackPawn, (5, 5)));
        var targetPosition = (5, 5); // Move to (5, 5)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = queen.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Queen_IsValidMove_ShouldAllowCapture");
        CustomOutput.WriteLine($"Queen Position: {ChessNotationUtility.ToChessNotation(queen.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The queen should be able to capture an opponent's piece.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldRejectMoveToOccupiedSquare()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var whitePawn = new Pawn("White", (5, 5));
        var chessBoard = InitializeCustomBoard((queen, (7, 3)), (whitePawn, (5, 5)));
        var targetPosition = (5, 5); // Move to (5, 5)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = queen.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Queen_IsValidMove_ShouldRejectMoveToOccupiedSquare");
        CustomOutput.WriteLine($"Queen Position: {ChessNotationUtility.ToChessNotation(queen.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The queen should not be able to move to a square occupied by a piece of the same color.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldRejectMoveOutOfBounds()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var chessBoard = InitializeCustomBoard((queen, (7, 3)));
        var targetPosition = (8, 3); // Move to (8, 3) - out of bounds

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = queen.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Queen_IsValidMove_ShouldRejectMoveOutOfBounds");
        CustomOutput.WriteLine($"Queen Position: {ChessNotationUtility.ToChessNotation(queen.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The queen should not be able to move out of bounds.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldRejectMoveIfPathIsNotClear()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var whitePawn = new Pawn("White", (6, 4));
        var chessBoard = InitializeCustomBoard((queen, (7, 3)), (whitePawn, (6, 4)));
        var targetPosition = (5, 5); // Move to (5, 5)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = queen.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Queen_IsValidMove_ShouldRejectMoveIfPathIsNotClear");
        CustomOutput.WriteLine($"Queen Position: {ChessNotationUtility.ToChessNotation(queen.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The queen should not be able to move if the path is not clear.");
    }
}











