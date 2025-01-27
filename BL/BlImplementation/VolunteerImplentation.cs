
using BlApi;
using Helpers;
using System.Net;


namespace BlImplementation;

internal class VolunteerImplentation : IVolunteer
{
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
            VolunteerManager.Create(doVolunteer);
        }
        catch
        {
            throw new Exception($"volunteer with Id {volunteer.Id} already exist");
        }
    }

   
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Role EnterSystem(string name, string password)
    {
        throw new NotImplementedException();
    }

    public BO.Volunteer Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive = null, BO.Volunteer_In_List_Fields? sort = null)
    {
        throw new NotImplementedException();
    }

    public void Update(int id, BO.Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}
