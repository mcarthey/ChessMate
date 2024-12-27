// File: ChessMate.Tests/Services/StateServiceTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services;

public class StateServiceTests : TestHelper
{
    public StateServiceTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void StateService_InitialState_ShouldSetCurrentPlayerToWhite()
    {
        // Arrange & Act
        var currentPlayer = StateService.CurrentPlayer;

        // Assert
        Assert.Equal("White", currentPlayer);
    }

    [Fact]
    public void StateService_SwitchPlayer_ShouldToggleCurrentPlayer()
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
    public void StateService_SetPlayer_ShouldSetCurrentPlayer()
    {
        // Arrange & Act
        StateService.SetPlayer("Black");

        // Assert
        Assert.Equal("Black", StateService.CurrentPlayer);
    }

    [Fact]
    public void StateService_SetPlayer_ShouldThrowExceptionForInvalidPlayer()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => StateService.SetPlayer("Green"));
        Assert.Equal("Invalid player. Must be 'White' or 'Black'.", exception.Message);
    }

    [Fact]
    public void StateService_SetEnPassantTarget_ShouldSetTargetForPawn()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 0));
        var targetPosition = (5, 0);

        // Act
        StateService.SetEnPassantTarget(targetPosition, pawn);

        // Assert
        Assert.Equal(targetPosition, StateService.EnPassantTarget);
    }

    [Fact]
    public void StateService_SetEnPassantTarget_ShouldNotSetTargetForNonPawn()
    {
        // Arrange
        var rook = new Rook("White", (6, 0));
        var targetPosition = (5, 0);

        // Act
        StateService.SetEnPassantTarget(targetPosition, rook);

        // Assert
        Assert.Null(StateService.EnPassantTarget);
    }

    [Fact]
    public void StateService_ResetEnPassantTarget_ShouldClearTarget()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 0));
        var targetPosition = (5, 0);
        StateService.SetEnPassantTarget(targetPosition, pawn);

        // Act
        StateService.ResetEnPassantTarget();

        // Assert
        Assert.Null(StateService.EnPassantTarget);
    }

    [Fact]
    public void StateService_ResetState_ShouldResetAllProperties()
    {
        // Arrange
        // Change various properties
        StateService.SetPlayer("Black");
        StateService.IsCheck = true;
        StateService.IsCheckmate = true;
        var pawn = new Pawn("White", (6, 0));
        StateService.SetEnPassantTarget((5, 0), pawn);
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
    public void StateService_CastlingFlags_ShouldDefaultToFalse()
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
    public void StateService_MoveLog_ShouldInitializeEmpty()
    {
        // Arrange & Act
        var moveLog = StateService.MoveLog;

        // Assert
        Assert.NotNull(moveLog);
        Assert.Empty(moveLog);
    }

    [Fact]
    public void StateService_MoveLog_ShouldClearOnReset()
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
    public void StateService_IsCheck_ShouldBeSettable()
    {
        // Arrange & Act
        StateService.IsCheck = true;

        // Assert
        Assert.True(StateService.IsCheck);
    }

    [Fact]
    public void StateService_IsCheckmate_ShouldBeSettable()
    {
        // Arrange & Act
        StateService.IsCheckmate = true;

        // Assert
        Assert.True(StateService.IsCheckmate);
    }
}
