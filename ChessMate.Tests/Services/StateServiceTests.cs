// File: ChessMate.Tests/Services/StateServiceTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services;

public class StateServiceTests : TestHelper
{
    private readonly StateService _stateService;

    public StateServiceTests(ITestOutputHelper output) : base(output)
    {
        _stateService = new StateService();
    }

    [Fact]
    public void InitialState_CurrentPlayer_ShouldBeWhite()
    {
        // Arrange & Act
        var currentPlayer = _stateService.CurrentPlayer;

        // Assert
        Assert.Equal("White", currentPlayer);
    }

    [Fact]
    public void SwitchPlayer_TogglesCurrentPlayer()
    {
        // Arrange & Act
        _stateService.SwitchPlayer();

        // Assert
        Assert.Equal("Black", _stateService.CurrentPlayer);

        // Switch back
        _stateService.SwitchPlayer();
        Assert.Equal("White", _stateService.CurrentPlayer);
    }

    [Fact]
    public void SetPlayer_ValidPlayer_SetsCurrentPlayer()
    {
        // Arrange & Act
        _stateService.SetPlayer("Black");

        // Assert
        Assert.Equal("Black", _stateService.CurrentPlayer);
    }

    [Fact]
    public void SetPlayer_InvalidPlayer_ThrowsException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _stateService.SetPlayer("Green"));
        Assert.Equal("Invalid player. Must be 'White' or 'Black'.", exception.Message);
    }

    [Fact]
    public void SetEnPassantTarget_Pawn_SetsTarget()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("a2"));
        var targetPosition = new Position("a3");

        // Act
        _stateService.SetEnPassantTarget(targetPosition, pawn);

        // Assert
        Assert.Equal(targetPosition, _stateService.EnPassantTarget);
    }

    [Fact]
    public void SetEnPassantTarget_NonPawn_DoesNotSetTarget()
    {
        // Arrange
        var rook = new Rook("White", new Position("a2"));
        var targetPosition = new Position("a3");

        // Act
        _stateService.SetEnPassantTarget(targetPosition, rook);

        // Assert
        Assert.Null(_stateService.EnPassantTarget);
    }

    [Fact]
    public void ResetEnPassantTarget_ClearsTarget()
    {
        // Arrange
        var pawn = new Pawn("White", new Position("a2"));
        var targetPosition = new Position("a3");
        _stateService.SetEnPassantTarget(targetPosition, pawn);

        // Act
        _stateService.ResetEnPassantTarget();

        // Assert
        Assert.Null(_stateService.EnPassantTarget);
    }

    [Fact]
    public void ResetState_ResetsAllProperties()
    {
        // Arrange
        // Change various properties
        _stateService.SetPlayer("Black");
        _stateService.IsCheck = true;
        _stateService.IsCheckmate = true;
        var pawn = new Pawn("White", new Position("a2"));
        _stateService.SetEnPassantTarget(new Position("a3"), pawn);
        _stateService.WhiteKingMoved = true;
        _stateService.BlackKingMoved = true;
        _stateService.WhiteRookKingSideMoved = true;
        _stateService.WhiteRookQueenSideMoved = true;
        _stateService.BlackRookKingSideMoved = true;
        _stateService.BlackRookQueenSideMoved = true;
        _stateService.MoveLog.Add("Sample Move");

        // Manually set attack maps to include all positions
        var allPositions = Enumerable.Range(0, 8)
            .SelectMany(row => Enumerable.Range(0, 8)
                .Select(col => new Position(row, col))).ToList();

        _stateService.WhiteAttacks.UnionWith(allPositions);
        _stateService.BlackAttacks.UnionWith(allPositions);

        // Act
        _stateService.ResetState();

        // Assert
        // Verify all properties are reset
        Assert.Equal("White", _stateService.CurrentPlayer);
        Assert.False(_stateService.IsCheck);
        Assert.False(_stateService.IsCheckmate);
        Assert.Null(_stateService.EnPassantTarget);
        Assert.False(_stateService.WhiteKingMoved);
        Assert.False(_stateService.BlackKingMoved);
        Assert.False(_stateService.WhiteRookKingSideMoved);
        Assert.False(_stateService.WhiteRookQueenSideMoved);
        Assert.False(_stateService.BlackRookKingSideMoved);
        Assert.False(_stateService.BlackRookQueenSideMoved);
        Assert.Empty(_stateService.MoveLog);

        // Check that all elements in WhiteAttacks are false
        Assert.All(_stateService.WhiteAttacks.Cast<bool>(), b => Assert.False(b));

        // Check that all elements in BlackAttacks are false
        Assert.All(_stateService.BlackAttacks.Cast<bool>(), b => Assert.False(b));
    }


    [Fact]
    public void CastlingFlags_DefaultToFalse()
    {
        // Arrange & Act

        // Assert
        Assert.False(_stateService.WhiteKingMoved);
        Assert.False(_stateService.BlackKingMoved);
        Assert.False(_stateService.WhiteRookKingSideMoved);
        Assert.False(_stateService.WhiteRookQueenSideMoved);
        Assert.False(_stateService.BlackRookKingSideMoved);
        Assert.False(_stateService.BlackRookQueenSideMoved);
    }

    [Fact]
    public void MoveLog_InitializesEmpty()
    {
        // Arrange & Act
        var moveLog = _stateService.MoveLog;

        // Assert
        Assert.NotNull(moveLog);
        Assert.Empty(moveLog);
    }

    [Fact]
    public void MoveLog_ClearsOnReset()
    {
        // Arrange
        _stateService.MoveLog.Add("e2e4");
        _stateService.MoveLog.Add("e7e5");
        Assert.NotEmpty(_stateService.MoveLog);

        // Act
        _stateService.ResetState();

        // Assert
        Assert.Empty(_stateService.MoveLog);
    }

    [Fact]
    public void IsCheck_Settable()
    {
        // Arrange & Act
        _stateService.IsCheck = true;

        // Assert
        Assert.True(_stateService.IsCheck);
    }

    [Fact]
    public void IsCheckmate_Settable()
    {
        // Arrange & Act
        _stateService.IsCheckmate = true;

        // Assert
        Assert.True(_stateService.IsCheckmate);
    }

    [Fact]
    public void UpdateGameStateAfterMove_LogsMoveAndSwitchesPlayer()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        var whitePawn = new Pawn("White", from);

        // Add a black king to the board
        var blackKingPosition = new Position("e8");
        var blackKing = new King("Black", blackKingPosition);

        // Initialize the board with both pieces
        var board = InitializeCustomBoard(
            (whitePawn, from),
            (blackKing, blackKingPosition)
        );

        // Create a mock game context with the board and state
        var mockGameContext = new Mock<IGameContext>();
        mockGameContext.Setup(gc => gc.Board).Returns(board);
        mockGameContext.Setup(gc => gc.State).Returns(_stateService);

        // Act
        _stateService.UpdateGameStateAfterMove(whitePawn, from, to, mockGameContext.Object);

        // Assert
        Assert.Single(_stateService.MoveLog);
        Assert.Equal("White Pawn from a2 to a3", _stateService.MoveLog.First());
        Assert.Equal("Black", _stateService.CurrentPlayer);
    }


    [Fact]
    public void UpdateAttackMaps_CorrectlyUpdatesAttackMaps()
    {
        // Arrange
        var whiteRookPosition = new Position("a1");
        var blackRookPosition = new Position("h8");

        var whiteRook = new Rook("White", whiteRookPosition);
        var blackRook = new Rook("Black", blackRookPosition);

        var board = InitializeCustomBoard(
            (whiteRook, whiteRookPosition),
            (blackRook, blackRookPosition)
        );

        var gameContext = new Mock<IGameContext>();
        gameContext.Setup(gc => gc.Board).Returns(board);
        gameContext.Setup(gc => gc.State).Returns(_stateService);

        // Act
        _stateService.UpdateAttackMaps(gameContext.Object);

        PrintAttackMap(_stateService.WhiteAttacks);

        // Assert
        // White rook attacks along column 'a' and row '1', excluding its own square 'a1'
        for (char file = 'a'; file <= 'h'; file++)
        {
            var position = new Position($"{file}1");
            if (position != whiteRookPosition)
            {
                Assert.Contains(position, _stateService.WhiteAttacks);
            }
            else
            {
                Assert.DoesNotContain(position, _stateService.WhiteAttacks);
            }
        }

        for (int rank = 1; rank <= 8; rank++)
        {
            var position = new Position($"a{rank}");
            if (position != whiteRookPosition)
            {
                Assert.Contains(position, _stateService.WhiteAttacks);
            }
            else
            {
                Assert.DoesNotContain(position, _stateService.WhiteAttacks);
            }
        }

        // Black rook attacks along column 'h' and row '8', excluding its own square 'h8'
        for (char file = 'a'; file <= 'h'; file++)
        {
            var position = new Position($"{file}8");
            if (position != blackRookPosition)
            {
                Assert.Contains(position, _stateService.BlackAttacks);
            }
            else
            {
                Assert.DoesNotContain(position, _stateService.BlackAttacks);
            }
        }

        for (int rank = 1; rank <= 8; rank++)
        {
            var position = new Position($"h{rank}");
            if (position != blackRookPosition)
            {
                Assert.Contains(position, _stateService.BlackAttacks);
            }
            else
            {
                Assert.DoesNotContain(position, _stateService.BlackAttacks);
            }
        }
    }

    [Fact]
    public void IsKingInCheck_KingUnderAttack_ReturnsTrue()
    {
        // Arrange
        var whiteKing = new King("White", new Position("e1"));
        var blackRook = new Rook("Black", new Position("e8"));

        var board = InitializeCustomBoard(
            (whiteKing, whiteKing.Position),
            (blackRook, blackRook.Position)
        );

        var gameContext = new Mock<IGameContext>();
        gameContext.Setup(gc => gc.Board).Returns(board);
        gameContext.Setup(gc => gc.State).Returns(_stateService);

        // Update attack maps
        _stateService.UpdateAttackMaps(gameContext.Object);

        // Act
        var isWhiteKingInCheck = _stateService.IsKingInCheck("White", gameContext.Object);

        // Assert
        Assert.True(isWhiteKingInCheck);
    }

    [Fact]
    public void HasLegalMoves_PlayerHasNoLegalMoves_ReturnsFalse()
    {
        // Arrange
        var whiteKingPosition = new Position("h1");
        var whiteKing = new King("White", whiteKingPosition);
        var blackRook1 = new Rook("Black", new Position("f2"));
        var blackRook2 = new Rook("Black", new Position("f1"));

        var board = InitializeCustomBoard(
            (whiteKing, whiteKingPosition),
            (blackRook1, blackRook1.Position),
            (blackRook2, blackRook2.Position)
        );

        var gameContext = new Mock<IGameContext>();
        gameContext.Setup(gc => gc.Board).Returns(board);
        gameContext.Setup(gc => gc.State).Returns(_stateService);

        // Update attack maps
        _stateService.UpdateAttackMaps(gameContext.Object);

        // Act
        var hasLegalMoves = _stateService.HasLegalMoves("White", gameContext.Object);

        // Assert
        Assert.False(hasLegalMoves);
    }

    [Fact]
    public void WouldMoveCauseSelfCheck_MovingPieceExposesKing_ReturnsTrue()
    {
        // Arrange
        var whiteKingPosition = new Position("e1");
        var whiteBishopPosition = new Position("e2");
        var moveToPosition = new Position("d3");

        var whiteKing = new King("White", whiteKingPosition);
        var whiteBishop = new Bishop("White", whiteBishopPosition);
        var blackRook = new Rook("Black", new Position("e8"));

        var board = InitializeCustomBoard(
            (whiteKing, whiteKingPosition),
            (whiteBishop, whiteBishopPosition),
            (blackRook, blackRook.Position)
        );

        var gameContext = new Mock<IGameContext>();
        gameContext.Setup(gc => gc.Board).Returns(board);
        gameContext.Setup(gc => gc.State).Returns(_stateService);

        // Update attack maps
        _stateService.UpdateAttackMaps(gameContext.Object);

        // Act
        var wouldCauseSelfCheck = _stateService.WouldMoveCauseSelfCheck(whiteBishop, whiteBishopPosition, moveToPosition, gameContext.Object);

        // Assert
        Assert.True(wouldCauseSelfCheck);
    }

    [Fact]
    public void UpdateGameStateAfterMove_IdentifiesCheckmate()
    {
        // Arrange
        var blackKing = new King("Black", new Position("h8"));
        var whiteQueen = new Queen("White", new Position("g6"));
        var whiteRook = new Rook("White", new Position("h7"));

        var board = InitializeCustomBoard(
            (blackKing, blackKing.Position),
            (whiteQueen, whiteQueen.Position),
            (whiteRook, whiteRook.Position)
        );

        var gameContext = new Mock<IGameContext>();
        gameContext.Setup(gc => gc.Board).Returns(board);
        gameContext.Setup(gc => gc.State).Returns(_stateService);

        // Update attack maps before move
        _stateService.UpdateAttackMaps(gameContext.Object);

        // Act
        _stateService.UpdateGameStateAfterMove(whiteQueen, whiteQueen.Position, whiteQueen.Position, gameContext.Object);

        // Assert
        Assert.True(_stateService.IsCheck);
        Assert.True(_stateService.IsCheckmate);
    }
}
