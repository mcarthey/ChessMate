using ChessMate.Models;

namespace ChessMate.Services;

public class GameEngine : IGameEngine
{
    public IMoveService Move { get; set; }


    public GameEngine(IMoveService move)
    {
        Move = move;
    }

    public bool ProcessMove(Position from, Position to)
    {
        return Move.TryMove(from, to);
    }
}
