using ChessMate.Models;

namespace ChessMate.Services;

public class GameEngine : IGameEngine
{
    public IGameContext Context { get; set; }
    public IMoveService Move { get; set; }

    public IChessBoard Board => Context.Board;
    public IStateService State => Context.State;

    public GameEngine(IGameContext context, IMoveService move)
    {
        Context = context;
        Move = move;

        Board.InitializeBoard();
        State.UpdateAttackMaps(Context);
    }

    public bool ProcessMove(Position from, Position to)
    {
        return Move.TryMove(from, to);
    }

    public string GetCurrentPlayer() => State.CurrentPlayer;
}
