

namespace Dal;
using DalApi;
using DO;
using System.Diagnostics;

/// <summary>
/// Inheriting and realizing IDal = Interface which contains fields of the entities' interfaces and declare their reset 
/// </summary>
sealed internal class DalXml : IDal
{
    /// <summary>
    /// A field who keep the only one dalXml;
    /// </summary>
    public static IDal Instance { get; } = new DalXml();

    /// <summary>
    /// private constructor
    /// </summary>
    private DalXml() { }

    /// <summary>
    /// A VolunteerImplantation field
    /// </summary>
    public IVolunteer Volunteer { get; } = new VolunteerImplementation();

    /// <summary>
    /// A CallImplantation field
    /// </summary>

    public ICall Call { get; } = new CallImplementation();

    /// <summary>
    /// An AssignmentImplantation field
    /// </summary>
    public IAssignment Assignment { get; } = new AssignmenImplementaion();

    /// <summary>
    /// A ConfigImplantation field
    /// </summary>
    public IConfig Config { get; } = new ConfigImplementation();

    /// <summary>
    /// Reseting all entities through the fields
    /// </summary>
    public void ResetDB()
    {
        Volunteer.DeleteAll();
        Call.DeleteAll();
        Assignment.DeleteAll();
        Config.Reset();
    }
}
