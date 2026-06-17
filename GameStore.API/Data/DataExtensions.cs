using System;

using Microsoft.EntityFrameworkCore;

using GameStore.API.Data;
using GameStore.API.Models;

namespace GameStore.API;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate(); // If this line causes an error that Migrate() is not found, make sure you have the Microsoft.EntityFrameworkCore Nuget package installed and that you have the "using Microsoft.EntityFrameworkCore;" statement at the top of this file.
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        // Load the connection string from appSettings.json.
        // NOTE: Connection strings in the appsettings.json file can be overriden. For example, if you set an environment variable named "ConnectionStrings__GameStore" to a different value, that value will be used instead of the one in appsettings.json. This is a common way to provide different connection strings for development and production environments without having to change the code or the appsettings.json file.
        //       You can do this by going into the terminal, and typing "$env:ConnectionStrings__GameStore="Data Source=Production.db"
        //       If you then run the program again, it will create a database named Production.db instead of the normal GameStore.db specified in appsettings.json. This is a very useful feature for managing different environments like development, staging, and production.
        //       This environment variable will persist until you kill the terminal tab you created it in. If you then run the program again, it will go back to using the connection string from appsettings.json and create a database named GameStore.db if it doesn't already exist in the project.
        //       In VS Code, you can kill the termain by going to the terminal tab, and in its right pane, right-click the terminal you want to kill and select "Kill Terminal". You can also just select the terminal in the list and press Del.
        var connectionString = builder.Configuration.GetConnectionString("GameStore");

        // DbContext has a Scoped service lifetime because:
        // 1. It ensures that a new instance of DbContext is created per request, which is important for maintaining the integrity of the unit of work pattern. Each request can be treated as a separate unit of work, and having a new DbContext instance for each request helps to ensure that changes made during one request do not interfere with changes made during another request.
        // 2. Database connections are a limited and expensive resource. Using a Scoped lifetime allows for efficient management of database connections by creating a new DbContext instance for each request and disposing of it at the end of the request, which helps to prevent connection leaks and ensures that connections are properly released back to the connection pool.
        // 3. DbContext is not thread-safe. Scoped avoids concurrency issues that can arise from sharing a single DbContext instance across multiple requests or threads. Each request gets its own DbContext instance, preventing conflicts and ensuring thread safety.
        // 4. Makes it easier to manage transactions and ensure data consistency within a single request
        // 5. Reusing a DbContext instance can lead to increased memory usage and potential memory leaks due to tracking of entities. Scoped allows for proper disposal of DbContext instances at the end of each request, preventing memory leaks and ensuring efficient resource management.

        builder.Services.AddSqlite<GameStoreContext>(
            connectionString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                // If the Genres table is empty, seed it with some initial genres.
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        new Genre { Name = "Fighting" },
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Platformer" },
                        new Genre { Name = "Racing" },
                        new Genre { Name = "Sports" },
                        new Genre { Name = "Action-Adventure" },
                        new Genre { Name = "Metroidvania" }
                    );

                    context.SaveChanges();
                }
            })
        );

    }
}
