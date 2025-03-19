using BlApi;
using System.Globalization;



namespace BlImplementation;

internal class CallImplentation : ICall
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
                Status = Helpers.CallManager.GetCallStatus(call.Id),
                AmountOfAssignments = Helpers.CallManager.GetAmountOfAssignments(call)
            });
        switch (sortBy)
        {
            case BO.Call_In_List_Fields.Id:
                callListtoReturn.OrderBy(call => call.Id);
                break;
            case BO.Call_In_List_Fields.OpenTime:
                callListtoReturn.OrderBy(call => call.OpenTime);
                break;
            case BO.Call_In_List_Fields.MaxCloseTime:
                callListtoReturn.OrderBy(call => call.MaxCloseTime);
                break;
            case BO.Call_In_List_Fields.LastVolunteerName:
                callListtoReturn.OrderBy(call => call.LastVolunteerName);
                break;
            default:
                callListtoReturn.OrderBy(call => call.CallId);
                break;
        }
        if (filterParam != null)
        {
            switch (filterBy)
            {
                case BO.Call_In_List_Fields.Id:
                    callListtoReturn= callListtoReturn.Where(call => call.Id.Equals(filterParam)).ToList();
                    break;
                case BO.Call_In_List_Fields.OpenTime:
                    callListtoReturn = callListtoReturn.Where(call => call.OpenTime.Equals(filterParam)).ToList();
                    break;
                case BO.Call_In_List_Fields.MaxCloseTime:
                    callListtoReturn = callListtoReturn.Where(call => call.MaxCloseTime.Equals(filterParam)).ToList();
                    break;
                case BO.Call_In_List_Fields.LastVolunteerName:
                    callListtoReturn = callListtoReturn.Where(call => call.LastVolunteerName.Equals(filterParam)).ToList();
                    break;
                case BO.Call_In_List_Fields.CallType:
                    callListtoReturn = callListtoReturn.Where(call => call.CallType.Equals(filterParam)).ToList();
                    break;
                case BO.Call_In_List_Fields.Status:
                    callListtoReturn = callListtoReturn.Where(call => call.Status.Equals(filterParam)).ToList();
                    break;
                case BO.Call_In_List_Fields.AmountOfAssignments:
                    callListtoReturn = callListtoReturn.Where(call => call.AmountOfAssignments.Equals(filterParam)).ToList();
                    break;
                case BO.Call_In_List_Fields.TotalProcessingTime:
                    callListtoReturn = callListtoReturn.Where(call => call.TotalProcessingTime.Equals(filterParam)).ToList();
                    break;
                default:
                    break;
            }
        }
            return callListtoReturn;
    }
    public BO.Call? Read(int id)
    {
       var DOCall=_dal.Call.Read(i=>i.Id==id);
        if (DOCall==null) return null;
        var assignments = Helpers.CallManager.AssignmentsListForCall(id);
        BO.Call call = new BO.Call
        {
            Id = id,
            Type=(BO.Call_Type)(DOCall.Call_Type),
            Address=DOCall.Address,
            Latitude=DOCall.Latitude,
            Longitude=DOCall.Longitude,
            OpenTime=DOCall.OpenTime,
            MaxCloseTime=DOCall.MaxCloseTime,
            Status = Helpers.CallManager.GetCallStatus(DOCall.Id),
            CallAssignList = assignments.Select(
            a => new BO.CallAssignInList 
            {
                VolunteerId=a.VolunteerId,
                Name=Helpers.CallManager.GetVolunteer(id).Name,
                Insersion =a.OpenTime,
                FinishTime=a.FinishTime,
                FinishType=(BO.FinishType)a.FinishType
            })
        };
        return call;
    }
    public void Update(BO.Call call)
    {
        var callToUpdate = new DO.Call
        {
              Call_Type=(DO.Call_Type)call.Type,
              Address=call.Address,
              Latitude=call.Latitude,
              Longitude=call.Longitude,
              OpenTime=call.OpenTime,
              MaxCloseTime=call.MaxCloseTime,
              Description = call.Description
        };
        try
        {
            Helpers.CallManager.UpdateCall(callToUpdate);
        }
        catch (Exception e)
        {
            throw new Exception("לא קיימת כזו קריאה");
        }
    }
    public void Delete(int id)
    {
        if(Helpers.CallManager.AssignmentsListForCall(id)==null&& Helpers.CallManager.GetCallStatus(id)==BO.FinishCallType.Open)
        {
            try
            {
                Helpers.CallManager.DeleteCall(id);
            }
            catch (Exception e)
            {
                throw new Exception("לא קיימת כזו קריאה");
            }

        }
        else
        {
            throw new Exception("לא ניתן למחוק את הקריאה");
        }
    }
    public void Add(BO.Call call)
    {
        var callToAdd = new DO.Call
        {
            Call_Type = (DO.Call_Type)call.Type,
            Address = call.Address,
            Latitude = call.Latitude,
            Longitude = call.Longitude,
            OpenTime = call.OpenTime,
            MaxCloseTime = call.MaxCloseTime,
            Description = call.Description
        };
        Helpers.CallManager.CreateCall(callToAdd);
    }
    public IEnumerable<BO.ClosedCallInList> ReadAllVolunteerClosedCalls(int volunteerId, BO.Call_Type? callType = null, BO.Closed_Call_In_List_Fields? sort = null)
    {
        var closedCalls =Helpers.CallManager.AssignmentsListForVolunteer(volunteerId).Where(a=>
            Helpers.CallManager.GetCallStatus(a.Id)==BO.FinishCallType.InProcessAtRisk||
             Helpers.CallManager.GetCallStatus(a.Id) == BO.FinishCallType.InProcess||
              Helpers.CallManager.GetCallStatus(a.Id) == BO.FinishCallType.Close||
               Helpers.CallManager.GetCallStatus(a.Id) == BO.FinishCallType.Expired
            ).Select(
                        a => new BO.ClosedCallInList
                        {
                            Id = a.Id,
                            CallType = (BO.Call_Type)(_dal.Call.Read(i=>i.Id==a.Id).Call_Type),
                            Address = _dal.Call.Read(i => i.Id == a.Id).Address,
                            OpenCallTime =_dal.Call.Read(i => i.Id == a.Id).OpenTime,
                            StartCallTime = a.OpenTime,
                            FinishCallTime = a.FinishTime,
                            Finish_Type = a.FinishType
                        }
            );
        if (callType!=null)
        {
            switch (callType)
            {
                case BO.Call_Type.Take_Care_At_Home:
                    closedCalls = closedCalls.Where(call => call.CallType == BO.Call_Type.Take_Care_At_Home);
                    break;
                case BO.Call_Type.Take_Care_Out:
                    closedCalls = closedCalls.Where(call => call.CallType == BO.Call_Type.Take_Care_At_Home);
                    break;
                case BO.Call_Type.Physiotherapy:
                    closedCalls = closedCalls.Where(call => call.CallType == BO.Call_Type.Take_Care_At_Home);
                    break;
                    default:
                    break;
            }
        }
        switch (sort)
        {
            case BO.Closed_Call_In_List_Fields.FinishCallTime:
                closedCalls = closedCalls.OrderBy(call => call.FinishCallTime).ToList();
                break;
            case BO.Closed_Call_In_List_Fields.OpenCallTime:
                closedCalls = closedCalls.OrderBy(call => call.OpenCallTime).ToList();
                break;
            case BO.Closed_Call_In_List_Fields.StartCallTime:
                closedCalls = closedCalls.OrderBy(call => call.StartCallTime).ToList();
                break;
            case BO.Closed_Call_In_List_Fields.Addres:
                closedCalls = closedCalls.OrderBy(call => call.Address).ToList();
                break;
            default:
                closedCalls = closedCalls.OrderBy(call => call.Id).ToList();
                break;
        }
        return closedCalls;
    }
    public IEnumerable<BO.OpenCallInList> ReadAllVolunteerOpenCalls(int volunteerId, BO.Call_Type? callType = null, BO.Open_Call_In_List_Fields? sort = null)
    {
        var openedCalls = Helpers.CallManager.AssignmentsListForVolunteer(volunteerId).Where(a =>
    Helpers.CallManager.GetCallStatus(a.Id) == BO.FinishCallType.InProcessAtRisk ||
     Helpers.CallManager.GetCallStatus(a.Id) == BO.FinishCallType.InProcess ||
      Helpers.CallManager.GetCallStatus(a.Id) == BO.FinishCallType.Close ||
       Helpers.CallManager.GetCallStatus(a.Id) == BO.FinishCallType.Expired
    ).Select(
                a => new BO.OpenCallInList
                {
                    Id = a.Id,
                    CallType = (BO.Call_Type)(_dal.Call.Read(i => i.Id == a.Id).Call_Type),
                    Address = _dal.Call.Read(i => i.Id == a.Id).Address,
                    OpenTime = _dal.Call.Read(i => i.Id == a.Id).OpenTime,
                    MaxCloseTime = _dal.Call.Read(i => i.Id == a.Id).MaxCloseTime,
                    Distance = Helpers.CallManager.DistanceBetweenVolunteerAndCall(volunteerId, a.Id)
                }
    //public int Id { get; init; }
    //public Call_Type CallType { get; set; }
    //required public string Address { get; set; }
    //public DateTime OpenTime { get; init; }
    //public DateTime? MaxCloseTime { get; set; }
    //public double Distance { get; init; }

    );
    }
}


