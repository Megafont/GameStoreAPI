
// This project was built starting with this course:
// https://www.youtube.com/watch?v=YbRe4iIVYJk

using GameStore.API;
using GameStore.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation(); // This adds validation services to the DI container, which allows us to use data annotations for validating our DTOs. These are the attributes like [Required] that appear in the DTO classes.

var connectionString = "Data Source=GameStore.db";
builder.Services.AddSqlite<GameStoreContext>(connectionString);


var app = builder.Build();

app.MapGamesEndpoints(); // This is the extension method we declared in GamesEndpoints.cs that maps all the endpoints related to games.

app.MigrateDb(); // This is the extension method we declared in DataExtensions.cs that applies any pending migrations to the database. This ensures that our database schema is up to date with our EF Core models in the Data folder when the application starts.

app.Run();

