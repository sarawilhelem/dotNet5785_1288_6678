using DalApi;

namespace Helpers;

internal class AssignmentManager
{
    private static IDal s_dal = Factory.Get;
    /// <summary>
    /// create a new assignment between call and volunteer
    /// </summary>
    /// <param name="callId">the call Id</param>
    /// <param name="volunteerId">the volunteer id</param>
    public static void CreateAssignment(int callId,int volunteerId)
    {
        var assignment = new DO.Assignment
        {
            CallId = callId,
            VolunteerId = volunteerId,
            OpenTime = ClockManager.Now,
            FinishTime = null, 
            FinishType = null
        };
        s_dal.Assignment.Create(assignment);
    }

}
