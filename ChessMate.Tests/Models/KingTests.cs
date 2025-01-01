// File: ChessMate.Tests/Models/KingTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Xunit;
using Xunit.Abstractions;
using Moq;

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
        var chessBoard = InitializeCustomBoard((king, king.Position));

        var targetPosition = new Position("e2"); // Move to e2

        var gameContextBuilder = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White");

        // Configure the StateService mock if needed
        var gameContext = gameContextBuilder.Build();

        // Update attack maps to ensure accurate state
        gameContext.State.UpdateAttackMaps(gameContext);

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
        var chessBoard = InitializeCustomBoard((king, king.Position));
        var targetPosition = new Position("e3"); // Move to e3 - more than one square

        var gameContextBuilder = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White");

        var gameContext = gameContextBuilder.Build();

        // No need to update attack maps since move is invalid due to distance

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
        var chessBoard = InitializeCustomBoard((king, king.Position), (blackPawn, blackPawn.Position));
        var targetPosition = blackPawn.Position; // Capture at e2

        var gameContextBuilder = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White");

        var gameContext = gameContextBuilder.Build();

        // Update attack maps
        gameContext.State.UpdateAttackMaps(gameContext);

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
        var chessBoard = InitializeCustomBoard((king, king.Position), (whitePawn, whitePawn.Position));
        var targetPosition = whitePawn.Position; // Attempt to move to e2

        var gameContextBuilder = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White");

        var gameContext = gameContextBuilder.Build();

        // No need to update attack maps since move is invalid due to same color piece

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
        var chessBoard = InitializeCustomBoard((king, king.Position));
        var targetPosition = new Position(-1, 0); // Out of bounds position

        var gameContextBuilder = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White");

        var gameContext = gameContextBuilder.Build();

        // Act
        bool isValid = king.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The king should not be able to move out of bounds.");
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectMoveIntoCheck()
    {
        // Arrange
        var king = new King("White", new Position("e1"));
        var blackRook = new Rook("Black", new Position("e3"));
        var chessBoard = InitializeCustomBoard((king, king.Position), (blackRook, blackRook.Position));
        var targetPosition = new Position("e2"); // Attempt to move to e2

        var gameContextBuilder = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White");

        // Configure the StateService mock if needed
        var gameContext = gameContextBuilder.Build();

        // Update attack maps to reflect the black rook's attack
        gameContext.State.UpdateAttackMaps(gameContext);

        // Act
        bool isValid = king.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The king should not be able to move into check.");
    }

    [Fact]
    public void King_IsValidMove_ShouldConsiderAttackMaps()
    {
        // Arrange
        var king = new King("White", new Position("e1"));
        var blackQueen = new Queen("Black", new Position("d2"));
        var chessBoard = InitializeCustomBoard((king, king.Position), (blackQueen, blackQueen.Position));

        var targetPosition = new Position("e2"); // Square that is under attack by black queen

        var gameContextBuilder = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White");

        var gameContext = gameContextBuilder.Build();

        // Update attack maps to ensure accurate state
        gameContext.State.UpdateAttackMaps(gameContext);

        // Act
        bool isValid = king.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The king should not be able to move into a square that is under attack.");

        gameContextBuilder.StateServiceMock.Verify(
            s => s.UpdateAttackMaps(It.IsAny<IGameContext>()), Times.Once());
    }

}

