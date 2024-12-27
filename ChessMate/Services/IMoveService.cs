using ChessMate.Models;

namespace ChessMate.Services
{
    public interface IMoveService
    {
        bool TryMove((int Row, int Col) from, (int Row, int Col) to);
        }
}
