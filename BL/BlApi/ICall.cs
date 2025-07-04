﻿
namespace BlApi;

public interface ICall: IObservable
{
    public List<Tuple<string, int>> GetCountsGroupByStatus();
    public IEnumerable<BO.CallInList> ReadAll(BO.CallInListFields? sort=null , Object? findVal = null, BO.CallInListFields? findField=null);
    public BO.Call? Read(int id);
    public Task Update( BO.Call call);
    public void Delete(int id);
    public Task Create(BO.Call call);
    public IEnumerable<BO.ClosedCallInList> ReadAllVolunteerClosedCalls(int VolunteerId, BO.CallType? callType = null, BO.ClosedCallInListFields? sort=null);
    public IEnumerable<BO.OpenCallInList> ReadAllVolunteerOpenCalls(int VolunteerId, BO.CallType? callType = null, BO.OpenCallInListFields? sort = null);
    public void FinishProcess(int volunteerId,int assignmentId);
    public void CancelProcess(int userId, int assignmentId);
    public void ChooseCall(int volunteerId, int callId);
    string? GetDirectionsLink(string? startAddress, string endAddress);
}
