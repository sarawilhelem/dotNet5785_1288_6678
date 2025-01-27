using Helpers;


namespace BO;

/// <summary>
/// Details of an open call
/// </summary>
/// <param name="Id">ID of the call</param>
/// <param name="Call_Type">type of call:Take_Care_At_Home/Take_Care_Out/Physiotherapy</param>
/// <param name="Description">details abaut the call</param>
/// <param name="Address">The call address</param>
/// <param name="OpenTime">the time the call was opened volunteer</param>
/// <param name="MaxCloseTime">when the call has to be closed</param>
/// <param name="Distance">the distance between this volunteer to the call address</param>

public class OpenCallInList
{
    public int Id { get; init; }
    public CallType CallType { get; set; }
    required public string Address { get; set; }
    public DateTime OpenTime { get; init; }
    public DateTime? MaxCloseTime { get; set; }
    public double Distance { get; init; }
}