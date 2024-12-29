// File: ChessMate.Tests/Services/GameEngineTests.cs

using ChessMate.Models;
using ChessMate.Services;
using ChessMate.Utilities;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services;

public class GameEngineTests : TestHelper
{
    private readonly Mock<IChessBoard> _mockChessBoard;
    private readonly Mock<IMoveService> _mockMoveService;
    private readonly GameEngine _gameEngine;

    public GameEngineTests(ITestOutputHelper output) : base(output)
    {
        _mockChessBoard = new Mock<IChessBoard>();
        _mockMoveService = new Mock<IMoveService>();
        _gameEngine = new GameEngine(_mockChessBoard.Object, StateService, _mockMoveService.Object);
    }

    [Fact]
    public void GameEngine_Initialize_ShouldSetUpBoardAndState()
    {
        // Arrange & Act
        // Use the _gameEngine instance created in the constructor
        // var gameEngine = new GameEngine(_mockChessBoard.Object, StateService, _mockMoveService.Object);

        // Assert
        _mockChessBoard.Verify(board => board.InitializeBoard(), Times.Once);
        Assert.NotNull(_gameEngine.Board);
        Assert.NotNull(_gameEngine.State);
        Assert.NotNull(_gameEngine.Move);

        // Debugging output
        CustomOutput.WriteLine("Test: GameEngine_Initialize_ShouldSetUpBoardAndState");
        CustomOutput.Flush();
    }

    [Fact]
    public void GameEngine_ProcessMove_ShouldCallTryMove()
    {
        // Arrange
        var fromNotation = "e2";
        var toNotation = "e4";
        var from = new Position(fromNotation);
        var to = new Position(toNotation);
        _mockMoveService.Setup(move => move.TryMove(from, to)).Returns(true);

        // Act
        var result = _gameEngine.ProcessMove(from, to);

        // Assert
        Assert.True(result);
        _mockMoveService.Verify(move => move.TryMove(from, to), Times.Once);

        // Debugging output
        CustomOutput.WriteLine("Test: GameEngine_ProcessMove_ShouldCallTryMove");
        CustomOutput.WriteLine($"From: {fromNotation}, To: {toNotation}, Result: {result}");
        CustomOutput.Flush();
    }

    [Fact]
    public void GameEngine_ProcessMove_ShouldReturnFalseForInvalidMove()
    {
        // Arrange
        var fromNotation = "e2";
        var toNotation = "e5";
        var from = new Position(fromNotation);
        var to = new Position(toNotation);
        _mockMoveService.Setup(move => move.TryMove(from, to)).Returns(false);

        // Act
        var result = _gameEngine.ProcessMove(from, to);

        // Assert
        Assert.False(result);
        _mockMoveService.Verify(move => move.TryMove(from, to), Times.Once);

        // Debugging output
        CustomOutput.WriteLine("Test: GameEngine_ProcessMove_ShouldReturnFalseForInvalidMove");
        CustomOutput.WriteLine($"From: {fromNotation}, To: {toNotation}, Result: {result}");
        CustomOutput.Flush();
    }

    [Fact]
    public void GameEngine_GetCurrentPlayer_ShouldReturnCurrentPlayer()
    {
        // Arrange
        StateService.SetPlayer("White");

        // Act
        var currentPlayer = _gameEngine.GetCurrentPlayer();

        // Assert
        Assert.Equal("White", currentPlayer);

        // Debugging output
        CustomOutput.WriteLine("Test: GameEngine_GetCurrentPlayer_ShouldReturnCurrentPlayer");
        CustomOutput.WriteLine($"Current Player: {currentPlayer}");
        CustomOutput.Flush();
    }

    [Fact]
    public void GameEngine_InitializeBoard_ShouldCallInitializeBoardOnChessBoard()
    {
        // Arrange & Act
        // The _gameEngine instance is created in the constructor, which calls InitializeBoard

        // Assert
        _mockChessBoard.Verify(board => board.InitializeBoard(), Times.Once);

        // Debugging output
        CustomOutput.WriteLine("Test: GameEngine_InitializeBoard_ShouldCallInitializeBoardOnChessBoard");
        CustomOutput.Flush();
    }

}


