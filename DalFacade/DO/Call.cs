
namespace DO;

/// <summary>
/// Call Entity
/// </summary>
/// <param name="Id">unique ID (created automatically)</param>
/// <param name="Call_Type">type of call:Take_Care_At_Home/Take_Care_Out/Physiotherapy</param>
/// <param name="Address">to where do they want volunteer</param>
/// <param name="Latitude">vertical distance from the equator</param>
/// <param name="Longitude">horizontal distance from the equator</param>
/// <param name="OpenTime">the time they call a volunteer</param>
/// <param name="Description">details abaut the call</param>
/// <param name="MaxCloseTime">when it has to be closed</param>
public record Call
(

 Call_Type Call_Type,
 string Address,
 double Latitude,
 double Longitude,
 DateTime OpenTime,
 DateTime MaxCloseTime,
 string? Description = null

)
{
    public int Id { get; init; }
    public Call() : this(Call_Type.Take_Care_At_Home, "Zayit 1, Jerusalem, Israel", 0, 0, DateTime.Now, DateTime.Now, "no more details")
    { }
}