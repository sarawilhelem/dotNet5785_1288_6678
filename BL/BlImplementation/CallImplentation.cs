using BlApi;


using Helpers;

namespace BlImplementation;

internal class CallImplentation:ICall
{
    private static readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public int[] GetCountsGroupByStatus() 
    {
    
        int[] counts = new int[3];
        var groupedCount = Dal.DataSource.Calls
            .GroupBy(call => call.Call_Type);

        int index = 0;

        foreach (var group in groupedCount)
        {
            counts[index] = group.Count();
            index++;
        }

        return counts;

    }

    public IEnumerable<BO.CallInList> ReadAll(BO.Call_In_List_Fields? filterBy = null, Object? filterParam = null, BO.Call_In_List_Fields? sortBy = null)
    {
        IEnumerable<DO.Call> callList = _dal.Call.ReadAll();
        IEnumerable<BO.CallInList> callListtoReturn = callList.Select(
            call => new BO.CallInList
            {
                Id = call.Id,
                CallId = call.Id,
                CallType = (BO.Call_Type)call.Call_Type,
                OpenTime = call.OpenTime,
                MaxCloseTime = Helpers.CallManager.RestTimeForCall(call),
                LastVolunteerName = Helpers.CallManager.GetLastVolunteerName(call),
                TotalProcessingTime = Helpers.CallManager.RestTimeForTreatment(call),
                Status = Helpers.CallManager.GetCallStatus(call),
                AmountOfAssignments = Helpers.CallManager.GetAmountOfAssignments(call)
            });

}

        //if (filterParam != null)
        //{
        //    if (filterBy != null)
        //    {
        //        var propertyInfo = typeof(BO.CallInList).GetProperty(filterBy.ToString());
        //        if (propertyInfo != null)
        //        {
        //            callList = _dal.Call.ReadAll(call=>call);
        //            //  callList = callList.Where(call => propertyInfo.GetValue(call)).ToList();
        //        }
        //    }
        //}
        //else
        //{
        //    callList = _dal.Call.ReadAll();           
        //}
    }
    public BO.Call? Read(int id)
    {
       // var list=_dal.Call.Read(i=>i.Id==id);
        return null;
    //        public int Id { get; init; }
    //public Call_Type Type { get; set; }
    //public string? Description { get; set; }
    //public string Address { get; set; }
    //public double Latitude { get; set; }
    //public double Longitude { get; set; }
    //public DateTime OpenTime { get; init; }
    //public DateTime MaxCloseTime { get; set; }
    //public Finish_Call_Type Status { get; set; }
    //public List<BO.CallAssignInList>? CallAssignList { get; set; }
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
