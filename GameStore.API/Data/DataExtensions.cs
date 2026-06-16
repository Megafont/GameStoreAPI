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
