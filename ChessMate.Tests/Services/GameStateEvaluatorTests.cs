// File: ChessMate.Tests/Services/GameStateEvaluatorTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services;

public class GameStateEvaluatorTests : TestHelper
{
    private readonly Mock<IChessBoard> _mockChessBoard;
    private readonly GameStateEvaluator _gameStateEvaluator;

    public GameStateEvaluatorTests(ITestOutputHelper output) : base(output)
    {
        _mockChessBoard = new Mock<IChessBoard>();
        _gameStateEvaluator = new GameStateEvaluator();
    }

    [Fact]
    public void WouldMoveCauseSelfCheck_MoveLeavesKingInCheck_ReturnsTrue()
    {
        // Arrange
        var from = new Position("e5");
        var to = new Position("e6");
        var whiteKing = new King("White", new Position("e1"));
        var whitePawn = new Pawn("White", from);
        var blackRook = new Rook("Black", new Position("a1"));

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        // Set up the board
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);
        _mockChessBoard.Setup(board => board.GetPieceAt(whiteKing.Position)).Returns(whiteKing);
        _mockChessBoard.Setup(board => board.FindKing("White")).Returns(whiteKing.Position);

        // Add opponent piece that can attack the king after the move
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { whitePawn, whiteKing, blackRook });

        // Act
        var result = _gameStateEvaluator.WouldMoveCauseSelfCheck(whitePawn, from, to, gameContext);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void WouldMoveCauseSelfCheck_MoveDoesNotLeaveKingInCheck_ReturnsFalse()
    {
        // Arrange
        var from = new Position("e5");
        var to = new Position("e6");
        var whiteKing = new King("White", new Position("e1"));
        var whitePawn = new Pawn("White", from);

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        // Set up the board
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);
        _mockChessBoard.Setup(board => board.GetPieceAt(whiteKing.Position)).Returns(whiteKing);
        _mockChessBoard.Setup(board => board.FindKing("White")).Returns(whiteKing.Position);

        // No opponent pieces that can attack the king after the move
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { whitePawn, whiteKing });

        // Act
        var result = _gameStateEvaluator.WouldMoveCauseSelfCheck(whitePawn, from, to, gameContext);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsKingInCheck_KingIsInCheck_ReturnsTrue()
    {
        // Arrange
        var whiteKing = new King("White", new Position("e1"));
        var blackRook = new Rook("Black", new Position("a1"));

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        // Set up the board
        _mockChessBoard.Setup(board => board.GetPieceAt(whiteKing.Position)).Returns(whiteKing);
        _mockChessBoard.Setup(board => board.FindKing("White")).Returns(whiteKing.Position);

        // Add opponent piece that can attack the king
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { whiteKing, blackRook });
        _mockChessBoard.Setup(board => board.GetPieceAt(blackRook.Position)).Returns(blackRook);

        // Mock the black rook's IsValidMove to return true when targeting the king's position
        var mockRook = new Mock<Rook>("Black", blackRook.Position) { CallBase = true };
        mockRook.Setup(r => r.IsValidMove(whiteKing.Position, gameContext)).Returns(true);
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { whiteKing, mockRook.Object });

        // Act
        var result = _gameStateEvaluator.IsKingInCheck("White", gameContext);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsKingInCheck_KingIsNotInCheck_ReturnsFalse()
    {
        // Arrange
        var whiteKing = new King("White", new Position("e1"));

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        // Set up the board
        _mockChessBoard.Setup(board => board.GetPieceAt(whiteKing.Position)).Returns(whiteKing);
        _mockChessBoard.Setup(board => board.FindKing("White")).Returns(whiteKing.Position);

        // No opponent pieces that can attack the king
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { whiteKing });

        // Act
        var result = _gameStateEvaluator.IsKingInCheck("White", gameContext);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasLegalMoves_PlayerHasLegalMoves_ReturnsTrue()
    {
        // Arrange
        var whiteKing = new King("White", new Position("e1"));
        var whitePawn = new Pawn("White", new Position("e2"));

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        // Set up the board
        _mockChessBoard.Setup(board => board.GetPieceAt(whiteKing.Position)).Returns(whiteKing);
        _mockChessBoard.Setup(board => board.GetPieceAt(whitePawn.Position)).Returns(whitePawn);
        _mockChessBoard.Setup(board => board.FindKing("White")).Returns(whiteKing.Position);
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { whiteKing, whitePawn });

        // Mock the pawn's IsValidMove to return true for a valid move
        var mockPawn = new Mock<Pawn>("White", whitePawn.Position) { CallBase = true };
        mockPawn.Setup(p => p.IsValidMove(It.IsAny<Position>(), gameContext))
                .Returns<Position, IGameContext>((pos, ctx) => pos.Equals(new Position("e3")));
        _mockChessBoard.Setup(board => board.GetPieceAt(whitePawn.Position)).Returns(mockPawn.Object);

        // Act
        var result = _gameStateEvaluator.HasLegalMoves("White", gameContext);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasLegalMoves_PlayerHasNoLegalMoves_ReturnsFalse()
    {
        // Arrange
        var whiteKing = new King("White", new Position("e1"));
        var blackRook = new Rook("Black", new Position("e5"));
        var blackQueen = new Queen("Black", new Position("e6"));

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        // Set up the board
        _mockChessBoard.Setup(board => board.GetPieceAt(whiteKing.Position)).Returns(whiteKing);
        _mockChessBoard.Setup(board => board.GetPieceAt(blackQueen.Position)).Returns(blackQueen);
        _mockChessBoard.Setup(board => board.GetPieceAt(blackRook.Position)).Returns(blackRook);
        _mockChessBoard.Setup(board => board.FindKing("White")).Returns(whiteKing.Position);
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { whiteKing, blackQueen, blackRook });

        // Mock opponent pieces' IsValidMove methods
        var mockBlackQueen = new Mock<Queen>("Black", blackQueen.Position) { CallBase = true };
        var mockBlackRook = new Mock<Rook>("Black", blackRook.Position) { CallBase = true };
        mockBlackQueen.Setup(q => q.IsValidMove(whiteKing.Position, gameContext)).Returns(true);
        mockBlackRook.Setup(r => r.IsValidMove(whiteKing.Position, gameContext)).Returns(true);
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { whiteKing, mockBlackQueen.Object, mockBlackRook.Object });

        // Act
        var result = _gameStateEvaluator.HasLegalMoves("White", gameContext);

        // Assert
        Assert.False(result);
    }
}



