using GameStore.API;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGamesEndpoints(); // This is the extension method we declared in GamesEndpoints.cs that maps all the endpoints related to games.
app.Run();
