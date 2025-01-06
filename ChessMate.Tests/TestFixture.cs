using ChessMate.Hubs;
using ChessMate.Models;
using ChessMate.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ChessMate.Tests;

public class TestFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }
    public Mock<IHubContext<ChessHub>> MockHubContext => _mockHubContext;

    private readonly Mock<IHubContext<ChessHub>> _mockHubContext;

    public TestFixture()
    {
        var serviceCollection = new ServiceCollection();

        // Register services for DI
        serviceCollection.AddSingleton<IChessBoard, ChessBoard>();
        serviceCollection.AddSingleton<IStateService, StateService>();
        serviceCollection.AddSingleton<IMoveService, MoveService>();
        serviceCollection.AddSingleton<IMoveValidatorService, MoveValidatorService>();

        // Create and configure the mock IHubContext<ChessHub>
        _mockHubContext = new Mock<IHubContext<ChessHub>>();

        // Optionally, set up mock behavior for Clients and Groups if needed
        var mockClients = new Mock<IHubClients>();
        var mockClientProxy = new Mock<IClientProxy>();

        mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
        _mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

        // Register the mock IHubContext<ChessHub> with the service collection
        serviceCollection.AddSingleton(_mockHubContext.Object);

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}