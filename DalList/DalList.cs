

namespace Dal;
using DalApi;
using DO;
using System.Diagnostics;
using System.Runtime.CompilerServices;

/// <summary>
/// Class which contains all entities' implementations and has a function to reset them
/// </summary>
sealed internal class DalList : IDal
{

    /// <summary>
    /// A field who keep the only one dalllist
    /// </summary>
    public static IDal Instance { get; } = new DalList();

    /// <summary>
    /// private constructor
    /// </summary>
    private DalList() { }

    /// <summary>
    /// A field of lazy VolunteerImplemantation type
    /// </summary>
    public IVolunteer Volunteer { get; } = new VolunteerImplementation();

    /// <summary>
    /// A field of CallImplemantation type
    /// </summary>
    public ICall Call { get; } = new CallImplementation();

    /// <summary>
    /// A field of AssignmentImplemantation type
    /// </summary>
    public IAssignment Assignment { get; } = new AssignmentImplementation();


    /// <summary>
    /// A field of ConfigImplemantation type
    /// </summary>
    public IConfig Config { get; } = new ConfigImplementation();

    /// <summary>
    /// Reset all the entities through the fields
    /// </summary>
    public void ResetDB()
    {
        Volunteer.DeleteAll();
        Call.DeleteAll();
        Assignment.DeleteAll();
        Config.Reset();
    }
}



///// <summary>
///// A field of lazy VolunteerImplemantation type
///// </summary>
//public IVolunteer Volunteer { get; } = new Lazy<IVolunteer>(() => new VolunteerImplementation()).Value;

///// <summary>
///// A field of CallImplemantation type
///// </summary>
//public ICall Call { get; } = new Lazy<ICall>(() => new CallImplementation()).Value;

///// <summary>
///// A field of AssignmentImplemantation type
///// </summary>
//public IAssignment Assignment { get; } = new Lazy<IAssignment>(() => new AssignmentImplementation()).Value;


///// <summary>
///// A field of ConfigImplemantation type
///// </summary>
//public IConfig Config { get; } = new Lazy<IConfig>(() => new ConfigImplementation()).Value;
