using DalApi;
using DO;

namespace Dal;
/// <summary>
/// Realization of volunteer CRUD
/// </summary>
internal class VolunteerImplementation : IVolunteer
{
    /// <summary>
    /// create a new volunteer and add it to the list
    /// </summary>
    /// <param name="item">a volunteer to add to the list</param>
    /// <exception cref="DalAlreadyExistsException">throwed when there is not volunteer with that id</exception>
    public void Create(Volunteer item)
    {
        if (DataSource.Volunteers.Any(v => v.Id == item.Id))
            throw new DalAlreadyExistsException($"Volunteer with id {item.Id} is yet exist");
        DataSource.Volunteers.Add(item);
    }

    /// <summary>
    /// delete volunteer with the id
    /// </summary>
    /// <param name="id">id of volunteer</param>
    /// <exception cref="DalDeleteImpossible">throwed when there is not volunteer with that id</exception>

    public void Delete(int id)
    {
        Volunteer? thisVolunteer = DataSource.Volunteers.Find(v => v.Id == id);
        if (DataSource.Volunteers.RemoveAll(it => it.Id == id) == 0)
            throw new DalDeleteImpossible($"Volunteer with id {id} is not exist");
    }

    /// <summary>
    /// delete all volunteers
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    /// <summary>
    /// read volunteer according to id
    /// </summary>
    /// <param name="id">id of a volunteer</param>
    /// <returns>a volunteer with that id</returns>
    public Volunteer? Read(int id)
    {
        return DataSource.Volunteers.FirstOrDefault(v => v.Id == id);
    }

    /// <summary>
    /// read first volunteer in datasource.volunteers which return true to filter function
    /// </summary>
    /// <param name="filter">function which get a volunteer and returns true of false</param>
    /// <returns>first volunteer in datasource.volunteers which return true to filter function</returns>
    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        return DataSource.Volunteers.FirstOrDefault(v => filter(v));
    }

    /// <summary>
    /// read all volunteers which the filter returns true to them
    /// </summary>
    /// <param name="filter">function which get a volunteer and returns true of false. Can be null</param>
    /// <returns>all volunteers which the filter returns true to them, of all the volunteer if filter is null</returns>
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null) //stage 2
    {
        return filter == null
            ? DataSource.Volunteers.Select(item => item)
            : DataSource.Volunteers.Where(filter);

    }

    /// <summary>
    /// Updates the volunteer with id = item.id to be like the item parameter
    /// </summary>
    /// <param name="item">a volunteer</param>
    /// <exception cref="DalDoesNotExistException">throwed when there is not volunteer with that id</exception>
    public void Update(Volunteer item)
    {
        if (DataSource.Volunteers.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Volunteer with id {item.Id} is not exist");
        DataSource.Volunteers.Add(item);
    }
}
