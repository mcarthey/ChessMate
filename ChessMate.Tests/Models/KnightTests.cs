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
        var knight = new Knight("White", (4, 4));
        var chessBoard = InitializeCustomBoard((knight, (4, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var validMoves = new List<(int Row, int Col)>
        {
            (2, 3), // Up 2, Left 1
            (2, 5), // Up 2, Right 1
            (3, 2), // Up 1, Left 2
            (3, 6), // Up 1, Right 2
            (5, 2), // Down 1, Left 2
            (5, 6), // Down 1, Right 2
            (6, 3), // Down 2, Left 1
            (6, 5), // Down 2, Right 1
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
        var knight = new Knight("White", (4, 4));
        var chessBoard = InitializeCustomBoard((knight, (4, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var invalidMoves = new List<(int Row, int Col)>
        {
            (4, 5), // One square right
            (5, 5), // One square diagonal
            (7, 7), // Far away
            (4, 4), // Same position
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
        var knight = new Knight("White", (4, 4));
        var opponentPawn = new Pawn("Black", (2, 3));
        var chessBoard = InitializeCustomBoard((knight, (4, 4)), (opponentPawn, (2, 3)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (2, 3); // Opponent's piece

        // Act
        bool isValid = knight.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The knight should be able to capture an opponent's piece.");
    }

    [Fact]
    public void Knight_IsValidMove_ShouldNotCaptureOwnPiece()
    {
        // Arrange
        var knight = new Knight("White", (4, 4));
        var ownPawn = new Pawn("White", (2, 3));
        var chessBoard = InitializeCustomBoard((knight, (4, 4)), (ownPawn, (2, 3)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (2, 3); // Own piece

        // Act
        bool isValid = knight.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The knight should not be able to capture its own piece.");
    }

    [Fact]
    public void Knight_IsValidMove_ShouldIgnorePiecesInTheWay()
    {
        // Arrange
        var knight = new Knight("White", (7, 1));
        var blockingPiece1 = new Pawn("White", (6, 1));
        var blockingPiece2 = new Pawn("Black", (6, 2));

        var chessBoard = InitializeCustomBoard(
            (knight, (7, 1)),
            (blockingPiece1, (6, 1)),
            (blockingPiece2, (6, 2))
        );

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (5, 2); // Valid L-shaped move

        // Act
        bool isValid = knight.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The knight should be able to move regardless of pieces in the way.");
    }

    [Fact]
    public void Knight_IsValidMove_ShouldRejectOutOfBoundsMove()
    {
        // Arrange
        var knight = new Knight("White", (0, 0));
        var chessBoard = InitializeCustomBoard((knight, (0, 0)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (-2, -1); // Out of bounds

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => knight.IsValidMove(targetPosition, gameContext));
    }
}
