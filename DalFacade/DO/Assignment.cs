
namespace DO;

/// <summary>
/// Assinment Entity represents an assignment with all its props
/// </summary>
/// <param name="Id">unique ID (created automatically)</param>
/// <param name="callId">ID of the call</param>
/// <param name="volunteerId\">ID of the volunteer</param>
/// <param name="insersionTime">the date of the first time this volunteer chose this call</param>
/// <param name="finishTime">the date the volunteer finish process this call</param>
/// <param name="finishType">why the volunteer finish proces this call</param>

public record Assignment
    (
    int CallId,
    int VolunteerId,
    DateTime Insersion,
    DateTime? FinishTime = null,
    Finish_Type? FinishType = null
    )
{
    public int Id { get; init; }
    public Assignment() : this(0,0,DateTime.Now)   { }

}