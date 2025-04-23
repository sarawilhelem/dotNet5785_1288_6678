using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// Details of call assigned in list
/// </summary>
///<param name="VolunteerId">Id of volunteer</param>
///<param name="Name">volunteerName</param>
///<param name="Insersion">the date of the first time this volunteer chose this call</param>
///<param name="FinishTime">the time the assignment finished</param>
/// <param name="FinishType">the reason the assinment finished</param>

public class CallAssignInList
{
    public int? VolunteerId {  get; set; }
    public string? Name { get; set; }
    public DateTime Insersion {  get; set; }
    public DateTime? FinishTime {  get; set; }
    public FinishType? FinishType { get; set; }

    public CallAssignInList(int? volunteerId, string? name, DateTime insersion, DateTime? finishTime, FinishType? finishType)
    {
        VolunteerId = volunteerId;
        Name = name;
        Insersion = insersion;
        FinishTime = finishTime;
        FinishType = finishType;
    }
    public override string ToString() => Helpers.Tools.ToStringProperty(this);

}
