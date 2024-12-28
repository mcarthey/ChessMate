// File: ChessMate.Tests/GameContextBuilder.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;

namespace ChessMate.Tests;

public class GameContextBuilder
{
    private readonly Mock<IGameContext> _gameContextMock = new();
    private readonly Mock<IStateService> _stateServiceMock = new();
    private IChessBoard _board;
    
    public GameContextBuilder()
    {
        // Default setups
        _stateServiceMock.SetupAllProperties();
        _stateServiceMock.Object.SetPlayer("White");

        _board = new ChessBoard();
    }

    public GameContextBuilder WithBoard(IChessBoard board)
    {
        _board = board;
        return this;
    }

    public GameContextBuilder WithCurrentPlayer(string player)
    {
        _stateServiceMock.Object.SetPlayer(player);
        return this;
    }

    public GameContextBuilder WithEnPassantTarget((int Row, int Col) target, ChessPiece piece)
    {
        // Since EnPassantTarget does not have a public setter, we simulate setting it through the method
        _stateServiceMock.Setup(s => s.EnPassantTarget).Returns(target);
        _stateServiceMock.Object.SetEnPassantTarget(target, piece);
        return this;
    }

    // Additional methods to configure the mock state can be added here
    // For example:
    public GameContextBuilder WithCheck(bool isCheck)
    {
        _stateServiceMock.Object.IsCheck = isCheck;
        return this;
    }

    public GameContextBuilder WithCheckmate(bool isCheckmate)
    {
        _stateServiceMock.Object.IsCheckmate = isCheckmate;
        return this;
    }

    // Method to verify interactions with IStateService mock
    public void VerifyStateService(Action<Mock<IStateService>> verifyAction)
    {
        verifyAction(_stateServiceMock);
    }

    public IGameContext Build()
    {
        _gameContextMock.Setup(c => c.Board).Returns(_board);
        _gameContextMock.Setup(c => c.State).Returns(_stateServiceMock.Object);

        return _gameContextMock.Object;
    }
}
