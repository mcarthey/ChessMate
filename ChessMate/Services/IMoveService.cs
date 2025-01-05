// File: ChessMate/Services/IMoveService.cs

using ChessMate.Models;

namespace ChessMate.Services;

/// <summary>
/// Interface for move services.
/// </summary>
public interface IMoveService
{
    /// <summary>
    /// Attempts to move a piece from one position to another.
    /// </summary>
    /// <param name="from">The starting position.</param>
    /// <param name="to">The target position.</param>
    /// <returns>True if the move was successful; otherwise, false.</returns>
    bool TryMove(Position from, Position to);
}
