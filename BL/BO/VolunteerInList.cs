
namespace BO;

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