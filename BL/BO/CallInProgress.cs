//using Helpers;
namespace BO;

/// <summary>
/// Extention of assignment
/// </summary>
/// <param name="Id">unique ID of the assinment</param>
/// <param name="callId">ID of the call</param>
/// <param name="Call_Type">type of call:Take_Care_At_Home/Take_Care_Out/Physiotherapy</param>
/// <param name="Description">details abaut the call</param>
/// <param name="Address">The call address</param>
/// <param name="OpenTime">the time the call was opened volunteer</param>
/// <param name="MaxCloseTime">when the call has to be closed</param>
/// <param name="Insersion">the date of the first time this volunteer chose this call</param>
/// <param name="Distance">the distance between this volunteer to the call address</param>
/// <param name="Status">Status of the call: in care now/ in care and in range risk</param>

public class CallInProgress
{
    public int Id { get; }
    public int CallId { get; }
    public Call_Type Call_Type { get; }
    public string Address { get; }
    public double Distance { get; }
    public DateTime OpenTime { get; }
    public DateTime Insersion { get; }
    public CallStatus Status { get; }
    public string? Description { get; }
    public DateTime? MaxCloseTime { get; }

    public CallInProgress(
        int id,
        int callId,
        Call_Type call_Type,
        string address,
        double distance,
        DateTime openTime,
        DateTime insersion,
        CallStatus status,
        string? description = null,
        DateTime? maxCloseTime = null)
    {
        Id = id;
        CallId = callId;
        Call_Type = call_Type;
        Address = address;
        Distance = distance;
        OpenTime = openTime;
        Insersion = insersion;
        Status = status;
        Description = description;
        MaxCloseTime = maxCloseTime;
    }
}


