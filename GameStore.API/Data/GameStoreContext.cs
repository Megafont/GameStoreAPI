using System;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) 
    : DbContext(options)
{
    public DbSet<Models.Game> Games => Set<Models.Game>();
    public DbSet<Models.Genre> Genres => Set<Models.Genre>();
}
