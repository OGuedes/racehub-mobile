using Microsoft.EntityFrameworkCore;
using RaceHub.Api.Data;
using RaceHub.Api.Domain;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<RaceHubDbContext>(options =>
{
    if (string.IsNullOrWhiteSpace(connectionString)) options.UseInMemoryDatabase("racehub-demo");
    else options.UseNpgsql(connectionString);
});

var app = builder.Build();
app.UseCors();
if (app.Environment.IsDevelopment()) app.MapOpenApi();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RaceHubDbContext>();
    db.Database.EnsureCreated();
    DemoData.Seed(db);
}

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "racehub-api" }));
var races = app.MapGroup("/api/races");

races.MapGet("/", async (string? city, RaceHubDbContext db) =>
{
    var query = db.Races.AsNoTracking().AsQueryable();
    if (!string.IsNullOrWhiteSpace(city)) query = query.Where(race => race.City.ToLower() == city.ToLower());
    var favorites = await db.Favorites.AsNoTracking()
        .Where(favorite => favorite.UserId == DemoData.DemoUserId)
        .Select(favorite => favorite.RaceEventId).ToListAsync();
    var result = await query.OrderBy(race => race.StartDate).ToListAsync();
    return Results.Ok(result.Select(race => RaceSummary.From(race, favorites.Contains(race.Id))));
});

races.MapGet("/{id:guid}", async (Guid id, RaceHubDbContext db) =>
{
    var race = await db.Races.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id);
    return race is null ? Results.NotFound() : Results.Ok(race);
});

races.MapPut("/{id:guid}/favorite", async (Guid id, RaceHubDbContext db) =>
{
    if (!await db.Races.AnyAsync(race => race.Id == id)) return Results.NotFound();
    var favorite = await db.Favorites.SingleOrDefaultAsync(item =>
        item.UserId == DemoData.DemoUserId && item.RaceEventId == id);
    if (favorite is null) db.Favorites.Add(new Favorite { UserId = DemoData.DemoUserId, RaceEventId = id });
    else db.Favorites.Remove(favorite);
    await db.SaveChangesAsync();
    return Results.Ok(new { raceId = id, isFavorite = favorite is null });
});

app.MapGet("/api/me/journey", async (RaceHubDbContext db) =>
{
    var enrollment = await db.Enrollments.AsNoTracking().Include(item => item.Race)
        .SingleAsync(item => item.UserId == DemoData.DemoUserId);
    var delivery = await db.KitDeliveries.AsNoTracking()
        .SingleAsync(item => item.EnrollmentId == enrollment.Id);
    return Results.Ok(JourneyResponse.From(enrollment, delivery));
});

app.Run();
public partial class Program;
