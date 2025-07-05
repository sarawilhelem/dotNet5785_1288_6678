
namespace DO;

/// <summary>
/// Call Entity
/// </summary>
/// <param name="Id">unique ID (created automatically)</param>
/// <param name="CallType">type of call:TakeCareAtHome/TakeCareOut/Physiotherapy</param>
/// <param name="Address">to where do they want volunteer</param>
/// <param name="Latitude">vertical distance from the equator</param>
/// <param name="Longitude">horizontal distance from the equator</param>
/// <param name="OpenTime">the time they call a volunteer</param>
/// <param name="Description">details abaut the call</param>
/// <param name="MaxCloseTime">when it has to be closed</param>
public record Call
(
     CallType CallType,
     string Address,
     DateTime OpenTime,
     DateTime MaxCloseTime,
     double? Latitude,
     double? Longitude,
     string? Description = null
)
{
    public int Id { get; init; }
    public Call() : this(CallType.TakeCareAtHome, "Zayit 1, Jerusalem, Israel",  DateTime.Now, DateTime.Now, null, null, "no more details")
    { }
}