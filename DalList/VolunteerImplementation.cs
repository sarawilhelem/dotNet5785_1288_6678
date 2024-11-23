

namespace Dal;

using DAL;
using DalApi;
using DalApi.DO;
using DO;


public class VolunteerImplementation : IVolunteer
{
    public void Create(Volunteer item)
    {
        if(DataSource.Volunteers.Find(v => v.Id == item.Id)!=null)
            throw new Exception($"Volunteer with id {item.Id} is not exist");
        else
            DataSource.Volunteers.Add(item);
    }

    public void Delete(int id)
    {
        Volunteer? thisVolunteer = DataSource.Volunteers.Find(v => v.Id == item.Id);
        if (thisVolunteer != null)
        {
            DataSource.Volunteers.Remove(thisVolunteer);
        }
        else
            throw new Exception($"Volunteer with id {id} is not exist");
    }

    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    public Volunteer? Read(int id)
    {
        return DataSource.Volunteers.Find(v => v.Id == id);


    }

    public List<Volunteer> ReadAll()
    {
        List<Volunteer> list = new List<Volunteer>();
        foreach (Volunteer v in DataSource.Volunteers)
            list.Add(v);
        return list;
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
            throw new Exception($"Volunteer with id {item.Id} is not exist");

    }
}
