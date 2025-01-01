// File: ChessMate.Tests/GameContextBuilder.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;

namespace ChessMate.Tests;

public class GameContextBuilder
{
    private readonly Mock<IGameContext> _gameContextMock = new();
    private readonly Mock<StateService> _stateServiceMock;
    private IChessBoard _board;
    private string _currentPlayer;
    public IChessBoard Board => _board;

    // Expose the StateService mock for verification in tests
    public Mock<StateService> StateServiceMock => _stateServiceMock;

    public GameContextBuilder()
    {
        // Initialize the state service partial mock with CallBase = true
        _stateServiceMock = new Mock<StateService>() { CallBase = true };

        // Initialize the current player
        _currentPlayer = "White";
        _stateServiceMock.Object.SetPlayer(_currentPlayer);

        // Initialize the board
        _board = new ChessBoard();
    }

    public GameContextBuilder WithBoard(IChessBoard board)
    {
        _board = board;
        return this;
    }

    public GameContextBuilder WithCurrentPlayer(string player)
    {
        _currentPlayer = player;
        _stateServiceMock.Object.SetPlayer(player);
        return this;
    }

    public GameContextBuilder WithEnPassantTarget(Position target, ChessPiece piece)
    {
        _stateServiceMock.Object.SetEnPassantTarget(target, piece);
        return this;
    }

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

    // Optional: Method to set custom MoveLog for testing purposes
    public GameContextBuilder WithMoveLog(List<string> moveLog)
    {
        _stateServiceMock.Object.MoveLog.Clear();
        _stateServiceMock.Object.MoveLog.AddRange(moveLog);
        return this;
    }

    // Method to configure the StateService mock for custom setups or verifications
    public GameContextBuilder ConfigureStateService(Action<Mock<StateService>> configure)
    {
        configure(_stateServiceMock);
        return this;
    }

    public IGameContext Build()
    {
        _gameContextMock.Setup(c => c.Board).Returns(_board);
        _gameContextMock.Setup(c => c.State).Returns(_stateServiceMock.Object);
        return _gameContextMock.Object;
    }

}
