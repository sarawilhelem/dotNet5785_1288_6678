
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
    public Call_Type? Call_Type { get; set; }
    public VolunteerInList(int id, string name, bool isActive, int numCallsHandle, int numCallsCancel, int numCallsNotValid, int? callId, Call_Type? callType)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        NumCallsHandle = numCallsHandle;
        NumCallsCancel = numCallsCancel;
        NumCallsNotValid = numCallsNotValid;
        CallId = callId;
        Call_Type = callType;
    }
}