using DalApi;

using DO;

namespace Dal;
internal class CallImplementation : ICall
{
    public void Create(Call item)
    {
        int id = Config.NextCallId;
        Call newCall = item with { Id = id };
        DataSource.Calls.Add(item);
    }

    public void Delete(int id)
    {
        Call? thisCall = DataSource.Calls.Find(c => c.Id == id);
        if (thisCall != null)
        {
            DataSource.Calls.Remove(thisCall);
        }
        else
            throw new DalDeleteImpossible($"Call with id {id} is not exist");

    }

    public void DeleteAll()
    {
       DataSource.Calls.Clear();
    }

    public Call? Read(int id)
    {
        return DataSource.Calls.FirstOrDefault(c => c.Id == id);


    }

    public Call? Read(Func<Call, bool> filter)
    {
        // return first call in datasource.calls which return true to filter function
        return DataSource.Calls.FirstOrDefault(c => filter(c));
    }

    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null) //stage 2
    {
        return filter == null
            ? DataSource.Calls.Select(item => item)
            : DataSource.Calls.Where(filter);

    }


    public void Update(Call item)
    {
        Call? thisCall = DataSource.Calls.Find(c => c.Id == item.Id);
        if (thisCall != null)
        {
            DataSource.Calls.Remove(thisCall);
            DataSource.Calls.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Call with id {item.Id} is not exist");

    }
}
