using ChessMate.Models;
using ChessMate.Services;
using Moq;

namespace ChessMate.Tests;

public class GameContextBuilder
{
    private readonly Mock<IGameContext> _gameContextMock = new();
    private readonly Mock<IStateService> _stateServiceMock = new();
    private IChessBoard _board;
    private string _currentPlayer;
    private List<string> _moveLog;

    public GameContextBuilder()
    {
        // Default setups
        _stateServiceMock.SetupAllProperties();

        // Initialize the current player
        _currentPlayer = "White";
        _stateServiceMock.Setup(s => s.CurrentPlayer).Returns(() => _currentPlayer);
        _stateServiceMock.Setup(s => s.SwitchPlayer())
            .Callback(() => _currentPlayer = _currentPlayer == "White" ? "Black" : "White");
        _stateServiceMock.Setup(s => s.SetPlayer(It.IsAny<string>()))
            .Callback<string>(player => _currentPlayer = player);

        // Initialize MoveLog
        _moveLog = new List<string>();
        _stateServiceMock.Setup(s => s.MoveLog).Returns(() => _moveLog);

        _board = new ChessBoard();
    }

    public GameContextBuilder WithBoard(IChessBoard board)
    {
        _board = board;
        return this;
    }

    // Invokes the mocked SetPlayer method.
    // Triggers the callback set up in the constructor.
    // Updates _currentPlayer to the given player value.
    // Ensures that CurrentPlayer reflects the updated value due to the setup in the constructor.
    public GameContextBuilder WithCurrentPlayer(string player)
    {
        _stateServiceMock.Object.SetPlayer(player);
        return this;
    }

    public GameContextBuilder WithEnPassantTarget(Position target, ChessPiece piece)
    {
        _stateServiceMock.Setup(s => s.EnPassantTarget).Returns(target);
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
        _moveLog = moveLog;
        _stateServiceMock.Setup(s => s.MoveLog).Returns(() => _moveLog);
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





