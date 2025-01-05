// File: ChessMate.Tests/Models/QueenTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;

namespace ChessMate.Tests.Models
{
    public class QueenTests
    {
        private readonly Mock<IChessBoard> _mockChessBoard;
        private readonly Mock<IStateService> _mockStateService;

        public QueenTests()
        {
            _mockChessBoard = new Mock<IChessBoard>();
            _mockStateService = new Mock<IStateService>();
        }

        [Fact]
        public void Queen_IsValidMove_ShouldAllowVerticalMove()
        {
            // Arrange
            var queen = new Queen("White", new Position("e4"));
            var targetPosition = new Position("e1"); // Move vertically

            _mockChessBoard.Setup(board => board.GetPieceAt(queen.Position)).Returns(queen);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("e2"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("e3"))).Returns((ChessPiece)null);

            // Act
            bool isValid = queen.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The queen should be able to move vertically.");
        }

        [Fact]
        public void Queen_IsValidMove_ShouldAllowHorizontalMove()
        {
            // Arrange
            var queen = new Queen("White", new Position("e4"));
            var targetPosition = new Position("h4"); // Move horizontally

            _mockChessBoard.Setup(board => board.GetPieceAt(queen.Position)).Returns(queen);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("f4"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("g4"))).Returns((ChessPiece)null);

            // Act
            bool isValid = queen.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The queen should be able to move horizontally.");
        }

        [Fact]
        public void Queen_IsValidMove_ShouldAllowDiagonalMove()
        {
            // Arrange
            var queen = new Queen("White", new Position("e4"));
            var targetPosition = new Position("g6"); // Move diagonally

            _mockChessBoard.Setup(board => board.GetPieceAt(queen.Position)).Returns(queen);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("f5"))).Returns((ChessPiece)null);

            // Act
            bool isValid = queen.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The queen should be able to move diagonally.");
        }

        [Fact]
        public void Queen_IsValidMove_ShouldRejectMoveThroughPieces()
        {
            // Arrange
            var queen = new Queen("White", new Position("e4"));
            var blockingPiece = new Pawn("White", new Position("f5"));
            var targetPosition = new Position("g6"); // Attempt to move diagonally through the pawn

            _mockChessBoard.Setup(board => board.GetPieceAt(queen.Position)).Returns(queen);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("f5"))).Returns(blockingPiece);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = queen.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The queen should not be able to move through other pieces.");
        }

        [Fact]
        public void Queen_IsValidMove_ShouldRejectInvalidMove()
        {
            // Arrange
            var queen = new Queen("White", new Position("e4"));
            var targetPosition = new Position("d6"); // Invalid move (not straight or diagonal)

            _mockChessBoard.Setup(board => board.GetPieceAt(queen.Position)).Returns(queen);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = queen.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The queen should reject invalid moves.");
        }

        [Fact]
        public void Queen_IsValidMove_ShouldCaptureOpponentPiece()
        {
            // Arrange
            var queen = new Queen("White", new Position("e4"));
            var opponentPawn = new Pawn("Black", new Position("g6"));
            var targetPosition = opponentPawn.Position; // Capture opponent's pawn

            _mockChessBoard.Setup(board => board.GetPieceAt(queen.Position)).Returns(queen);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns(opponentPawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("f5"))).Returns((ChessPiece)null);

            // Act
            bool isValid = queen.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The queen should be able to capture an opponent's piece.");
        }
    }
}
