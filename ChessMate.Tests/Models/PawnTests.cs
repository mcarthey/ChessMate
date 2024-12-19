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

    [Fact]
    public void Pawn_IsValidMove_ShouldAllowEnPassantCapture()
    {
        // Arrange
        var whitePawn = new Pawn("White", (4, 4));
        var blackPawn = new Pawn("Black", (6, 5));
        var chessBoard = InitializeCustomBoard((whitePawn, (4, 4)), (blackPawn, (6, 5)));

        // Simulate black pawn moving two squares forward
        var blackPawnTarget = (4, 5);
        blackPawn.OnMoveEffect(blackPawnTarget);
        chessBoard.SetPieceAt(blackPawnTarget, blackPawn);
        chessBoard.RemovePieceAt((6, 5));

        var targetPosition = (5, 5); // En passant capture

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool isValid = whitePawn.IsValidMove(targetPosition, chessBoard);

        // Debugging output
        CustomOutput.WriteLine("Test: Pawn_IsValidMove_ShouldAllowEnPassantCapture");
        CustomOutput.WriteLine($"White Pawn Position: {ChessNotationUtility.ToChessNotation(whitePawn.Position)}");
        CustomOutput.WriteLine($"Black Pawn Position: {ChessNotationUtility.ToChessNotation(blackPawn.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Is Valid Move: {isValid}");
        CustomOutput.Flush();

        // Assert
        Assert.True(isValid, "The pawn should be able to capture en passant.");
    }

    [Fact]
    public void Pawn_OnMoveEffect_ShouldSetEnPassantTarget()
    {
        // Arrange
        var pawn = new Pawn("White", (6, 0));
        var chessBoard = InitializeCustomBoard((pawn, (6, 0)));
        var targetPosition = (4, 0); // Move to (4, 0)

        // Act
        pawn.OnMoveEffect(targetPosition);

        // Assert
        Assert.True(pawn.HasMovedTwoSquares, "The pawn should have moved two squares.");
        Assert.Equal((5, 0), pawn.EnPassantTarget);

        // Debugging output
        CustomOutput.WriteLine("Test: Pawn_OnMoveEffect_ShouldSetEnPassantTarget");
        CustomOutput.WriteLine($"Pawn Position: {ChessNotationUtility.ToChessNotation(pawn.Position)}");
        CustomOutput.WriteLine($"Target Position: {ChessNotationUtility.ToChessNotation(targetPosition)}");
        CustomOutput.WriteLine($"Has Moved Two Squares: {pawn.HasMovedTwoSquares}");
        CustomOutput.WriteLine($"En Passant Target: {ChessNotationUtility.ToChessNotation(pawn.EnPassantTarget.Value)}");
        CustomOutput.Flush();
    }
}









