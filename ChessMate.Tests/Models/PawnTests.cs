// File: ChessMate.Tests/Models/PawnTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;

namespace ChessMate.Tests.Models
{
    public class PawnTests
    {
        private readonly Mock<IChessBoard> _mockChessBoard;
        private readonly Mock<IStateService> _mockStateService;

        public PawnTests()
        {
            _mockChessBoard = new Mock<IChessBoard>();
            _mockStateService = new Mock<IStateService>();
        }

        [Fact]
        public void Pawn_IsValidMove_ShouldAllowSingleSquareMove()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("e2"));
            var targetPosition = new Position("e3"); // Move forward one square

            _mockChessBoard.Setup(board => board.GetPieceAt(pawn.Position)).Returns(pawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = pawn.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The pawn should be able to move one square forward.");
        }

        [Fact]
        public void Pawn_IsValidMove_ShouldAllowDoubleSquareMoveOnFirstMove()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("e2"));
            var targetPosition = new Position("e4"); // Move forward two squares
            var middlePosition = new Position("e3");

            _mockChessBoard.Setup(board => board.GetPieceAt(pawn.Position)).Returns(pawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(middlePosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = pawn.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The pawn should be able to move two squares forward on its first move.");
        }

        [Fact]
        public void Pawn_IsValidMove_ShouldRejectDoubleSquareMoveAfterFirstMove()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("e3"));
            var targetPosition = new Position("e5"); // Attempt to move two squares forward
            var middlePosition = new Position("e4");

            _mockChessBoard.Setup(board => board.GetPieceAt(pawn.Position)).Returns(pawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(middlePosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = pawn.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The pawn should not be able to move two squares forward after its first move.");
        }

        [Fact]
        public void Pawn_IsValidMove_ShouldAllowDiagonalCapture()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("e2"));
            var opponentPawn = new Pawn("Black", new Position("f3"));
            var targetPosition = new Position("f3"); // Capture diagonally

            _mockChessBoard.Setup(board => board.GetPieceAt(pawn.Position)).Returns(pawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns(opponentPawn);

            // Act
            bool isValid = pawn.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The pawn should be able to capture an opponent's piece diagonally.");
        }

        [Fact]
        public void Pawn_IsValidMove_ShouldRejectInvalidDiagonalMove()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("e2"));
            var targetPosition = new Position("f3"); // Diagonal move without capture

            _mockChessBoard.Setup(board => board.GetPieceAt(pawn.Position)).Returns(pawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = pawn.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The pawn should not be able to move diagonally without capturing.");
        }

        [Fact]
        public void Pawn_IsValidMove_ShouldRejectBackwardMove()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("e2"));
            var targetPosition = new Position("e1"); // Move backward

            _mockChessBoard.Setup(board => board.GetPieceAt(pawn.Position)).Returns(pawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = pawn.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The pawn should not be able to move backward.");
        }

        [Fact]
        public void Pawn_IsValidMove_ShouldRejectMoveToOccupiedSquare()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("e2"));
            var blockingPawn = new Pawn("Black", new Position("e3"));
            var targetPosition = new Position("e3"); // Move to occupied square

            _mockChessBoard.Setup(board => board.GetPieceAt(pawn.Position)).Returns(pawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns(blockingPawn);

            // Act
            bool isValid = pawn.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The pawn should not be able to move forward to an occupied square.");
        }

        [Fact]
        public void Pawn_IsValidMove_ShouldRejectOutOfBoundsMove()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("a8"));
            var targetPosition = new Position(-1, 0); // Move out of bounds

            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Throws(new ArgumentOutOfRangeException());

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                pawn.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object));
        }

        [Fact]
        public void Pawn_IsValidMove_ShouldAllowEnPassantCapture()
        {
            // Arrange
            var whitePawn = new Pawn("White", new Position("e5"));
            var blackPawn = new Pawn("Black", new Position("d5"));
            var targetPosition = new Position("d6"); // En passant capture

            _mockChessBoard.Setup(board => board.GetPieceAt(whitePawn.Position)).Returns(whitePawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("d5"))).Returns(blackPawn);

            _mockStateService.Setup(state => state.EnPassantTarget).Returns(targetPosition);

            // Act
            bool isValid = whitePawn.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

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

            _mockChessBoard.Setup(board => board.GetPieceAt(pawn.Position)).Returns(pawn);

            // Act
            pawn.OnMoved(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            _mockStateService.Verify(s => s.SetEnPassantTarget(expectedEnPassantTarget, pawn), Times.Once);
            _mockStateService.Verify(s => s.ResetEnPassantTarget(), Times.Never);
            Assert.Equal(targetPosition, pawn.Position);
        }

        [Fact]
        public void Pawn_OnMoved_ShouldResetEnPassantTarget_WhenNotDoubleMoved()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("e4"));
            var targetPosition = new Position("e5"); // Single move forward

            _mockChessBoard.Setup(board => board.GetPieceAt(pawn.Position)).Returns(pawn);

            // Act
            pawn.OnMoved(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            _mockStateService.Verify(s => s.ResetEnPassantTarget(), Times.Once);
            _mockStateService.Verify(s => s.SetEnPassantTarget(It.IsAny<Position>(), It.IsAny<ChessPiece>()), Times.Never);
            Assert.Equal(targetPosition, pawn.Position);
        }

        [Fact]
        public void Pawn_OnMoved_ShouldPromoteAtEndOfBoard()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("e7"));
            var targetPosition = new Position("e8"); // Move to promotion rank

            _mockChessBoard.Setup(board => board.GetPieceAt(pawn.Position)).Returns(pawn);
            _mockChessBoard.Setup(board => board.SetPieceAt(targetPosition, It.IsAny<Queen>())).Verifiable();

            // Act
            pawn.OnMoved(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            _mockChessBoard.Verify(board => board.SetPieceAt(targetPosition, It.Is<Queen>(q =>
                q.Color == "White" && q.Position.Equals(targetPosition))),
                Times.Once);
        }
    }
}
