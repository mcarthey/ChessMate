// File: ChessMate.Tests/Models/BishopTests.cs

using ChessMate.Models;
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
        var bishop = new Bishop("White", new Position("c1"));
        var chessBoard = InitializeCustomBoard((bishop, new Position("c1")));
        var targetPosition = new Position("e3"); // Move to e3

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The bishop should be able to move diagonally.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectNonDiagonalMove()
    {
        // Arrange
        var bishop = new Bishop("White", new Position("c1"));
        var chessBoard = InitializeCustomBoard((bishop, new Position("c1")));
        var targetPosition = new Position("c2"); // Move to c2 - non-diagonal move

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The bishop should not be able to move non-diagonally.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldAllowCapture()
    {
        // Arrange
        var bishop = new Bishop("White", new Position("c1"));
        var blackPawn = new Pawn("Black", new Position("e3"));
        var chessBoard = InitializeCustomBoard((bishop, new Position("c1")), (blackPawn, new Position("e3")));
        var targetPosition = new Position("e3"); // Capture at e3

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The bishop should be able to capture an opponent's piece diagonally.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectMoveToOccupiedSquareBySameColor()
    {
        // Arrange
        var bishop = new Bishop("White", new Position("c1"));
        var whitePawn = new Pawn("White", new Position("e3"));
        var chessBoard = InitializeCustomBoard((bishop, new Position("c1")), (whitePawn, new Position("e3")));
        var targetPosition = new Position("e3"); // Attempt to move to e3

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The bishop should not be able to move to a square occupied by a piece of the same color.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectMoveOutOfBounds()
    {
        // Arrange
        var bishop = new Bishop("White", new Position("c1"));
        var chessBoard = InitializeCustomBoard((bishop, new Position("c1")));
        var targetPosition = new Position("i5"); // Out of bounds position

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => bishop.IsValidMove(targetPosition, gameContext));
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectMoveIfPathIsNotClear()
    {
        // Arrange
        var bishop = new Bishop("White", new Position("c1"));
        var blockingPawn = new Pawn("White", new Position("d2"));
        var chessBoard = InitializeCustomBoard((bishop, new Position("c1")), (blockingPawn, new Position("d2")));
        var targetPosition = new Position("e3"); // Attempt to move to e3

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The bishop should not be able to move if the path is blocked by a piece.");
    }

    [Fact]
    public void Bishop_IsValidMove_ShouldRejectMoveThroughOpponentPiece()
    {
        // Arrange
        var bishop = new Bishop("White", new Position("c1"));
        var blackKnight = new Knight("Black", new Position("d2"));
        var chessBoard = InitializeCustomBoard((bishop, new Position("c1")), (blackKnight, new Position("d2")));
        var targetPosition = new Position("e3"); // Attempt to move to e3

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = bishop.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The bishop should not be able to move through an opponent's piece.");
    }
}

