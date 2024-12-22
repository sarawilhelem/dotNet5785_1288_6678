

namespace Dal;
using DalApi;
using DO;

public class DalXml : IDal
{
    //Inheriting and realizing IDal = Interface which contains fields of the entities' interfaces and declare their reset 
    public IVolunteer Volunteer { get; } = new VolunteerImplementation();   //A VolunteerImplantation field

    public ICall Call { get; } = new CallImplementation();  //A CallImplantation field

    public IAssignment Assignment { get; } = new AssignmenImplementaion();  //A AssignmentImplantation field

    public IConfig Config { get; } = new ConfigImplementation();    //A ConfigImplantation field

    public void ResetDB()
    {
        //Reseting all entities through the fields

        Volunteer.DeleteAll();
        Call.DeleteAll();
        Assignment.DeleteAll();
        Config.Reset();
    }
}
