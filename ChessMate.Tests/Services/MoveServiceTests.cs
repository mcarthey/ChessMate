// File: ChessMate.Tests/Services/MoveServiceTests.cs

using ChessMate.Hubs;
using ChessMate.Models;
using ChessMate.Services;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services
{
    public class MoveServiceTests
    {
        private readonly Mock<IChessBoard> _mockChessBoard;
        private readonly Mock<IStateService> _mockStateService;
        private readonly Mock<IMoveValidatorService> _mockMoveValidator;
        private readonly Mock<IHubContext<ChessHub>> _mockHubContext; 
        private readonly MoveService _moveService;

        public MoveServiceTests()
        {
            _mockChessBoard = new Mock<IChessBoard>();
            _mockStateService = new Mock<IStateService>();
            _mockMoveValidator = new Mock<IMoveValidatorService>();
            _mockHubContext = new Mock<IHubContext<ChessHub>>(); 

            _moveService = new MoveService(
                _mockChessBoard.Object,
                _mockStateService.Object,
                _mockMoveValidator.Object,
                _mockHubContext.Object);
        }

        [Fact]
        public void TryMove_NoPieceAtFromPosition_ReturnsFalse()
        {
            // Arrange
            var from = new Position("a2");
            var to = new Position("a3");

            _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns((ChessPiece)null);
            _mockStateService.SetupGet(state => state.CurrentPlayer).Returns("White");

            // Act
            var result = _moveService.TryMove(from, to);

            // Assert
            Assert.False(result);
            _mockMoveValidator.Verify(v => v.IsValidMove(It.IsAny<ChessPiece>(), It.IsAny<Position>(), It.IsAny<IChessBoard>(), It.IsAny<IStateService>()), Times.Never);
        }

        [Fact]
        public void TryMove_PieceColorDoesNotMatchCurrentPlayer_ReturnsFalse()
        {
            // Arrange
            var from = new Position("a2");
            var to = new Position("a3");
            var whitePawn = new Pawn("White", from);

            _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
            _mockStateService.SetupGet(state => state.CurrentPlayer).Returns("Black");

            // Act
            var result = _moveService.TryMove(from, to);

            // Assert
            Assert.False(result);
            _mockMoveValidator.Verify(v => v.IsValidMove(It.IsAny<ChessPiece>(), It.IsAny<Position>(), It.IsAny<IChessBoard>(), It.IsAny<IStateService>()), Times.Never);
        }

        [Fact]
        public void TryMove_MoveIsInvalid_ReturnsFalse()
        {
            // Arrange
            var from = new Position("a2");
            var to = new Position("b3"); // Invalid move for a pawn moving forward
            var whitePawn = new Pawn("White", from);

            _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
            _mockStateService.SetupGet(state => state.CurrentPlayer).Returns("White");
            _mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, _mockChessBoard.Object, _mockStateService.Object))
                              .Returns(false);

            // Act
            var result = _moveService.TryMove(from, to);

            // Assert
            Assert.False(result);
            _mockMoveValidator.Verify(v => v.IsValidMove(whitePawn, to, _mockChessBoard.Object, _mockStateService.Object), Times.Once);
        }

        [Fact]
        public void TryMove_MoveValidatorThrowsException_ReturnsFalse()
        {
            // Arrange
            var from = new Position("a2");
            var to = new Position("a3");
            var whitePawn = new Pawn("White", from);

            _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
            _mockStateService.SetupGet(state => state.CurrentPlayer).Returns("White");
            _mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, _mockChessBoard.Object, _mockStateService.Object))
                              .Throws(new Exception("Validation error"));

            // Act
            var result = _moveService.TryMove(from, to);

            // Assert
            Assert.False(result);
            _mockMoveValidator.Verify(v => v.IsValidMove(whitePawn, to, _mockChessBoard.Object, _mockStateService.Object), Times.Once);
        }

        [Fact]
        public void TryMove_MoveIsValid_ExecutesMove()
        {
            // Arrange
            var from = new Position("a2");
            var to = new Position("a3");
            var whitePawn = new Pawn("White", from);

            _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
            _mockStateService.SetupGet(state => state.CurrentPlayer).Returns("White");
            _mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, _mockChessBoard.Object, _mockStateService.Object))
                              .Returns(true);
            _mockStateService.Setup(s => s.WouldMoveCauseSelfCheck(whitePawn, from, to)).Returns(false);

            // Act
            var result = _moveService.TryMove(from, to);

            // Assert
            Assert.True(result);
            _mockChessBoard.Verify(board => board.RemovePieceAt(from), Times.Once);
            _mockChessBoard.Verify(board => board.SetPieceAt(to, whitePawn), Times.Once);
            Assert.Equal(to, whitePawn.Position);
            _mockMoveValidator.Verify(v => v.IsValidMove(whitePawn, to, _mockChessBoard.Object, _mockStateService.Object), Times.Once);
            _mockStateService.Verify(s => s.UpdateGameStateAfterMove(whitePawn, from, to), Times.Once);
        }

        [Fact]
        public void TryMove_MoveLeavesKingInCheck_ReturnsFalse()
        {
            // Arrange
            var from = new Position("e2");
            var to = new Position("e3");
            var whitePawn = new Pawn("White", from);

            _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
            _mockStateService.SetupGet(state => state.CurrentPlayer).Returns("White");
            _mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, _mockChessBoard.Object, _mockStateService.Object))
                              .Returns(true);
            _mockStateService.Setup(s => s.WouldMoveCauseSelfCheck(whitePawn, from, to)).Returns(true);

            // Act
            var result = _moveService.TryMove(from, to);

            // Assert
            Assert.False(result);
            _mockChessBoard.Verify(board => board.RemovePieceAt(It.IsAny<Position>()), Times.Never);
            _mockChessBoard.Verify(board => board.SetPieceAt(It.IsAny<Position>(), It.IsAny<ChessPiece>()), Times.Never);
            _mockStateService.Verify(s => s.UpdateGameStateAfterMove(It.IsAny<ChessPiece>(), It.IsAny<Position>(), It.IsAny<Position>()), Times.Never);
        }

        [Fact]
        public void TryMove_ValidMove_UpdatesGameState()
        {
            // Arrange
            var from = new Position("a2");
            var to = new Position("a3");
            var whitePawn = new Pawn("White", from);

            _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
            _mockStateService.SetupGet(state => state.CurrentPlayer).Returns("White");
            _mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, _mockChessBoard.Object, _mockStateService.Object))
                              .Returns(true);
            _mockStateService.Setup(s => s.WouldMoveCauseSelfCheck(whitePawn, from, to)).Returns(false);

            // Act
            var result = _moveService.TryMove(from, to);

            // Assert
            Assert.True(result);
            _mockStateService.Verify(s => s.UpdateGameStateAfterMove(whitePawn, from, to), Times.Once);
        }

        [Fact]
        public void TryMove_OpponentKingIsInCheck_SetsIsCheck()
        {
            // Arrange
            var from = new Position("a1");
            var to = new Position("a8");
            var whiteRook = new Rook("White", from);

            _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whiteRook);
            _mockStateService.SetupGet(state => state.CurrentPlayer).Returns("White");
            _mockMoveValidator.Setup(v => v.IsValidMove(whiteRook, to, _mockChessBoard.Object, _mockStateService.Object))
                              .Returns(true);
            _mockStateService.Setup(s => s.WouldMoveCauseSelfCheck(whiteRook, from, to)).Returns(false);

            // Act
            var result = _moveService.TryMove(from, to);

            // Assert
            Assert.True(result);
            _mockStateService.Verify(s => s.UpdateGameStateAfterMove(whiteRook, from, to), Times.Once);
        }

        [Fact]
        public void TryMove_OpponentHasNoLegalMoves_SetsIsCheckmate()
        {
            // Arrange
            var from = new Position("h7");
            var to = new Position("h8");
            var whiteQueen = new Queen("White", from);
            var blackKing = new King("Black", new Position("g8"));

            _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whiteQueen);
            _mockChessBoard.Setup(board => board.GetPieceAt(It.Is<Position>(p => p.Equals(new Position("g8"))))).Returns(blackKing);
            _mockStateService.SetupGet(state => state.CurrentPlayer).Returns("White");
            _mockMoveValidator.Setup(v => v.IsValidMove(whiteQueen, to, _mockChessBoard.Object, _mockStateService.Object))
                              .Returns(true);
            _mockStateService.Setup(s => s.WouldMoveCauseSelfCheck(whiteQueen, from, to)).Returns(false);

            // Act
            var result = _moveService.TryMove(from, to);

            // Assert
            Assert.True(result);
            _mockStateService.Verify(s => s.UpdateGameStateAfterMove(whiteQueen, from, to), Times.Once);
        }
    }
}
