using System.Text;
using ChessMate.Models;
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
                var piece = board.ChessPieces[row, col];
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
    }

    protected ChessBoard InitializeCustomBoard(params (ChessPiece piece, (int Row, int Col) position)[] pieces)
    {
        var chessBoard = new ChessBoard();
        chessBoard.ChessPieces = new ChessPiece[8, 8]; // Clear the board

        foreach (var (piece, position) in pieces)
        {
            chessBoard.ChessPieces[position.Row, position.Col] = piece;
            piece.Position = position;
        }

        return chessBoard;
    }
}
