

using DO;

namespace DalApi;

public interface IDal
{
    //Interface which contains fields of the entities' interfaces and declare their reset 
    IVolunteer Volunteer { get; }   //A field of  IVolunteer type
    ICall Call { get; } //A field of  ICall type
    IAssignment Assignment { get; } //A field of  IAssignment type
    IConfig Config { get; } //A field of  IConfig type
    void ResetDB(); //Reset all fields
}
