
namespace BlApi;

public interface IVolunteer: IObservable
{
    public BO.Volunteer EnterSystem(string name, string? password = null);
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive=null ,BO.VolunteerInListFields? sort =null );
    public BO.Volunteer Read(int id);
    public void Update(int id, BO.Volunteer volunteer);
    public void Delete(int id);
    public void Create(BO.Volunteer volunteer);
}
