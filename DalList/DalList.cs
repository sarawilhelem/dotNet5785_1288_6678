

namespace Dal;
using DalApi;
using DO;
using System.Diagnostics;

sealed public class DalList : IDal
{
    //Class which contains all entities' implementations and has a function to reset them

    
    public IVolunteer Volunteer { get; } = new VolunteerImplementation();   //A field of VolunteerImplemantation type

    public ICall Call { get; } = new CallImplementation();  //A field of CallImplemantation type

    public IAssignment Assignment { get; } = new AssignmentImplementation();    //A field of AssignmentImplemantation type

    public IConfig Config { get; } = new ConfigImplementation();    //A field of ConfigImplemantation type

    public void ResetDB()
    {
        //Reset all the entities through the fields
        Volunteer.DeleteAll();
        Call.DeleteAll();
        Assignment.DeleteAll();
        Config.Reset();
    }
}
