using GameStore.API;

const string GET_GAME_BY_ID_ENDPOINT_NAME = "GetGameById";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDTO> games =
[
    new (1,
        "Street Fighter II",
        "Fighting",
        19.99m,
        new DateOnly(1992, 7, 15)),

    new (2,
        "Final Fantasy VII Rebirth",
        "RPG",
        69.99m,
        new DateOnly(2024, 2, 29)),

    new (3,
        "Astro Bot",
        "Platformer",
        59.99m,
        new DateOnly(2024, 9, 6)),

    new (4, 
        "The Legend of Zelda: Breath of the Wild", 
        "Open-world action-adventure",
        59.99m,
        new DateOnly(2017, 3, 3))
];

// GET /games
app.MapGet("/games", () => games);

// GET /games/{id}
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(GET_GAME_BY_ID_ENDPOINT_NAME);

// POST /games
app.MapPost("/games", (CreateGameDTO newGame) =>
{
    GameDTO game = new(
        games.Count + 1,
        newGame.Title,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);

    games.Add(game);

    return Results.CreatedAtRoute(GET_GAME_BY_ID_ENDPOINT_NAME, new { id = game.Id }, game);
});

// PUT /games/{id}
app.MapPut("/games/{id}", (int id, UpdateGameDTO updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    games[index] = new GameDTO(
        id,
        updatedGame.Title,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

// DELETE /games/{id}
app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

// GET /hello
app.MapGet("/hello", () => "Hello World!");

app.Run();
