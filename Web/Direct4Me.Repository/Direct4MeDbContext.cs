using Direct4Me.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Direct4Me.Repository;

public class Direct4MeDbContext : DbContext
{
    public Direct4MeDbContext(DbContextOptions<Direct4MeDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> UserEntities { get; set; }
}