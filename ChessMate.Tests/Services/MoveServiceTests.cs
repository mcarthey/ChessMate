// File: ChessMate.Tests/Services/MoveServiceTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services;

public class MoveServiceTests : TestHelper
{
    private readonly ITestOutputHelper _output;

    public MoveServiceTests(ITestOutputHelper output) : base(output)
    {
        _output = output;
    }

    /// <summary>
    /// Helper method to initialize kings on the board.
    /// </summary>
    private void InitializeKings(GameContextBuilder builder)
    {
        var whiteKing = new King("White", new Position("e1"));
        var blackKing = new King("Black", new Position("e8"));

        builder.Board.SetPieceAt(new Position("e1"), whiteKing);
        builder.Board.SetPieceAt(new Position("e8"), blackKing);
    }

    [Fact]
    public void TryMove_NoPieceAtFromPosition_ReturnsFalse()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");

        var gameContextBuilder = new GameContextBuilder();
        InitializeKings(gameContextBuilder); // Initialize kings
        var gameContext = gameContextBuilder.Build();

        var mockMoveValidator = new Mock<IMoveValidatorService>().Object;
        var moveService = new MoveService(gameContext, mockMoveValidator);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_PieceColorDoesNotMatchCurrentPlayer_ReturnsFalse()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        var whitePawn = new Pawn("White", from);

        var gameContextBuilder = new GameContextBuilder()
            .WithCurrentPlayer("Black");
        InitializeKings(gameContextBuilder); // Initialize kings
        gameContextBuilder.Board.SetPieceAt(from, whitePawn);
        var gameContext = gameContextBuilder.Build();

        var mockMoveValidator = new Mock<IMoveValidatorService>().Object;
        var moveService = new MoveService(gameContext, mockMoveValidator);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_MoveIsInvalid_ReturnsFalse()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("b3"); // Invalid move for a pawn moving forward
        var whitePawn = new Pawn("White", from);

        var gameContextBuilder = new GameContextBuilder();
        InitializeKings(gameContextBuilder); // Initialize kings
        gameContextBuilder.Board.SetPieceAt(from, whitePawn);
        var gameContext = gameContextBuilder.Build();

        var mockMoveValidator = new Mock<IMoveValidatorService>();
        mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, gameContext)).Returns(false);

        var moveService = new MoveService(gameContext, mockMoveValidator.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_MoveValidatorThrowsException_ReturnsFalse()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        var whitePawn = new Pawn("White", from);

        var gameContextBuilder = new GameContextBuilder();
        InitializeKings(gameContextBuilder); // Initialize kings
        gameContextBuilder.Board.SetPieceAt(from, whitePawn);
        var gameContext = gameContextBuilder.Build();

        var mockMoveValidator = new Mock<IMoveValidatorService>();
        mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, gameContext))
            .Throws(new Exception("Test exception"));

        var moveService = new MoveService(gameContext, mockMoveValidator.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_MoveIsValid_ExecutesMove()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        var whitePawn = new Pawn("White", from);

        var gameContextBuilder = new GameContextBuilder();
        InitializeKings(gameContextBuilder); // Initialize kings
        gameContextBuilder.Board.SetPieceAt(from, whitePawn);
        var gameContext = gameContextBuilder.Build();

        var mockMoveValidator = new Mock<IMoveValidatorService>();
        mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, gameContext)).Returns(true);

        var moveService = new MoveService(gameContext, mockMoveValidator.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);
        Assert.Null(gameContext.Board.GetPieceAt(from));
        Assert.Equal(whitePawn, gameContext.Board.GetPieceAt(to));
        Assert.Equal(to, whitePawn.Position);
    }

    [Fact]
    public void TryMove_MoveLeavesKingInCheck_ReturnsFalse()
    {
        // Arrange
        var from = new Position("e2");
        var to = new Position("e3");
        var whitePawn = new Pawn("White", from);

        var gameContextBuilder = new GameContextBuilder();
        InitializeKings(gameContextBuilder); // Initialize kings
        gameContextBuilder.Board.SetPieceAt(from, whitePawn);
        var gameContext = gameContextBuilder.Build();

        var mockStateService = gameContextBuilder.StateServiceMock;
        mockStateService.Setup(s => s.WouldMoveCauseSelfCheck(whitePawn, from, to, gameContext)).Returns(true);

        var mockMoveValidator = new Mock<IMoveValidatorService>();
        mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, gameContext)).Returns(true);

        var moveService = new MoveService(gameContext, mockMoveValidator.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_ValidMove_UpdatesGameState()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        var whitePawn = new Pawn("White", from);

        var gameContextBuilder = new GameContextBuilder();
        InitializeKings(gameContextBuilder); // Initialize kings
        gameContextBuilder.Board.SetPieceAt(from, whitePawn);
        var gameContext = gameContextBuilder.Build();

        var mockMoveValidator = new Mock<IMoveValidatorService>();
        mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, gameContext)).Returns(true);

        var moveService = new MoveService(gameContext, mockMoveValidator.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);

        // Verify that the CurrentPlayer has switched
        Assert.Equal("Black", gameContext.State.CurrentPlayer);

        // Verify that game state is updated
        Assert.Contains("White Pawn from a2 to a3", gameContext.State.MoveLog);
    }

    [Fact]
    public void TryMove_OpponentKingIsInCheck_SetsIsCheck()
    {
        // Arrange
        var from = new Position("a1");
        var to = new Position("a8");
        var whiteRook = new Rook("White", from);
        var blackKing = new King("Black", to);

        var gameContextBuilder = new GameContextBuilder();
        InitializeKings(gameContextBuilder); // Initialize kings
        gameContextBuilder.Board.SetPieceAt(from, whiteRook);
        gameContextBuilder.Board.SetPieceAt(to, blackKing);
        var gameContext = gameContextBuilder.Build();

        var mockMoveValidator = new Mock<IMoveValidatorService>();
        mockMoveValidator.Setup(v => v.IsValidMove(whiteRook, to, gameContext)).Returns(true);

        var moveService = new MoveService(gameContext, mockMoveValidator.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);

        // Verify that IsCheck is true
        Assert.True(gameContext.State.IsCheck);
    }

    [Fact]
    public void TryMove_OpponentHasNoLegalMoves_SetsIsCheckmate()
    {
        // Arrange
        var from = new Position("h7");
        var to = new Position("h8");
        var whiteQueen = new Queen("White", from);
        var blackKing = new King("Black", new Position("g8"));

        var gameContextBuilder = new GameContextBuilder();
        InitializeKings(gameContextBuilder); // Initialize kings
        gameContextBuilder.Board.SetPieceAt(from, whiteQueen);
        gameContextBuilder.Board.SetPieceAt(blackKing.Position, blackKing);
        var gameContext = gameContextBuilder.Build();

        var mockMoveValidator = new Mock<IMoveValidatorService>();
        mockMoveValidator.Setup(v => v.IsValidMove(whiteQueen, to, gameContext)).Returns(true);

        var moveService = new MoveService(gameContext, mockMoveValidator.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);

        // Verify that IsCheck and IsCheckmate are true
        Assert.True(gameContext.State.IsCheck);
        Assert.True(gameContext.State.IsCheckmate);
    }
}

