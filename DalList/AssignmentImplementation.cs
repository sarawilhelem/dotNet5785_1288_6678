using DalApi;
using DO;


namespace Dal;
internal class AssignmentImplementation : IAssignment
{
    //  Realization of assignment CRUD
    public void Create(Assignment item)
    {
        //create new assignment
        int id = Config.NextAssignmentId;
        Assignment newAssignment = item with { Id = id };
        DataSource.Assignments.Add(newAssignment);
    }

    public void Delete(int id)
    {
        //delete assignment according to id
        Assignment? thisAssignment = DataSource.Assignments.Find(a => a.Id == id);
        if (thisAssignment != null)
        {
            DataSource.Assignments.Remove(thisAssignment);
        }
        else
            throw new DalDeleteImpossible($"Assignment with id {id} is not exist");
    }

    public void DeleteAll()
    {
        //delete all assignments
        DataSource.Assignments.Clear();
    }

    public Assignment? Read(int id)
    {
        //read assignment according to id
        return DataSource.Assignments.FirstOrDefault(a => a.Id == id); //stage2

    }

    public Assignment?  Read(Func<Assignment, bool> filter)
    {
        // return first assignment in datasource.assignments which return true to filter function
        return DataSource.Assignments.FirstOrDefault(a => filter(a));
    }
    

    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null) //stage 2
    {
        //read all assignments
        return filter == null
            ? DataSource.Assignments.Select(item => item)
            : DataSource.Assignments.Where(filter);

    }


    public void Update(Assignment item)
    {
        //update assignment by item id
        Assignment? thisAssignment = DataSource.Assignments.Find(a => a.Id == item.Id);
        if (thisAssignment != null)
        {
            DataSource.Assignments.Remove(thisAssignment);
            DataSource.Assignments.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Assignment with id {item.Id} is not exist");


    }
}
