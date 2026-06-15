using System;
using GameStore.API.DTOs;

namespace GameStore.API;

public static class GamesEndpoints
{
    const string GET_GAME_BY_ID_ENDPOINT_NAME = "GetGameById";

    private static readonly List<GameDTO> games =
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

    public static void MapGamesEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/games");


        // GET /games
        group.MapGet("/", () => games);


        // GET /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            GameDTO game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
            .WithName(GET_GAME_BY_ID_ENDPOINT_NAME);


        // POST /games
        group.MapPost("/", (CreateGameDTO newGame) =>
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
        group.MapPut("/{id}", (int id, UpdateGameDTO updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            // If the game with the specified ID doesn't exist, return a 404 Not Found response
            if (index == -1)
                return Results.NotFound();
            

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
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });


        // GET /hello
        group.MapGet("/hello", () => "Hello World!");

    }

}
