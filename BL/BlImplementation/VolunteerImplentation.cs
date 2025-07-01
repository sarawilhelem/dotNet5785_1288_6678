

using DalApi;
using Helpers;

namespace BlImplementation;

internal class VolunteerImplentation : BlApi.IVolunteer
{
    /// <summary>
    /// A field which we approach throw it to the dal object's crud
    /// </summary>
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// add the volunteer to the lists in dal
    /// </summary>
    /// <param name="volunteer">the volunteer to add</param>
    /// <exception cref="BO.BlIllegalValues">to when the volunteers' details are illegal</exception>
    /// <exception cref="BO.BlAlreadyExistsException">to when trying to add a volunteer with already exist id</exception>
    public void Create(BO.Volunteer volunteer)
    {
        AdminManager.ThrowOnSimulatorIsRunning();  //stage 7
        VolunteerManager.CheckValidation(volunteer);

        if (volunteer.Address is not null)
        {

            var (latitude, longitude) = Tools.GetCoordinates(volunteer.Address);

            volunteer.Latitude = latitude;
            volunteer.Longitude = longitude;
        }

        DO.Volunteer doVolunteer = new(volunteer.Id, volunteer.Name, volunteer.Phone, volunteer.Email, volunteer.Address, volunteer.Latitude, volunteer.Longitude,
            volunteer.MaxDistance, (DO.Role)volunteer.Role, (DO.DistanceType)volunteer.DistanceType, VolunteerManager.Encrypt(volunteer.Password), volunteer.IsActive);

        try
        {
            VolunteerManager.Create(doVolunteer);
            VolunteerManager.Observers.NotifyListUpdated();
            VolunteerManager.Observers.NotifyItemUpdated(doVolunteer.Id);
            CallManager.Observers.NotifyListUpdated();
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"volunteer with Id {volunteer.Id} already exist", ex);
        }

    }

    /// <summary>
    /// delete a volunteer from the list in dal
    /// </summary>
    /// <param name="id">the id of the volunteer which has to be deleted</param>
    /// <exception cref="BO.BlDeleteImpossible">to when there is not any volunteer with that id</exception>
    public void Delete(int id)
    {
        AdminManager.ThrowOnSimulatorIsRunning();  //stage 7
        IEnumerable<DO.Assignment> assignments;
        lock (AdminManager.BlMutex)
            assignments = _dal.Assignment.ReadAll();
        if (assignments.Any(a => a.VolunteerId == id && a.FinishTime == null))
            throw new BO.BlDeleteImpossible($"volunteer with Id {id} cannot be deleted because he is handling a call.");

        try
        {
            VolunteerManager.Delete(id);
            VolunteerManager.Observers.NotifyListUpdated();
            CallManager.Observers.NotifyListUpdated();
        }
        catch (DO.DalDeleteImpossible ex)
        {
            throw new BO.BlDeleteImpossible($"volunteer with Id {id} is not exist", ex);
        }
    }

    /// <summary>
    /// checks if password is correct (hashing it and compare to the hashed password in the db)
    /// </summary>
    /// <param name="name">volunteer's name</param>
    /// <param name="password">volunteer's password</param>
    /// <returns>volunteer's role</returns>
    /// <exception cref="BO.BlDoesNotExistException">if there is not a volunteer with this name and this password - the entering failed</exception>
    public BO.Volunteer EnterSystem(string name, string? password = null)
    {
        DO.Volunteer volunteer = _dal.Volunteer.Read(v => v.Name == name && (v.Password == "" || v.Password is null || VolunteerManager.Decrypt(v.Password) == password)) ??
            throw new BO.BlDoesNotExistException($"Entering was not succeeded. no volunteer with that name and password");
        return Read(volunteer.Id);
    }

    /// <summary>
    /// Reading a volunteer from the list in dal
    /// </summary>
    /// <param name="id">the id of the volunteer who we want to read</param>
    /// <returns>the bo volunteer</returns>
    /// <exception cref="BO.BlDoesNotExistException">when there is not any volunteer with that id</exception>
    public BO.Volunteer Read(int id)
    {
        DO.Volunteer doVolunteer = _dal.Volunteer.Read(id) ??
            throw new BO.BlDoesNotExistException($"volunteer with Id {id} is not exist");
        IEnumerable<DO.Assignment> assignments = _dal.Assignment.ReadAll(a => a.VolunteerId == id);
        DO.Assignment? assignment = assignments.FirstOrDefault(a => a.FinishType is null);
        DO.Call? call = assignment is null ? null : _dal.Call.Read(assignment.CallId);

        BO.CallInProgress? callInProgress = call is null ? null :
            new BO.CallInProgress(assignment!.Id, call.Id, (BO.CallType)call.CallType, call.Address,
            VolunteerManager.CalculateDistance(doVolunteer.Latitude, doVolunteer.Longitude, call.Latitude, call.Longitude),
            call.OpenTime, assignment.OpenTime,
            VolunteerManager.IsWithinRiskRange(call.MaxCloseTime) ? BO.CallStatus.InProcessInRiskRange : BO.CallStatus.InProcess,
            call.Description, call.MaxCloseTime);

        return new BO.Volunteer(id, doVolunteer.Name, doVolunteer.Phone, doVolunteer.Email,VolunteerManager.Decrypt(doVolunteer.Password),
            doVolunteer.Address, doVolunteer.Latitude, doVolunteer.Longitude, (BO.Role)doVolunteer.Role, doVolunteer.IsActive,
            doVolunteer.MaxDistanceCall, (BO.DistanceType)doVolunteer.DistanceType,
            assignments.Count(a => a.FinishType.Equals(DO.FinishType.Processed)),
            assignments.Count(a => a.FinishType.Equals(DO.FinishType.SelfCancel)),
            assignments.Count(a => a.FinishType.Equals(DO.FinishType.Expired)),
            callInProgress
        );
    }

    /// <summary>
    /// read all the volunteers
    /// </summary>
    /// <param name="isActive">do we want just active = true or not active = false or all volunteers = null</param>
    /// <param name="sort">according to which field to sort the volunteers</param>
    /// <returns>a list of bo volunteers</returns>
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive = null, BO.VolunteerInListFields? sort = null)
    {
        var volunteersInList = from v in _dal.Volunteer.ReadAll(isActive is null ? null : v => v.IsActive == isActive)
                               join a in _dal.Assignment.ReadAll() on v.Id equals a.VolunteerId into assignments
                               let callId = assignments.FirstOrDefault(a => a.FinishTime == null)?.CallId
                               let callType = callId is null ? null : (BO.CallType?)_dal.Call.Read(callId.Value)!.CallType
                               select new BO.VolunteerInList(
                                   v.Id,
                                   v.Name,
                                   v.IsActive,
                                   assignments.Count(a => a.FinishType.Equals(DO.FinishType.Processed)),
                                   assignments.Count(a => a.FinishType.Equals(DO.FinishType.ManagerCancel) || a.FinishType.Equals(DO.FinishType.SelfCancel)),
                                   assignments.Count(a => a.FinishType.Equals(DO.FinishType.Expired)),
                                   callId,
                                   callType
                               );

        if (sort != null)
        {
            var propertyInfo = typeof(BO.VolunteerInList).GetProperty(sort.Value.ToString());
            if (propertyInfo != null)
            {
                volunteersInList = volunteersInList.OrderBy(v => propertyInfo.GetValue(v)).ToList();
            }
        }

        return volunteersInList.ToList();
    }

    /// <summary>
    /// update one volunteer
    /// </summary>
    /// <param name="id">who wants to update the voluneer</param>
    /// <param name="volunteer">the updated volunteer</param>
    /// <exception cref="BO.BlIllegalValues">if the volunteer details are illegal</exception>
    /// <exception cref="BO.BlDoesNotExistException">there is not any volunteer with the id like the parameter volunteer</exception>
    public void Update(int id, BO.Volunteer volunteer)
    {
        AdminManager.ThrowOnSimulatorIsRunning();  //stage 7
        DO.Volunteer? requester;
        lock (AdminManager.BlMutex) 
            requester = _dal.Volunteer.Read(id);
        if (requester is null || (volunteer.Id != id && requester.Role != DO.Role.Manager))
            return;
        VolunteerManager.CheckValidation(volunteer);

        if (volunteer.Address != null && volunteer.Address != "")
        {
            var (latitude, longitude) = Tools.GetCoordinates(volunteer.Address);
            volunteer.Latitude = latitude;
            volunteer.Longitude = longitude;
        }
        try
        {
            DO.Volunteer? prevDoVolunteer;
            lock (AdminManager.BlMutex) 
                prevDoVolunteer = _dal.Volunteer.Read(volunteer.Id) ??
                throw new BO.BlDoesNotExistException($"volunteer with id {volunteer.Id} does not exist");
            lock (AdminManager.BlMutex)
                if (volunteer.Role == BO.Role.Volunteer && prevDoVolunteer.Role == DO.Role.Manager && !_dal.Volunteer.ReadAll(v => v.Role == DO.Role.Manager).Any())
                    throw new BO.BlUpdateImpossibleException($"Unable to update volunteer as there will be no managers left");
            if (requester.Role != DO.Role.Manager && (DO.Role)volunteer.Role != prevDoVolunteer.Role)
                volunteer.Role = (BO.Role)prevDoVolunteer.Role;
            if(volunteer.IsActive == false && prevDoVolunteer.IsActive ==  true)
            {
                IEnumerable<DO.Assignment> assignments;
                lock (AdminManager.BlMutex)
                    assignments = _dal.Assignment.ReadAll();
                if (assignments.Any(a => a.VolunteerId == volunteer.Id && a.FinishTime == null))
                    throw new BO.BlUpdateImpossibleException($"Volunteer {volunteer.Id} cannot be marked as inactive because there is a call in his care.");
            }
            DO.Volunteer updateDoVolunteer = new(volunteer.Id, volunteer.Name, volunteer.Phone, volunteer.Email, volunteer.Address,
                volunteer.Latitude, volunteer.Longitude, volunteer.MaxDistance, (DO.Role)volunteer.Role,
                (DO.DistanceType)volunteer.DistanceType, VolunteerManager.Encrypt(volunteer.Password), volunteer.IsActive);
            VolunteerManager.Update(updateDoVolunteer);
            VolunteerManager.Observers.NotifyListUpdated();
            VolunteerManager.Observers.NotifyItemUpdated(prevDoVolunteer.Id);
            CallManager.Observers.NotifyListUpdated();
        }
        catch (Exception ex)
        {
            throw new BO.BlDoesNotExistException($"volunteer with id {volunteer.Id} does not exist", ex);
        }
    }
    public void AddObserver(Action listObserver) =>
        VolunteerManager.Observers.AddListObserver(listObserver); 
    public void AddObserver(int id, Action observer) =>
        VolunteerManager.Observers.AddObserver(id, observer); //stage 5
    public void RemoveObserver(Action listObserver) =>
        VolunteerManager.Observers.RemoveListObserver(listObserver); //stage 5
    public void RemoveObserver(int id, Action observer) =>
        VolunteerManager.Observers.RemoveObserver(id, observer); //stage 5

}