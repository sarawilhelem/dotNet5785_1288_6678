using DalApi;

using DO;

namespace Dal;

/// <summary>
/// Realization of call CRUD
/// </summary>
internal class CallImplementation : ICall
{
    /// <summary>
    /// create a new call and add to the list
    /// </summary>
    /// <param name="item">a call object to add to the list</param>
    public void Create(Call item)
    {
        int id = Config.NextCallId;
        Call newCall = item with { Id = id };
        DataSource.Calls.Add(newCall);
    }

    /// <summary>
    /// delete call with the id
    /// </summary>
    /// <param name="id">id of call</param>
    /// <exception cref="DalDeleteImpossible">throwed when there is not call with that id</exception>
    public void Delete(int id)
    { 
        Call? thisCall = DataSource.Calls.Find(c => c.Id == id);
        if (DataSource.Calls.RemoveAll(it => it.Id == id) == 0)
            throw new DalDeleteImpossible($"Call with id {id} is not exist");

    }

    /// <summary>
    /// delete all calls
    /// </summary>
    public void DeleteAll()
    {
       DataSource.Calls.Clear();
    }

    /// <summary>
    /// read call according to id
    /// </summary>
    /// <param name="id">id of a call</param>
    /// <returns>a call with that id</returns>
    public Call? Read(int id)
    {
        return DataSource.Calls.FirstOrDefault(c => c.Id == id);
    }

    /// <summary>
    /// read first call in datasource.calls which return true to filter function
    /// </summary>
    /// <param name="filter">function which get a call and returns true of false</param>
    /// <returns>first call in datasource.calls which return true to filter function</returns>
    public Call? Read(Func<Call, bool> filter)
    {
        return DataSource.Calls.FirstOrDefault(c => filter(c));
    }

    /// <summary>
    /// read all calls which the filter returns true to them
    /// </summary>
    /// <param name="filter">function which get a call and returns true of false. Can be null</param>
    /// <returns>all calls which the filter returns true to them, of all the call if filter is null</returns>
    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null) //stage 2

    {
        return filter == null
            ? DataSource.Calls.Select(item => item)
            : DataSource.Calls.Where(filter);

    }

    /// <summary>
    /// Updates the call with id = item.id to be like the item parameter
    /// </summary>
    /// <param name="item">a call</param>
    /// <exception cref="DalDoesNotExistException">throwed when there is not call with that id</exception>
    public void Update(Call item)
    {
        //update call according to id of item
        if (DataSource.Calls.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Call with id {item.Id} is not exist");
        DataSource.Calls.Add(item);
    }
}