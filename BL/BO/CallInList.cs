using Helpers;
namespace BO;

/// <summary>
/// Details of a call in the list
/// </summary>
/// <param name="Id">ID of the assignment</param>
/// <param name="CallId">ID of the call</param>
/// <param name="CallType">type of call:TakeCareAtHome/TakeCareOut/Physiotherapy</param>
/// <param name="OpenTime">the time the call was opened volunteer</param>
/// <param name="MaxCloseTime">when the call has to be closed</param>
/// <param name="LastVolunteerName">Last volunteer who assigned to this call</param>
/// <param name="TotalProcessingTime">How much time was from opening call to finish process</param>
/// <param name="Status">What is the call's status</param>
/// <param name="TotalAssignment">How many assignments this call already assignmented</param>

public class CallInList
{
    public int Id { get; init; }
    public int CallId { get; set; }
    public CallType CallType { get; set; }
    public DateTime OpenTime { get; init; }
    public TimeSpan? MaxCloseTime { get; set; }
    public string? LastVolunteerName {get;set;}
    public TimeSpan? TotalProcessingTime {get; set; }
    public FinishCallType Status {get; set; }
    public int AmountOfAssignments {get; set; }
    public override string ToString() => Helpers.Tools.ToStringProperty(this);

}