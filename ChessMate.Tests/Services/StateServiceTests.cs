// File: ChessMate.Tests/Services/StateServiceTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services
{
    public class StateServiceTests : TestHelper
    {
        private readonly Mock<IChessBoard> _mockChessBoard;
        private readonly StateService _stateService;

        public StateServiceTests(ITestOutputHelper output) : base(output)
        {
            _mockChessBoard = new Mock<IChessBoard>();
            _stateService = new StateService(_mockChessBoard.Object);
        }

        [Fact]
        public void InitialState_CurrentPlayer_ShouldBeWhite()
        {
            // Arrange & Act
            var currentPlayer = _stateService.CurrentPlayer;

            // Assert
            Assert.Equal("White", currentPlayer);
        }

        [Fact]
        public void SwitchPlayer_TogglesCurrentPlayer()
        {
            // Arrange & Act
            _stateService.SwitchPlayer();

            // Assert
            Assert.Equal("Black", _stateService.CurrentPlayer);

            // Switch back
            _stateService.SwitchPlayer();
            Assert.Equal("White", _stateService.CurrentPlayer);
        }

        [Fact]
        public void SetPlayer_ValidPlayer_SetsCurrentPlayer()
        {
            // Arrange & Act
            _stateService.SetPlayer("Black");

            // Assert
            Assert.Equal("Black", _stateService.CurrentPlayer);
        }

        [Fact]
        public void SetPlayer_InvalidPlayer_ThrowsException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _stateService.SetPlayer("Green"));
            Assert.Equal("Invalid player. Must be 'White' or 'Black'.", exception.Message);
        }

        [Fact]
        public void SetEnPassantTarget_Pawn_SetsTarget()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("a2"));
            var targetPosition = new Position("a3");

            // Act
            _stateService.SetEnPassantTarget(targetPosition, pawn);

            // Assert
            Assert.Equal(targetPosition, _stateService.EnPassantTarget);
        }

        [Fact]
        public void SetEnPassantTarget_NonPawn_DoesNotSetTarget()
        {
            // Arrange
            var rook = new Rook("White", new Position("a2"));
            var targetPosition = new Position("a3");

            // Act
            _stateService.SetEnPassantTarget(targetPosition, rook);

            // Assert
            Assert.Null(_stateService.EnPassantTarget);
        }

        [Fact]
        public void ResetEnPassantTarget_ClearsTarget()
        {
            // Arrange
            var pawn = new Pawn("White", new Position("a2"));
            var targetPosition = new Position("a3");
            _stateService.SetEnPassantTarget(targetPosition, pawn);

            // Act
            _stateService.ResetEnPassantTarget();

            // Assert
            Assert.Null(_stateService.EnPassantTarget);
        }

        [Fact]
        public void ResetState_ResetsAllProperties()
        {
            // Arrange
            // Change various properties
            _stateService.SetPlayer("Black");
            _stateService.IsCheck = true;
            _stateService.IsCheckmate = true;
            var pawn = new Pawn("White", new Position("a2"));
            _stateService.SetEnPassantTarget(new Position("a3"), pawn);
            _stateService.WhiteKingMoved = true;
            _stateService.BlackKingMoved = true;
            _stateService.WhiteRookKingSideMoved = true;
            _stateService.WhiteRookQueenSideMoved = true;
            _stateService.BlackRookKingSideMoved = true;
            _stateService.BlackRookQueenSideMoved = true;
            _stateService.MoveLog.Add("Sample Move");

            // Simulate attack maps having entries
            // Since WhiteAttacks and BlackAttacks are now managed internally,
            // we avoid directly modifying them. Instead, we assume that setting up the board
            // would populate the attack maps if there were attacking pieces.
            // For this test, we'll ensure that after ResetState, the attack maps are empty.

            // Act
            _stateService.ResetState();

            // Assert
            // Verify all properties are reset
            Assert.Equal("White", _stateService.CurrentPlayer);
            Assert.False(_stateService.IsCheck);
            Assert.False(_stateService.IsCheckmate);
            Assert.Null(_stateService.EnPassantTarget);
            Assert.False(_stateService.WhiteKingMoved);
            Assert.False(_stateService.BlackKingMoved);
            Assert.False(_stateService.WhiteRookKingSideMoved);
            Assert.False(_stateService.WhiteRookQueenSideMoved);
            Assert.False(_stateService.BlackRookKingSideMoved);
            Assert.False(_stateService.BlackRookQueenSideMoved);
            Assert.Empty(_stateService.MoveLog);

            // Check that attack maps are cleared
            var allPositions = Enumerable.Range(0, 8)
                .SelectMany(row => Enumerable.Range(0, 8)
                    .Select(col => new Position(row, col))).ToList();

            foreach (var pos in allPositions)
            {
                Assert.Empty(_stateService.GetWhiteAttackers(pos));
                Assert.Empty(_stateService.GetBlackAttackers(pos));
            }
        }


        [Fact]
        public void CastlingFlags_DefaultToFalse()
        {
            // Arrange & Act

            // Assert
            Assert.False(_stateService.WhiteKingMoved);
            Assert.False(_stateService.BlackKingMoved);
            Assert.False(_stateService.WhiteRookKingSideMoved);
            Assert.False(_stateService.WhiteRookQueenSideMoved);
            Assert.False(_stateService.BlackRookKingSideMoved);
            Assert.False(_stateService.BlackRookQueenSideMoved);
        }

        [Fact]
        public void MoveLog_InitializesEmpty()
        {
            // Arrange & Act
            var moveLog = _stateService.MoveLog;

            // Assert
            Assert.NotNull(moveLog);
            Assert.Empty(moveLog);
        }

        [Fact]
        public void MoveLog_ClearsOnReset()
        {
            // Arrange
            _stateService.MoveLog.Add("e2e4");
            _stateService.MoveLog.Add("e7e5");
            Assert.NotEmpty(_stateService.MoveLog);

            // Act
            _stateService.ResetState();

            // Assert
            Assert.Empty(_stateService.MoveLog);
        }

        [Fact]
        public void IsCheck_Settable()
        {
            // Arrange & Act
            _stateService.IsCheck = true;

            // Assert
            Assert.True(_stateService.IsCheck);
        }

        [Fact]
        public void IsCheckmate_Settable()
        {
            // Arrange & Act
            _stateService.IsCheckmate = true;

            // Assert
            Assert.True(_stateService.IsCheckmate);
        }

        [Fact]
        public void UpdateGameStateAfterMove_LogsMoveAndSwitchesPlayer()
        {
            // Arrange
            var from = new Position("a2");
            var to = new Position("a3");
            var whitePawn = new Pawn("White", from);

            // Setup board behavior
            _mockChessBoard.Setup(cb => cb.GetAllPieces()).Returns(new List<ChessPiece> { whitePawn });
            _mockChessBoard.Setup(cb => cb.FindKing(It.IsAny<string>())).Returns(new Position("e1"));

            // Act
            _stateService.UpdateGameStateAfterMove(whitePawn, from, to);

            // Assert
            Assert.Single(_stateService.MoveLog);
            Assert.Equal("White Pawn from a2 to a3", _stateService.MoveLog.First());
            Assert.Equal("Black", _stateService.CurrentPlayer);
        }

        [Fact]
        public void UpdateAttackMaps_CorrectlyUpdatesAttackMaps()
        {
            // Arrange
            var whiteRookPosition = new Position("a1");
            var blackRookPosition = new Position("h8");

            var whiteRook = new Rook("White", whiteRookPosition);
            var blackRook = new Rook("Black", blackRookPosition);

            var pieces = new List<ChessPiece> { whiteRook, blackRook };
            _mockChessBoard.Setup(cb => cb.GetAllPieces()).Returns(pieces);

            // Act
            _stateService.UpdateAttackMaps();

            // Assert
            // White rook attacks along column 'a' and row '1', excluding its own square 'a1'
            for (char file = 'a'; file <= 'h'; file++)
            {
                var position = new Position($"{file}1");
                if (position != whiteRookPosition)
                {
                    Assert.Contains(whiteRook, _stateService.GetWhiteAttackers(position));
                }
                else
                {
                    Assert.DoesNotContain(whiteRook, _stateService.GetWhiteAttackers(position));
                }
            }

            for (int rank = 1; rank <= 8; rank++)
            {
                var position = new Position($"a{rank}");
                if (position != whiteRookPosition)
                {
                    Assert.Contains(whiteRook, _stateService.GetWhiteAttackers(position));
                }
                else
                {
                    Assert.DoesNotContain(whiteRook, _stateService.GetWhiteAttackers(position));
                }
            }

            // Black rook attacks along column 'h' and row '8', excluding its own square 'h8'
            for (char file = 'a'; file <= 'h'; file++)
            {
                var position = new Position($"{file}8");
                if (position != blackRookPosition)
                {
                    Assert.Contains(blackRook, _stateService.GetBlackAttackers(position));
                }
                else
                {
                    Assert.DoesNotContain(blackRook, _stateService.GetBlackAttackers(position));
                }
            }

            for (int rank = 1; rank <= 8; rank++)
            {
                var position = new Position($"h{rank}");
                if (position != blackRookPosition)
                {
                    Assert.Contains(blackRook, _stateService.GetBlackAttackers(position));
                }
                else
                {
                    Assert.DoesNotContain(blackRook, _stateService.GetBlackAttackers(position));
                }
            }
        }


        [Fact]
        public void IsKingInCheck_KingUnderAttack_ReturnsTrue()
        {
            // Arrange
            var whiteKing = new King("White", new Position("e1"));
            var blackRook = new Rook("Black", new Position("e8"));

            var pieces = new List<ChessPiece> { whiteKing, blackRook };
            _mockChessBoard.Setup(cb => cb.FindKing("White")).Returns(whiteKing.Position);
            _mockChessBoard.Setup(cb => cb.GetAllPieces()).Returns(pieces);

            // Act
            _stateService.UpdateAttackMaps();

            // Assert
            Assert.True(_stateService.IsKingInCheck("White"));
        }


        [Fact]
        public void HasLegalMoves_PlayerHasNoLegalMoves_ReturnsFalse()
        {
            // Arrange
            var whiteKingPosition = new Position("h1");
            var whiteKing = new King("White", whiteKingPosition);
            var blackRook1 = new Rook("Black", new Position("f2"));
            var blackRook2 = new Rook("Black", new Position("f1"));

            var pieces = new List<ChessPiece> { whiteKing, blackRook1, blackRook2 };
            _mockChessBoard.Setup(cb => cb.GetAllPieces()).Returns(pieces);

            // Mocking GetPieceAt to return the correct piece for each position
            _mockChessBoard.Setup(cb => cb.GetPieceAt(It.IsAny<Position>())).Returns<Position>(pos =>
                pos.Equals(whiteKingPosition) ? whiteKing :
                pos.Equals(blackRook1.Position) ? blackRook1 :
                pos.Equals(blackRook2.Position) ? blackRook2 : null);

            // Mocking FindKing to return the correct position of the white king
            _mockChessBoard.Setup(cb => cb.FindKing("White")).Returns(whiteKingPosition);

            PrintBoard(_mockChessBoard.Object);

            // Act
            var hasLegalMoves = _stateService.HasLegalMoves("White");

            // Assert
            Assert.False(hasLegalMoves);
        }

        [Fact]
        public void WouldMoveCauseSelfCheck_MovingPieceExposesKing_ReturnsTrue()
        {
            // Arrange
            var whiteKing = new King("White", new Position("e1"));
            var whiteBishop = new Bishop("White", new Position("e2"));
            var blackRook = new Rook("Black", new Position("e8"));

            // Initialize a real chess board with the specified pieces
            var chessBoard = InitializeCustomBoard(
                (whiteKing, whiteKing.Position),
                (whiteBishop, whiteBishop.Position),
                (blackRook, blackRook.Position)
            );

            // Instantiate a new StateService with the real chess board
            var realStateService = new StateService(chessBoard);

            // Update attack maps based on the current board state
            realStateService.UpdateAttackMaps();

            // Act
            var wouldCauseSelfCheck = realStateService.WouldMoveCauseSelfCheck(whiteBishop, whiteBishop.Position, new Position("d3"));

            // Assert
            Assert.True(wouldCauseSelfCheck);
        }

        [Fact]
        public void UpdateGameStateAfterMove_IdentifiesCheckmate()
        {
            // Arrange
            var blackKing = new King("Black", new Position("h8"));
            var whiteQueen = new Queen("White", new Position("g6"));
            var whiteRook = new Rook("White", new Position("h7"));

            var pieces = new List<ChessPiece> { blackKing, whiteQueen, whiteRook };
            _mockChessBoard.Setup(cb => cb.FindKing("Black")).Returns(blackKing.Position);
            _mockChessBoard.Setup(cb => cb.GetAllPieces()).Returns(pieces);

            _stateService.UpdateAttackMaps();

            // Act
            _stateService.UpdateGameStateAfterMove(whiteQueen, whiteQueen.Position, whiteQueen.Position); // Assuming move is a placeholder

            // Assert
            Assert.True(_stateService.IsCheck);
            Assert.True(_stateService.IsCheckmate);
        }

    }
}
