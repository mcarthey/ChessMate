// File: ChessMate.Tests/Models/RookTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;

namespace ChessMate.Tests.Models
{
    public class RookTests
    {
        private readonly Mock<IChessBoard> _mockChessBoard;
        private readonly Mock<IStateService> _mockStateService;

        public RookTests()
        {
            _mockChessBoard = new Mock<IChessBoard>();
            _mockStateService = new Mock<IStateService>();
        }

        [Fact]
        public void Rook_IsValidMove_ShouldAllowStraightMove()
        {
            // Arrange
            var rook = new Rook("White", new Position("a1"));
            var targetPosition = new Position("a6"); // Move horizontally

            _mockChessBoard.Setup(board => board.GetPieceAt(rook.Position)).Returns(rook);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a2"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a3"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a4"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a5"))).Returns((ChessPiece)null);

            // Act
            bool isValid = rook.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The rook should be able to move horizontally.");
        }

        [Fact]
        public void Rook_IsValidMove_ShouldAllowVerticalMove()
        {
            // Arrange
            var rook = new Rook("White", new Position("a1"));
            var targetPosition = new Position("d1"); // Move vertically

            _mockChessBoard.Setup(board => board.GetPieceAt(rook.Position)).Returns(rook);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("b1"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("c1"))).Returns((ChessPiece)null);

            // Act
            bool isValid = rook.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The rook should be able to move vertically.");
        }

        [Fact]
        public void Rook_IsValidMove_ShouldRejectDiagonalMove()
        {
            // Arrange
            var rook = new Rook("White", new Position("a1"));
            var targetPosition = new Position("c3"); // Diagonal move

            _mockChessBoard.Setup(board => board.GetPieceAt(rook.Position)).Returns(rook);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = rook.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The rook should not be able to move diagonally.");
        }

        [Fact]
        public void Rook_IsValidMove_ShouldAllowCapture()
        {
            // Arrange
            var rook = new Rook("White", new Position("a1"));
            var opponentPawn = new Pawn("Black", new Position("a6"));
            var targetPosition = opponentPawn.Position; // Capture opponent's piece

            _mockChessBoard.Setup(board => board.GetPieceAt(rook.Position)).Returns(rook);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns(opponentPawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a2"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a3"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a4"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a5"))).Returns((ChessPiece)null);

            // Act
            bool isValid = rook.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The rook should be able to capture an opponent's piece.");
        }

        [Fact]
        public void Rook_IsValidMove_ShouldRejectMoveToOccupiedSquareByOwnPiece()
        {
            // Arrange
            var rook = new Rook("White", new Position("a1"));
            var ownPawn = new Pawn("White", new Position("a6"));
            var targetPosition = ownPawn.Position; // Square occupied by own piece

            _mockChessBoard.Setup(board => board.GetPieceAt(rook.Position)).Returns(rook);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns(ownPawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a2"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a3"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a4"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a5"))).Returns((ChessPiece)null);

            // Act
            bool isValid = rook.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The rook should not be able to move to a square occupied by its own piece.");
        }

        [Fact]
        public void Rook_IsValidMove_ShouldRejectMoveIfPathIsBlocked()
        {
            // Arrange
            var rook = new Rook("White", new Position("a1"));
            var blockingPiece = new Pawn("White", new Position("a4"));
            var targetPosition = new Position("a6"); // Path is blocked

            _mockChessBoard.Setup(board => board.GetPieceAt(rook.Position)).Returns(rook);
            _mockChessBoard.Setup(board => board.GetPieceAt(blockingPiece.Position)).Returns(blockingPiece);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a2"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a3"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("a5"))).Returns((ChessPiece)null);

            // Act
            bool isValid = rook.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The rook should not be able to move if the path is blocked.");
        }

        [Fact]
        public void Rook_IsValidMove_ShouldRejectOutOfBoundsMove()
        {
            // Arrange
            var rook = new Rook("White", new Position("a1"));
            var targetPosition = new Position(-1, 0); // Out of bounds

            _mockChessBoard.Setup(board => board.GetPieceAt(rook.Position)).Returns(rook);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Throws<ArgumentOutOfRangeException>();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => rook.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object));
        }

    }
}
