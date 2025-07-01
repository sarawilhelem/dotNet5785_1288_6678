using BlApi;
using BO;
using DO;
using Helpers;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;



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

    public IEnumerable<BO.CallInList> ReadAll(BO.CallInListFields? filterBy = null, Object? filterParam = null, BO.CallInListFields? sortBy = null)
    {
        return CallManager.ReadAll(filterBy, filterParam, sortBy);
    }

    /// <summary>
    /// read a call
    /// </summary>
    /// <param name="id">the call id to read</param>
    /// <returns>The call with that id</returns>
    public BO.Call? Read(int id)
    {
        DO.Call? DOCall;
        lock (AdminManager.BlMutex)
            DOCall = _dal.Call.Read(i => i.Id == id);
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
    public async Task Update(BO.Call call)
    {
        AdminManager.ThrowOnSimulatorIsRunning();  //stage 7
        CallManager.CheckValidation(call);

        // יצירת DO.Call מבלי לחשב את הקואורדינטות
        DO.Call callToUpdate = new DO.Call
        {
            CallType = (DO.CallType)call.Type,
            Address = call.Address,
            OpenTime = call.OpenTime,
            MaxCloseTime = call.MaxCloseTime,
            Description = call.Description,
            Id = call.Id
        };

        try
        {
            CallManager.Update(callToUpdate); 
            VolunteerManager.Observers.NotifyListUpdated();
            CallManager.Observers.NotifyListUpdated();
            CallManager.Observers.NotifyItemUpdated(callToUpdate.Id);
            _ = UpdateCoordinatesForCallAddressAsync(callToUpdate); //stage 7
        }
        catch
        {
            throw new BO.BlDoesNotExistException("That call does not exist");
        }
    }


    /// <summary>
    /// delete a call
    /// </summary>
    /// <param name="id">the id of the call which has to be deleted</param>
    /// <exception cref="BO.BlDeleteImpossible">if there is not a call with that id or call is not open or assigned to a volunteer</exception>
    public void Delete(int id)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        lock (AdminManager.BlMutex)
            if (_dal.Call.Read(id) == null)
                throw new BO.BlDoesNotExistException($"Call with id {id} does not exists");
        if (!Helpers.CallManager.AssignmentsListForCall(id).Any() && Helpers.CallManager.GetCallStatus(id) == BO.FinishCallType.Open)
        {
            try
            {
                Helpers.CallManager.Delete(id);
                CallManager.Observers.NotifyListUpdated();
            }
            catch
            {
                throw new BO.BlDeleteImpossible($"Call with id {id} does not exists");
            }

        }
        else
        {
            throw new BO.BlDeleteImpossible("Call is not open or assigned to a volunteer");
        }
        CallManager.Observers.NotifyListUpdated();
    }

    /// <summary>
    /// add a new call to the db
    /// </summary>
    /// <param name="call">the new call to add</param>
    /// <exception cref="BO.BlIllegalValues">if a field in the new call is not legal</exception>
    /// <exception cref="BO.BlIllegalDatesOrder">if the dates order in the new call is illegal</exception>
    /// <exception cref="BO.BlCoordinatesException">if there is an exception when calculate the lat and lon</exception>
    public async Task Create(BO.Call call)
    {
        AdminManager.ThrowOnSimulatorIsRunning();  //stage 7
        if (call.Address is null)
            throw new BO.BlIllegalValues("address can not be null");
        if (call.OpenTime > call.MaxCloseTime || call.MaxCloseTime < AdminManager.Now)
            throw new BO.BlIllegalDatesOrder("MaxCloseTime can't be before now or open time");

        DO.Call callToAdd = new DO.Call
        {
            CallType = (DO.CallType)call.Type,
            Address = call.Address,
            OpenTime = Helpers.AdminManager.Now,
            MaxCloseTime = call.MaxCloseTime,
            Description = call.Description
        };

        lock (AdminManager.BlMutex)
        {
            Helpers.CallManager.Create(callToAdd);
        }

        CallManager.Observers.NotifyListUpdated();
         _=UpdateCoordinatesForCallAddressAsync(callToAdd); 
    }

    private static async Task UpdateCoordinatesForCallAddressAsync(DO.Call doCall)
    {
        if (doCall.Address is not null)
        {
            var (latitude, longitude) = await Tools.GetCoordinatesAsync(doCall.Address);
            if (latitude is not null && longitude is not null)
            {
                doCall = doCall with { Latitude = latitude.Value, Longitude = longitude.Value };
                lock (AdminManager.BlMutex)
                {
                    Helpers.CallManager.Update(doCall);
                }
                CallManager.Observers.NotifyListUpdated();
            }
        }
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
        IEnumerable<BO.ClosedCallInList> closedCalls;
        lock (AdminManager.BlMutex)
        {
            closedCalls = Helpers.CallManager.AssignmentsListForVolunteer(volunteerId).Select(
                        a => new BO.ClosedCallInList
                        {
                            Id = a.CallId,
                            CallType = (BO.CallType)_dal.Call.Read(c => c.Id == a.CallId)!.CallType,
                            Address = _dal.Call.Read(c => c.Id == a.CallId)!.Address,
                            OpenCallTime = _dal.Call.Read(c => c.Id == a.CallId)!.OpenTime,
                            StartCallTime = a.OpenTime,
                            FinishCallTime = a.FinishTime,
                            FinishType = (BO.FinishType?)a.FinishType
                        }
            ).ToList();
        }
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
        var openedCalls = CallManager.ReadAll();
        openedCalls = openedCalls.Where(a =>
      Helpers.CallManager.GetCallStatus(a.CallId) == BO.FinishCallType.Open ||
       Helpers.CallManager.GetCallStatus(a.CallId) == BO.FinishCallType.OpenInRisk);
        var openCallsToReturn = openedCalls.Select(a =>
        {
            DO.Call call;
            lock (AdminManager.BlMutex)
                call = _dal.Call.Read(c => c.Id == a.CallId)!;
            return new BO.OpenCallInList
            {
                Id = a.CallId,
                CallType = (BO.CallType)call.CallType,
                Address = call.Address,
                OpenTime = call.OpenTime,
                MaxCloseTime = call.MaxCloseTime,
                Distance = Helpers.CallManager.DistanceBetweenVolunteerAndCall(volunteerId, a.CallId),
                Description = call.Description
            };
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
            BO.OpenCallInListFields.Description => openCallsToReturn.OrderBy(call => call.Description),
            _ => openCallsToReturn.OrderBy(call => call.Id),
        };
        return openCallsToReturn;
    }

    public void FinishProcess(int volunteerId, int assignmentId)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        CallManager.FinishProcess(volunteerId, assignmentId);
    }

    /// <summary>
    /// cancel the assitnment between volunteer and call
    /// </summary>
    /// <param name="userId">id of the volunteer</param>
    /// <param name="assignmentId">id of the assignment to cancel</param>
   public void CancelProcess(int userId, int assignmentId)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        CallManager.CancelProcess(userId, assignmentId);
    }

    /// <summary>
    /// creating assignment between a volunteer and a call
    /// </summary>
    /// <param name="volunteerId">the volunteer id</param>
    /// <param name="callId">the call id</param>
    public void ChooseCall(int volunteerId, int callId)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        CallManager.ChooseCall(volunteerId, callId);
    }




    public void AddObserver(Action listObserver) =>
        CallManager.Observers.AddListObserver(listObserver);
    public void AddObserver(int id, Action observer) =>
        CallManager.Observers.AddObserver(id, observer);
    public void RemoveObserver(Action listObserver) =>
        CallManager.Observers.RemoveListObserver(listObserver);
    public void RemoveObserver(int id, Action observer) =>
        CallManager.Observers.RemoveObserver(id, observer);

}


