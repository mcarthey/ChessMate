// File: ChessMate.Tests/Models/BishopTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
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
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (5, 4); // Move to (5, 4)

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The bishop should be able to move diagonally.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectNonDiagonalMove()
    {
        // Arrange
        var bishop = new Bishop("White", (7, 2));
        var chessBoard = InitializeCustomBoard((bishop, (7, 2)));
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (6, 2); // Move to (6, 2)

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

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
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (5, 4); // Move to (5, 4)

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

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
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (5, 4); // Move to (5, 4)

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The bishop should not be able to move to a square occupied by a piece of the same color.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectMoveOutOfBounds()
    {
        // Arrange
        var bishop = new Bishop("White", (7, 2));
        var chessBoard = InitializeCustomBoard((bishop, (7, 2)));
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (8, 3); // Move to (8, 3) - out of bounds

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => bishop.IsValidMove(targetPosition, gameContext));
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectMoveIfPathIsNotClear()
    {
        // Arrange
        var bishop = new Bishop("White", (7, 2));
        var blockingPawn = new Pawn("White", (6, 3));
        var chessBoard = InitializeCustomBoard((bishop, (7, 2)), (blockingPawn, (6, 3)));
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (5, 4); // Move to (5, 4)

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The bishop should not be able to move if the path is not clear.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectMoveThroughOpponentPiece()
    {
        // Arrange
        var bishop = new Bishop("White", (7, 2));
        var blackKnight = new Knight("Black", (6, 3));
        var chessBoard = InitializeCustomBoard((bishop, (7, 2)), (blackKnight, (6, 3)));
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (5, 4); // Move to (5, 4)

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The bishop should not be able to move through an opponent's piece.");
    }

}
