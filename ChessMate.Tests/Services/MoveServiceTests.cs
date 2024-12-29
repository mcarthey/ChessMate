// File: ChessMate.Tests/Services/MoveServiceTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services;

public class MoveServiceTests : TestHelper
{
    private readonly Mock<IChessBoard> _mockChessBoard;
    private readonly Mock<IMoveValidatorService> _mockMoveValidator;
    private readonly Mock<IGameStateEvaluator> _mockGameStateEvaluator;

    public MoveServiceTests(ITestOutputHelper output) : base(output)
    {
        _mockChessBoard = new Mock<IChessBoard>();
        _mockMoveValidator = new Mock<IMoveValidatorService>();
        _mockGameStateEvaluator = new Mock<IGameStateEvaluator>();
    }

    [Fact]
    public void TryMove_NoPieceAtFromPosition_ReturnsFalse()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns((ChessPiece)null);

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        var moveService = new MoveService(gameContext, _mockMoveValidator.Object, _mockGameStateEvaluator.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_PieceColorDoesNotMatchCurrentPlayer_ReturnsFalse()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        var whitePawn = new Pawn("White", from);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("Black")
            .Build();

        var moveService = new MoveService(gameContext, _mockMoveValidator.Object, _mockGameStateEvaluator.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_MoveIsInvalid_ReturnsFalse()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("b3"); // Invalid move for a pawn moving forward
        var whitePawn = new Pawn("White", from);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        var moveService = new MoveService(gameContext, _mockMoveValidator.Object, _mockGameStateEvaluator.Object);

        // Mock the pawn's IsValidMove to return false
        _mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, gameContext)).Returns(false);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_MoveValidatorThrowsException_ReturnsFalse()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        var whitePawn = new Pawn("White", from);

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        var moveService = new MoveService(gameContext, _mockMoveValidator.Object, _mockGameStateEvaluator.Object);

        // Mock the pawn's IsValidMove to throw an exception
        _mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, gameContext)).Throws(new Exception("Test exception"));

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_MoveIsValid_ExecutesMove()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        var whitePawn = new Pawn("White", from);

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        var moveService = new MoveService(gameContext, _mockMoveValidator.Object, _mockGameStateEvaluator.Object);

        // Set up the board
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);

        // Mock IsValidMove to return true
        _mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, gameContext)).Returns(true);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);
        _mockChessBoard.Verify(board => board.SetPieceAt(to, whitePawn), Times.Once);
        _mockChessBoard.Verify(board => board.RemovePieceAt(from), Times.Once);
        Assert.Equal(to, whitePawn.Position);
    }

    [Fact]
    public void TryMove_MoveLeavesKingInCheck_ReturnsFalse()
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

        var moveService = new MoveService(gameContext, _mockMoveValidator.Object, _mockGameStateEvaluator.Object);

        // Set up the board
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);
        _mockChessBoard.Setup(board => board.GetPieceAt(whiteKing.Position)).Returns(whiteKing);
        _mockChessBoard.Setup(board => board.FindKing("White")).Returns(whiteKing.Position);

        // Add opponent piece that can attack the king after the move
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { whitePawn, whiteKing, blackRook });

        // Mock IsValidMove to return true for pawn
        _mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, gameContext)).Returns(true);

        // Mock the black rook's IsValidMove to return true when targeting the king's position
        _mockGameStateEvaluator.Setup(e => e.WouldMoveCauseSelfCheck(whitePawn, from, to, gameContext)).Returns(true);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_ValidMove_UpdatesGameState()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        var whitePawn = new Pawn("White", from);

        var gameContextBuilder = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White");

        var gameContext = gameContextBuilder.Build();

        var moveService = new MoveService(gameContext, _mockMoveValidator.Object, _mockGameStateEvaluator.Object);

        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);

        // Mock IsValidMove to return true
        _mockMoveValidator.Setup(v => v.IsValidMove(whitePawn, to, gameContext)).Returns(true);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);

        // Verify that the player was switched
        gameContextBuilder.VerifyStateService(s => s.Verify(ss => ss.SwitchPlayer(), Times.Once));
    }

    [Fact]
    public void TryMove_OpponentKingIsInCheck_SetsIsCheck()
    {
        // Arrange
        var from = new Position("a1");
        var to = new Position("h1");
        var blackKing = new King("Black", new Position("e8"));
        var whiteRook = new Rook("White", from);

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .WithCheck(false)
            .WithCheckmate(false)
            .Build();

        var moveService = new MoveService(gameContext, _mockMoveValidator.Object, _mockGameStateEvaluator.Object);

        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whiteRook);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);
        _mockChessBoard.Setup(board => board.GetPieceAt(blackKing.Position)).Returns(blackKing);
        _mockChessBoard.Setup(board => board.FindKing("Black")).Returns(blackKing.Position);

        // Mock IsValidMove to return true
        _mockMoveValidator.Setup(v => v.IsValidMove(whiteRook, to, gameContext)).Returns(true);

        // Mock opponent pieces
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { blackKing, whiteRook });

        // Mock that after the move, rook can attack the black king
        _mockGameStateEvaluator.Setup(e => e.IsKingInCheck("Black", gameContext)).Returns(true);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);
        Assert.True(gameContext.State.IsCheck);
    }

    [Fact]
    public void TryMove_OpponentHasNoLegalMoves_SetsIsCheckmate()
    {
        // Arrange
        var from = new Position("a1");
        var to = new Position("a2");
        var blackKing = new King("Black", new Position("e8"));

        // Only pieces on the board are white rook and black king
        var whiteRook = new Rook("White", from);

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .WithCheck(false)
            .WithCheckmate(false)
            .Build();

        var moveService = new MoveService(gameContext, _mockMoveValidator.Object, _mockGameStateEvaluator.Object);

        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whiteRook);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);
        _mockChessBoard.Setup(board => board.FindKing("Black")).Returns(blackKing.Position);
        _mockChessBoard.Setup(board => board.GetPieceAt(blackKing.Position)).Returns(blackKing);

        // Mock IsValidMove to return true
        _mockMoveValidator.Setup(v => v.IsValidMove(whiteRook, to, gameContext)).Returns(true);

        // Mock opponent pieces
        _mockChessBoard.SetupSequence(board => board.GetAllPieces())
            .Returns(new List<ChessPiece> { whiteRook, blackKing }) // Before move
            .Returns(new List<ChessPiece> { whiteRook, blackKing }); // After move

        // Mock that black king has no legal moves
        _mockGameStateEvaluator.Setup(e => e.HasLegalMoves("Black", gameContext)).Returns(false);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);
        Assert.True(gameContext.State.IsCheckmate);
    }
}




