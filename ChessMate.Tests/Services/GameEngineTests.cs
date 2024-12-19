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
    private readonly Mock<IStateService> _mockStateService;
    private readonly Mock<IMoveService> _mockMoveService;
    private readonly GameEngine _gameEngine;

    public GameEngineTests(ITestOutputHelper output) : base(output)
    {
        _mockChessBoard = new Mock<IChessBoard>();
        _mockStateService = new Mock<IStateService>();
        _mockMoveService = new Mock<IMoveService>();
        _gameEngine = new GameEngine(_mockChessBoard.Object, _mockStateService.Object, _mockMoveService.Object);
    }

    [Fact]
    public void GameEngine_Initialize_ShouldSetUpBoardAndState()
    {
        // Arrange & Act
        var gameEngine = new GameEngine(_mockChessBoard.Object, _mockStateService.Object, _mockMoveService.Object);

        // Assert
        _mockChessBoard.Verify(board => board.InitializeBoard(), Times.Once);
        Assert.NotNull(gameEngine.Board);
        Assert.NotNull(gameEngine.State);
        Assert.NotNull(gameEngine.Move);

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
        var from = ChessNotationUtility.FromChessNotation(fromNotation);
        var to = ChessNotationUtility.FromChessNotation(toNotation);
        _mockMoveService.Setup(move => move.TryMove(from, to)).Returns(true);

        // Act
        var result = _gameEngine.ProcessMove(fromNotation, toNotation);

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
        var from = ChessNotationUtility.FromChessNotation(fromNotation);
        var to = ChessNotationUtility.FromChessNotation(toNotation);
        _mockMoveService.Setup(move => move.TryMove(from, to)).Returns(false);

        // Act
        var result = _gameEngine.ProcessMove(fromNotation, toNotation);

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
        _mockStateService.Setup(state => state.CurrentPlayer).Returns("White");

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
        _gameEngine.Board.InitializeBoard();

        // Assert
        _mockChessBoard.Verify(board => board.InitializeBoard(), Times.Once);

        // Debugging output
        CustomOutput.WriteLine("Test: GameEngine_InitializeBoard_ShouldCallInitializeBoardOnChessBoard");
        CustomOutput.Flush();
    }
}




