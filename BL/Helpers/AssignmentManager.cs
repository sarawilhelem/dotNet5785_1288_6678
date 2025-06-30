using DalApi;

namespace Helpers;

internal class AssignmentManager
{
    private static IDal s_dal = Factory.Get;

    internal static ObserverManager Observers = new();
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
            OpenTime = AdminManager.Now,
            FinishTime = null, 
            FinishType = null
        };
        s_dal.Assignment.Create(assignment);
        AssignmentManager.Observers.NotifyListUpdated();
    }
    public static void Update(DO.Assignment assignmentToUpdate)
    {
        s_dal.Assignment.Update(assignmentToUpdate);
    }

}
