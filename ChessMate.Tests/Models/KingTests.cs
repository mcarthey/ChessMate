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
    public void King_IsValidMove_ShouldAllowOneSquareMoveInAnyDirection()
    {
        // Arrange
        var king = new King("White", (4, 4));
        var chessBoard = InitializeCustomBoard((king, (4, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var validMoves = new List<(int Row, int Col)>
        {
            (3, 4), // Up
            (5, 4), // Down
            (4, 3), // Left
            (4, 5), // Right
            (3, 3), // Up-Left
            (3, 5), // Up-Right
            (5, 3), // Down-Left
            (5, 5), // Down-Right
        };

        foreach (var targetPosition in validMoves)
        {
            // Act
            bool isValid = king.IsValidMove(targetPosition, gameContext);

            // Assert
            Assert.True(isValid, $"The king should be able to move to {targetPosition}.");
        }
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectMoveMoreThanOneSquare()
    {
        // Arrange
        var king = new King("White", (4, 4));
        var chessBoard = InitializeCustomBoard((king, (4, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var invalidMoves = new List<(int Row, int Col)>
        {
            (2, 4), // Two squares up
            (6, 4), // Two squares down
            (4, 2), // Two squares left
            (4, 6), // Two squares right
            (2, 2), // Two squares up-left
            (6, 6), // Two squares down-right
        };

        foreach (var targetPosition in invalidMoves)
        {
            // Act
            bool isValid = king.IsValidMove(targetPosition, gameContext);

            // Assert
            Assert.False(isValid, $"The king should not be able to move to {targetPosition}.");
        }
    }

    [Fact]
    public void King_IsValidMove_ShouldAllowCaptureOfOpponentPiece()
    {
        // Arrange
        var king = new King("White", (4, 4));
        var opponentPawn = new Pawn("Black", (3, 4));
        var chessBoard = InitializeCustomBoard((king, (4, 4)), (opponentPawn, (3, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (3, 4); // Opponent's piece

        // Act
        bool isValid = king.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The king should be able to capture an opponent's piece.");
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectMoveToOccupiedSquareByOwnPiece()
    {
        // Arrange
        var king = new King("White", (4, 4));
        var ownPawn = new Pawn("White", (3, 4));
        var chessBoard = InitializeCustomBoard((king, (4, 4)), (ownPawn, (3, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (3, 4); // Own piece

        // Act
        bool isValid = king.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The king should not be able to move to a square occupied by its own piece.");
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectCastlingWhenNotImplemented()
    {
        // Arrange
        var king = new King("White", (7, 4));
        var chessBoard = InitializeCustomBoard((king, (7, 4)));
        var targetPosition = (7, 6); // Attempt to castle king-side

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act
        bool isValid = king.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The king should not be able to castle as castling is not implemented.");
    }

    [Fact]
    public void King_IsValidMove_ShouldRejectOutOfBoundsMove()
    {
        // Arrange
        var king = new King("White", (0, 0));
        var chessBoard = InitializeCustomBoard((king, (0, 0)));
        var targetPosition = (-1, 0); // Out of bounds

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => king.IsValidMove(targetPosition, gameContext));
    }
}

