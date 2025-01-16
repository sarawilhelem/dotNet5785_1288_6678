using DO;
namespace DalApi;

/// <summary>
/// Interface which contains fields of the entities' interfaces and declare their reset 
/// </summary>
public interface IDal
{
    /// <summary>
    /// A field of  IVolunteer type
    /// </summary>
    IVolunteer Volunteer { get; }

    /// <summary>
    /// A field of  ICall type
    /// </summary>
    ICall Call { get; }

    /// <summary>
    /// A field of  IAssignment type
    /// </summary>
    IAssignment Assignment { get; }

    /// <summary>
    /// A field of  IConfig type
    /// </summary>
    IConfig Config { get; }

    /// <summary>
    /// Reset all fields
    /// </summary>
    void ResetDB(); 
}
