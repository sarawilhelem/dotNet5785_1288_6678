using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
/// <summary>
/// Details of call that accepted
/// </summary>
///<param name="Id">Id of call</param>
///<param name="Type">type of call:TakeCareAtHome/TakeCareOut/Physiotherapy</param>
///<param name="Description">details about the call</param>
///<param name="Address">to where do they want volunteer</param>
/// <param name="Latitude">vertical distance from the equator</param>
/// <param name="Longitude">horizontal distance from the equator</param>
/// <param name="OpenTime">the time they call a volunteer</param>
/// <param name="MaxCloseTime">when it has to be closed</param>
///<param name="Status"></param>
///<param name="CallAssignList">list that keep any assigment that accepted from volunteer</param>




public class Call
{
    public int Id { get; init; }
    public BO.CallType Type { get; set; }
    public string? Description { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime OpenTime { get; init; }
    public DateTime MaxCloseTime { get; set; }
    public FinishCallType Status { get; set; }
    public List<BO.CallAssignInList>? CallAssignList { get; set; }
    public Call(BO.CallType type = CallType.All, string address ="", DateTime openTime = default(DateTime), DateTime maxCloseTime = default(DateTime),
        string? description = null, FinishCallType status = FinishCallType.Open,
        double latitude = 0, double longitude = 0, List<BO.CallAssignInList>? callAssignList = null)
    {
        Type = type;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
        OpenTime = openTime;
        MaxCloseTime = maxCloseTime;
        Status = status;
        CallAssignList = callAssignList;
        Description = description;
    }
    public override string ToString() => Helpers.Tools.ToStringProperty(this);
}