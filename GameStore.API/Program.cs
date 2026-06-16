
// This project was built starting with this course:
// https://www.youtube.com/watch?v=YbRe4iIVYJk

using GameStore.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation(); // This adds validation services to the DI container, which allows us to use data annotations for validating our DTOs. These are the attributes like [Required] that appear in the DTO classes.
builder.AddGameStoreDb(); // This is the second extension method we declared in DataExtensions.cs that configures the GameStoreContext to use SQLite and also seeds the Genres table with some initial data if it's empty.


var app = builder.Build();

app.MapGamesEndpoints(); // This is the extension method we declared in GamesEndpoints.cs that maps all the endpoints related to games.

app.MigrateDb(); // This is the extension method we declared in DataExtensions.cs that applies any pending migrations to the database. This ensures that our database schema is up to date with our EF Core models in the Data folder when the application starts.

app.Run();


