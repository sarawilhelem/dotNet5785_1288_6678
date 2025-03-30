
namespace BlApi;

public interface IVolunteer
{
    public BO.Role EnterSystem(string name, string? password = null);
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive=null ,BO.VolunteerInListFields? sort =null );
    public BO.Volunteer Read(int id);
    public void Update(int id, BO.Volunteer volunteer);
    public void Delete(int id);
    public void Create(BO.Volunteer volunteer);
}
