using ChessMate.Models;
using ChessMate.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Models;

public class RookTests : TestHelper
{
    public RookTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Rook_IsValidMove_ShouldAllowStraightMove()
    {
        // Arrange
        var rook = new Rook("White", (7, 0));
        var chessBoard = InitializeCustomBoard((rook, (7, 0)));
        var targetPosition = (7, 5); // Move to (7, 5)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = rook.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Rook_IsValidMove_ShouldAllowStraightMove");
        CustomOutput.WriteLine($"Rook Position: {ChessNotationUtility.ToChessNotation(rook.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The rook should be able to move straight.");
    }

    [Fact]
    public void Rook_IsValidMove_ShouldRejectDiagonalMove()
    {
        // Arrange
        var rook = new Rook("White", (7, 0));
        var chessBoard = InitializeCustomBoard((rook, (7, 0)));
        var targetPosition = (5, 2); // Move to (5, 2)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = rook.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Rook_IsValidMove_ShouldRejectDiagonalMove");
        CustomOutput.WriteLine($"Rook Position: {ChessNotationUtility.ToChessNotation(rook.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The rook should not be able to move diagonally.");
    }

    [Fact]
    public void Rook_IsValidMove_ShouldAllowCapture()
    {
        // Arrange
        var rook = new Rook("White", (7, 0));
        var blackPawn = new Pawn("Black", (7, 5));
        var chessBoard = InitializeCustomBoard((rook, (7, 0)), (blackPawn, (7, 5)));
        var targetPosition = (7, 5); // Move to (7, 5)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = rook.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Rook_IsValidMove_ShouldAllowCapture");
        CustomOutput.WriteLine($"Rook Position: {ChessNotationUtility.ToChessNotation(rook.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The rook should be able to capture an opponent's piece.");
    }

    [Fact]
    public void Rook_IsValidMove_ShouldRejectMoveToOccupiedSquare()
    {
        // Arrange
        var rook = new Rook("White", (7, 0));
        var whitePawn = new Pawn("White", (7, 5));
        var chessBoard = InitializeCustomBoard((rook, (7, 0)), (whitePawn, (7, 5)));
        var targetPosition = (7, 5); // Move to (7, 5)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = rook.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Rook_IsValidMove_ShouldRejectMoveToOccupiedSquare");
        CustomOutput.WriteLine($"Rook Position: {ChessNotationUtility.ToChessNotation(rook.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The rook should not be able to move to a square occupied by a piece of the same color.");
    }

    [Fact]
    public void Rook_IsValidMove_ShouldRejectMoveOutOfBounds()
    {
        // Arrange
        var rook = new Rook("White", (7, 0));
        var chessBoard = InitializeCustomBoard((rook, (7, 0)));
        var targetPosition = (8, 0); // Move to (8, 0) - out of bounds

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = rook.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Rook_IsValidMove_ShouldRejectMoveOutOfBounds");
        CustomOutput.WriteLine($"Rook Position: {ChessNotationUtility.ToChessNotation(rook.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The rook should not be able to move out of bounds.");
    }

    [Fact]
    public void Rook_IsValidMove_ShouldRejectMoveIfPathIsNotClear()
    {
        // Arrange
        var rook = new Rook("White", (7, 0));
        var whitePawn = new Pawn("White", (7, 3));
        var chessBoard = InitializeCustomBoard((rook, (7, 0)), (whitePawn, (7, 3)));
        var targetPosition = (7, 5); // Move to (7, 5)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = rook.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Rook_IsValidMove_ShouldRejectMoveIfPathIsNotClear");
        CustomOutput.WriteLine($"Rook Position: {ChessNotationUtility.ToChessNotation(rook.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The rook should not be able to move if the path is not clear.");
    }
}






