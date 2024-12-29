// File: ChessMate.Tests/Models/RookTests.cs

using ChessMate.Models;
using Moq;
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
        var rook = new Rook("White", new Position("a1"));
        var chessBoard = InitializeCustomBoard((rook, new Position("a1")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();
        var targetPosition = new Position("a6"); // Move horizontally

        // Act
        bool isValid = rook.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The rook should be able to move horizontally.");
    }

    [Fact]
    public void Rook_IsValidMove_ShouldAllowVerticalMove()
    {
        // Arrange
        var rook = new Rook("White", new Position("a1"));
        var chessBoard = InitializeCustomBoard((rook, new Position("a1")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();
        var targetPosition = new Position("d1"); // Move vertically

        // Act
        bool isValid = rook.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The rook should be able to move vertically.");
    }

    [Fact]
    public void Rook_IsValidMove_ShouldRejectDiagonalMove()
    {
        // Arrange
        var rook = new Rook("White", new Position("a1"));
        var chessBoard = InitializeCustomBoard((rook, new Position("a1")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();
        var targetPosition = new Position("c3"); // Diagonal move

        // Act
        bool isValid = rook.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The rook should not be able to move diagonally.");
    }

    [Fact]
    public void Rook_IsValidMove_ShouldAllowCapture()
    {
        // Arrange
        var rook = new Rook("White", new Position("a1"));
        var opponentPawn = new Pawn("Black", new Position("a6"));
        var chessBoard = InitializeCustomBoard(
            (rook, new Position("a1")),
            (opponentPawn, new Position("a6"))
        );

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();
        var targetPosition = new Position("a6"); // Capture opponent's piece

        // Act
        bool isValid = rook.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The rook should be able to capture an opponent's piece.");
    }

    [Fact]
    public void Rook_IsValidMove_ShouldRejectMoveToOccupiedSquareByOwnPiece()
    {
        // Arrange
        var rook = new Rook("White", new Position("a1"));
        var ownPawn = new Pawn("White", new Position("a6"));
        var chessBoard = InitializeCustomBoard(
            (rook, new Position("a1")),
            (ownPawn, new Position("a6"))
        );

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();
        var targetPosition = new Position("a6"); // Square occupied by own piece

        // Act
        bool isValid = rook.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The rook should not be able to move to a square occupied by its own piece.");
    }

    [Fact]
    public void Rook_IsValidMove_ShouldRejectMoveIfPathIsBlocked()
    {
        // Arrange
        var rook = new Rook("White", new Position("a1"));
        var blockingPiece = new Pawn("White", new Position("a4"));
        var chessBoard = InitializeCustomBoard(
            (rook, new Position("a1")),
            (blockingPiece, new Position("a4"))
        );

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();
        var targetPosition = new Position("a6"); // Path is blocked

        // Act
        bool isValid = rook.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The rook should not be able to move if the path is blocked.");
    }

    [Fact]
    public void Rook_IsValidMove_ShouldRejectOutOfBoundsMove()
    {
        // Arrange
        var rook = new Rook("White", new Position("a1"));
        var chessBoard = InitializeCustomBoard((rook, new Position("a1")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();
        var targetPosition = new Position(-1, 0); // Out of bounds

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => rook.IsValidMove(targetPosition, gameContext));
    }
}



