// File: ChessMate.Tests/Models/PawnTests.cs

using ChessMate.Models;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Models;

public class PawnTests : TestHelper
{
    public PawnTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldAllowSingleSquareMove()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 4));
        var chessBoard = InitializeCustomBoard((pawn, (6, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (5, 4); // Move forward one square

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The pawn should be able to move one square forward.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldAllowDoubleSquareMoveOnFirstMove()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 4));
        var chessBoard = InitializeCustomBoard((pawn, (6, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (4, 4); // Move forward two squares

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The pawn should be able to move two squares forward on its first move.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectDoubleSquareMoveAfterFirstMove()
    {
        // Arrange
        var pawn = new Pawn("White", (5, 4));
        var chessBoard = InitializeCustomBoard((pawn, (5, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (3, 4); // Attempt to move two squares forward

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The pawn should not be able to move two squares forward after its first move.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldAllowDiagonalCapture()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 4));
        var opponentPawn = new Pawn("Black", (5, 5));
        var chessBoard = InitializeCustomBoard((pawn, (6, 4)), (opponentPawn, (5, 5)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (5, 5); // Capture diagonally

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The pawn should be able to capture an opponent's piece diagonally.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectInvalidDiagonalMove()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 4));
        var chessBoard = InitializeCustomBoard((pawn, (6, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (5, 5); // Diagonal move without capture

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The pawn should not be able to move diagonally without capturing.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectBackwardMove()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 4));
        var chessBoard = InitializeCustomBoard((pawn, (6, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (7, 4); // Move backward

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The pawn should not be able to move backward.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectMoveToOccupiedSquare()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 4));
        var blockingPawn = new Pawn("Black", (5, 4));
        var chessBoard = InitializeCustomBoard((pawn, (6, 4)), (blockingPawn, (5, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = (5, 4); // Move to occupied square

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The pawn should not be able to move forward to an occupied square.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectMoveOutOfBounds()
    {
        // Arrange
        var pawn = new Pawn("White", (0, 0));
        var chessBoard = InitializeCustomBoard((pawn, (0, 0)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        PrintBoard(chessBoard);
        var targetPosition = (-1, 0); // Move out of bounds

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => pawn.IsValidMove(targetPosition, gameContext));
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldAllowEnPassantCapture()
    {
        // Arrange
        var whitePawn = new Pawn("White", (3, 4));
        var blackPawn = new Pawn("Black", (3, 5));

        var chessBoard = InitializeCustomBoard(
            (whitePawn, (3, 4)),
            (blackPawn, (3, 5))
        );

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .WithEnPassantTarget((2, 5), blackPawn)
            .Build();

        var targetPosition = (2, 5); // White pawn captures en passant

        // Act
        bool isValid = whitePawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The white pawn should be able to capture en passant.");
    }

    [Fact]
    public void Pawn_OnMoved_ShouldSetEnPassantTarget()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 4));
        var targetPosition = (4, 4); // Double move forward
        var expectedEnPassantTarget = (5, 4);

        var chessBoard = InitializeCustomBoard((pawn, (6, 4)));

        var gameContextBuilder = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithEnPassantTarget(expectedEnPassantTarget, pawn)
            .WithCurrentPlayer("White");
        var gameContext = gameContextBuilder.Build();

        // Act
        pawn.OnMoved(targetPosition, gameContext);

        // Assert
        // Verify that SetEnPassantTarget was called with correct parameters
        gameContextBuilder.VerifyStateService(s =>
            s.Verify(ss => ss.SetEnPassantTarget(expectedEnPassantTarget, pawn), Times.Exactly(2)));

        // Optionally, check the EnPassantTarget property
        var enPassantTarget = gameContext.State.EnPassantTarget;
        Assert.Equal(expectedEnPassantTarget, enPassantTarget);
    }

    [Fact]
    public void Pawn_OnMoved_ShouldPromoteAtEndOfBoard()
    {
        // Arrange
        var pawn = new Pawn("White", (1, 4));
        var chessBoard = InitializeCustomBoard((pawn, (1, 4)));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .Build();

        var targetPosition = (0, 4); // Move to promotion rank

        // Act
        pawn.OnMoved(targetPosition, gameContext);

        var promotedPiece = chessBoard.GetPieceAt(targetPosition);

        // Assert
        Assert.IsType<Queen>(promotedPiece);
        Assert.Equal("White", promotedPiece.Color);
        Assert.Equal(targetPosition, promotedPiece.Position);
    }
}

