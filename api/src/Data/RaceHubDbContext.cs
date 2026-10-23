using Microsoft.EntityFrameworkCore;
using RaceHub.Api.Domain;

namespace RaceHub.Api.Data;

public sealed class RaceHubDbContext(DbContextOptions<RaceHubDbContext> options) : DbContext(options)
{
    public DbSet<RaceEvent> Races => Set<RaceEvent>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<KitDelivery> KitDeliveries => Set<KitDelivery>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favorite>().HasIndex(item => new { item.UserId, item.RaceEventId }).IsUnique();
        modelBuilder.Entity<Enrollment>().HasOne(item => item.Race).WithMany().HasForeignKey(item => item.RaceEventId);
    }
}

public static class DemoData
{
    public static readonly Guid DemoUserId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid GoldenRunId = Guid.Parse("20000000-0000-0000-0000-000000000001");

    public static void Seed(RaceHubDbContext db)
    {
        if (db.Races.Any()) return;
        db.Races.AddRange(
            new RaceEvent { Id = GoldenRunId, Name = "Golden Run São Paulo", City = "São Paulo", Venue = "Parque do Povo", Distances = ["5 km", "10 km"], StartDate = new(2026, 11, 15), RegistrationClosesOn = new(2026, 11, 5) },
            new RaceEvent { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Name = "Night Run Campinas", City = "Campinas", Venue = "Lagoa do Taquaral", Distances = ["5 km", "10 km", "21 km"], StartDate = new(2026, 12, 5), RegistrationClosesOn = new(2026, 11, 28) });
        var enrollment = new Enrollment { Id = Guid.Parse("30000000-0000-0000-0000-000000000001"), UserId = DemoUserId, RaceEventId = GoldenRunId, Distance = "10 km", ConfirmedAt = new(2026, 10, 23, 9, 42, 0, TimeSpan.FromHours(-3)) };
        db.Enrollments.Add(enrollment);
        db.KitDeliveries.Add(new KitDelivery { Id = Guid.Parse("40000000-0000-0000-0000-000000000001"), EnrollmentId = enrollment.Id, Status = "preparing", Destination = "São Paulo, SP", ExpectedOn = new(2026, 10, 28) });
        db.Favorites.Add(new Favorite { UserId = DemoUserId, RaceEventId = GoldenRunId });
        db.SaveChanges();
    }
}
