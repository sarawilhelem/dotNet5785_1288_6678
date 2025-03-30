using BlApi;
using BO;
using Helpers;
using System.Globalization;



namespace BlImplementation;

internal class CallImplentation : ICall
{
    private static readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public int[] GetCountsGroupByStatus()
    {

        int[] counts = new int[3];
        var groupedCount = Dal.DataSource.Calls
            .GroupBy(call => call.CallType);

        int index = 0;

        foreach (var group in groupedCount)
        {
            counts[index] = group.Count();
            index++;
        }

        return counts;

    }

    public IEnumerable<BO.CallInList> ReadAll(BO.CallInListFields? filterBy = null, Object? filterParam = null, BO.CallInListFields? sortBy = null)
    {
        IEnumerable<DO.Call> callList = _dal.Call.ReadAll();
        IEnumerable<BO.CallInList> callListtoReturn = callList.Select(
            call => new BO.CallInList
            {
                Id = call.Id,
                CallId = call.Id,
                CallType = (BO.CallType)call.CallType,
                OpenTime = call.OpenTime,
                MaxCloseTime = Helpers.CallManager.RestTimeForCall(call),
                LastVolunteerName = Helpers.CallManager.GetLastVolunteerName(call),
                TotalProcessingTime = Helpers.CallManager.RestTimeForTreatment(call),
                Status = Helpers.CallManager.GetCallStatus(call.Id),
                AmountOfAssignments = Helpers.CallManager.GetAmountOfAssignments(call)
            });
        switch (sortBy)
        {
            case BO.CallInListFields.Id:
                callListtoReturn.OrderBy(call => call.Id);
                break;
            case BO.CallInListFields.OpenTime:
                callListtoReturn.OrderBy(call => call.OpenTime);
                break;
            case BO.CallInListFields.MaxCloseTime:
                callListtoReturn.OrderBy(call => call.MaxCloseTime);
                break;
            case BO.CallInListFields.LastVolunteerName:
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
                case BO.CallInListFields.Id:
                    callListtoReturn = callListtoReturn.Where(call => call.Id.Equals(filterParam)).ToList();
                    break;
                case BO.CallInListFields.OpenTime:
                    callListtoReturn = callListtoReturn.Where(call => call.OpenTime.Equals(filterParam)).ToList();
                    break;
                case BO.CallInListFields.MaxCloseTime:
                    callListtoReturn = callListtoReturn.Where(call => call.MaxCloseTime.Equals(filterParam)).ToList();
                    break;
                case BO.CallInListFields.LastVolunteerName:
                    callListtoReturn = callListtoReturn.Where(call => call.LastVolunteerName.Equals(filterParam)).ToList();
                    break;
                case BO.CallInListFields.CallType:
                    callListtoReturn = callListtoReturn.Where(call => call.CallType.Equals(filterParam)).ToList();
                    break;
                case BO.CallInListFields.Status:
                    callListtoReturn = callListtoReturn.Where(call => call.Status.Equals(filterParam)).ToList();
                    break;
                case BO.CallInListFields.AmountOfAssignments:
                    callListtoReturn = callListtoReturn.Where(call => call.AmountOfAssignments.Equals(filterParam)).ToList();
                    break;
                case BO.CallInListFields.TotalProcessingTime:
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
        var DOCall = _dal.Call.Read(i => i.Id == id);
        if (DOCall == null) return null;
        var assignments = Helpers.CallManager.AssignmentsListForCall(id);
        BO.Call call = new((BO.CallType)(DOCall.CallType), DOCall.Address, DOCall.OpenTime,
            DOCall.MaxCloseTime, DOCall.Description, Helpers.CallManager.GetCallStatus(DOCall.Id),
            DOCall.Latitude, DOCall.Longitude, assignments.Select(
                a => new CallAssignInList(a.VolunteerId, CallManager.GetVolunteer(id).Name, a.OpenTime, a.FinishTime, (BO.FinishType?)(a.FinishType))).ToList()
)
        {
            Id = id,
        };
        return call;
    }
    public void Update(BO.Call call)
    {
        var callToUpdate = new DO.Call
        {
            CallType = (DO.CallType)call.Type,
            Address = call.Address,
            Latitude = call.Latitude,
            Longitude = call.Longitude,
            OpenTime = call.OpenTime,
            MaxCloseTime = call.MaxCloseTime,
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
        if (Helpers.CallManager.AssignmentsListForCall(id) == null && Helpers.CallManager.GetCallStatus(id) == BO.FinishCallType.Open)
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
    public void Create(BO.Call call)
    {
        var callToAdd = new DO.Call
        {
            CallType = (DO.CallType)call.Type,
            Address = call.Address,
            Latitude = call.Latitude,
            Longitude = call.Longitude,
            OpenTime = call.OpenTime,
            MaxCloseTime = call.MaxCloseTime,
            Description = call.Description
        };
        Helpers.CallManager.CreateCall(callToAdd);
    }
    public IEnumerable<BO.ClosedCallInList> ReadAllVolunteerClosedCalls(int volunteerId, BO.CallType? callType = null, BO.ClosedCallInListFields? sort = null)
    {
        var closedCalls = Helpers.CallManager.AssignmentsListForVolunteer(volunteerId).Where(a =>
            Helpers.CallManager.GetCallStatus(a.Id) == BO.FinishCallType.InProcessAtRisk ||
             Helpers.CallManager.GetCallStatus(a.Id) == BO.FinishCallType.InProcess ||
              Helpers.CallManager.GetCallStatus(a.Id) == BO.FinishCallType.Close ||
               Helpers.CallManager.GetCallStatus(a.Id) == BO.FinishCallType.Expired
            ).Select(
                        a => new BO.ClosedCallInList
                        {
                            Id = a.Id,
                            CallType = (BO.CallType)_dal.Call.Read(i => i.Id == a.Id)!.CallType,
                            Address = _dal.Call.Read(i => i.Id == a.Id)!.Address,
                            OpenCallTime = _dal.Call.Read(i => i.Id == a.Id)!.OpenTime,
                            StartCallTime = a.OpenTime,
                            FinishCallTime = a.FinishTime,
                            FinishType = a.FinishType is not null ? (BO.FinishType)a.FinishType : BO.FinishType.Processed
                        }
            );
        if (callType != null)
        {
            switch (callType)
            {
                case BO.CallType.TakeCareAtHome:
                    closedCalls = closedCalls.Where(call => call.CallType == BO.CallType.TakeCareAtHome);
                    break;
                case BO.CallType.TakeCareOut:
                    closedCalls = closedCalls.Where(call => call.CallType == BO.CallType.TakeCareAtHome);
                    break;
                case BO.CallType.Physiotherapy:
                    closedCalls = closedCalls.Where(call => call.CallType == BO.CallType.TakeCareAtHome);
                    break;
                default:
                    break;
            }
        }
        switch (sort)
        {
            case BO.ClosedCallInListFields.FinishCallTime:
                closedCalls = closedCalls.OrderBy(call => call.FinishCallTime).ToList();
                break;
            case BO.ClosedCallInListFields.OpenCallTime:
                closedCalls = closedCalls.OrderBy(call => call.OpenCallTime).ToList();
                break;
            case BO.ClosedCallInListFields.StartCallTime:
                closedCalls = closedCalls.OrderBy(call => call.StartCallTime).ToList();
                break;
            case BO.ClosedCallInListFields.Addres:
                closedCalls = closedCalls.OrderBy(call => call.Address).ToList();
                break;
            default:
                closedCalls = closedCalls.OrderBy(call => call.Id).ToList();
                break;
        }
        return closedCalls;
    }
    public IEnumerable<BO.OpenCallInList> ReadAllVolunteerOpenCalls(int volunteerId, BO.CallType? callType = null, BO.OpenCallInListFields? sort = null)
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
                    CallType = (BO.CallType)(_dal.Call.Read(i => i.Id == a.Id)!.CallType),
                    Address = _dal.Call.Read(i => i.Id == a.Id)!.Address,
                    OpenTime = _dal.Call.Read(i => i.Id == a.Id)!.OpenTime,
                    MaxCloseTime = _dal.Call.Read(i => i.Id == a.Id)!.MaxCloseTime,
                    Distance = Helpers.CallManager.DistanceBetweenVolunteerAndCall(volunteerId, a.Id)
                });
        if (callType != null)
        {
            switch (callType)
            {
                case BO.CallType.TakeCareAtHome:
                    openedCalls = openedCalls.Where(call => call.CallType == BO.CallType.TakeCareAtHome);
                    break;
                case BO.CallType.TakeCareOut:
                    openedCalls = openedCalls.Where(call => call.CallType == BO.CallType.TakeCareAtHome);
                    break;
                case BO.CallType.Physiotherapy:
                    openedCalls = openedCalls.Where(call => call.CallType == BO.CallType.TakeCareAtHome);
                    break;
                default:
                    break;
            }
        }
        switch (sort)
        {
            case BO.OpenCallInListFields.Address:
                openedCalls = openedCalls.OrderBy(call => call.Address).ToList();
                break;
            case BO.OpenCallInListFields.CallType:
                openedCalls = openedCalls.OrderBy(call => call.CallType).ToList();
                break;
            case BO.OpenCallInListFields.OpenTime:
                openedCalls = openedCalls.OrderBy(call => call.OpenTime).ToList();
                break;
            case BO.OpenCallInListFields.MaxCloseTime:
                openedCalls = openedCalls.OrderBy(call => call.MaxCloseTime).ToList();
                break;
            case BO.OpenCallInListFields.Distance:
                openedCalls = openedCalls.OrderBy(call => call.Distance).ToList();
                break;
            default:
                openedCalls = openedCalls.OrderBy(call => call.Id).ToList();
                break;
        }
        return openedCalls;
    }
    public void FinishProcess(int volunteerId, int assignmentId)
    {
        var assignment = _dal.Assignment.Read(a => a.Id == assignmentId) ?? throw new BO.BlDoesNotExistException($"Assignment with id {assignmentId} does not exist");
        if (assignment.VolunteerId == volunteerId && assignment.FinishType == null && assignment.FinishTime == null)
        {
            var assignmentToUpdate = new DO.Assignment
            {
                Id = assignmentId,
                CallId = assignment.Id,
                VolunteerId = volunteerId,
                OpenTime = assignment.OpenTime,
                FinishTime = DateTime.Now,
                FinishType = DO.FinishType.Processed
            };
            Helpers.AssignmentManager.UpdateAssignment(assignmentToUpdate);
        }
        else
        {
            throw new Exception("לא ניתן לסיים את הטיפול");
        }

    }
    public void CanceleProcess(int volunteerId, int assignmentId)
    {
        var volunteer = _dal.Volunteer.Read(v => v.Id == volunteerId) ??
            throw new BO.BlDoesNotExistException($"Volunteer with id {volunteerId} does not exists");
        var assignment = _dal.Assignment.Read(a => a.Id == assignmentId) ??
            throw new BO.BlDoesNotExistException($"assignment with id {assignmentId} does not exists");
        if (volunteer.Role == DO.Role.Manager || assignment.VolunteerId == volunteerId)
        {
            var finishType = volunteer.Role == DO.Role.Manager ? DO.FinishType.ManagerCancel : DO.FinishType.SelfCancel;
            if (assignment.FinishType == null && assignment.FinishTime == null)
            {
                var newAssignment = new DO.Assignment
                {
                    Id = assignment.Id,
                    VolunteerId = volunteerId,
                    CallId = assignment.Id,
                    OpenTime = assignment.OpenTime,
                    FinishTime = DateTime.Now,
                    FinishType = finishType
                };
                try
                {
                    Helpers.AssignmentManager.UpdateAssignment(newAssignment);
                }
                catch (Exception e)
                {
                    throw new Exception(" לא קיימת כזו הקצאה במערכת");
                }
            }
            else
            {
                throw new Exception("הביטול לא חוקי");
            }
        }
    }
    public void ChooseCall(int volunteerId, int callId)
    {
        var assignments = Helpers.CallManager.AssignmentsListForCall(callId).Where(a => a.FinishType == null);
        var call = _dal.Call.Read(call => call.Id == callId) ??
            throw new BO.BlDoesNotExistException($"Volunteer with id {volunteerId} does not exists");
        if (assignments != null && Helpers.CallManager.RestTimeForCall(call) != null)
        {
            Helpers.AssignmentManager.CreateAssignment(callId, volunteerId);
        }
        else
        {
            throw new Exception("הבקשה לא  חוקית- פג תוקף או...");
        }

    }
}


