using System;
using GameStore.API.Data;
using GameStore.API.DTOs;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Endpoints;

public static class GamesEndpoints
{
    const string GET_GAME_BY_ID_ENDPOINT_NAME = "GetGameById";


    public static void MapGamesEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/games");


        // GET /games
        group.MapGet("/", async (GameStoreContext dbContext) 
            => await dbContext.Games
                              .Include(game => game.Genre) // This is an example of how to include related data (the genre) when querying the Games. This assumes that the Genre navigation property is properly configured in the Game entity and that the Genre data is being loaded when querying the Games.
                              .Select(game => new GameSummaryDTO(
                                game.Id,
                                game.Title,
                                game.Genre!.Name, // This is an example of how to include related data (the genre name) in the DTO. This assumes that the Genre navigation property is properly configured in the Game entity and that the Genre data is being loaded when querying the Games.
                                game.Price,
                                game.ReleaseDate
                            ))
                            .AsNoTracking() // We are not going to modify these games and then store them back in the database, so calling AsNoTracking() tells EF Core that it doesn't need to track changes to these entities, which can improve performance and reduce memory usage.
                            .ToListAsync()); // We call ToListAsync() here, as otherwise this code is not async, and thus the compiler will get mad since it is preceded by the await statement because we want it to be an async operation. The reason we want it to be async is because database operations can be slow, and making them async allows the server to handle other requests while waiting for the database operation to complete, which can improve the scalability and responsiveness of the application.


        // GET /games/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDTO(
                    game.Id,
                    game.Title,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            );
        })
            .WithName(GET_GAME_BY_ID_ENDPOINT_NAME);


        // POST /games
        group.MapPost("/", async (CreateGameDTO newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Title = newGame.Title,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDTO gameDetailsDTO = new(
                game.Id,
                game.Title,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GET_GAME_BY_ID_ENDPOINT_NAME, new { id = gameDetailsDTO.Id }, gameDetailsDTO);
        });


        // PUT /games/{id}
        group.MapPut("/{id}", async (
            int id, 
            UpdateGameDTO updatedGame, 
            GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            // If the game with the specified ID doesn't exist, return a 404 Not Found response
            if (existingGame is null)
                return Results.NotFound();
            

            existingGame.Title = updatedGame.Title;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });


        // DELETE /games/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                            .Where(game => game.Id == id)
                            .ExecuteDeleteAsync();

            // We don't call dbContext.SaveChangesAsync() here because ExecuteDeleteAsync() is a method that executes a delete operation directly in the database, and it does not require calling SaveChangesAsync() to persist the changes. This is because ExecuteDeleteAsync() generates and executes a SQL DELETE statement directly against the database, and it does not track changes to entities in the DbContext like other methods such as Remove() or RemoveRange() do. Therefore, there are no changes to track or save when using ExecuteDeleteAsync(), and it can be more efficient for deleting large numbers of records without needing to load them into memory first.

            return Results.NoContent();
        });


        // GET /hello
        group.MapGet("/hello", () => "Hello World!");

    }

}
