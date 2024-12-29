// File: ChessMate.Tests/Models/KingTests.cs

using ChessMate.Models;
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
        var king = new King("White", new Position("e1"));
        var chessBoard = InitializeCustomBoard((king, new Position("e1")));
        var targetPosition = new Position("e2"); // Move to e2

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = king.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The king should be able to move one square in any direction.");
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectMoveMoreThanOneSquare()
    {
        // Arrange
        var king = new King("White", new Position("e1"));
        var chessBoard = InitializeCustomBoard((king, new Position("e1")));
        var targetPosition = new Position("e3"); // Move to e3 - more than one square

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = king.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The king should not be able to move more than one square in any direction.");
    }

    [Fact]
    public void King_IsValidMove_ShouldAllowCapture()
    {
        // Arrange
        var king = new King("White", new Position("e1"));
        var blackPawn = new Pawn("Black", new Position("e2"));
        var chessBoard = InitializeCustomBoard((king, new Position("e1")), (blackPawn, new Position("e2")));
        var targetPosition = new Position("e2"); // Capture at e2

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = king.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The king should be able to capture an opponent's piece.");
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectMoveToOccupiedSquareBySameColor()
    {
        // Arrange
        var king = new King("White", new Position("e1"));
        var whitePawn = new Pawn("White", new Position("e2"));
        var chessBoard = InitializeCustomBoard((king, new Position("e1")), (whitePawn, new Position("e2")));
        var targetPosition = new Position("e2"); // Attempt to move to e2

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = king.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The king should not be able to move to a square occupied by a piece of the same color.");
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectMoveOutOfBounds()
    {
        // Arrange
        var king = new King("White", new Position("e1"));
        var chessBoard = InitializeCustomBoard((king, new Position("e1")));
        var targetPosition = new Position(-1, 0); // Out of bounds position

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => king.IsValidMove(targetPosition, gameContext));
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectMoveIntoCheck()
    {
        // Arrange
        var king = new King("White", new Position("e1"));
        var blackRook = new Rook("Black", new Position("e8"));
        var chessBoard = InitializeCustomBoard((king, new Position("e1")), (blackRook, new Position("e8")));
        var targetPosition = new Position("e2"); // Attempt to move to e2

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = king.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The king should not be able to move into check.");
    }
}


