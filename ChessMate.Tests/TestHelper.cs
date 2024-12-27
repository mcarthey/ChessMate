using System.Text;
using ChessMate.Models;
using ChessMate.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit.Abstractions;

namespace ChessMate.Tests;

public abstract class TestHelper : TestFixture
{
    protected readonly IStateService StateService;
    public IMoveService MoveService { get; set; }
    public IChessBoard ChessBoard { get; set; }
    protected CustomTestOutputHelper CustomOutput { get; set; }

    protected TestHelper(ITestOutputHelper output) : base()
    {
        CustomOutput = new CustomTestOutputHelper(output);
        StateService = ServiceProvider.GetRequiredService<IStateService>();
        MoveService = ServiceProvider.GetRequiredService<IMoveService>();
        ChessBoard = ServiceProvider.GetRequiredService<IChessBoard>();
    }

    protected void PrintBoard(IChessBoard board)
    {
        var whiteSpace = " ";
        var columnHeaders = new StringBuilder($"{whiteSpace,2}");
        for (char col = 'A'; col <= 'H'; col++)
        {
            columnHeaders.Append($"{col,5}");
        }

        string separator = "  +--------------------------------------+";

        CustomOutput.WriteLine(columnHeaders.ToString());
        CustomOutput.WriteLine(separator);
        for (int row = 0; row < 8; row++)
        {
            var rowBuilder = new StringBuilder();
            rowBuilder.Append($"{8 - row} |");
            for (int col = 0; col < 8; col++)
            {
                var piece = board.GetPieceAt((row, col));
                if (piece?.Representation is null)
                {
                    rowBuilder.Append($"{" . ",5}");
                }
                else
                {
                    rowBuilder.Append($"{piece?.Representation,4}");
                }
            }
            rowBuilder.Append($"| {8 - row}");
            CustomOutput.WriteLine(rowBuilder.ToString());
        }
        CustomOutput.WriteLine(separator);
        CustomOutput.WriteLine(columnHeaders.ToString());
        CustomOutput.Flush(); // Ensure output is flushed
    }

    protected ChessBoard InitializeCustomBoard(params (ChessPiece piece, (int Row, int Col) position)[] pieces)
    {
        var chessBoard = new ChessBoard();
        chessBoard.SetCustomBoard(pieces);
        return chessBoard;
    }

    // Helper method to get a mocked IGameContext
    protected IGameContext GetMockedGameContext(IChessBoard board, string currentPlayer)
    {
        var mockStateService = new Mock<IStateService>();
        mockStateService.Setup(s => s.CurrentPlayer).Returns(currentPlayer);

        var mockGameContext = new Mock<IGameContext>();
        mockGameContext.Setup(c => c.Board).Returns(board);
        mockGameContext.Setup(c => c.State).Returns(mockStateService.Object);

        return mockGameContext.Object;
    }
}
