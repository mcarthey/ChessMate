// File: ChessMate.Tests/Services/GameEngineTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services
{
    public class GameEngineTests
    {
        private readonly Mock<IMoveService> _mockMoveService;
        private readonly GameEngine _gameEngine;

        public GameEngineTests()
        {
            _mockMoveService = new Mock<IMoveService>();
            _gameEngine = new GameEngine(_mockMoveService.Object);
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
        }

        [Fact]
        public void GameEngine_MoveProperty_ShouldReturnMoveService()
        {
            // Arrange & Act
            var moveService = _gameEngine.Move;

            // Assert
            Assert.Equal(_mockMoveService.Object, moveService);
        }
    }
}
