using System;

using Microsoft.EntityFrameworkCore;

using GameStore.API.Data;

namespace GameStore.API;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate(); // If this line causes an error that Migrate() is not found, make sure you have the Microsoft.EntityFrameworkCore Nuget package installed and that you have the "using Microsoft.EntityFrameworkCore;" statement at the top of this file.
    }
}
