using BlApi;
using Helpers;

namespace BlImplementation;

internal class CallImplentation:ICall
{
    public int[] GetCountsGroupByStatus() 
    {
    
        int[] counts = new int[3];
        var groupedCount = Dal.DataSource.Calls
            .GroupBy(call => call.Call_Type);
        
        //foreach (var call in groupedCount)
        //{
        //    counts[call.Key]=call.legnth
        //}
    
    }

    public IEnumerable<BO.CallInList> ReadAll(BO.Call_In_List_Fields? filterBy = null, Object? filterParam = null, BO.Call_In_List_Fields? sortBy = null)
    {
        IEnumerable<BO.CallInList> callList= CallManager.ReadAll();
       
        if (filterParam!=null)
        {
             
            if (filterBy != null)
            {
                var propertyInfo = typeof(BO.CallInList).GetProperty(filterBy.ToString());
                if (propertyInfo != null)
                {
                   
                    callList = callList.OrderBy(call => propertyInfo.GetValue(call)).ToList();
                }
        }
       
    }
//    public BO.Call? Read(int id);
//    public void Update(int id, BO.Call call);
 //   public void Delete(int id);
 //   public void Add(BO.Volunteer volunteer);
 //   public IEnumerable<BO.Call> ReadAllVolunteerClosedCalls(int VolunteerId, BO.Call_Type? callType = null, BO.Closed_Call_In_List_Fields? sort = null);
  //  public IEnumerable<BO.OpenCallInList> ReadAllVolunteerOpenCalls(int VolunteerId, BO.Call_Type? callType = null, BO.Open_Call_In_List_Fields? sort = null);
//    public void FinishProcess(int volunteerId, int assignmentId);
 //   public void CanceleProcess(int volunteerId, int assignmentId);
 //   public void ChooseCall(int volunteerId, int callId);
}
