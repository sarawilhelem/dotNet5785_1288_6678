

namespace Dal;
/// <summary>
/// lists of the entities
/// </summary>
internal static class DataSource
{
    /// <summary>
    /// A list of the assignments
    /// </summary>
    internal static List<DO.Assignment> Assignments { get; } = [];

    /// <summary>
    /// A list of the calls
    /// </summary>
    internal static List<DO.Call> Calls { get; set; } = [];

    /// <summary>
    /// A list of the volunteers
    /// </summary>
    internal static List<DO.Volunteer> Volunteers { get; } = []; 
}