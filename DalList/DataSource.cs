

namespace Dal;
/// <summary>
/// lists of the entities
/// </summary>
public static class DataSource
{
    /// <summary>
    /// A list of the assignments
    /// </summary>
    public static List<DO.Assignment> Assignments { get; } = [];

    /// <summary>
    /// A list of the calls
    /// </summary>
    public static List<DO.Call> Calls { get; set; } = [];

    /// <summary>
    /// A list of the volunteers
    /// </summary>
    public static List<DO.Volunteer> Volunteers { get; } = []; 
}