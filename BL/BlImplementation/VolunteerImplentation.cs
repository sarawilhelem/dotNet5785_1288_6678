
using BlApi;


namespace BlImplementation;

internal class VolunteerImplentation : IVolunteer
{
    public void Add(BO.Volunteer volunteer)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Role EnterSystem(string name, string password)
    {
        throw new NotImplementedException();
    }

    public Volunteer Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<VolunteerInList> ReadAll(bool? isActive = null, Volunteer_In_List_Fields? sort = null)
    {
        throw new NotImplementedException();
    }

    public void Update(int id, Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}
