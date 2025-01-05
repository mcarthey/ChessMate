// File: ChessMate.Tests/Services/MoveValidatorServiceTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services
{
    public class MoveValidatorServiceTests
    {
        private readonly Mock<IChessBoard> _mockChessBoard;
        private readonly Mock<IStateService> _mockStateService;
        private readonly MoveValidatorService _moveValidatorService;

        public MoveValidatorServiceTests()
        {
            _mockChessBoard = new Mock<IChessBoard>();
            _mockStateService = new Mock<IStateService>();
            _moveValidatorService = new MoveValidatorService();
        }

        [Fact]
        public void IsValidMove_ValidMove_ReturnsTrue()
        {
            // Arrange
            var from = new Position("a2");
            var to = new Position("a3");
            var whitePawn = new Mock<Pawn>("White", from) { CallBase = true };
            whitePawn.Setup(p => p.IsValidMove(to, _mockChessBoard.Object, _mockStateService.Object)).Returns(true);

            // Act
            var result = _moveValidatorService.IsValidMove(whitePawn.Object, to, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMove_InvalidMove_ReturnsFalse()
        {
            // Arrange
            var from = new Position("a2");
            var to = new Position("b3"); // Invalid move for a pawn moving forward
            var whitePawn = new Mock<Pawn>("White", from) { CallBase = true };
            whitePawn.Setup(p => p.IsValidMove(to, _mockChessBoard.Object, _mockStateService.Object)).Returns(false);

            // Act
            var result = _moveValidatorService.IsValidMove(whitePawn.Object, to, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidMove_NullPiece_ReturnsFalse()
        {
            // Arrange
            var to = new Position("a3");

            // Act
            var result = _moveValidatorService.IsValidMove(null, to, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidMove_PieceIsNull_ReturnsFalse()
        {
            // Arrange
            var to = new Position("e4");

            // Act
            var result = _moveValidatorService.IsValidMove(null, to, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidMove_PieceIsValidButMoveCausesCheck_ReturnsTrue()
        {
            // Arrange
            var from = new Position("d2");
            var to = new Position("d4");
            var whitePawn = new Mock<Pawn>("White", from) { CallBase = true };
            whitePawn.Setup(p => p.IsValidMove(to, _mockChessBoard.Object, _mockStateService.Object)).Returns(true);

            // Act
            var result = _moveValidatorService.IsValidMove(whitePawn.Object, to, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMove_PieceIsInvalidDueToBoardState_ReturnsFalse()
        {
            // Arrange
            var from = new Position("e2");
            var to = new Position("e5"); // Assuming this move is invalid based on the board state
            var whitePawn = new Mock<Pawn>("White", from) { CallBase = true };
            whitePawn.Setup(p => p.IsValidMove(to, _mockChessBoard.Object, _mockStateService.Object)).Returns(false);

            // Act
            var result = _moveValidatorService.IsValidMove(whitePawn.Object, to, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(result);
        }
    }
}
