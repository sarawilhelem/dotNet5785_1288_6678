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
    public int Id { get; init; }
    public int CallId { get; init; }
    public Call_Type CallType { get; set; }
    public string? Description { get; set; }
    required public string Address { get; set; }
    public DateTime OpenTime { get; init; }
    public DateTime? MaxCloseTime { get; set; }

    public DateTime Insersion {  get; init;}
    public double Distance { get; init; }
    public CallStatus Status { get; set; }
}

