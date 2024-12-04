using DalApi;
using DO;

namespace Dal;

internal class VolunteerImplementation : IVolunteer
{
    public void Create(Volunteer item)
    {
        if (DataSource.Volunteers.Find(v => v.Id == item.Id) != null)
            throw new DalAlreadyExistsException($"Volunteer with id {item.Id} is yet exist");
        else
            DataSource.Volunteers.Add(item);
    }

    public void Delete(int id)
    {
        Volunteer? thisVolunteer = DataSource.Volunteers.Find(v => v.Id == id);
        if (thisVolunteer != null)
        {
            DataSource.Volunteers.Remove(thisVolunteer);
        }
        else
            throw new DalDeleteImpossible($"Volunteer with id {id} is not exist");
    }

    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    public Volunteer? Read(int id)
    {
        return DataSource.Volunteers.FirstOrDefault(v => v.Id == id);

    }

    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        // return first volunteer in datasource.volunteers which return true to filter function
        return DataSource.Volunteers.FirstOrDefault(v => filter(v));
    }

    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null) //stage 2
    {
        return filter == null
            ? DataSource.Volunteers.Select(item => item)
            : DataSource.Volunteers.Where(filter);

    }

    public void Update(Volunteer item)
    {
        Volunteer? thisVolunteer = DataSource.Volunteers.Find(c => c.Id == item.Id);
        if (thisVolunteer != null)
        {
            DataSource.Volunteers.Remove(thisVolunteer);
            DataSource.Volunteers.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Volunteer with id {item.Id} is not exist");

    }
}
