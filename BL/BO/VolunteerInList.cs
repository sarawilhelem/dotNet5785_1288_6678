namespace BO;

/// <summary>
/// Volunteer in list
/// </summary> 
/// <param name=" Id "> Personal unique ID of the volunteer  </param> 
/// <param name=" Name "> Private Name of the volunteer </param> 
/// <param name=" IsActive ">if volunteer is active he can procces</param>
/// <param name=" NumCallsHandle ">how many calls this volunteer handled</param>
/// <param name=" NumCallsCancel ">how many calls this volunteer canceled</param>
/// <param name=" NumCallsNotValid">how many calls assigned to this volunteer and expired</param>
/// <param name=" CallId">a call id the volunteer process now</param>
/// <param name=" CallType">the call type the volunteer process now</param>

public class VolunteerInList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public int NumCallsHandle { get; set; }
    public int NumCallsCancel { get; set; }
    public int NumCallsNotValid { get; set; }
    public int? CallId { get; set; }
    public CallType? CallType { get; set; }
    public VolunteerInList(int id, string name, bool isActive, int numCallsHandle, int numCallsCancel, int numCallsNotValid, int? callId, CallType? callType)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        NumCallsHandle = numCallsHandle;
        NumCallsCancel = numCallsCancel;
        NumCallsNotValid = numCallsNotValid;
        CallId = callId;
        CallType = callType;
    }
    public override string ToString() => Helpers.Tools.ToStringProperty(this);

}