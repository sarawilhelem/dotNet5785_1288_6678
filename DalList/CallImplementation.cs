using DalApi;

using DO;

namespace Dal;
internal class CallImplementation : ICall
{
    //Realization of call CRUD
    public void Create(Call item)
    {
        //create new call
        int id = Config.NextCallId;
        Call newCall = item with { Id = id };
        DataSource.Calls.Add(newCall);

    }

    public void Delete(int id)
    {
        //delete call according id
        Call? thisCall = DataSource.Calls.Find(c => c.Id == id);
        if (DataSource.Calls.RemoveAll(it => it.Id == id) == 0)
            throw new DalDeleteImpossible($"Call with id {id} is not exist");

    }

    public void DeleteAll()
    {
        //delete all calls
       DataSource.Calls.Clear();
    }

    public Call? Read(int id)
    {
        //read call according to id
        return DataSource.Calls.FirstOrDefault(c => c.Id == id);


    }

    public Call? Read(Func<Call, bool> filter)
    {
        // return first call in datasource.calls which return true to filter function
        return DataSource.Calls.FirstOrDefault(c => filter(c));
    }

    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null) //stage 2

    {
        //accept all calls
        return filter == null
            ? DataSource.Calls.Select(item => item)
            : DataSource.Calls.Where(filter);

    }


    public void Update(Call item)
    {
        //update call according to id of item
        if (DataSource.Calls.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Call with id {item.Id} is not exist");
        DataSource.Calls.Add(item);
    }
}
