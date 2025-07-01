using DalApi;
using BlImplementation;

namespace Helpers;

internal class CallManager
{
    /// <summary>
    /// a static field to access thad do crud
    /// </summary>
    private static IDal s_dal = DalApi.Factory.Get;

    internal static ObserverManager Observers = new();



    public static void CheckValidation(BO.Call call)
    {
        DO.Call prevCall;
        lock (AdminManager.BlMutex)
            prevCall = s_dal.Call.Read(c => c.Id == call.Id) ??
                throw new BO.BlDoesNotExistException($"Call with id {call.Id} does not exists");
        if (call.Status ==BO.FinishCallType.Close || call.Status == BO.FinishCallType.Expired)
            throw new BO.BlUpdateImpossibleException("Closed and expired call can not be updated");
        if (call.Status == BO.FinishCallType.InProcess || call.Status == BO.FinishCallType.InProcessInRisk)
        {
            if(prevCall!.Description != call.Description || (BO.CallType)prevCall.CallType != call.Type || 
                prevCall.Address != call.Address )
                throw new BO.BlUpdateImpossibleException("Processed call can not be updated except for max close time");

        }
        if (call.OpenTime > call.MaxCloseTime || call.MaxCloseTime < AdminManager.Now)
            throw new BO.BlIllegalDatesOrder("MaxCloseTime can't be before now or open time");
        if (call == null)
            throw new BO.BlIllegalValues("Address can not be null");
    }


    /// <summary>
    /// read assignments with the call
    /// </summary>
    /// <param name="id">the call id</param>
    /// <returns>all assitnments with that call</returns>
    public static IEnumerable<DO.Assignment> AssignmentsListForCall(int id)
    {
        IEnumerable<DO.Assignment> assignmentsList;
        lock (AdminManager.BlMutex) 
         assignmentsList = s_dal.Assignment.ReadAll(a => a.CallId == id);
        return assignmentsList;
    }

    /// <summary>
    /// read assignments with the volunteer
    /// </summary>
    /// <param name="id">the volunteer id</param>
    /// <returns>all assignments with that volunteer</returns>
    public static IEnumerable<DO.Assignment> AssignmentsListForVolunteer(int id)
    {
        IEnumerable<DO.Assignment> assignmentsList;
        lock (AdminManager.BlMutex)
             assignmentsList = s_dal.Assignment.ReadAll(a => a.VolunteerId == id);
        return assignmentsList;
    }

    /// <summary>
    /// get the volunteer with that id
    /// </summary>
    /// <param name="id">the volunteer id</param>
    /// <returns>the volunteer</returns>
    /// <exception cref="BO.BlDoesNotExistException">if there is not volunteer with that id</exception>
    public static DO.Volunteer GetVolunteer(int id)
    {
        lock (AdminManager.BlMutex)
            return s_dal.Volunteer.Read(v => v.Id == id) ??
            throw new BO.BlDoesNotExistException($"Volunteer with id {id} does not exists");
    }
    
    /// <summary>
    /// calculate how much time is rest to a call return timeSpan.zero if expired
    /// </summary>
    /// <param name="call">the call to check</param>
    /// <returns>the time span rest</returns>
    public static TimeSpan RestTimeForCall(DO.Call call)
    {
        return AdminManager.Now < call.MaxCloseTime ? (TimeSpan)(call.MaxCloseTime - AdminManager.Now) : TimeSpan.Zero; 
    }

    /// <summary>
    /// the status of a call
    /// </summary>
    /// <param name="id">the call id</param>
    /// <returns>the call status</returns>
    public static BO.FinishCallType GetCallStatus(int id)
    {
        DO.Call? call;
        IEnumerable<DO.Assignment> assignmentsList;
        lock (AdminManager.BlMutex)
            call = s_dal.Call.Read(c => c.Id == id);
        lock (AdminManager.BlMutex)
            assignmentsList = s_dal.Assignment.ReadAll(a => a.CallId == id);
        var isProcesseds = assignmentsList.Any(a => a.FinishType == DO.FinishType.Processed);
        if (isProcesseds)
        {
            return BO.FinishCallType.Close;
        }
        else
        {
            if (call is null || RestTimeForCall(call) == TimeSpan.Zero)
            {
                return BO.FinishCallType.Expired;
            }
            else
            {
                var isInProcess = assignmentsList.Any(a => a.FinishType == null);
                if(isInProcess)
                {
                    if ((call.MaxCloseTime - AdminManager.Now) <= s_dal.Config.RiskRange)
                        return BO.FinishCallType.InProcessInRisk;
                    return BO.FinishCallType.InProcess;
                }

                else
                {
                    if ((call.MaxCloseTime - AdminManager.Now) <= s_dal.Config.RiskRange)
                        return BO.FinishCallType.OpenInRisk;
                    return BO.FinishCallType.Open;
                }
            }
        }
    }

    /// <summary>
    /// convert an Object to timeSpan   
    /// </summary>
    /// <param name="value">the object to convert</param>
    /// <returns>the time span</returns>
    /// <exception cref="BO.BlIllegalValues">if cannot convert this object to timeSpan</exception>
    static public TimeSpan ConvertToTimeSpan(object value)
    {
        if (value is string timeString)
        {
            return TimeSpan.Parse(timeString);
        }
        else if (value is TimeSpan timeSpan)
        {
            return timeSpan;
        }
        else if (value is double hours)
        {
            return TimeSpan.FromHours(hours);
        }
        else
        {
            throw new BO.BlIllegalValues("Value cannot be converted to TimeSpan");
        }
    }

    /// <summary>
    /// calculate how much time this call proccessed 
    /// </summary>
    /// <param name="call">the call</param>
    /// <returns>before how many time, or null if never</returns>
    public static TimeSpan? CalculateAssignmentDuration(DO.Call call)
    {
        DO.Assignment? assignment;
        lock (AdminManager.BlMutex)
            assignment = s_dal.Assignment.Read(a => a.CallId == call.Id && a.FinishType == DO.FinishType.Processed);
        if (assignment != null)
            return (TimeSpan?)(assignment.FinishTime - assignment.OpenTime);
        return null;
    }

    /// <summary>
    /// calculate distance between volunteer and call
    /// </summary>
    /// <param name="volunteerId">the volunteer id</param>
    /// <param name="callId">the call id</param>
    /// <returns>the distance betweent volunteer and call, or 0 if volunteer lat or lon is null</returns>
    /// <exception cref="BO.BlDoesNotExistException">if there is not a volunter of call with these ids</exception>
    public static double DistanceBetweenVolunteerAndCall(int volunteerId, int callId)
    {
        DO.Volunteer volunteer;
        DO.Call call;

        lock (AdminManager.BlMutex)
            volunteer = s_dal.Volunteer.Read(v => v.Id == volunteerId) ??
                throw new BO.BlDoesNotExistException($"volunteer with id {volunteerId} does not exist");
        lock (AdminManager.BlMutex)
            call = s_dal.Call.Read(c => c.Id == callId) ??
                throw new BO.BlDoesNotExistException($"call with id {callId} does not exist");

        if (volunteer.Latitude == null || volunteer.Longitude == null || call.Latitude == null || call.Longitude == null)
            return 0;

        double earthRadiusKm = 6371;

        double dLat = ToRadians(call.Latitude.Value - volunteer.Latitude.Value);
        double dLon = ToRadians(call.Longitude.Value - volunteer.Longitude.Value);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(ToRadians(volunteer.Latitude.Value)) * Math.Cos(ToRadians(call.Latitude.Value)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return earthRadiusKm * c; // Distance in kilometers
    }

    private static double ToRadians(double angle)
    {
        return angle * Math.PI / 180;
    }

    /// <summary>
    /// calculate how many assignments this call has
    /// </summary>
    /// <param name="call">the call</param>
    /// <returns>how many assignments is has</returns>
    public static int GetAmountOfAssignments(DO.Call call)
    {
        IEnumerable<DO.Assignment> assignmentsList;
        lock (AdminManager.BlMutex)
            assignmentsList = s_dal.Assignment.ReadAll(a => a.CallId == call.Id);
        return assignmentsList.Count();
    }

    /// <summary>
    /// read all calls, filtered and sorted according to the params
    /// </summary>
    /// <param name="filterBy">enum value to choose the field to filter</param>
    /// <param name="filterParam">value to filter the field in Object type</param>
    /// <param name="sortBy">enum value to choose the field to sort by it</param>
    /// <returns>all calls filtered and sorted</returns>
    /// <exception cref="BO.BlIllegalValues">if filterParam is not suitable to filterBy type</exception>
    public static IEnumerable<BO.CallInList> ReadAll(BO.CallInListFields? filterBy = null, Object? filterParam = null, BO.CallInListFields? sortBy = null)
    {

        IEnumerable<DO.Call> callList = s_dal.Call.ReadAll();
        IEnumerable<DO.Assignment> assignments = s_dal.Assignment.ReadAll();
        IEnumerable<BO.CallInList> callsListToReturn = callList.Select(
            call =>
            {
                var lastAssignment = assignments.Where(a => a.CallId == call.Id)
                                                .OrderByDescending(a => a.OpenTime)
                                                .FirstOrDefault();

                int? id = lastAssignment?.Id;
                var lastVolunteerName = lastAssignment != null
                    ? s_dal.Volunteer.Read(lastAssignment.VolunteerId)?.Name
                    : null;

                return new BO.CallInList
                {
                    Id = id,
                    CallId = call.Id,
                    CallType = (BO.CallType)call.CallType,
                    OpenTime = call.OpenTime,
                    MaxCloseTime = Helpers.CallManager.RestTimeForCall(call),
                    LastVolunteerName = lastVolunteerName,
                    TotalProcessingTime = Helpers.CallManager.CalculateAssignmentDuration(call),
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
                        callsListToReturn = callsListToReturn.Where(call => call.CallType.Equals(filterParam)).ToList();
                        break;
                    case BO.CallInListFields.Status:
                        callsListToReturn = callsListToReturn.Where(call => call.Status.Equals(filterParam)).ToList();
                        break;
                    case BO.CallInListFields.AmountOfAssignments:
                        callsListToReturn = callsListToReturn.Where(call => call.AmountOfAssignments.Equals(Convert.ToInt32(filterParam))).ToList();
                        break;
                    case BO.CallInListFields.TotalProcessingTime:
                        callsListToReturn = callsListToReturn.Where(call => call.TotalProcessingTime is not null
                            && call.TotalProcessingTime!.Equals(CallManager.ConvertToTimeSpan(filterParam!))).ToList();
                        break;
                }
            }
        }
        catch (BO.BlIllegalValues ex)
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

    internal static void PeriodicSCallsUpdates(DateTime newClock) //stage 4
    {
        bool callUpdated; //stage 5
        List<BO.CallInList> callsList;
        lock (AdminManager.BlMutex) //stage 7
        callsList = ReadAll().ToList();
        callUpdated = false; //stage 5
        foreach (var call in callsList) //stage 4
        {
            if (call.MaxCloseTime <= TimeSpan.Zero && (call.Status == BO.FinishCallType.InProcessInRisk || call.Status == BO.FinishCallType.InProcess))
            {
                callUpdated = true; //stage 5
                DO.Assignment doAssignment;
                lock (AdminManager.BlMutex)
                {
                    doAssignment = s_dal.Assignment.Read(call.Id ?? 0)!;
                    s_dal.Assignment.Update(doAssignment with { FinishTime = newClock, FinishType = DO.FinishType.Expired });
                }
                CallManager.Observers.NotifyItemUpdated(call.CallId);
            }
        }
        if (callUpdated) //stage 5
        {
            CallManager.Observers.NotifyListUpdated();
            VolunteerManager.Observers.NotifyListUpdated();
        }
    }

    internal static void Update(DO.Call call)
    {
        s_dal.Call.Update(call);
    }

    internal static void Create(DO.Call call)
    {
        s_dal.Call.Create(call);
    }

    internal static void Delete(int callId)
    {
        s_dal.Call.Delete(callId);
    }

}