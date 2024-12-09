

namespace Dal;

internal static class DataSource
{
    //lists of the entities
    internal static List<DO.Assignment> Assignments { get; } = new();   //A list of the assignments
    internal static List<DO.Call> Calls { get; set; } = new();  //A list of the calls
    internal static List<DO.Volunteer> Volunteers { get; } = new(); //A list of the volunteers


}
