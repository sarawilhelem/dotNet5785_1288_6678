using DalApi;

namespace Helpers;

internal class AssignmentManager
{
    private static IDal s_dal = Factory.Get;
    public static void UpdateAssignment(DO.Assignment assignment)
    {
        s_dal.Assignment.Update(assignment);
    }

}
