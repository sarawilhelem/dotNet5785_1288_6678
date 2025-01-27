
namespace BlApi;

public interface ICall
{
    public int[] GetCountsGroupByStatus();
    public IEnumerable<BO.CallInList> ReadAll(BO.Call_In_List_Fields? sort=null , Object? findVal = null, BO.Call_In_List_Fields? findField=null);
    public BO.Call? Read(int id);
    public void Update(int id, BO.Call call);
    public void Delete(int id);
    public void Add(BO.Volunteer volunteer);
    public IEnumerable<BO.Call> ReadAllVolunteerClosedCalls(int VolunteerId, BO.Call_Type? callType = null, BO.Closed_Call_In_List_Fields? sort=null);
    public IEnumerable<BO.OpenCallInList> ReadAllVolunteerOpenCalls(int VolunteerId, BO.Call_Type? callType = null, BO.Open_Call_In_List_Fields? sort = null);
    public void FinishProcess(int volunteerId,int assignmentId);
    public void CanceleProcess(int volunteerId, int assignmentId);
    public void ChooseCall(int volunteerId, int callId);


}
