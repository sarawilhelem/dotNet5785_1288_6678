

namespace Dal;

using DAL;
using DalApi;
using DalApi.DO;
using DO;


public class CallImplementation : ICall
{
    public void Create(Call item)
    {
        int id = Config.NextCallId;
        Call newCall = item with { id = id };
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
            throw new Exception($"Call with id {id} is not exist");

    }

    public void DeleteAll()
    {
       DataSource.Calls.Clear();
    }

    public Call? Read(int id)
    {
        return DataSource.Calls.Find(c => c.Id == id);


    }

    public List<Call> ReadAll()
    {
        List<Call> list = new List<Call>();
        foreach (Call c in DataSource.Calls)
            list.Add(c);
        return list;
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
            throw new Exception($"Call with id {item.Id} is not exist");

    }
}
