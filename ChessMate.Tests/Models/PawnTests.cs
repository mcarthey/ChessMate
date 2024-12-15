using ChessMate.Models;
using ChessMate.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Models;

public class PawnTests : TestHelper
{
    public PawnTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldAllowSingleSquareMove()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 0));
        var chessBoard = InitializeCustomBoard((pawn, (6, 0)));
        var targetPosition = (5, 0); // Move to (5, 0)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Pawn_IsValidMove_ShouldAllowSingleSquareMove");
        CustomOutput.WriteLine($"Pawn Position: {ChessNotationUtility.ToChessNotation(pawn.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The pawn should be able to move one square forward.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldAllowDoubleSquareMoveOnFirstMove()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 0));
        var chessBoard = InitializeCustomBoard((pawn, (6, 0)));
        var targetPosition = (4, 0); // Move to (4, 0)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Pawn_IsValidMove_ShouldAllowDoubleSquareMoveOnFirstMove");
        CustomOutput.WriteLine($"Pawn Position: {ChessNotationUtility.ToChessNotation(pawn.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The pawn should be able to move two squares forward on its first move.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectDoubleSquareMoveAfterFirstMove()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 0));
        var chessBoard = InitializeCustomBoard((pawn, (6, 0)));
        var firstMovePosition = (5, 0); // Move to (5, 0)
        var secondMovePosition = (3, 0); // Attempt to move to (3, 0)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool firstMoveValid = pawn.IsValidMove(firstMovePosition, chessBoard);
        if (firstMoveValid)
        {
            chessBoard.MovePiece(pawn.Position, firstMovePosition);
            pawn = chessBoard.ChessPieces[5, 0] as Pawn;
        }
        bool secondMoveValid = pawn.IsValidMove(secondMovePosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Pawn_IsValidMove_ShouldRejectDoubleSquareMoveAfterFirstMove");
        CustomOutput.WriteLine($"First Move Position: {ChessNotationUtility.ToChessNotation(firstMovePosition)}");
        CustomOutput.WriteLine($"First Move Valid: {firstMoveValid}");
        CustomOutput.WriteLine($"Second Move Position: {ChessNotationUtility.ToChessNotation(secondMovePosition)}");
        CustomOutput.WriteLine($"Second Move Valid: {secondMoveValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(firstMoveValid, "The pawn should be able to move one square forward.");
        Assert.False(secondMoveValid, "The pawn should not be able to move two squares forward after the first move.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldAllowDiagonalCapture()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 0));
        var blackPawn = new Pawn("Black", (5, 1));
        var chessBoard = InitializeCustomBoard((pawn, (6, 0)), (blackPawn, (5, 1)));
        var targetPosition = (5, 1); // Move to (5, 1)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Pawn_IsValidMove_ShouldAllowDiagonalCapture");
        CustomOutput.WriteLine($"Pawn Position: {ChessNotationUtility.ToChessNotation(pawn.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The pawn should be able to capture diagonally.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectInvalidDiagonalMove()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 0));
        var chessBoard = InitializeCustomBoard((pawn, (6, 0)));
        var targetPosition = (5, 1); // Move to (5, 1)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Pawn_IsValidMove_ShouldRejectInvalidDiagonalMove");
        CustomOutput.WriteLine($"Pawn Position: {ChessNotationUtility.ToChessNotation(pawn.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The pawn should not be able to move diagonally without capturing.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectBackwardMove()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 0));
        var chessBoard = InitializeCustomBoard((pawn, (6, 0)));
        var targetPosition = (7, 0); // Move to (7, 0)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Pawn_IsValidMove_ShouldRejectBackwardMove");
        CustomOutput.WriteLine($"Pawn Position: {ChessNotationUtility.ToChessNotation(pawn.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The pawn should not be able to move backward.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectMoveToOccupiedSquare()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 0));
        var whitePawn = new Pawn("White", (5, 0));
        var chessBoard = InitializeCustomBoard((pawn, (6, 0)), (whitePawn, (5, 0)));
        var targetPosition = (5, 0); // Move to (5, 0)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Pawn_IsValidMove_ShouldRejectMoveToOccupiedSquare");
        CustomOutput.WriteLine($"Pawn Position: {ChessNotationUtility.ToChessNotation(pawn.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The pawn should not be able to move to an occupied square.");
    }

    [Fact]
    public void Pawn_IsValidMove_ShouldRejectMoveOutOfBounds()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 0));
        var chessBoard = InitializeCustomBoard((pawn, (6, 0)));
        var targetPosition = (8, 0); // Move to (8, 0) - out of bounds

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = pawn.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Pawn_IsValidMove_ShouldRejectMoveOutOfBounds");
        CustomOutput.WriteLine($"Pawn Position: {ChessNotationUtility.ToChessNotation(pawn.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.False(isValid, "The pawn should not be able to move out of bounds.");
    }
}





