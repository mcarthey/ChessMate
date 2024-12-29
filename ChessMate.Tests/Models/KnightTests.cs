// File: ChessMate.Tests/Models/KnightTests.cs

using ChessMate.Models;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Models;

public class KnightTests : TestHelper
{
    public KnightTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Knight_IsValidMove_ShouldAllowLShapedMoves()
    {
        // Arrange
        var knight = new Knight("White", new Position("e4"));
        var chessBoard = InitializeCustomBoard((knight, new Position("e4")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var validMoves = new List<Position>
        {
            new Position("d6"), // Up 2, Left 1
            new Position("f6"), // Up 2, Right 1
            new Position("c5"), // Up 1, Left 2
            new Position("g5"), // Up 1, Right 2
            new Position("c3"), // Down 1, Left 2
            new Position("g3"), // Down 1, Right 2
            new Position("d2"), // Down 2, Left 1
            new Position("f2"), // Down 2, Right 1
        };

        foreach (var targetPosition in validMoves)
        {
            // Act
            bool isValid = knight.IsValidMove(targetPosition, gameContext);

            // Assert
            Assert.True(isValid, $"The knight should be able to move to {targetPosition}.");
        }
    }

    [Fact]
    public void Knight_IsValidMove_ShouldRejectInvalidMoves()
    {
        // Arrange
        var knight = new Knight("White", new Position("e4"));
        var chessBoard = InitializeCustomBoard((knight, new Position("e4")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var invalidMoves = new List<Position>
        {
            new Position("e5"), // One square right
            new Position("f5"), // One square diagonal
            new Position("h7"), // Far away
            new Position("e4"), // Same position
        };

        foreach (var targetPosition in invalidMoves)
        {
            // Act
            bool isValid = knight.IsValidMove(targetPosition, gameContext);

            // Assert
            Assert.False(isValid, $"The knight should not be able to move to {targetPosition}.");
        }
    }

    [Fact]
    public void Knight_IsValidMove_ShouldAllowCaptureOfOpponentPiece()
    {
        // Arrange
        var knight = new Knight("White", new Position("e4"));
        var opponentPawn = new Pawn("Black", new Position("d6"));
        var chessBoard = InitializeCustomBoard((knight, new Position("e4")), (opponentPawn, new Position("d6")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("d6"); // Opponent's piece

        // Act
        bool isValid = knight.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The knight should be able to capture an opponent's piece.");
    }

    [Fact]
    public void Knight_IsValidMove_ShouldNotCaptureOwnPiece()
    {
        // Arrange
        var knight = new Knight("White", new Position("e4"));
        var ownPawn = new Pawn("White", new Position("d6"));
        var chessBoard = InitializeCustomBoard((knight, new Position("e4")), (ownPawn, new Position("d6")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("d6"); // Own piece

        // Act
        bool isValid = knight.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The knight should not be able to capture its own piece.");
    }

    [Fact]
    public void Knight_IsValidMove_ShouldIgnorePiecesInTheWay()
    {
        // Arrange
        var knight = new Knight("White", new Position("b1"));
        var blockingPiece1 = new Pawn("White", new Position("b2"));
        var blockingPiece2 = new Pawn("Black", new Position("c2"));

        var chessBoard = InitializeCustomBoard(
            (knight, new Position("b1")),
            (blockingPiece1, new Position("b2")),
            (blockingPiece2, new Position("c2"))
        );

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("c3"); // Valid L-shaped move

        // Act
        bool isValid = knight.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The knight should be able to move regardless of pieces in the way.");
    }

    [Fact]
    public void Knight_IsValidMove_ShouldRejectOutOfBoundsMove()
    {
        // Arrange
        var knight = new Knight("White", new Position("a1"));
        var chessBoard = InitializeCustomBoard((knight, new Position("a1")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position(-2, -1); // Out of bounds

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => knight.IsValidMove(targetPosition, gameContext));
    }
}


