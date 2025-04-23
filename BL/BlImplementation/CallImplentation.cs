using BlApi;
using BO;
using Helpers;
using System;
using System.Globalization;



namespace BlImplementation;

internal class CallImplentation : ICall
{
    /// <summary>
    /// static field to access do crud
    /// </summary>
    private static readonly DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// read all calls and calculate the number of times each status in the calls
    /// </summary>
    /// <returns>an array of the count statuses</returns>
    public int[] GetCountsGroupByStatus()
    {
        var calls = ReadAll();

        int[] statusCounts = new int[Enum.GetNames(typeof(BO.FinishCallType)).Length];

        var counts =
         (from call in calls
          group call by call.Status into g
          select new { Status = g.Key, Count = g.Count() });

        foreach (var count in counts)
        {
            statusCounts[(int)count.Status] = count.Count;
        }

        return statusCounts;
    }

    /// <summary>
    /// read all calls, filtered and sorted according to the params
    /// </summary>
    /// <param name="filterBy">enum value to choose the field to filter</param>
    /// <param name="filterParam">value to filter the field in Object type</param>
    /// <param name="sortBy">enum value to choose the field to sort by it</param>
    /// <returns>all calls filtered and sorted</returns>
    /// <exception cref="BO.BlIllegalValues">if filterParam is not suitable to filterBy type</exception>
    public IEnumerable<BO.CallInList> ReadAll(BO.CallInListFields? filterBy = null, Object? filterParam = null, BO.CallInListFields? sortBy = null)
    {

        IEnumerable<DO.Call> callList = _dal.Call.ReadAll();
        IEnumerable<DO.Assignment> assinments = _dal.Assignment.ReadAll();
        IEnumerable<BO.CallInList> callsListToReturn = callList.Select(
            call =>
            {
                var lastAssignment = assinments.Where(a => a.CallId == call.Id).
                OrderByDescending(a=>a.OpenTime).FirstOrDefault();
                int? id;
                if (lastAssignment == null)
                    id = null;
                else
                    id = lastAssignment.Id;
                return new BO.CallInList
                {
                    Id = id,
                    CallId = call.Id,
                    CallType = (BO.CallType)call.CallType,
                    OpenTime = call.OpenTime,
                    MaxCloseTime = Helpers.CallManager.RestTimeForCall(call),
                    LastVolunteerName = lastAssignment is not null? _dal.Volunteer.Read(lastAssignment.VolunteerId)!.Name : null,
                    TotalProcessingTime = Helpers.CallManager.RestTimeForTreatment(call),
                    Status = Helpers.CallManager.GetCallStatus(call.Id),
                    AmountOfAssignments = Helpers.CallManager.GetAmountOfAssignments(call)
                };
            });
        try
        {
            if (filterParam != null && filterParam.ToString() != "")
            {
                switch (filterBy)
                {
                    case BO.CallInListFields.Id:
                        callsListToReturn = callsListToReturn.Where(call => call.Id is not null && call.Id!.Equals(Convert.ToInt32(filterParam))).ToList();
                        break;
                    case BO.CallInListFields.CallId:
                        callsListToReturn = callsListToReturn.Where(call => call.CallId.Equals(Convert.ToInt32(filterParam))).ToList();
                        break;
                    case BO.CallInListFields.OpenTime:
                        callsListToReturn = callsListToReturn.Where(call => call.OpenTime.Equals(Convert.ToDateTime(filterParam))).ToList();
                        break;
                    case BO.CallInListFields.MaxCloseTime:
                        callsListToReturn = callsListToReturn.Where(call => call.MaxCloseTime.Equals(Convert.ToDateTime(filterParam))).ToList();
                        break;
                    case BO.CallInListFields.LastVolunteerName:
                        callsListToReturn = callsListToReturn.Where(call => call.LastVolunteerName is not null && call.LastVolunteerName!.Equals(filterParam.ToString())).ToList();
                        break;
                    case BO.CallInListFields.CallType:
                        if (filterParam is string strCallTypeParam)
                        {
                            if (Enum.TryParse<BO.CallType>(strCallTypeParam, true, out BO.CallType callTypeParam))
                            {
                                callsListToReturn = callsListToReturn.Where(call => call.CallType.Equals(callTypeParam)).ToList();
                            }
                            else
                                throw new BO.BlIllegalValues($"{strCallTypeParam} is not a call type");
                        }
                        else
                            throw new BO.BlIllegalValues("Can't read call type");
                        break;
                    case BO.CallInListFields.Status:
                        if (filterParam is string strFieldParam)
                        {
                            if (Enum.TryParse<BO.FinishCallType>(strFieldParam, true, out BO.FinishCallType statusParam))
                            {
                                callsListToReturn = callsListToReturn.Where(call => call.Status.Equals(statusParam)).ToList();
                            }
                            else
                                throw new BO.BlIllegalValues($"{strFieldParam} is not a status");
                        }
                        else
                            throw new BO.BlIllegalValues($"Can't read status");
                        break;
                    case BO.CallInListFields.AmountOfAssignments:
                        callsListToReturn = callsListToReturn.Where(call => call.AmountOfAssignments.Equals(Convert.ToInt32(filterParam))).ToList();
                        break;
                    case BO.CallInListFields.TotalProcessingTime:
                        callsListToReturn = callsListToReturn.Where(call => call.TotalProcessingTime is not null
                            && call.TotalProcessingTime!.Equals(CallManager.ConvertToTimeSpan(filterParam!))).ToList();
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            throw new BO.BlIllegalValues(ex.Message);
        }
        if (sortBy != null)
           switch (sortBy)
            {
                case BO.CallInListFields.Id:
                    callsListToReturn = callsListToReturn.OrderBy(call => call.Id);
                    break;
                case BO.CallInListFields.CallId:
                    callsListToReturn = callsListToReturn.OrderBy(call => call.CallId);
                    break;
                case BO.CallInListFields.CallType:
                    callsListToReturn = callsListToReturn.OrderBy(call => call.CallType);
                    break;
                case BO.CallInListFields.OpenTime:
                    callsListToReturn = callsListToReturn.OrderBy(call => call.OpenTime);
                    break;
                case BO.CallInListFields.MaxCloseTime:
                    callsListToReturn = callsListToReturn.OrderBy(call => call.MaxCloseTime);
                    break;
                case BO.CallInListFields.LastVolunteerName:
                    callsListToReturn = callsListToReturn.OrderBy(call => call.LastVolunteerName);
                    break;
                case BO.CallInListFields.TotalProcessingTime:
                    callsListToReturn = callsListToReturn.OrderBy(call => call.TotalProcessingTime);
                    break;
                case BO.CallInListFields.Status:
                    callsListToReturn = callsListToReturn.OrderBy(call => call.Status);
                    break;
                case BO.CallInListFields.AmountOfAssignments:
                    callsListToReturn = callsListToReturn.OrderBy(call => call.AmountOfAssignments);
                    break;
            }
        
        return callsListToReturn;
    }
    
    /// <summary>
    /// read a call
    /// </summary>
    /// <param name="id">the call id to read</param>
    /// <returns>The call with that id</returns>
    public BO.Call? Read(int id)
    {
        var DOCall = _dal.Call.Read(i => i.Id == id);
        if (DOCall == null) return null;
        var assignments = Helpers.CallManager.AssignmentsListForCall(id);
        BO.Call call = new((BO.CallType)(DOCall.CallType), DOCall.Address, DOCall.OpenTime,
            DOCall.MaxCloseTime, DOCall.Description, Helpers.CallManager.GetCallStatus(DOCall.Id),
            DOCall.Latitude, DOCall.Longitude, assignments.Select(
                a => new CallAssignInList(a.VolunteerId, CallManager.GetVolunteer(a.VolunteerId).Name, a.OpenTime, a.FinishTime, (BO.FinishType?)(a.FinishType))).ToList()
)
        {
            Id = id,
        };
        return call;
    }
    
    /// <summary>
    /// update a call
    /// </summary>
    /// <param name="call">the updated call</param>
    /// <exception cref="BO.BlIllegalDatesOrder">if the dates order in the updated call is illegal</exception>
    /// <exception cref="BO.BlIllegalValues">if a field in the updated call is not legal</exception>
    /// <exception cref="BO.BlCoordinatesException">if there is an exception when calculate the lat and lon</exception>
    /// <exception cref="BO.BlDoesNotExistException">if there is not a call with that id</exception>
    public  void Update(BO.Call call)
    {
        if (call.OpenTime > call.MaxCloseTime || call.MaxCloseTime < ClockManager.Now)
            throw new BO.BlIllegalDatesOrder("MaxCloseTime can't be before now or open time");
        if (call == null)
            throw new BO.BlIllegalValues("Address can not be null");
        var (latitude, longitude) =  Tools.GetCoordinates(call.Address);
        var callToUpdate = new DO.Call
        {
            CallType = (DO.CallType)call.Type,
            Address = call.Address,
            Latitude = latitude ?? throw new BO.BlCoordinatesException("Can't approach the address"),
            Longitude = longitude ?? throw new BO.BlCoordinatesException("Can't approach the address"),
            OpenTime = call.OpenTime,
            MaxCloseTime = call.MaxCloseTime,
            Description = call.Description,
            Id = call.Id
        };
        try
        {
            _dal.Call.Update(callToUpdate);
        }
        catch
        {
            throw new BO.BlDoesNotExistException("That call does not exists");
        }
    }

    /// <summary>
    /// delete a call
    /// </summary>
    /// <param name="id">the id of the call which has to be deleted</param>
    /// <exception cref="BO.BlDeleteImpossible">if there is not a call with that id or call is not open or assigned to a volunteer</exception>
    public void Delete(int id)
    {
        if (_dal.Call.Read(id) == null)
            throw new BO.BlDoesNotExistException($"Call with id {id} does not exists");
        if (!Helpers.CallManager.AssignmentsListForCall(id).Any() && Helpers.CallManager.GetCallStatus(id) == BO.FinishCallType.Open)
        {
            try
            {
                _dal.Call.Delete(id);
            }
            catch
            {
                throw new BO.BlDeleteImpossible($"Call with id {id} does not exists");
            }

        }
        else
        {
            throw new BO.BlDeleteImpossible("Call is not Open or assigned to a volunteer");
        }
    }

    /// <summary>
    /// add a new call to the db
    /// </summary>
    /// <param name="call">the new call to add</param>
    /// <exception cref="BO.BlIllegalValues">if a field in the new call is not legal</exception>
    /// <exception cref="BO.BlIllegalDatesOrder">if the dates order in the new call is illegal</exception>
    /// <exception cref="BO.BlCoordinatesException">if there is an exception when calculate the lat and lon</exception>
    public  void Create(BO.Call call)
    {
        if (call.Address is null)
            throw new BO.BlIllegalValues("address can not be null");
        if (call.OpenTime > call.MaxCloseTime || call.MaxCloseTime < ClockManager.Now)
            throw new BO.BlIllegalDatesOrder("MaxCloseTime can't be before now or open time");
        var (latitude, longitude) =  Tools.GetCoordinates(call.Address);
        var callToAdd = new DO.Call
        {
            CallType = (DO.CallType)call.Type,
            Address = call.Address,
            Latitude = latitude ?? throw new BO.BlCoordinatesException("Can't approch address"),
            Longitude = longitude ?? throw new BO.BlCoordinatesException("Can't approch address"),
            OpenTime = Helpers.ClockManager.Now,
            MaxCloseTime = call.MaxCloseTime,
            Description = call.Description
        };
        _dal.Call.Create(callToAdd);
    }

    /// <summary>
    /// read all closed calls which this volunteer had an assignment with them
    /// </summary>
    /// <param name="volunteerId">the volunteer id</param>
    /// <param name="callType">calls with which calltpye to return, can be null and then all the calls will return</param>
    /// <param name="sort">enum value to choose by which field to sort the calls</param>
    /// <returns>the closed calls</returns>
    public IEnumerable<BO.ClosedCallInList> ReadAllVolunteerClosedCalls(int volunteerId, BO.CallType? callType = null, BO.ClosedCallInListFields? sort = null)
    {

        var closedCalls = Helpers.CallManager.AssignmentsListForVolunteer(volunteerId).Where(a => {
            var status = Helpers.CallManager.GetCallStatus(a.CallId);
            return
            status == BO.FinishCallType.InProcessInRisk ||
             status == BO.FinishCallType.InProcess ||
              status == BO.FinishCallType.Close ||
               status == BO.FinishCallType.Expired;
            }).Select(
                        a => new BO.ClosedCallInList
                        {
                            Id = a.CallId,
                            CallType = (BO.CallType)_dal.Call.Read(c => c.Id == a.CallId)!.CallType,
                            Address = _dal.Call.Read(c => c.Id == a.CallId)!.Address,
                            OpenCallTime = _dal.Call.Read(c => c.Id == a.CallId)!.OpenTime,
                            StartCallTime = a.OpenTime,
                            FinishCallTime = a.FinishTime,
                            FinishType = a.FinishType is not null ? (BO.FinishType)a.FinishType : BO.FinishType.Processed
                        }
            );
        if (callType != null)
            closedCalls = closedCalls.Where(call => call.CallType == callType);
        closedCalls = sort switch
        {
            BO.ClosedCallInListFields.FinishCallTime => closedCalls.OrderBy(call => call.FinishCallTime),
            BO.ClosedCallInListFields.OpenCallTime => closedCalls.OrderBy(call => call.OpenCallTime),
            BO.ClosedCallInListFields.StartCallTime => closedCalls.OrderBy(call => call.StartCallTime),
            BO.ClosedCallInListFields.Addres => closedCalls.OrderBy(call => call.Address),
            _ => closedCalls.OrderBy(call => call.Id),
        };
        return closedCalls;
    }

    /// <summary>
    /// read all open calls with the distance to this volunteer
    /// </summary>
    /// <param name="volunteerId">The volunteer id</param>
    /// <param name="callType">calls with which calltpye to return, can be null and then all the calls will return</param>
    /// <param name="sort">enum value to choose by which field to sort the calls</param>
    /// <returns>the open calls</returns>
    public IEnumerable<BO.OpenCallInList> ReadAllVolunteerOpenCalls(int volunteerId, BO.CallType? callType = null, BO.OpenCallInListFields? sort = null)
    {
        var openedCalls = ReadAll();
        openedCalls = openedCalls.Where(a =>
      Helpers.CallManager.GetCallStatus(a.CallId) == BO.FinishCallType.Open ||
       Helpers.CallManager.GetCallStatus(a.CallId) == BO.FinishCallType.OpenInRisk);
        var openCallsToReturn = openedCalls.Select(
                a => new BO.OpenCallInList
                {
                    Id = a.CallId,
                    CallType = (BO.CallType)(_dal.Call.Read(c => c.Id == a.CallId)!.CallType),
                    Address = _dal.Call.Read(c => c.Id == a.CallId)!.Address,
                    OpenTime = _dal.Call.Read(c => c.Id == a.CallId)!.OpenTime,
                    MaxCloseTime = _dal.Call.Read(c => c.Id == a.CallId)!.MaxCloseTime,
                    Distance = Helpers.CallManager.DistanceBetweenVolunteerAndCall(volunteerId, a.CallId)
                });
        if (callType != null)
            openCallsToReturn = openCallsToReturn.Where(call => call.CallType == callType);
        openCallsToReturn = sort switch
        {
            BO.OpenCallInListFields.Address => openCallsToReturn.OrderBy(call => call.Address),
            BO.OpenCallInListFields.CallType => openCallsToReturn.OrderBy(call => call.CallType),
            BO.OpenCallInListFields.OpenTime => openCallsToReturn.OrderBy(call => call.OpenTime),
            BO.OpenCallInListFields.MaxCloseTime => openCallsToReturn.OrderBy(call => call.MaxCloseTime),
            BO.OpenCallInListFields.Distance => openCallsToReturn.OrderBy(call => call.Distance),
            _ => openCallsToReturn.OrderBy(call => call.Id),
        };
        return openCallsToReturn;
    }
    
    /// <summary>
    /// to update a volunteer went to the call
    /// </summary>
    /// <param name="volunteerId">the volunteer id</param>
    /// <param name="assignmentId">the assignment id the volunteer finished</param>
    /// <exception cref="BO.BlDoesNotExistException">if there is not volunteer or assignment with these ids</exception>
    /// <exception cref="BO.BlFinishProcessIllegalException">if the assignment is not with this volunteer or the call wasn't in prociss</exception>
    public void FinishProcess(int volunteerId, int assignmentId)
    {
        var assignment = _dal.Assignment.Read(a => a.CallId == assignmentId) ?? throw new BO.BlDoesNotExistException($"Assignment with id {assignmentId} does not exist");
        if (assignment.VolunteerId != volunteerId)
            throw new BO.BlFinishProcessIllegalException("this assignment is not with this volunteer");

        if (assignment.FinishTime != null)
            throw new BO.BlFinishProcessIllegalException("Call is not in process");


        var assignmentToUpdate = new DO.Assignment
        {
            Id = assignmentId,
            CallId = assignment.CallId,
            VolunteerId = volunteerId,
            OpenTime = assignment.OpenTime,
            FinishTime = ClockManager.Now,
            FinishType = DO.FinishType.Processed
        };
        _dal.Assignment.Update(assignmentToUpdate);
    }

    /// <summary>
    /// cancel the assitnment between volunteer and call
    /// </summary>
    /// <param name="volunteerId">id of the volunteer</param>
    /// <param name="assignmentId">id of the assignment to cancel</param>
    /// <exception cref="BO.BlDoesNotExistException">if there is not volunteer or assignment with these ids</exception>
    /// <exception cref="BO.BlCancelProcessIllegalException">when trying to cancel finished assignment</exception>
    public void CanceleProcess(int volunteerId, int assignmentId)
    {
        var volunteer = _dal.Volunteer.Read(v => v.Id == volunteerId) ??
            throw new BO.BlDoesNotExistException($"Volunteer with id {volunteerId} does not exists");
        var assignment = _dal.Assignment.Read(a => a.Id == assignmentId) ??
            throw new BO.BlDoesNotExistException($"assignment with id {assignmentId} does not exists");
        if (volunteer.Role == DO.Role.Manager || assignment.VolunteerId == volunteerId)
        {
            var finishType = assignment.VolunteerId == volunteerId ? DO.FinishType.SelfCancel : DO.FinishType.ManagerCancel;
            if (assignment.FinishTime == null)
            {
                var newAssignment = new DO.Assignment
                {
                    Id = assignment.Id,
                    VolunteerId = volunteerId,
                    CallId = assignment.CallId,
                    OpenTime = assignment.OpenTime,
                    FinishTime = ClockManager.Now,
                    FinishType = finishType
                };

                try
                {
                    _dal.Assignment.Update(newAssignment);
                }
                catch
                {
                    throw new BO.BlDoesNotExistException("This assignment does not exist");
                }
            }
            else
            {
                throw new BO.BlCancelProcessIllegalException("Can cancel just not finised assignments");
            }
        }
    }

    /// <summary>
    /// creating assignment between a volunteer and a call
    /// </summary>
    /// <param name="volunteerId">the volunteer id</param>
    /// <param name="callId">the call id</param>
    /// <exception cref="BO.BlIllegalChoseCallException">if call is already in process</exception>
    /// <exception cref="BO.BlDoesNotExistException">if there is not volunteer or assignment with these ids</exception>
    public void ChooseCall(int volunteerId, int callId)
    {
        var assignments = Helpers.CallManager.AssignmentsListForCall(callId).Where(a => a.FinishType == null);
        if (assignments.Any())
            throw new BO.BlIllegalChoseCallException($"Call {callId} is in process");
        var call = _dal.Call.Read(call => call.Id == callId) ??
            throw new BO.BlDoesNotExistException($"Call with id {callId} does not exists");
        var volunteer = _dal.Volunteer.Read(v => v.Id == volunteerId) ??
            throw new BO.BlDoesNotExistException($"Volunteer with id {volunteerId} does not exists");

        if (Helpers.CallManager.RestTimeForCall(call) != TimeSpan.Zero)
        {
            Helpers.AssignmentManager.CreateAssignment(callId, volunteerId);
        }
        else
        {
            throw new BO.BlIllegalChoseCallException($"Call {callId} expired");
        }

    }
}


