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
    public void Queen_IsValidMove_ShouldAllowVerticalMove()
    {
        // Arrange
        var queen = new Queen("White", new Position("e4"));
        var chessBoard = InitializeCustomBoard((queen, new Position("e4")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("e1"); // Move vertically

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The queen should be able to move vertically.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldAllowHorizontalMove()
    {
        // Arrange
        var queen = new Queen("White", new Position("e4"));
        var chessBoard = InitializeCustomBoard((queen, new Position("e4")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("h4"); // Move horizontally

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The queen should be able to move horizontally.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldAllowDiagonalMove()
    {
        // Arrange
        var queen = new Queen("White", new Position("e4"));
        var chessBoard = InitializeCustomBoard((queen, new Position("e4")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("g6"); // Move diagonally

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The queen should be able to move diagonally.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldRejectMoveThroughPieces()
    {
        // Arrange
        var queen = new Queen("White", new Position("e4"));
        var blockingPiece = new Pawn("White", new Position("f5"));
        var chessBoard = InitializeCustomBoard(
            (queen, new Position("e4")),
            (blockingPiece, new Position("f5"))
        );

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("g6"); // Attempt to move diagonally through the pawn

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The queen should not be able to move through other pieces.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldRejectInvalidMove()
    {
        // Arrange
        var queen = new Queen("White", new Position("e4"));
        var chessBoard = InitializeCustomBoard((queen, new Position("e4")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("d6"); // Invalid move (not straight or diagonal)

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The queen should reject invalid moves.");
    }

    [Fact]
    public void Queen_IsValidMove_ShouldCaptureOpponentPiece()
    {
        // Arrange
        var queen = new Queen("White", new Position("e4"));
        var opponentPawn = new Pawn("Black", new Position("g6"));
        var chessBoard = InitializeCustomBoard(
            (queen, new Position("e4")),
            (opponentPawn, new Position("g6"))
        );

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("g6"); // Capture opponent's pawn

        // Act
        bool isValid = queen.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The queen should be able to capture an opponent's piece.");
    }

    [Fact]
    public void Queen_OnMoved_ShouldNotUpdatePositionOrSwitchPlayer()
    {
        // Arrange
        var queen = new Queen("White", new Position("e4"));
        var chessBoard = InitializeCustomBoard((queen, new Position("e4")));

        var gameContextBuilder = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White");

        var gameContext = gameContextBuilder.Build();

        var targetPosition = new Position("g6");

        // Act
        queen.OnMoved(targetPosition, gameContext);

        // Assert
        Assert.NotEqual(targetPosition, queen.Position); // Position should not be updated
        gameContextBuilder.StateServiceMock.Verify(s => s.SwitchPlayer(), Times.Never()); // Player should not be switched
    }
}



