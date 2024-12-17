using ChessMate.Models;
using ChessMate.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Models;

public class ChessBoardTests : TestHelper
{
    public ChessBoardTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ChessBoard_InitializeBoard_ShouldSetUpPiecesCorrectly()
    {
        // Arrange
        var chessBoard = new ChessBoard();

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        var whitePawn = chessBoard.ChessPieces[6, 0];
        var blackPawn = chessBoard.ChessPieces[1, 0];
        var whiteRook = chessBoard.ChessPieces[7, 0];
        var blackKing = chessBoard.ChessPieces[0, 4];

        // Debugging output
        CustomOutput.WriteLine("Test: ChessBoard_InitializeBoard_ShouldSetUpPiecesCorrectly");
        CustomOutput.WriteLine($"White Pawn Position: {ChessNotationUtility.ToChessNotation((6, 0))}");
        CustomOutput.WriteLine($"Black Pawn Position: {ChessNotationUtility.ToChessNotation((1, 0))}");
        CustomOutput.WriteLine($"White Rook Position: {ChessNotationUtility.ToChessNotation((7, 0))}");
        CustomOutput.WriteLine($"Black King Position: {ChessNotationUtility.ToChessNotation((0, 4))}");
        CustomOutput.Flush();

        // Assert
        Assert.NotNull(whitePawn);
        Assert.NotNull(blackPawn);
        Assert.IsType<Pawn>(whitePawn);
        Assert.IsType<Pawn>(blackPawn);
        Assert.IsType<Rook>(whiteRook);
        Assert.IsType<King>(blackKing);
    }

    [Fact]
    public void ChessBoard_MovePiece_ShouldMovePieceCorrectly()
    {
        // Arrange
        var chessBoard = new ChessBoard();
        var from = (6, 0); // a2
        var to = (5, 0); // a3

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Debugging output
        CustomOutput.WriteLine("Test: ChessBoard_MovePiece_ShouldMovePieceCorrectly");
        CustomOutput.WriteLine($"From Position: {ChessNotationUtility.ToChessNotation(from)}");
        CustomOutput.WriteLine($"To Position: {ChessNotationUtility.ToChessNotation(to)}");
        CustomOutput.WriteLine($"Move Success: {moveSuccess}");
        CustomOutput.Flush();

        // Assert
        Assert.True(moveSuccess);
        Assert.Null(chessBoard.ChessPieces[6, 0]);
        Assert.IsType<Pawn>(chessBoard.ChessPieces[5, 0]);
    }

    [Fact]
    public void ChessBoard_MovePiece_ShouldRejectInvalidMove()
    {
        // Arrange
        var chessBoard = new ChessBoard();
        var from = (6, 0); // a2
        var to = (5, 1); // b3 (invalid move for a pawn)

        // Debugging output
        PrintBoard(chessBoard);

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Debugging output
        CustomOutput.WriteLine("Test: ChessBoard_MovePiece_ShouldRejectInvalidMove");
        CustomOutput.WriteLine($"From Position: {ChessNotationUtility.ToChessNotation(from)}");
        CustomOutput.WriteLine($"To Position: {ChessNotationUtility.ToChessNotation(to)}");
        CustomOutput.WriteLine($"Move Success: {moveSuccess}");
        CustomOutput.Flush();

        // Assert
        Assert.False(moveSuccess);
        Assert.IsType<Pawn>(chessBoard.ChessPieces[6, 0]);
        Assert.Null(chessBoard.ChessPieces[5, 1]);
    }

    [Fact]
    public void ChessBoard_MovePiece_ShouldAllowValidPawnMove()
    {
        // Arrange
        var whitePawn = new Pawn("White", (6, 0));
        var whiteKing = new King("White", (7, 4)); // Added White King
        var blackKing = new King("Black", (0, 4)); // Added Black King
        var chessBoard = InitializeCustomBoard(
            (whitePawn, (6, 0)),
            (whiteKing, (7, 4)),
            (blackKing, (0, 4))
        );
        var from = (6, 0);
        var to = (5, 0);

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Assert
        Assert.True(moveSuccess, "Pawn should be able to move forward one square.");
        Assert.Null(chessBoard.ChessPieces[6, 0]);
        Assert.IsType<Pawn>(chessBoard.ChessPieces[5, 0]);

        // Debug output
        CustomOutput.WriteLine("Test: ChessBoard_MovePiece_ShouldAllowValidPawnMove");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_MovePiece_ShouldPreventInvalidPawnMove()
    {
        // Arrange
        var whitePawn = new Pawn("White", (6, 0));
        var whiteKing = new King("White", (7, 4)); // Added White King
        var blackKing = new King("Black", (0, 4)); // Added Black King
        var chessBoard = InitializeCustomBoard(
            (whitePawn, (6, 0)),
            (whiteKing, (7, 4)),
            (blackKing, (0, 4))
        );
        var from = (6, 0);
        var to = (4, 1); // Invalid move for pawn

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Assert
        Assert.False(moveSuccess, "Pawn should not be able to move to an invalid square.");
        Assert.IsType<Pawn>(chessBoard.ChessPieces[6, 0]);
        Assert.Null(chessBoard.ChessPieces[4, 1]);

        // Debug output
        CustomOutput.WriteLine("Test: ChessBoard_MovePiece_ShouldPreventInvalidPawnMove");
        CustomOutput.WriteLine($"Invalid Move Reason: {chessBoard.InvalidMoveReason}");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_IsKingInCheck_ShouldDetectCheck()
    {
        // Arrange
        var whiteKing = new King("White", (7, 4));
        var blackRook = new Rook("Black", (0, 4));
        var blackKing = new King("Black", (0, 0)); // Added Black King
        var chessBoard = InitializeCustomBoard(
            (whiteKing, (7, 4)),
            (blackRook, (0, 4)),
            (blackKing, (0, 0))
        );

        // Act
        bool isCheck = chessBoard.IsKingInCheck("White");

        // Assert
        Assert.True(isCheck, "White king should be in check from black rook.");

        // Debug output
        CustomOutput.WriteLine("Test: ChessBoard_IsKingInCheck_ShouldDetectCheck");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_MovePiece_ShouldNotAllowMovingIntoCheck()
    {
        // Arrange
        var whiteKing = new King("White", (7, 4));
        var blackRook = new Rook("Black", (0, 4));
        var whitePawn = new Pawn("White", (6, 4)); // Blocking the rook
        var blackKing = new King("Black", (0, 0)); // Added Black King
        var chessBoard = InitializeCustomBoard(
            (whiteKing, (7, 4)),
            (blackRook, (0, 4)),
            (whitePawn, (6, 4)),
            (blackKing, (0, 0))
        );

        var from = (6, 4);
        var to = (5, 4); // Moving pawn exposes king to check

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Assert
        Assert.False(moveSuccess, "Should not allow move that puts own king in check.");
        Assert.Equal("Move would put or leave the king in check.", chessBoard.InvalidMoveReason);

        // Debug output
        CustomOutput.WriteLine("Test: ChessBoard_MovePiece_ShouldNotAllowMovingIntoCheck");
        CustomOutput.WriteLine($"Invalid Move Reason: {chessBoard.InvalidMoveReason}");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_MovePiece_ShouldAllowCapture()
    {
        // Arrange
        var whitePawn = new Pawn("White", (6, 3));
        var blackPawn = new Pawn("Black", (5, 4));
        var whiteKing = new King("White", (7, 4)); // Added White King
        var blackKing = new King("Black", (0, 4)); // Added Black King
        var chessBoard = InitializeCustomBoard(
            (whitePawn, (6, 3)),
            (blackPawn, (5, 4)),
            (whiteKing, (7, 4)),
            (blackKing, (0, 4))
        );

        var from = (6, 3);
        var to = (5, 4);

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Assert
        Assert.True(moveSuccess, "Pawn should be able to capture diagonally.");
        Assert.Null(chessBoard.ChessPieces[6, 3]);
        var movedPiece = chessBoard.ChessPieces[5, 4];
        Assert.IsType<Pawn>(movedPiece);
        Assert.Equal("White", movedPiece.Color);

        // Debug output
        CustomOutput.WriteLine("Test: ChessBoard_MovePiece_ShouldAllowCapture");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_IsKingInCheck_ShouldReturnFalseWhenNotInCheck()
    {
        // Arrange
        var whiteKing = new King("White", (7, 4));
        var blackRook = new Rook("Black", (0, 0));
        var blackKing = new King("Black", (0, 4)); // Added Black King
        var chessBoard = InitializeCustomBoard(
            (whiteKing, (7, 4)),
            (blackRook, (0, 0)),
            (blackKing, (0, 4))
        );

        // Act
        bool isCheck = chessBoard.IsKingInCheck("White");

        // Assert
        Assert.False(isCheck, "White king should not be in check.");

        // Debug output
        CustomOutput.WriteLine("Test: ChessBoard_IsKingInCheck_ShouldReturnFalseWhenNotInCheck");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_IsKingInCheck_ShouldThrowExceptionWhenKingNotFound()
    {
        // Arrange
        var chessBoard = InitializeCustomBoard(); // Empty board

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => chessBoard.IsKingInCheck("White"));
        Assert.Equal("King not found!", exception.Message);

        // Debug output
        CustomOutput.WriteLine("Test: ChessBoard_IsKingInCheck_ShouldThrowExceptionWhenKingNotFound");
        CustomOutput.WriteLine($"Exception Message: {exception.Message}");
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_MovePiece_ShouldNotAllowMovingOpponentPiece()
    {
        // Arrange
        var whitePawn = new Pawn("White", (6, 0));
        var blackPawn = new Pawn("Black", (1, 0));
        var whiteKing = new King("White", (7, 4)); // Added White King
        var blackKing = new King("Black", (0, 4)); // Added Black King
        var chessBoard = InitializeCustomBoard(
            (whitePawn, (6, 0)),
            (blackPawn, (1, 0)),
            (whiteKing, (7, 4)),
            (blackKing, (0, 4))
        );

        var from = (1, 0); // Attempting to move black pawn on white's turn
        var to = (2, 0);

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Assert
        Assert.False(moveSuccess, "Should not allow moving opponent's piece.");
        Assert.Equal("It's not your turn.", chessBoard.InvalidMoveReason);
        Assert.IsType<Pawn>(chessBoard.ChessPieces[1, 0]);
        Assert.Null(chessBoard.ChessPieces[2, 0]);

        // Debug output
        CustomOutput.WriteLine("Test: ChessBoard_MovePiece_ShouldNotAllowMovingOpponentPiece");
        CustomOutput.WriteLine($"Invalid Move Reason: {chessBoard.InvalidMoveReason}");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_MovePiece_ShouldSwitchPlayersAfterValidMove()
    {
        // Arrange
        var whitePawn = new Pawn("White", (6, 0));
        var whiteKing = new King("White", (7, 4)); // Added White King
        var blackKing = new King("Black", (0, 4)); // Added Black King
        var chessBoard = InitializeCustomBoard(
            (whitePawn, (6, 0)),
            (whiteKing, (7, 4)),
            (blackKing, (0, 4))
        );

        var from = (6, 0);
        var to = (5, 0);

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Assert
        Assert.True(moveSuccess, "Move should be successful.");
        Assert.Equal("Black", chessBoard.CurrentPlayer);

        // Debug output
        CustomOutput.WriteLine("Test: ChessBoard_MovePiece_ShouldSwitchPlayersAfterValidMove");
        CustomOutput.WriteLine($"Current Player: {chessBoard.CurrentPlayer}");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    [Fact]
    public void ChessBoard_MovePiece_ShouldNotAllowMoveFromEmptySquare()
    {
        // Arrange
        var whiteKing = new King("White", (7, 4)); // Added White King
        var blackKing = new King("Black", (0, 4)); // Added Black King
        var chessBoard = InitializeCustomBoard(
            (whiteKing, (7, 4)),
            (blackKing, (0, 4))
        );
        var from = (6, 0); // Empty square
        var to = (5, 0);

        // Act
        bool moveSuccess = chessBoard.MovePiece(from, to);

        // Assert
        Assert.False(moveSuccess, "Should not allow move from empty square.");
        Assert.Equal("No piece at the starting position.", chessBoard.InvalidMoveReason);

        // Debug output
        CustomOutput.WriteLine("Test: ChessBoard_MovePiece_ShouldNotAllowMoveFromEmptySquare");
        CustomOutput.WriteLine($"Invalid Move Reason: {chessBoard.InvalidMoveReason}");
        PrintBoard(chessBoard);
        CustomOutput.Flush();
    }

    // Additional tests can be added here to cover en passant, castling, checkmate, etc.
}
