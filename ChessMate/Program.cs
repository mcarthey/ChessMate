using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ChessMate.Data;
using Microsoft.AspNetCore.SignalR;
using ChessMate.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<WeatherForecastService>();

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
