using ChessMate.Models;
using ChessMate.Utilities;

namespace ChessMate.Services;

public class ChessGame
{
    public IChessBoard Board { get; set; }
    public List<string> MoveLog { get; private set; } = new List<string>();
    public string InvalidMoveReason => Board.InvalidMoveReason;

    // Constructor accepting an IChessBoard, to be provided via DI
    public ChessGame(IChessBoard board)
    {
        Board = board;
    }

    public bool MovePiece(string fromNotation, string toNotation)
    {
        if (!ChessNotationUtility.IsValidChessNotation(fromNotation) ||
            !ChessNotationUtility.IsValidChessNotation(toNotation))
        {
            Board.InvalidMoveReason = "Invalid chess notation provided.";
            Console.WriteLine(Board.InvalidMoveReason);
            return false;
        }

        var from = ChessNotationUtility.FromChessNotation(fromNotation);
        var to = ChessNotationUtility.FromChessNotation(toNotation);

        bool success = Board.MovePiece(from, to);

        if (success)
        {
            // Directly use ChessNotationUtility for logging moves
            MoveLog.Add($"{fromNotation}->{toNotation}");
            Console.WriteLine($"Moved piece from {fromNotation} to {toNotation}");
        }
        else
        {
            Console.WriteLine($"Failed to move piece from {fromNotation} to {toNotation}: {Board.InvalidMoveReason}");
        }

        return success;
    }

    public async Task<string> GetGameAnalysisAsync()
    {
        // Simulate a delay to mimic an API call
        await Task.Delay(500);

        // Mocked response for now
        var mockedResponses = new Dictionary<int, string>
        {
            { 0, "The game is in its early stages. Try to control the center." },
            { 5, "The position is balanced. Focus on developing your pieces." },
            { 10, "White has a slight advantage. Look for opportunities to attack." },
            { 20, "Black is in trouble. Avoid losing key pawns." },
            { 30, "Endgame approaches. Activate your king." }
        };

        // Provide feedback based on the number of moves
        var moveCount = MoveLog.Count;
        var closestMatch = mockedResponses.Keys.OrderBy(k => Math.Abs(k - moveCount)).First();
        return mockedResponses[closestMatch];
    }
}

