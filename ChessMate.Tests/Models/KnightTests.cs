// File: ChessMate.Tests/Models/KnightTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;

namespace ChessMate.Tests.Models
{
    public class KnightTests
    {
        private readonly Mock<IChessBoard> _mockChessBoard;
        private readonly Mock<IStateService> _mockStateService;

        public KnightTests()
        {
            _mockChessBoard = new Mock<IChessBoard>();
            _mockStateService = new Mock<IStateService>();
        }

        [Fact]
        public void Knight_IsValidMove_ShouldAllowLShapedMoves()
        {
            // Arrange
            var knight = new Knight("White", new Position("e4"));
            var validMoves = new List<Position>
            {
                new Position("d6"), // Up 2, Left 1
                new Position("f6"), // Up 2, Right 1
                new Position("c5"), // Up 1, Left 2
                new Position("g5"), // Up 1, Right 2
                new Position("c3"), // Down 1, Left 2
                new Position("g3"), // Down 1, Right 2
                new Position("d2"), // Down 2, Left 1
                new Position("f2"), // Down 2, Right 1
            };

            // Setup the board: Knight at e4, targets empty
            _mockChessBoard.Setup(board => board.GetPieceAt(knight.Position)).Returns(knight);
            foreach (var target in validMoves)
            {
                _mockChessBoard.Setup(board => board.GetPieceAt(target)).Returns((ChessPiece)null);
            }

            // Setup the state: No attacks affecting knight's movement
            _mockStateService.Setup(state => state.WhiteAttacks).Returns(new HashSet<Position>());
            _mockStateService.Setup(state => state.BlackAttacks).Returns(new HashSet<Position>());

            // Act & Assert
            foreach (var target in validMoves)
            {
                bool isValid = knight.IsValidMove(target, _mockChessBoard.Object, _mockStateService.Object);
                Assert.True(isValid, $"The knight should be able to move to {target}.");
            }
        }

        [Fact]
        public void Knight_IsValidMove_ShouldRejectInvalidMoves()
        {
            // Arrange
            var knight = new Knight("White", new Position("e4"));
            var invalidMoves = new List<Position>
            {
                new Position("e5"), // One square right
                new Position("f5"), // One square diagonal
                new Position("h7"), // Far away
                new Position("e4"), // Same position
            };

            // Setup the board: Knight at e4, targets empty
            _mockChessBoard.Setup(board => board.GetPieceAt(knight.Position)).Returns(knight);
            foreach (var target in invalidMoves)
            {
                _mockChessBoard.Setup(board => board.GetPieceAt(target)).Returns((ChessPiece)null);
            }

            // Setup the state: No attacks affecting knight's movement
            _mockStateService.Setup(state => state.WhiteAttacks).Returns(new HashSet<Position>());
            _mockStateService.Setup(state => state.BlackAttacks).Returns(new HashSet<Position>());

            // Act & Assert
            foreach (var target in invalidMoves)
            {
                bool isValid = knight.IsValidMove(target, _mockChessBoard.Object, _mockStateService.Object);
                Assert.False(isValid, $"The knight should not be able to move to {target}.");
            }
        }

        [Fact]
        public void Knight_IsValidMove_ShouldAllowCaptureOfOpponentPiece()
        {
            // Arrange
            var knight = new Knight("White", new Position("e4"));
            var opponentPawn = new Pawn("Black", new Position("d6"));
            var targetPosition = opponentPawn.Position;

            // Setup the board: Knight at e4, opponent pawn at d6
            _mockChessBoard.Setup(board => board.GetPieceAt(knight.Position)).Returns(knight);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns(opponentPawn);

            // Setup the state: No attacks affecting knight's movement
            _mockStateService.Setup(state => state.WhiteAttacks).Returns(new HashSet<Position>());
            _mockStateService.Setup(state => state.BlackAttacks).Returns(new HashSet<Position>());

            // Act
            bool isValid = knight.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The knight should be able to capture an opponent's piece.");
        }

        [Fact]
        public void Knight_IsValidMove_ShouldNotCaptureOwnPiece()
        {
            // Arrange
            var knight = new Knight("White", new Position("e4"));
            var ownPawn = new Pawn("White", new Position("d6"));
            var targetPosition = ownPawn.Position;

            // Setup the board: Knight at e4, own pawn at d6
            _mockChessBoard.Setup(board => board.GetPieceAt(knight.Position)).Returns(knight);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns(ownPawn);

            // Setup the state: No attacks affecting knight's movement
            _mockStateService.Setup(state => state.WhiteAttacks).Returns(new HashSet<Position>());
            _mockStateService.Setup(state => state.BlackAttacks).Returns(new HashSet<Position>());

            // Act
            bool isValid = knight.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.False(isValid, "The knight should not be able to capture its own piece.");
        }

        [Fact]
        public void Knight_IsValidMove_ShouldIgnorePiecesInTheWay()
        {
            // Arrange
            var knight = new Knight("White", new Position("b1"));
            var blockingPiece1 = new Pawn("White", new Position("b2"));
            var blockingPiece2 = new Pawn("Black", new Position("c2"));
            var targetPosition = new Position("c3"); // Valid L-shaped move

            // Setup the board: Knight at b1, blocking pieces at b2 and c2, target c3 empty
            _mockChessBoard.Setup(board => board.GetPieceAt(knight.Position)).Returns(knight);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("b2"))).Returns(blockingPiece1);
            _mockChessBoard.Setup(board => board.GetPieceAt(new Position("c2"))).Returns(blockingPiece2);
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Returns((ChessPiece)null);

            // Setup the state: No attacks affecting knight's movement
            _mockStateService.Setup(state => state.WhiteAttacks).Returns(new HashSet<Position>());
            _mockStateService.Setup(state => state.BlackAttacks).Returns(new HashSet<Position>());

            // Act
            bool isValid = knight.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object);

            // Assert
            Assert.True(isValid, "The knight should be able to move regardless of pieces in the way.");
        }

        [Fact]
        public void Knight_IsValidMove_ShouldRejectOutOfBoundsMove()
        {
            // Arrange
            var knight = new Knight("White", new Position("a1"));
            var targetPosition = new Position(8, 2); // Out of bounds

            // Setup the board: Knight at a1, target out of bounds
            _mockChessBoard.Setup(board => board.GetPieceAt(targetPosition)).Throws(new ArgumentOutOfRangeException());

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                knight.IsValidMove(targetPosition, _mockChessBoard.Object, _mockStateService.Object));
        }
    }
}
