

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
    public async void Create(BO.Volunteer volunteer)
    {
        if (VolunteerManager.CheckValidation(volunteer) == false)
            throw new BO.BlIllegalValues("Volunteer fields are illegal");
        if (volunteer.Address != null)
        {
            var (latitude, longitude) = await VolunteerManager.GetCoordinatesAsync(volunteer.Address);
            if (latitude != null && longitude != null)
            {
                volunteer.Latitude = latitude;
                volunteer.Longitude = longitude;
            }
            else
                throw new BO.BlIllegalValues("Volunteer address is illegal");

        }

        DO.Volunteer doVolunteer = new(volunteer.Id, volunteer.Name, volunteer.Phone, volunteer.Email, volunteer.Address, volunteer.Latitude, volunteer.Longitude,
            volunteer.MaxDistance, (DO.Role)volunteer.Role, (DO.Distance_Type)volunteer.Distance_Type, VolunteerManager.HashPassword(volunteer.Password), volunteer.IsActive);

        try
        {
            _dal.Volunteer.Create(doVolunteer);
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
        if (_dal.Assignment.ReadAll().Any(a => a.VolunteerId == id && a.FinishTime is not null))
            throw new BO.BlDeleteImpossible($"volunteer with Id {id} can't be deleted");

        try
        {
            _dal.Volunteer.Delete(id);
        }
        catch (DO.DalDeleteImpossible ex)
        {
            throw new BO.BlDeleteImpossible($"volunteer with Id {id} is not exist", ex);
        }
    }

    public BO.Role EnterSystem(string name, string? password = null)
    {
        DO.Volunteer volunteer = _dal.Volunteer.Read(v => v.Name == name && v.Password == VolunteerManager.HashPassword(password)) ??
            throw new BO.BlDoesNotExistException($"Entering was not succeeded");
        return (BO.Role)volunteer.Role;
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
        DO.Assignment? assignment = assignments.FirstOrDefault(a => a.FinishType != null);
        DO.Call? call = assignment is null ? null : _dal.Call.Read(assignment.CallId);

        BO.CallInProgress? callInProgress = call is null ? null :
            new BO.CallInProgress(assignment!.Id, call.Id, (BO.Call_Type)call.Call_Type, call.Address,
            VolunteerManager.CalculateDistance(doVolunteer.Latitude, doVolunteer.Longitude, call.Latitude, call.Longitude),
            call.OpenTime, assignment.Insersion,
            Tools.IsWithinRiskRange(call.MaxCloseTime) ? BO.CallStatus.InProcessInRiskRange : BO.CallStatus.InProcess,
            call.Description, call.MaxCloseTime);

        return new BO.Volunteer(id, doVolunteer.Name, doVolunteer.Phone, doVolunteer.Email, doVolunteer.Password,
            doVolunteer.Address, doVolunteer.Latitude, doVolunteer.Longitude, (BO.Role)doVolunteer.Role, doVolunteer.IsActive,
            doVolunteer.MaxDistanceCall, (BO.Distance_Type)doVolunteer.Distance_Type,
            assignments.Count(a => a.FinishType == DO.Finish_Type.Addressed),
            assignments.Count(a => a.FinishType == DO.Finish_Type.SelfCancel),
            assignments.Count(a => a.FinishType == DO.Finish_Type.Expired)
        );
    }

    /// <summary>
    /// read all the volunteers
    /// </summary>
    /// <param name="isActive">do we want just active = true or not active = false or all volunteers = null</param>
    /// <param name="sort">according to which field to sort the volunteers</param>
    /// <returns>a list of bo volunteers</returns>
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive = null, BO.Volunteer_In_List_Fields? sort = null)
    {
        //sort ??= BO.Volunteer_In_List_Fields.Id;
        //IEnumerable<DO.Volunteer> volunteers = _dal.Volunteer.ReadAll(isActive is null ? null : v => v.IsActive == isActive);
        //var volunteersInList = volunteers.Select(v =>
        //{
        //    int? callId = _dal.Assignment.Read(a => a.VolunteerId == v.Id && a.FinishTime == null)?.CallId;
        //    BO.Call_Type? callType = callId is null ? null : (BO.Call_Type?)_dal.Call.Read(callId.Value)!.Call_Type;
        //    return new BO.VolunteerInList(
        //    v.Id, v.Name, v.IsActive,
        //    _dal.Assignment.ReadAll(a => a.VolunteerId == v.Id && a.FinishType == DO.Finish_Type.Addressed).Count(),
        //    _dal.Assignment.ReadAll(a => a.VolunteerId == v.Id && (a.FinishType == DO.Finish_Type.ManageCancel || a.FinishType == DO.Finish_Type.SelfCancel)).Count(),
        //    _dal.Assignment.ReadAll(a => a.VolunteerId == v.Id && a.FinishType == DO.Finish_Type.Expired).Count(),
        //    callId, callType);
        //}).ToList();



        //var propertyInfo = typeof(BO.VolunteerInList).GetProperty(sort.Value.ToString());

        //if (propertyInfo != null)
        //{
        //    volunteersInList = volunteersInList.OrderBy(v => propertyInfo.GetValue(v)).ToList();
        //}

        //return volunteersInList;
        var volunteersInList = from v in _dal.Volunteer.ReadAll(isActive is null ? null : v => v.IsActive == isActive)
                               join a in _dal.Assignment.ReadAll() on v.Id equals a.VolunteerId into assignments
                               let callId = assignments.FirstOrDefault(a => a.FinishTime == null)?.CallId
                               let callType = callId is null ? null : (BO.Call_Type?)_dal.Call.Read(callId.Value)!.Call_Type
                               select new BO.VolunteerInList(
                                   v.Id,
                                   v.Name,
                                   v.IsActive,
                                   assignments.Count(a => a.FinishType == DO.Finish_Type.Addressed),
                                   assignments.Count(a => a.FinishType == DO.Finish_Type.ManageCancel || a.FinishType == DO.Finish_Type.SelfCancel),
                                   assignments.Count(a => a.FinishType == DO.Finish_Type.Expired),
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
    public async void Update(int id, BO.Volunteer volunteer)
    {
        DO.Volunteer? requester = _dal.Volunteer.Read(id);
        if (requester is null || (volunteer.Id != id && requester.Role != DO.Role.Manager))
            return;
        if (!VolunteerManager.CheckValidation(volunteer))
            throw new BO.BlIllegalValues("Volunteer fields are illegal");
        if (volunteer.Address != null)
        {
            var (latitude, longitude) = await VolunteerManager.GetCoordinatesAsync(volunteer.Address);
            if (latitude != null && longitude != null)
            {
                volunteer.Latitude = latitude;
                volunteer.Longitude = longitude;
            }
            else
                throw new BO.BlIllegalValues("Volunteer address is illegal");
        }

        try
        {
            DO.Volunteer? prevDoVolunteer = _dal.Volunteer.Read(volunteer.Id) ??
                throw new BO.BlDoesNotExistException($"volunteer with id {volunteer.Id} does not exist");
            if (requester.Role != DO.Role.Manager && (DO.Role)volunteer.Role != prevDoVolunteer.Role)
                volunteer.Role = (BO.Role)prevDoVolunteer.Role;
            DO.Volunteer updateDoVolunteer = new(volunteer.Id, volunteer.Name, volunteer.Phone, volunteer.Email, volunteer.Address,
                volunteer.Latitude, volunteer.Longitude, volunteer.MaxDistance, (DO.Role)volunteer.Role,
                (DO.Distance_Type)volunteer.Distance_Type, volunteer.Password, volunteer.IsActive);
            _dal.Volunteer.Update(updateDoVolunteer);
        }
        catch (BO.BlDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"volunteer with id {volunteer.Id} does not exist", ex);
        }
    }
}




