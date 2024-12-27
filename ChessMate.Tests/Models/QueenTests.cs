// File: ChessMate.Tests/Models/QueenTests.cs

using ChessMate.Models;
using Moq;
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
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (5, 5); // Move diagonally

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The queen should be able to move diagonally.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldAllowStraightMove()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var chessBoard = InitializeCustomBoard((queen, (7, 3)));
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (4, 3); // Move vertically

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The queen should be able to move straight.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldRejectInvalidMove()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var chessBoard = InitializeCustomBoard((queen, (7, 3)));
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (6, 5); // Invalid move

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The queen should not be able to move in an invalid pattern.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldAllowCapture()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var opponentPawn = new Pawn("Black", (5, 5));
        var chessBoard = InitializeCustomBoard(
            (queen, (7, 3)),
            (opponentPawn, (5, 5))
        );
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (5, 5); // Capture opponent's piece

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The queen should be able to capture an opponent's piece.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldRejectMoveToOccupiedSquareByOwnPiece()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var ownPawn = new Pawn("White", (5, 5));
        var chessBoard = InitializeCustomBoard(
            (queen, (7, 3)),
            (ownPawn, (5, 5))
        );
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (5, 5); // Square occupied by own piece

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The queen should not be able to move to a square occupied by its own piece.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldRejectMoveIfPathIsBlocked()
    {
        // Arrange
        var queen = new Queen("White", (7, 3));
        var blockingPiece = new Pawn("White", (6, 4));
        var chessBoard = InitializeCustomBoard(
            (queen, (7, 3)),
            (blockingPiece, (6, 4))
        );
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (5, 5); // Path is blocked

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The queen should not be able to move if the path is blocked.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldRejectOutOfBoundsMove()
    {
        // Arrange
        var queen = new Queen("White", (0, 0));
        var chessBoard = InitializeCustomBoard((queen, (0, 0)));
        var gameContext = GetMockedGameContext(chessBoard, "White");
        var targetPosition = (-1, -1); // Out of bounds

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => queen.IsValidMove(targetPosition, gameContext));
    }
}

