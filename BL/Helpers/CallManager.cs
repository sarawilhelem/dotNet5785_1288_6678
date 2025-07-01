using BO;
using DalApi;
using DO;

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
        if (call.Status == FinishCallType.Close || call.Status == FinishCallType.Expired)
            throw new BlUpdateImpossibleException("Closed and expired call can not be updated");
        if (call.Status == FinishCallType.InProcess || call.Status == FinishCallType.InProcessInRisk)
        {
            if(prevCall!.Description != call.Description || (BO.CallType)prevCall.CallType != call.Type || 
                prevCall.Address != call.Address )
                throw new BlUpdateImpossibleException("Processed call can not be updated except for max close time");

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
    public static FinishCallType GetCallStatus(int id)
    {
        DO.Call call;
        IEnumerable<Assignment> assignmentsList;
        lock (AdminManager.BlMutex)
         call = s_dal.Call.Read(c => c.Id == id);
        lock (AdminManager.BlMutex)
            assignmentsList = s_dal.Assignment.ReadAll(a => a.CallId == id);
        var isProcesseds = assignmentsList.Any(a => a.FinishType == DO.FinishType.Processed);
        if (isProcesseds)
        {
            return FinishCallType.Close;
        }
        else
        {
            if (call is null || RestTimeForCall(call) == TimeSpan.Zero)
            {
                return FinishCallType.Expired;
            }
            else
            {
                var isInProcess = assignmentsList.Any(a => a.FinishType == null);
                if(isInProcess)
                {
                    if ((call.MaxCloseTime - AdminManager.Now) <= s_dal.Config.RiskRange)
                        return FinishCallType.InProcessInRisk;
                    return FinishCallType.InProcess;
                }

                else
                {
                    if ((call.MaxCloseTime - AdminManager.Now) <= s_dal.Config.RiskRange)
                        return FinishCallType.OpenInRisk;
                    return FinishCallType.Open;
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
        Assignment assignment;
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
            throw new BO.BlDoesNotExistException($"volunteer with id {volunteerId} does not exists");
        lock (AdminManager.BlMutex)
            call = s_dal.Call.Read(c => c.Id == callId) ??
            throw new BO.BlDoesNotExistException($"call with id {callId} does not exists");

        if (volunteer.Latitude is null || volunteer.Longitude is null)
            return 0;
        var distance = Math.Sqrt(Math.Pow(((double)(volunteer.Latitude!) - call.Latitude), 2) + Math.Pow(((double)(volunteer.Longitude!) - call.Longitude), 2));
        return distance;
    }

    /// <summary>
    /// calculate how many assignments this call has
    /// </summary>
    /// <param name="call">the call</param>
    /// <returns>how many assignments is has</returns>
    public static int GetAmountOfAssignments(DO.Call call)
    {
        IEnumerable<Assignment> assignmentsList;
        lock (AdminManager.BlMutex)
            assignmentsList = s_dal.Assignment.ReadAll(a => a.CallId == call.Id);
        return assignmentsList.Count();
    }
    public static void Update(DO.Call callToUpdate)
    {
        lock (AdminManager.BlMutex)
            s_dal.Call.Update(callToUpdate);
    }
    public static void Delete(int id)
    {
        lock (AdminManager.BlMutex)
            s_dal.Call.Delete(id);
    }
    public static void Create(DO.Call call)
    {
        lock (AdminManager.BlMutex)
            s_dal.Call.Create(call);
    }
}
