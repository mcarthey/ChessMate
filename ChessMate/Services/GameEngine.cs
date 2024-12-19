using ChessMate.Models;
using ChessMate.Utilities;

namespace ChessMate.Services;

public class GameEngine : IGameEngine
{
    public IChessBoard Board { get; set; }
    public IStateService State { get; set; }
    public IMoveService Move { get; set; }

    public GameEngine(IChessBoard board, IStateService state, IMoveService move)
    {
        Board = board;
        State = state;
        Move = move;

        Board.InitializeBoard();
    }

    public bool ProcessMove(string fromNotation, string toNotation)
    {
        var from = ChessNotationUtility.FromChessNotation(fromNotation);
        var to = ChessNotationUtility.FromChessNotation(toNotation);

        return Move.TryMove(from, to);
    }

    public string GetCurrentPlayer() => State.CurrentPlayer;
}
