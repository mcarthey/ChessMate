using ChessMate.Models;

namespace ChessMate.Services
{
    public interface IGameEngine
    {
        IMoveService Move { get; }

        bool ProcessMove(Position from, Position to);
    }
}
