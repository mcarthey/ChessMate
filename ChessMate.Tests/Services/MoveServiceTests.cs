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
    private readonly MoveService _moveService;

    public MoveServiceTests(ITestOutputHelper output) : base(output)
    {
        _mockChessBoard = new Mock<IChessBoard>();
        _mockStateService = new Mock<IStateService>();
        _moveService = new MoveService(_mockChessBoard.Object, _mockStateService.Object);
    }

    [Fact]
    public void MoveService_TryMove_ShouldReturnFalseIfNoPieceAtFromPosition()
    {
        // Arrange
        var from = (6, 0);
        var to = (5, 0);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns((ChessPiece)null);

        // Act
        var result = _moveService.TryMove(from, to);

        // Assert
        Assert.False(result);

        // Debugging output
        CustomOutput.WriteLine("Test: MoveService_TryMove_ShouldReturnFalseIfNoPieceAtFromPosition");
        CustomOutput.WriteLine($"From: {from}, To: {to}, Result: {result}");
        CustomOutput.Flush();
    }

    [Fact]
    public void MoveService_TryMove_ShouldReturnFalseIfPieceColorDoesNotMatchCurrentPlayer()
    {
        // Arrange
        var from = (6, 0);
        var to = (5, 0);
        var whitePawn = new Pawn("White", from);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockStateService.Setup(state => state.CurrentPlayer).Returns("Black");

        // Act
        var result = _moveService.TryMove(from, to);

        // Assert
        Assert.False(result);

        // Debugging output
        CustomOutput.WriteLine("Test: MoveService_TryMove_ShouldReturnFalseIfPieceColorDoesNotMatchCurrentPlayer");
        CustomOutput.WriteLine($"From: {from}, To: {to}, Result: {result}");
        CustomOutput.Flush();
    }

    [Fact]
    public void MoveService_TryMove_ShouldReturnFalseIfMoveIsInvalid()
    {
        // Arrange
        var from = (6, 0);
        var to = (5, 1);
        var whitePawn = new Pawn("White", from);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockStateService.Setup(state => state.CurrentPlayer).Returns("White");
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);

        // Act
        var result = _moveService.TryMove(from, to);

        // Assert
        Assert.False(result);

        // Debugging output
        CustomOutput.WriteLine("Test: MoveService_TryMove_ShouldReturnFalseIfMoveIsInvalid");
        CustomOutput.WriteLine($"From: {from}, To: {to}, Result: {result}");
        CustomOutput.Flush();
    }

    [Fact]
    public void MoveService_TryMove_ShouldExecuteMoveIfValid()
    {
        // Arrange
        var from = (6, 0);
        var to = (5, 0);
        var whitePawn = new Pawn("White", from);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockStateService.Setup(state => state.CurrentPlayer).Returns("White");
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);

        // Act
        var result = _moveService.TryMove(from, to);

        // Assert
        Assert.True(result);
        _mockChessBoard.Verify(board => board.SetPieceAt(to, whitePawn), Times.Once);
        _mockChessBoard.Verify(board => board.RemovePieceAt(from), Times.Once);
        Assert.Equal(to, whitePawn.Position);

        // Debugging output
        CustomOutput.WriteLine("Test: MoveService_TryMove_ShouldExecuteMoveIfValid");
        CustomOutput.WriteLine($"From: {from}, To: {to}, Result: {result}");
        CustomOutput.Flush();
    }

    [Fact]
    public void MoveService_FinalizeMove_ShouldSwitchPlayers()
    {
        // Arrange
        var from = (6, 0);
        var to = (5, 0);
        var whitePawn = new Pawn("White", from);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockStateService.Setup(state => state.CurrentPlayer).Returns("White");
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);

        // Act
        _moveService.TryMove(from, to);

        // Assert
        _mockStateService.Verify(state => state.SwitchPlayer(), Times.Once);

        // Debugging output
        CustomOutput.WriteLine("Test: MoveService_FinalizeMove_ShouldSwitchPlayers");
        CustomOutput.Flush();
    }

    [Fact]
    public void MoveService_FinalizeMove_ShouldCheckForCheckAndCheckmate()
    {
        // Arrange
        var from = (6, 0);
        var to = (5, 0);
        var whitePawn = new Pawn("White", from);
        _mockChessBoard.Setup(board => board.GetPieceAt(from)).Returns(whitePawn);
        _mockStateService.Setup(state => state.CurrentPlayer).Returns("White");
        _mockChessBoard.Setup(board => board.GetPieceAt(to)).Returns((ChessPiece)null);

        // Act
        _moveService.TryMove(from, to);

        // Assert
        _mockStateService.Verify(state => state.IsKingInCheck("Black", _mockChessBoard.Object), Times.Once);
        _mockStateService.Verify(state => state.HasLegalMoves("Black", _mockChessBoard.Object), Times.Once);

        // Debugging output
        CustomOutput.WriteLine("Test: MoveService_FinalizeMove_ShouldCheckForCheckAndCheckmate");
        CustomOutput.Flush();
    }
}









