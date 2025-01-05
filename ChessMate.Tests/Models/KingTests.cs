// File: ChessMate.Tests/Models/KingTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;

namespace ChessMate.Tests.Models
{
    public class KingTests
    {
        private readonly Mock<IChessBoard> _mockChessBoard;
        private readonly Mock<IStateService> _mockStateService;

        public KingTests()
        {
            _mockChessBoard = new Mock<IChessBoard>();
            _mockStateService = new Mock<IStateService>();
        }

        [Fact]
        public void King_IsValidMove_ShouldAllowSingleSquareMove()
        {
            // Arrange
            var king = new King("White", new Position("e1"));
            var targetPosition = new Position("e2"); // Move to e2

            // Setup the board: King at e1, target e2 empty
            _mockChessBoard.Setup(board => board.GetPieceAt(king.Position)).Returns(king);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Setup the state: e2 not under attack
            _mockStateService.Setup(state => state.WhiteAttacks).Returns(new HashSet<Position>());
            _mockStateService.Setup(state => state.BlackAttacks).Returns(new HashSet<Position>());

            // Act
            bool isValid = king.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The king should be able to move one square in any direction.");
        }

        [Fact]
        public void King_IsValidMove_ShouldRejectMoveMoreThanOneSquare()
        {
            // Arrange
            var king = new King("White", new Position("e1"));
            var targetPosition = new Position("e3"); // Move to e3 - more than one square

            // Setup the board: King at e1, target e3 empty
            _mockChessBoard.Setup(board => board.GetPieceAt(king.Position)).Returns(king);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Act
            bool isValid = king.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The king should not be able to move more than one square in any direction.");
        }

        [Fact]
        public void King_IsValidMove_ShouldAllowCapture()
        {
            // Arrange
            var king = new King("White", new Position("e1"));
            var targetPosition = new Position("e2"); // Capture at e2
            var blackPawn = new Pawn("Black", targetPosition);

            // Setup the board: King at e1, black pawn at e2
            _mockChessBoard.Setup(board => board.GetPieceAt(king.Position)).Returns(king);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns(blackPawn);

            // Setup the state: e2 under attack by Black does not affect capture
            _mockStateService.Setup(state => state.WhiteAttacks).Returns(new HashSet<Position>());
            _mockStateService.Setup(state => state.BlackAttacks).Returns(new HashSet<Position>());

            // Act
            bool isValid = king.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The king should be able to capture an opponent's piece.");
        }

        [Fact]
        public void King_IsValidMove_ShouldRejectMoveToOccupiedSquareBySameColor()
        {
            // Arrange
            var king = new King("White", new Position("e1"));
            var targetPosition = new Position("e2"); // Attempt to move to e2
            var whitePawn = new Pawn("White", targetPosition);

            // Setup the board: King at e1, white pawn at e2
            _mockChessBoard.Setup(board => board.GetPieceAt(king.Position)).Returns(king);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns(whitePawn);

            // Act
            bool isValid = king.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The king should not be able to move to a square occupied by a piece of the same color.");
        }

        [Fact]
        public void King_IsValidMove_ShouldRejectMoveOutOfBounds()
        {
            // Arrange
            var king = new King("White", new Position("e1"));
            var targetPosition = new Position(8, 4); // Out of bounds position

            // Setup the board: King at e1
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Throws(new ArgumentOutOfRangeException());

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                king.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object));
        }

        [Fact]
        public void King_IsValidMove_ShouldRejectMoveIntoCheck()
        {
            // Arrange
            var king = new King("White", new Position("e1"));
            var targetPosition = new Position("e2"); // Attempt to move to e2
            var blackRook = new Rook("Black", new Position("e3"));

            // Setup the board: King at e1, black rook at e3, target e2 empty
            _mockChessBoard.Setup(board => board.GetPieceAt(king.Position)).Returns(king);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("e3"))).Returns(blackRook);

            // Setup the state: e2 is under attack by black rook
            var attackedPositions = new HashSet<Position> { targetPosition };
            _mockStateService.Setup(state => state.WhiteAttacks).Returns(new HashSet<Position>());
            _mockStateService.Setup(state => state.BlackAttacks).Returns(attackedPositions);

            // Act
            bool isValid = king.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The king should not be able to move into check.");
        }

        [Fact]
        public void King_IsValidMove_ShouldConsiderAttackMaps()
        {
            // Arrange
            var king = new King("White", new Position("e1"));
            var targetPosition = new Position("e2"); // Square under attack
            var blackQueen = new Queen("Black", new Position("d2"));

            // Setup the board: King at e1, black queen at d2, target e2 empty
            _mockChessBoard.Setup(board => board.GetPieceAt(king.Position)).Returns(king);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("d2"))).Returns(blackQueen);

            // Setup the state: e2 is under attack by black queen
            var attackedPositions = new HashSet<Position> { targetPosition };
            _mockStateService.Setup(state => state.WhiteAttacks).Returns(new HashSet<Position>());
            _mockStateService.Setup(state => state.BlackAttacks).Returns(attackedPositions);

            // Act
            bool isValid = king.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The king should not be able to move into a square that is under attack.");
        }

        [Fact]
        public void King_IsValidMove_ShouldAllowCastling_WhenConditionsAreMet()
        {
            // Arrange
            var king = new King("White", new Position("e1"));
            var targetPosition = new Position("g1"); // Castling kingside

            // Setup the board: King at e1, Rook at h1, path f1 and g1 empty
            var rook = new Rook("White", new Position("h1"));
            _mockChessBoard.Setup(board => board.GetPieceAt(king.Position)).Returns(king);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("f1"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("g1"))).Returns((ChessPiece)null);
            _mockChessBoard.Setup(board => board.GetPieceAt(rook.Position)).Returns(rook);

            // Setup the state: Castling rights not exercised and e1 not under attack
            _mockStateService.Setup(state => state.WhiteKingMoved).Returns(false);
            _mockStateService.Setup(state => state.WhiteRookKingSideMoved).Returns(false);
            _mockStateService.Setup(state => state.BlackAttacks).Returns(new HashSet<Position>());
            _mockStateService.Setup(state => state.WhiteAttacks).Returns(new HashSet<Position>());

            // Act
            bool isValid = king.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "Castling is not implemented and should return false.");
        }
    }
}
