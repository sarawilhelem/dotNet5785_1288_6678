


using DalApi;
using DO;


namespace Dal;
public class AssignmentImplementation : IAssignment
{
    public void Create(Assignment item)
    {
        int id = DAL.Config.NextAssignmentId;
        Assignment newAssignment = item with { Id = id };
        DataSource.Assignments.Add(item);
    }

    public void Delete(int id)
    {
        Assignment? thisAssignment = DataSource.Assignments.Find(a => a.Id == id);
        if (thisAssignment != null)
        {
            DataSource.Assignments.Remove(thisAssignment);
        }
        else
            throw new Exception($"Assignment with id {id} is not exist");
    }

    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }

    public Assignment? Read(int id)
    {
        return DataSource.Assignments.Find(a => a.Id == id);

    }

    public List<Assignment> ReadAll()
    {
        List<Assignment> list = new List<Assignment>();
        foreach (Assignment a in DataSource.Assignments)
            list.Add(a);
        return list;
    }


    public void Update(Assignment item)
    {
        Assignment? thisAssignment = DataSource.Assignments.Find(a => a.Id == item.Id);
        if (thisAssignment != null)
        {
            DataSource.Assignments.Remove(thisAssignment);
            DataSource.Assignments.Add(item);
        }
        else
            throw new Exception($"Assignment with id {item.Id} is not exist");


    }
}
