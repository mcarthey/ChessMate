using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ChessMate.Data;
using ChessMate.Hubs;
using ChessMate.Models;
using ChessMate.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Register services for DI
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<IChessBoard, ChessBoard>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<IMoveService, MoveService>();
builder.Services.AddScoped<IGameEngine, GameEngine>();
builder.Services.AddScoped<IGameContext, GameContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<ChessHub>("/chesshub"); // SignalR hub for the chess app
app.MapFallbackToPage("/_Host");

app.Run();
