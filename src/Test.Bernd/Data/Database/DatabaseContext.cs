using Microsoft.EntityFrameworkCore;
using Test.Bernd.Models.Domain;

namespace Test.Bernd.Data.Database
{
    public class DatabaseContext : DbContext
    {
        // to add a new migration: dotnet ef migrations add Init -o "Data/Database/Migrations" --context DatabaseContext
        
        // public DbSet<ParkingLot> ParkingLot { get; set; }
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            // Can init data for parking lots like this
            // modelBuilder.Entity<ParkingLot>().HasData(new List<ParkingLot>
            // {
            //     new ParkingLot(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100),
            //     new ParkingLot(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50),
            //     new ParkingLot(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 10),
            // });
        }
    }
}