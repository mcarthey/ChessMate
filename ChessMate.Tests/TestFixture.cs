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
        serviceCollection.AddSingleton<IChessBoard, ChessBoard>();
        serviceCollection.AddSingleton<IStateService, StateService>();
        serviceCollection.AddSingleton<IMoveService, MoveService>();
        serviceCollection.AddSingleton<IGameEngine, GameEngine>();
        serviceCollection.AddSingleton<IMoveValidatorService, MoveValidatorService>();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}