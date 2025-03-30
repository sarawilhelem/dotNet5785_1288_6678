
namespace BlApi;

public interface ICall
{
    public int[] GetCountsGroupByStatus();
    public IEnumerable<BO.CallInList> ReadAll(BO.CallInListFields? sort=null , Object? findVal = null, BO.CallInListFields? findField=null);
    public BO.Call? Read(int id);
    public void Update( BO.Call call);
    public void Delete(int id);
    public void Create(BO.Call call);
    public IEnumerable<BO.ClosedCallInList> ReadAllVolunteerClosedCalls(int VolunteerId, BO.CallType? callType = null, BO.ClosedCallInListFields? sort=null);
    public IEnumerable<BO.OpenCallInList> ReadAllVolunteerOpenCalls(int VolunteerId, BO.CallType? callType = null, BO.OpenCallInListFields? sort = null);
    public void FinishProcess(int volunteerId,int assignmentId);
    public void CanceleProcess(int volunteerId, int assignmentId);
    public void ChooseCall(int volunteerId, int callId);
}
