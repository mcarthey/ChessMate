using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services;

public class StateServiceTests : TestHelper
{
    private readonly StateService _stateService;
    private readonly Mock<IChessBoard> _mockChessBoard;

    public StateServiceTests(ITestOutputHelper output) : base(output)
    {
        _stateService = new StateService();
        _mockChessBoard = new Mock<IChessBoard>();
    }

    [Fact]
    public void StateService_InitialState_ShouldSetCurrentPlayerToWhite()
    {
        // Arrange & Act
        var currentPlayer = _stateService.CurrentPlayer;

        // Assert
        Assert.Equal("White", currentPlayer);

        // Debugging output
        CustomOutput.WriteLine("Test: StateService_InitialState_ShouldSetCurrentPlayerToWhite");
        CustomOutput.WriteLine($"Current Player: {currentPlayer}");
        CustomOutput.Flush();
    }

    [Fact]
    public void StateService_SwitchPlayer_ShouldToggleCurrentPlayer()
    {
        // Arrange
        _stateService.SwitchPlayer();

        // Act
        var currentPlayer = _stateService.CurrentPlayer;

        // Assert
        Assert.Equal("Black", currentPlayer);

        // Debugging output
        CustomOutput.WriteLine("Test: StateService_SwitchPlayer_ShouldToggleCurrentPlayer");
        CustomOutput.WriteLine($"Current Player: {currentPlayer}");
        CustomOutput.Flush();
    }

    [Fact]
    public void StateService_IsKingInCheck_ShouldReturnTrueIfKingIsInCheck()
    {
        // Arrange
        var whiteKing = new King("White", (7, 4));
        var blackRook = new Rook("Black", (0, 4));
        var blackKing = new King("Black", (0, 0));
        var chessBoard = InitializeCustomBoard(
            (whiteKing, (7, 4)),
            (blackRook, (0, 4)),
            (blackKing, (0, 0))
        );
        _mockChessBoard.Setup(board => board.GetPieceAt(It.IsAny<(int, int)>())).Returns<(int Row, int Col)>(pos => chessBoard.GetPieceAt(pos));
        _mockChessBoard.Setup(board => board.FindKing("White")).Returns((7, 4));
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(chessBoard.GetAllPieces());

        // Act
        var isInCheck = _stateService.IsKingInCheck("White", _mockChessBoard.Object);

        // Assert
        Assert.True(isInCheck);

        // Debugging output
        CustomOutput.WriteLine("Test: StateService_IsKingInCheck_ShouldReturnTrueIfKingIsInCheck");
        PrintBoard(_mockChessBoard.Object);
        CustomOutput.Flush();
    }

    [Fact]
    public void StateService_IsKingInCheck_ShouldReturnFalseIfKingIsNotInCheck()
    {
        // Arrange
        var whiteKing = new King("White", (7, 4));
        var blackRook = new Rook("Black", (0, 0));
        var blackKing = new King("Black", (0, 4));
        var chessBoard = InitializeCustomBoard(
            (whiteKing, (7, 4)),
            (blackRook, (0, 0)),
            (blackKing, (0, 4))
        );
        _mockChessBoard.Setup(board => board.GetPieceAt(It.IsAny<(int, int)>())).Returns<(int Row, int Col)>(pos => chessBoard.GetPieceAt(pos));
        _mockChessBoard.Setup(board => board.FindKing("White")).Returns((7, 4));
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(chessBoard.GetAllPieces());

        // Act
        var isInCheck = _stateService.IsKingInCheck("White", _mockChessBoard.Object);

        // Assert
        Assert.False(isInCheck);

        // Debugging output
        CustomOutput.WriteLine("Test: StateService_IsKingInCheck_ShouldReturnFalseIfKingIsNotInCheck");
        PrintBoard(_mockChessBoard.Object);
        CustomOutput.Flush();
    }

    [Fact]
    public void StateService_HasLegalMoves_ShouldReturnTrueIfLegalMovesExist()
    {
        // Arrange
        var whiteKing = new King("White", (7, 4));
        var blackRook = new Rook("Black", (0, 0));
        var blackKing = new King("Black", (0, 4));
        var whitePawn = new Pawn("White", (6, 4));
        var chessBoard = InitializeCustomBoard(
            (whiteKing, (7, 4)),
            (blackRook, (0, 0)),
            (blackKing, (0, 4)),
            (whitePawn, (6, 4))
        );
        _mockChessBoard.Setup(board => board.GetPieceAt(It.IsAny<(int, int)>())).Returns<(int Row, int Col)>(pos => chessBoard.GetPieceAt(pos));
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(chessBoard.GetAllPieces());

        // Act
        var hasLegalMoves = _stateService.HasLegalMoves("White", _mockChessBoard.Object);

        // Assert
        Assert.True(hasLegalMoves);

        // Debugging output
        CustomOutput.WriteLine("Test: StateService_HasLegalMoves_ShouldReturnTrueIfLegalMovesExist");
        PrintBoard(_mockChessBoard.Object);
        CustomOutput.Flush();
    }

    [Fact]
    public void StateService_HasLegalMoves_ShouldReturnFalseIfNoLegalMovesExist()
    {
        // Arrange
        var whiteKing = new King("White", (7, 4));
        var blackRook = new Rook("Black", (0, 4));
        var blackKing = new King("Black", (0, 0));
        var chessBoard = InitializeCustomBoard(
            (whiteKing, (7, 4)),
            (blackRook, (0, 4)),
            (blackKing, (0, 0))
        );
        _mockChessBoard.Setup(board => board.GetPieceAt(It.IsAny<(int, int)>())).Returns<(int Row, int Col)>(pos => chessBoard.GetPieceAt(pos));
        _mockChessBoard.Setup(board => board.GetAllPieces()).Returns(chessBoard.GetAllPieces());

        // Act
        var hasLegalMoves = _stateService.HasLegalMoves("White", _mockChessBoard.Object);

        // Assert
        Assert.False(hasLegalMoves);

        // Debugging output
        CustomOutput.WriteLine("Test: StateService_HasLegalMoves_ShouldReturnFalseIfNoLegalMovesExist");
        PrintBoard(_mockChessBoard.Object);
        CustomOutput.Flush();
    }
}









