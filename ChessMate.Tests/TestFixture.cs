using ChessMate.Models;
using ChessMate.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChessMate.Tests;

public class TestFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }

    public TestFixture()
    {
        var serviceCollection = new ServiceCollection();

        // Register services for DI
        serviceCollection.AddScoped<IChessBoard, ChessBoard>();
        serviceCollection.AddScoped<IStateService, StateService>();
        serviceCollection.AddScoped<IMoveService, MoveService>();
        serviceCollection.AddScoped<IGameContext, GameContext>();
        serviceCollection.AddScoped<IGameStateEvaluator, GameStateEvaluator>();
        serviceCollection.AddScoped<IMoveValidatorService, MoveValidatorService>();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}