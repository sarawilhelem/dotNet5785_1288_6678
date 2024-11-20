

namespace DO;

public record Volunteer
(
    int Id,
    string Name,
    string Phone,
    string Email,
    string? Password = null,
    string? Address = null,
    double? Latitude = null,
    double? Longitude = null,
    Role Role = Role.Volunteer,
    bool? IsActive = null,
    double? MaxDistanceCall = null,
    Distance_Type Distance_Type = Distance_Type.Air
    )
;
