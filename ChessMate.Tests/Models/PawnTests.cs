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
        var pawn = new Pawn("White", new Position("e2"));
        var chessBoard = InitializeCustomBoard((pawn, new Position("e2")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("e3"); // Move forward one square

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The pawn should be able to move one square forward.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldAllowDoubleSquareMoveOnFirstMove()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("e2"));
        var chessBoard = InitializeCustomBoard((pawn, new Position("e2")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("e4"); // Move forward two squares

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The pawn should be able to move two squares forward on its first move.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectDoubleSquareMoveAfterFirstMove()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("e3"));
        var chessBoard = InitializeCustomBoard((pawn, new Position("e3")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("e5"); // Attempt to move two squares forward

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The pawn should not be able to move two squares forward after its first move.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldAllowDiagonalCapture()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("e2"));
        var opponentPawn = new Pawn("Black", new Position("f3"));
        var chessBoard = InitializeCustomBoard((pawn, new Position("e2")), (opponentPawn, new Position("f3")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("f3"); // Capture diagonally

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The pawn should be able to capture an opponent's piece diagonally.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectInvalidDiagonalMove()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("e2"));
        var chessBoard = InitializeCustomBoard((pawn, new Position("e2")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("f3"); // Diagonal move without capture

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The pawn should not be able to move diagonally without capturing.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectBackwardMove()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("e2"));
        var chessBoard = InitializeCustomBoard((pawn, new Position("e2")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("e1"); // Move backward

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The pawn should not be able to move backward.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectMoveToOccupiedSquare()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("e2"));
        var blockingPawn = new Pawn("Black", new Position("e3"));
        var chessBoard = InitializeCustomBoard((pawn, new Position("e2")), (blockingPawn, new Position("e3")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        var targetPosition = new Position("e3"); // Move to occupied square

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The pawn should not be able to move forward to an occupied square.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectMoveOutOfBounds()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("a1"));
        var chessBoard = InitializeCustomBoard((pawn, new Position("a1")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .Build();

        PrintBoard(chessBoard);
        var targetPosition = new Position(-1, 0); // Move out of bounds

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.False(isValid, "The pawn should not be able to move out of bounds.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldAllowEnPassantCapture()
    {
        // Arrange
        var whitePawn = new Pawn("White", new Position("e5"));
        var blackPawn = new Pawn("Black", new Position("d5"));

        var chessBoard = InitializeCustomBoard(
            (whitePawn, new Position("e5")),
            (blackPawn, new Position("d5"))
        );

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithCurrentPlayer("White")
            .WithEnPassantTarget(new Position("d6"), blackPawn)
            .Build();

        var targetPosition = new Position("d6"); // White pawn captures en passant

        // Act
        bool isValid = whitePawn.IsValidMove(targetPosition, gameContext);

        // Assert
        Assert.True(isValid, "The white pawn should be able to capture en passant.");
    }

    [Fact]
    public void Pawn_OnMoved_ShouldSetEnPassantTarget()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("e2"));
        var targetPosition = new Position("e4"); // Double move forward
        var expectedEnPassantTarget = new Position("e3");

        var chessBoard = InitializeCustomBoard((pawn, new Position("e2")));

        var gameContextBuilder = new GameContextBuilder()
            .WithBoard(chessBoard)
            .WithEnPassantTarget(expectedEnPassantTarget, pawn)
            .WithCurrentPlayer("White");
        var gameContext = gameContextBuilder.Build();

        // Act
        pawn.OnMoved(targetPosition, gameContext);

        // Assert
        // Verify that SetEnPassantTarget was called with correct parameters
        gameContextBuilder.StateServiceMock.Verify(s => s.SetEnPassantTarget(expectedEnPassantTarget, pawn), Times.Exactly(2));

        // Optionally, check the EnPassantTarget property
        var enPassantTarget = gameContext.State.EnPassantTarget;
        Assert.Equal(expectedEnPassantTarget, enPassantTarget);
    }

    [Fact]
    public void Pawn_OnMoved_ShouldPromoteAtEndOfBoard()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("e7"));
        var chessBoard = InitializeCustomBoard((pawn, new Position("e7")));

        var gameContext = new GameContextBuilder()
            .WithBoard(chessBoard)
            .Build();

        var targetPosition = new Position("e8"); // Move to promotion rank

        // Act
        pawn.OnMoved(targetPosition, gameContext);

        var promotedPiece = chessBoard.GetPieceAt(targetPosition);

        // Assert
        Assert.IsType<Queen>(promotedPiece);
        Assert.Equal("White", promotedPiece.Color);
        Assert.Equal(targetPosition, promotedPiece.Position);
    }
}

