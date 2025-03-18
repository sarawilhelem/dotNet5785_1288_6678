using BlApi;
using BO;
using DO;
using System.Collections.Generic;
using System.Net;


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
                Status = Helpers.CallManager.GetCallStatus(call),
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
            Status = Helpers.CallManager.GetCallStatus(DOCall),
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
}


