// File: ChessMate.Tests/Services/MoveValidatorServiceTests.cs

using ChessMate.Models;
using ChessMate.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ChessMate.Tests.Services;

public class MoveValidatorServiceTests : TestHelper
{
    private readonly Mock<IGameContext> _mockGameContext;
    private readonly MoveValidatorService _moveValidatorService;

    public MoveValidatorServiceTests(ITestOutputHelper output) : base(output)
    {
        _mockGameContext = new Mock<IGameContext>();
        _moveValidatorService = new MoveValidatorService();
    }

    [Fact]
    public void IsValidMove_ValidMove_ReturnsTrue()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("a3");
        var whitePawn = new Pawn("White", from);

        var mockPawn = new Mock<Pawn>("White", from) { CallBase = true };
        mockPawn.Setup(p => p.IsValidMove(to, _mockGameContext.Object)).Returns(true);

        // Act
        var result = _moveValidatorService.IsValidMove(mockPawn.Object, to, _mockGameContext.Object);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidMove_InvalidMove_ReturnsFalse()
    {
        // Arrange
        var from = new Position("a2");
        var to = new Position("b3"); // Invalid move for a pawn moving forward
        var whitePawn = new Pawn("White", from);

        var mockPawn = new Mock<Pawn>("White", from) { CallBase = true };
        mockPawn.Setup(p => p.IsValidMove(to, _mockGameContext.Object)).Returns(false);

        // Act
        var result = _moveValidatorService.IsValidMove(mockPawn.Object, to, _mockGameContext.Object);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidMove_NullPiece_ReturnsFalse()
    {
        // Arrange
        var to = new Position("a3");

        // Act
        var result = _moveValidatorService.IsValidMove(null, to, _mockGameContext.Object);

        // Assert
        Assert.False(result);
    }
}


