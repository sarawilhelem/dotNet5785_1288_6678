

namespace Dal;

internal static class DataSource
{
    //lists of the entities
    internal static List<DO.Assignment> Assignments { get; } = [];   //A list of the assignments
    internal static List<DO.Call> Calls { get; set; } = [];  //A list of the calls
    internal static List<DO.Volunteer> Volunteers { get; } = []; //A list of the volunteers


}
