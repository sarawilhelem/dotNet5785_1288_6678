using DalApi;

namespace Helpers;

internal class AssignmentManager
{
    private static IDal s_dal = Factory.Get;
    public static void UpdateAssignment(DO.Assignment assignment)
    {
        s_dal.Assignment.Update(assignment);
    }
    public static void CreateAssignment(int callId,int volunteerId)
    {
        var assignment = new DO.Assignment
        {
            CallId = callId,
            VolunteerId = volunteerId,
            OpenTime = DateTime.Now,
            FinishTime=null,
            FinishType = null
        };
        s_dal.Assignment.Create(assignment);
    }

}
