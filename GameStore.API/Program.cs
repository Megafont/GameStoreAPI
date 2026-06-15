
// This project was built starting with this course:
// https://www.youtube.com/watch?v=YbRe4iIVYJk

using GameStore.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation(); // This adds validation services to the DI container, which allows us to use data annotations for validating our DTOs. These are the attributes like [Required] that appear in the DTO classes.

var app = builder.Build();

app.MapGamesEndpoints(); // This is the extension method we declared in GamesEndpoints.cs that maps all the endpoints related to games.
app.Run();

