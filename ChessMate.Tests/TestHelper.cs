using System.Text;
using ChessMate.Services;
using Xunit.Abstractions;

namespace ChessMate.Tests;

public abstract class TestHelper
{
    protected CustomTestOutputHelper CustomOutput { get; set; }

    protected TestHelper(ITestOutputHelper output)
    {
        CustomOutput = new CustomTestOutputHelper(output);
    }

    protected void PrintBoard(ChessGame chessGame)
    {
        CustomOutput.WriteLine("  A  B  C  D  E  F  G  H");
        CustomOutput.WriteLine(" +-----------------------+");
        for (int row = 0; row < 8; row++)
        {
            var rowBuilder = new StringBuilder();
            rowBuilder.Append(8 - row).Append("|");
            for (int col = 0; col < 8; col++)
            {
                var piece = chessGame.Board.ChessPieces[row, col];
                rowBuilder.Append((piece?.Representation ?? ".").PadRight(3));
            }
            rowBuilder.Append("|");
            CustomOutput.WriteLine(rowBuilder.ToString());
        }
        CustomOutput.WriteLine(" +-----------------------+");
    }
}
