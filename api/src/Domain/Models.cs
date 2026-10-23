namespace RaceHub.Api.Domain;

public sealed class RaceEvent
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string Venue { get; set; }
    public required string[] Distances { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly RegistrationClosesOn { get; set; }
}

public sealed class Favorite
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid RaceEventId { get; set; }
}

public sealed class Enrollment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RaceEventId { get; set; }
    public RaceEvent Race { get; set; } = null!;
    public required string Distance { get; set; }
    public DateTimeOffset ConfirmedAt { get; set; }
}

public sealed class KitDelivery
{
    public Guid Id { get; set; }
    public Guid EnrollmentId { get; set; }
    public required string Status { get; set; }
    public required string Destination { get; set; }
    public DateOnly ExpectedOn { get; set; }
}

public sealed record RaceSummary(Guid Id, string Name, string City, string Venue, string[] Distances, DateOnly StartDate, bool IsFavorite)
{
    public static RaceSummary From(RaceEvent race, bool isFavorite) =>
        new(race.Id, race.Name, race.City, race.Venue, race.Distances, race.StartDate, isFavorite);
}

public sealed record JourneyStep(string Label, string State, string Detail);

public sealed record JourneyResponse(Guid EnrollmentId, string Race, string Distance, IReadOnlyList<JourneyStep> Steps)
{
    public static JourneyResponse From(Enrollment enrollment, KitDelivery delivery) => new(
        enrollment.Id,
        enrollment.Race.Name,
        enrollment.Distance,
        [
            new("Inscrição confirmada", "complete", enrollment.ConfirmedAt.ToString("dd/MM/yyyy HH:mm")),
            new("Kit em preparação", delivery.Status == "preparing" ? "current" : "complete", $"Previsão: {delivery.ExpectedOn:dd/MM/yyyy}"),
            new("Receber o kit", delivery.Status == "delivered" ? "complete" : "pending", delivery.Destination)
        ]);
}
