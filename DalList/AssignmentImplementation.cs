using DalApi;
using DO;
namespace Dal;

/// <summary>
/// Realization of assignment CRUD
/// </summary>
internal class AssignmentImplementation : IAssignment
{

    /// <summary>
    /// create a new assignment and add to the list
    /// </summary>
    /// <param name="item">an assignment object to add to the list</param>
    public void Create(Assignment item)
    {
        int id = Config.NextAssignmentId;
        Assignment newAssignment = item with { Id = id };
        DataSource.Assignments.Add(newAssignment);
    }

    /// <summary>
    /// delete assignment with the id
    /// </summary>
    /// <param name="id">id of assignment</param>
    /// <exception cref="DalDeleteImpossible">throwed when there is not assignment with that id</exception>
    public void Delete(int id)
    {
        Assignment? thisAssignment = DataSource.Assignments.Find(a => a.Id == id);
        if (DataSource.Assignments.RemoveAll(it => it.Id == id) == 0)
            throw new DalDeleteImpossible($"Assignment with id {id} is not exist");
    }

    /// <summary>
    /// delete all assignments
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }

    /// <summary>
    /// read assignment according to id
    /// </summary>
    /// <param name="id">id of an assignment</param>
    /// <returns>an assignment with that id</returns>
    public Assignment? Read(int id)
    {
        return DataSource.Assignments.FirstOrDefault(a => a.Id == id); //stage2
    }

    /// <summary>
    /// read first assignment in datasource.assignments which return true to filter function
    /// </summary>
    /// <param name="filter">function which get an assignment and returns true of false</param>
    /// <returns>first assignment in datasource.assignments which return true to filter function</returns>
    public Assignment?  Read(Func<Assignment, bool> filter)
    {
        return DataSource.Assignments.FirstOrDefault(a => filter(a));
    }

    /// <summary>
    /// read all assignments which the filter returns true to them
    /// </summary>
    /// <param name="filter">function which get an assignment and returns true of false. Can be null</param>
    /// <returns>all assignments which the filter returns true to them, of all the assignments if filter is null</returns>
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null) 
    {
        return filter == null
            ? DataSource.Assignments.Select(item => item)
            : DataSource.Assignments.Where(filter);

    }

    /// <summary>
    /// Updates the assignment with id = item.id to be like the item parameter
    /// </summary>
    /// <param name="item">an assignment</param>
    /// <exception cref="DalDoesNotExistException">throwed when there is not assignment with that id</exception>
    public void Update(Assignment item)
    {
        if (DataSource.Assignments.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Assignment with id {item.Id} is not exist");
        DataSource.Assignments.Add(item);

    }
}
