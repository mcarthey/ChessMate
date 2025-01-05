// File: ChessMate.Tests/Models/BishopTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;

namespace ChessMate.Tests.Models
{
    public class BishopTests
    {
        private readonly Mock<IChessBoard> _mockChessBoard;
        private readonly Mock<IStateService> _mockStateService;

        public BishopTests()
        {
            _mockChessBoard = new Mock<IChessBoard>();
            _mockStateService = new Mock<IStateService>();
        }

        [Fact]
        public void Bishop_IsValidMove_ShouldAllowDiagonalMove()
        {
            // Arrange
            var bishop = new Bishop("White", new Position("c1"));
            var targetPosition = new Position("e3"); // Diagonal move

            // Setup the board: Bishop at c1, empty d2 and e3
            _mockChessBoard.Setup(board => board.GetPieceAt(bishop.Position)).Returns(bishop);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("d2"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = bishop.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The bishop should be able to move diagonally.");
        }

        [Fact]
        public void Bishop_IsValidMove_ShouldRejectNonDiagonalMove()
        {
            // Arrange
            var bishop = new Bishop("White", new Position("c1"));
            var targetPosition = new Position("c2"); // Non-diagonal move

            // Setup the board: Bishop at c1, target square c2 empty
            _mockChessBoard.Setup(board => board.GetPieceAt(bishop.Position)).Returns(bishop);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = bishop.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The bishop should not be able to move non-diagonally.");
        }

        [Fact]
        public void Bishop_IsValidMove_ShouldAllowCapture()
        {
            // Arrange
            var bishop = new Bishop("White", new Position("c1"));
            var targetPosition = new Position("e3"); // Diagonal capture
            var blackPawn = new Pawn("Black", targetPosition);

            // Setup the board: Bishop at c1, black pawn at e3, path d2 empty
            _mockChessBoard.Setup(board => board.GetPieceAt(bishop.Position)).Returns(bishop);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("d2"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns(blackPawn);

            // Act
            bool isValid = bishop.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The bishop should be able to capture an opponent's piece diagonally.");
        }

        [Fact]
        public void Bishop_IsValidMove_ShouldRejectMoveToOccupiedSquareBySameColor()
        {
            // Arrange
            var bishop = new Bishop("White", new Position("c1"));
            var targetPosition = new Position("e3"); // Diagonal move to occupied square
            var whitePawn = new Pawn("White", targetPosition);

            // Setup the board: Bishop at c1, white pawn at e3, path d2 empty
            _mockChessBoard.Setup(board => board.GetPieceAt(bishop.Position)).Returns(bishop);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("d2"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns(whitePawn);

            // Act
            bool isValid = bishop.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The bishop should not be able to move to a square occupied by a piece of the same color.");
        }

        [Fact]
        public void Bishop_IsValidMove_ShouldRejectMoveOutOfBounds()
        {
            // Arrange
            var bishop = new Bishop("White", new Position("c1"));
            var targetPosition = new Position(4, -1); // Out of bounds position

            // Setup the board: Bishop at c1
            _mockChessBoard.Setup(board => board.GetPieceAt(bishop.Position)).Returns(bishop);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Throws(new ArgumentOutOfRangeException());

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => bishop.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object));
        }

        [Fact]
        public void Bishop_IsValidMove_ShouldRejectMoveIfPathIsNotClear()
        {
            // Arrange
            var bishop = new Bishop("White", new Position("c1"));
            var targetPosition = new Position("e3"); // Diagonal move with blockage at d2
            var blockingPawn = new Pawn("White", new Position("d2"));

            // Setup the board: Bishop at c1, white pawn at d2, target e3 empty
            _mockChessBoard.Setup(board => board.GetPieceAt(bishop.Position)).Returns(bishop);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("d2"))).Returns(blockingPawn);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = bishop.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The bishop should not be able to move if the path is blocked by a piece.");
        }

        [Fact]
        public void Bishop_IsValidMove_ShouldRejectMoveThroughOpponentPiece()
        {
            // Arrange
            var bishop = new Bishop("White", new Position("c1"));
            var targetPosition = new Position("e3"); // Diagonal move with opposition at d2
            var blockingKnight = new Knight("Black", new Position("d2"));

            // Setup the board: Bishop at c1, black knight at d2, target e3 empty
            _mockChessBoard.Setup(board => board.GetPieceAt(bishop.Position)).Returns(bishop);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("d2"))).Returns(blockingKnight);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = bishop.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The bishop should not be able to move through an opponent's piece.");
        }
    }
}

