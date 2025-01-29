
using BlApi;
using BO;
using DalApi;
using Helpers;
using System.Net;
using System.Xml;


namespace BlImplementation;

internal class VolunteerImplentation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public async void Add(BO.Volunteer volunteer)
    {
        if (VolunteerManager.CheckValidation(volunteer) == false)
            return;
        if (volunteer.Address != null)
        {
            var (latitude, longitude) = await VolunteerManager.GetCoordinatesAsync(volunteer.Address);
            if (latitude != null && longitude != null)
            {
                volunteer.Latitude = latitude;
                volunteer.Longitude = longitude;
            }
            else
                return;

        }

        DO.Volunteer doVolunteer = new DO.Volunteer(volunteer.Id, volunteer.Name, volunteer.Phone, volunteer.Email, volunteer.Address, volunteer.Latitude, volunteer.Longitude,
            volunteer.MaxDistance, (DO.Role)volunteer.Role, (DO.Distance_Type)volunteer.Distance_Type, volunteer.Password, volunteer.IsActive);
        try
        {
            _dal.Volunteer.Create(doVolunteer);
        }
        catch
        {
            throw new BO.BlAlreadyExistsException($"volunteer with Id {volunteer.Id} already exist");
        }

    }


    public void Delete(int id)
    {
        if (_dal.Assignment.ReadAll().Any(a => a.VolunteerId == id && a.FinishTime is not null))
            throw new BO.BlDeleteImpossible($"volunteer with Id {id} can't be deleted");

        try
        {
            _dal.Volunteer.Delete(id);
        }
        catch
        {
            throw new BO.BlDeleteImpossible($"volunteer with Id {id} is not exist");
        }
    }

    public BO.Role EnterSystem(string name, string password)
    {
        DO.Volunteer volunteer = _dal.Volunteer.Read(v => v.Name == name && v.Password == password) ??
            throw new BO.BlDoesNotExistException($"Entering was not succeeded");
        return (BO.Role)volunteer.Role;
    }

    public BO.Volunteer Read(int id)
    {
        DO.Volunteer doVolunteer = _dal.Volunteer.Read(id) ??
            throw new BO.BlDoesNotExistException($"volunteer with Id {id} is not exist");
        DO.Assignment? assignment = _dal.Assignment.Read(a => a.VolunteerId == id && a.FinishType != null);
        DO.Call? call = assignment is null ? null : _dal.Call.Read(assignment.CallId);
        BO.CallInProgress? boVolunteerCall = call is null ? null : new BO.CallInProgress(assignment?.Id, call.Id, call.Call_Type, call.Description,
            call.Address, call.OpenTime, call.MaxCloseTime, assignment?.Insersion,
            VolunteerManager.CalculateDistance(doVolunteer.Latitude, doVolunteer.Longitude, call.Latitude, call.Longitude),
            VolunteerManager.IsWithinRiskRange(call.MaxCloseTime) ? BO.CallStatus.InProcessInRiskRange : BO.CallStatus.InProcess);
        BO.Volunteer boVolunteer = new BO.Volunteer(id, doVolunteer.Name, doVolunteer.Phone, doVolunteer.Email, doVolunteer.Password,
            doVolunteer.Address, doVolunteer.Latitude, doVolunteer.Longitude, (BO.Role)doVolunteer.Role, doVolunteer.IsActive,
            doVolunteer.MaxDistanceCall, doVolunteer.Distance_Type, );
    }

    public int Id { get; init; }
    public Role Role { get; set; }
    public bool IsActive { get; set; }
    public double? MaxDistance { get; set; }
    public Distance_Type Distance_Type { get; set; }
    public int NumCallsHanle { get; set; }
    public int NumCallsCancele { get; set; }
    public int NumCallsNotValid { get; set; }
    public BO.CallInProgress? Call { get; set; }

    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive = null, BO.Volunteer_In_List_Fields? sort = null)
    {
        throw new NotImplementedException();
    }

    public void Update(int id, BO.Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}
