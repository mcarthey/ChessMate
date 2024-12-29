using ChessMate.Models;

namespace ChessMate.Services
{
    public interface IMoveService
    {
        bool TryMove(Position from, Position to);
    }
}
