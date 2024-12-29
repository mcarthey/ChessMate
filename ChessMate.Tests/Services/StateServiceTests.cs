using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services;

public class StateServiceTests : TestHelper
{
    public StateServiceTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void InitialState_CurrentPlayer_ShouldBeWhite()
    {
        // Arrange & Act
        var currentPlayer = StateService.CurrentPlayer;

        // Assert
        Assert.Equal("White", currentPlayer);
    }

    [Fact]
    public void SwitchPlayer_TogglesCurrentPlayer()
    {
        // Arrange
        StateService.SwitchPlayer();

        // Act
        var currentPlayer = StateService.CurrentPlayer;

        // Assert
        Assert.Equal("Black", currentPlayer);

        // Switch back
        StateService.SwitchPlayer();
        Assert.Equal("White", StateService.CurrentPlayer);
    }

    [Fact]
    public void SetPlayer_ValidPlayer_SetsCurrentPlayer()
    {
        // Arrange & Act
        StateService.SetPlayer("Black");

        // Assert
        Assert.Equal("Black", StateService.CurrentPlayer);
    }

    [Fact]
    public void SetPlayer_InvalidPlayer_ThrowsException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => StateService.SetPlayer("Green"));
        Assert.Equal("Invalid player. Must be 'White' or 'Black'.", exception.Message);
    }

    [Fact]
    public void SetEnPassantTarget_Pawn_SetsTarget()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("a2"));
        var targetPosition = new Position("a3");

        // Act
        StateService.SetEnPassantTarget(targetPosition, pawn);

        // Assert
        Assert.Equal(targetPosition, StateService.EnPassantTarget);
    }

    [Fact]
    public void SetEnPassantTarget_NonPawn_DoesNotSetTarget()
    {
        // Arrange
        var rook = new Rook("White", new Position("a2"));
        var targetPosition = new Position("a3");

        // Act
        StateService.SetEnPassantTarget(targetPosition, rook);

        // Assert
        Assert.Null(StateService.EnPassantTarget);
    }

    [Fact]
    public void ResetEnPassantTarget_ClearsTarget()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("a2"));
        var targetPosition = new Position("a3");
        StateService.SetEnPassantTarget(targetPosition, pawn);

        // Act
        StateService.ResetEnPassantTarget();

        // Assert
        Assert.Null(StateService.EnPassantTarget);
    }

    [Fact]
    public void ResetState_ResetsAllProperties()
    {
        // Arrange
        // Change various properties
        StateService.SetPlayer("Black");
        StateService.IsCheck = true;
        StateService.IsCheckmate = true;
        var pawn = new Pawn("White", new Position("a2"));
        StateService.SetEnPassantTarget(new Position("a3"), pawn);
        StateService.WhiteKingMoved = true;
        StateService.BlackKingMoved = true;
        StateService.WhiteRookKingSideMoved = true;
        StateService.WhiteRookQueenSideMoved = true;
        StateService.BlackRookKingSideMoved = true;
        StateService.BlackRookQueenSideMoved = true;
        StateService.MoveLog.Add("Sample Move");

        // Act
        StateService.ResetState();

        // Assert
        Assert.Equal("White", StateService.CurrentPlayer);
        Assert.False(StateService.IsCheck);
        Assert.False(StateService.IsCheckmate);
        Assert.Null(StateService.EnPassantTarget);
        Assert.False(StateService.WhiteKingMoved);
        Assert.False(StateService.BlackKingMoved);
        Assert.False(StateService.WhiteRookKingSideMoved);
        Assert.False(StateService.WhiteRookQueenSideMoved);
        Assert.False(StateService.BlackRookKingSideMoved);
        Assert.False(StateService.BlackRookQueenSideMoved);
        Assert.Empty(StateService.MoveLog);
    }

    [Fact]
    public void CastlingFlags_DefaultToFalse()
    {
        // Arrange & Act
        var whiteKingMoved = StateService.WhiteKingMoved;
        var blackKingMoved = StateService.BlackKingMoved;
        var whiteRookKingSideMoved = StateService.WhiteRookKingSideMoved;
        var whiteRookQueenSideMoved = StateService.WhiteRookQueenSideMoved;
        var blackRookKingSideMoved = StateService.BlackRookKingSideMoved;
        var blackRookQueenSideMoved = StateService.BlackRookQueenSideMoved;

        // Assert
        Assert.False(whiteKingMoved);
        Assert.False(blackKingMoved);
        Assert.False(whiteRookKingSideMoved);
        Assert.False(whiteRookQueenSideMoved);
        Assert.False(blackRookKingSideMoved);
        Assert.False(blackRookQueenSideMoved);
    }

    [Fact]
    public void MoveLog_InitializesEmpty()
    {
        // Arrange & Act
        var moveLog = StateService.MoveLog;

        // Assert
        Assert.NotNull(moveLog);
        Assert.Empty(moveLog);
    }

    [Fact]
    public void MoveLog_ClearsOnReset()
    {
        // Arrange
        StateService.MoveLog.Add("e2e4");
        StateService.MoveLog.Add("e7e5");
        Assert.NotEmpty(StateService.MoveLog);

        // Act
        StateService.ResetState();

        // Assert
        Assert.Empty(StateService.MoveLog);
    }

    [Fact]
    public void IsCheck_Settable()
    {
        // Arrange & Act
        StateService.IsCheck = true;

        // Assert
        Assert.True(StateService.IsCheck);
    }

    [Fact]
    public void IsCheckmate_Settable()
    {
        // Arrange & Act
        StateService.IsCheckmate = true;

        // Assert
        Assert.True(StateService.IsCheckmate);
    }

    [Fact]
    public void UpdateGameStateAfterMove_LogsMove()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        var whitePawn = new Pawn("White", from);
        var gameContext = new GameContextBuilder().Build();
        var gameStateEvaluator = new Mock<IGameStateEvaluator>();

        // Act
        StateService.UpdateGameStateAfterMove(whitePawn, from, to, gameContext, gameStateEvaluator.Object);

        // Assert
        Assert.Single(StateService.MoveLog);
        Assert.Equal("White Pawn from a2 to a3", StateService.MoveLog.First());
    }
}
