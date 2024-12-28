using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services;

public class MoveServiceTests : TestHelper
{
    private readonly Mock<IChessBoard> _mockChessBoard;
    private readonly Mock<IStateService> _mockStateService;

    public MoveServiceTests(ITestOutputHelper output) : base(output)
    {
        _mockChessBoard = new Mock<IChessBoard>();
        _mockStateService = new Mock<IStateService>();
    }

    [Fact]
    public void TryMove_ShouldReturnFalseIfNoPieceAtFromPosition()
    {
        // Arrange
        var from = (6, 0);
        var to = (5, 0);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns((ChessPiece)null);
        _mockStateService.Setup(s => s.CurrentPlayer).Returns("White");

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        var moveService = new MoveService(gameContext);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_ShouldReturnFalseIfPieceColorDoesNotMatchCurrentPlayer()
    {
        // Arrange
        var from = (6, 0);
        var to = (5, 0);
        var whitePawn = new Pawn("White", from);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockStateService.Setup(s => s.CurrentPlayer).Returns("Black");

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("Black")
            .Build();

        var moveService = new MoveService(gameContext);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_ShouldReturnFalseIfMoveIsInvalid()
    {
        // Arrange
        var from = (6, 0);
        var to = (5, 1); // Invalid move for a pawn moving forward
        var whitePawn = new Pawn("White", from);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);
        _mockStateService.Setup(s => s.CurrentPlayer).Returns("White");

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        var moveService = new MoveService(gameContext);

        // Mock the pawn's IsValidMove to return false
        var mockPawn = new Mock<Pawn>("White", from) { CallBase = true };
        mockPawn.Setup(p => p.IsValidMove(to, gameContext)).Returns(false);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(mockPawn.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_ShouldExecuteMoveIfValid()
    {
        // Arrange
        var from = (6, 0);
        var to = (5, 0);
        var whitePawn = new Pawn("White", from);
        _mockStateService.Setup(s => s.CurrentPlayer).Returns("White");

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        var moveService = new MoveService(gameContext);

        // Set up the board
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);

        // Mock IsValidMove to return true
        var mockPawn = new Mock<Pawn>("White", from) { CallBase = true };
        mockPawn.Setup(p => p.IsValidMove(to, gameContext)).Returns(true);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(mockPawn.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);
        _mockChessBoard.Verify(board => board.SetPieceAt(to, mockPawn.Object), Times.Once);
        _mockChessBoard.Verify(board => board.RemovePieceAt(from), Times.Once);
        Assert.Equal(to, mockPawn.Object.Position);
    }

    [Fact]
    public void TryMove_ShouldReturnFalseIfMoveLeavesKingInCheck()
    {
        // Arrange
        var from = (4, 4);
        var to = (5, 4);
        var whiteKing = new King("White", (7, 4));
        var whitePawn = new Pawn("White", from);
        var blackRook = new Rook("Black", (7, 0));

        _mockStateService.Setup(s => s.CurrentPlayer).Returns("White");

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        var moveService = new MoveService(gameContext);

        // Set up the board
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);
        _mockChessBoard.Setup(board => board.GetPieceAt(whiteKing.Position)).Returns(whiteKing);
        _mockChessBoard.Setup(board => board.FindKing("White")).Returns(whiteKing.Position);

        // Add opponent piece that can attack the king after the move
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { whitePawn, whiteKing, blackRook });

        // Mock IsValidMove to return true for pawn
        var mockPawn = new Mock<Pawn>("White", from) { CallBase = true };
        mockPawn.Setup(p => p.IsValidMove(to, gameContext)).Returns(true);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(mockPawn.Object);

        // Mock the black rook's IsValidMove to return true when targeting the king's position
        var mockRook = new Mock<Rook>("Black", blackRook.Position) { CallBase = true };
        mockRook.Setup(r => r.IsValidMove(whiteKing.Position, gameContext)).Returns(true);
        _mockChessBoard.SetupSequence(board => board.GetAllPieces())
            .Returns(new List<ChessPiece> { mockPawn.Object, whiteKing, mockRook.Object }) // Before move
            .Returns(new List<ChessPiece> { mockPawn.Object, whiteKing, mockRook.Object }); // After move

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryMove_ShouldUpdateGameStateAfterValidMove()
    {
        // Arrange
        var from = (6, 0);
        var to = (5, 0);
        var whitePawn = new Pawn("White", from);

        _mockStateService.Setup(s => s.CurrentPlayer).Returns("White");

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        var moveService = new MoveService(gameContext);

        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);

        // Mock IsValidMove to return true
        var mockPawn = new Mock<Pawn>("White", from) { CallBase = true };
        mockPawn.Setup(p => p.IsValidMove(to, gameContext)).Returns(true);
        mockPawn.Setup(p => p.OnMoved(to, gameContext));
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(mockPawn.Object);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);

        // Verify that OnMoved was called
        mockPawn.Verify(p => p.OnMoved(to, gameContext), Times.Once);

        // Verify that the player was switched
        _mockStateService.Verify(s => s.SwitchPlayer(), Times.Once);
    }

    [Fact]
    public void TryMove_ShouldSetIsCheckWhenOpponentKingIsInCheck()
    {
        // Arrange
        var from = (0, 0);
        var to = (0, 7);
        var blackKing = new King("Black", (7, 4));
        var whiteRook = new Rook("White", from);

        _mockStateService.SetupProperty(s => s.CurrentPlayer, "White");
        _mockStateService.SetupProperty(s => s.IsCheck, false);
        _mockStateService.SetupProperty(s => s.IsCheckmate, false);

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        var moveService = new MoveService(gameContext);

        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whiteRook);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);
        _mockChessBoard.Setup(board => board.GetPieceAt(blackKing.Position)).Returns(blackKing);
        _mockChessBoard.Setup(board => board.FindKing("Black")).Returns(blackKing.Position);

        // Mock IsValidMove to return true
        var mockRook = new Mock<Rook>("White", from) { CallBase = true };
        mockRook.Setup(r => r.IsValidMove(to, gameContext)).Returns(true);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(mockRook.Object);

        // Mock opponent pieces
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { blackKing, mockRook.Object });

        // Mock that after the move, rook can attack the black king
        mockRook.Setup(r => r.IsValidMove(blackKing.Position, gameContext)).Returns(true);

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);
        Assert.True(_mockStateService.Object.IsCheck);
    }

    [Fact]
    public void TryMove_ShouldSetIsCheckmateWhenOpponentHasNoLegalMoves()
    {
        // Arrange
        var from = (0, 0);
        var to = (1, 0);
        var blackKing = new King("Black", (7, 4));

        // Only pieces on the board are white rook and black king
        var whiteRook = new Rook("White", from);

        _mockStateService.SetupProperty(s => s.CurrentPlayer, "White");
        _mockStateService.SetupProperty(s => s.IsCheck, false);
        _mockStateService.SetupProperty(s => s.IsCheckmate, false);

        var gameContext = new GameContextBuilder()
            .WithBoard(_mockChessBoard.Object)
            .WithCurrentPlayer("White")
            .Build();

        var moveService = new MoveService(gameContext);

        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whiteRook);
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);
        _mockChessBoard.Setup(board => board.FindKing("Black")).Returns(blackKing.Position);
        _mockChessBoard.Setup(board => board.GetPieceAt(blackKing.Position)).Returns(blackKing);

        // Mock IsValidMove to return true
        var mockRook = new Mock<Rook>("White", from) { CallBase = true };
        mockRook.Setup(r => r.IsValidMove(to, gameContext)).Returns(true);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(mockRook.Object);

        // Mock opponent pieces
        _mockChessBoard.SetupSequence(board => board.GetAllPieces())
            .Returns(new List<ChessPiece> { mockRook.Object, blackKing }) // Before move
            .Returns(new List<ChessPiece> { mockRook.Object, blackKing }); // After move

        // Mock that black king has no legal moves
        var mockKing = new Mock<King>("Black", blackKing.Position) { CallBase = true };
        mockKing.Setup(k => k.IsValidMove(It.IsAny<(int, int)>(), gameContext)).Returns(false);
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(new List<ChessPiece> { mockRook.Object, mockKing.Object });

        // Act
        var result = moveService.TryMove(from, to);

        // Assert
        Assert.True(result);
        Assert.True(_mockStateService.Object.IsCheckmate);
    }
}
