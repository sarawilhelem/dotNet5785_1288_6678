

using BO;
using DalApi;
using System.Diagnostics;

namespace Helpers;

internal class CallManager
{
    private static IDal s_dal = Factory.Get;

    public static string? GetLastVolunteerName(DO.Call call)
    {
     
        var assignmentsList=s_dal.Assignment.ReadAll(a => a.CallId == call.Id).OrderByDescending(a => a.OpenTime);
        var lastAssignment= assignmentsList.FirstOrDefault();
           var volunteer = s_dal.Volunteer.Read(v=>v.Id==lastAssignment.VolunteerId);
        return volunteer.Name;
    }
    public static TimeSpan? RestTimeForCall(DO.Call call)
    {
        return DateTime.Now > call.MaxCloseTime ? (TimeSpan?)(call.MaxCloseTime - DateTime.Now) : null;
    }
    public static FinishCallType GetCallStatus(DO.Call call)
    {
        var assignmentsList = s_dal.Assignment.ReadAll(a => a.CallId == call.Id);
        var Processed = assignmentsList.Select(a => a.FinishType == DO.FinishType.Processed);
        if (Processed!=null)
        {
            return FinishCallType.InProcess;
        }
        else
        {
            if (RestTimeForCall(call) == null)
            {
                return FinishCallType.Expired;
            }
            else
            {
                var InProcess = assignmentsList.Select(a => a.FinishType == DO.FinishType.SelfCancel||a.FinishType== DO.FinishType.ManagerCancel);
                
                   if (assignmentsList != null&&InProcess==null)
                    {
                        if ((call.MaxCloseTime - DateTime.Now).TotalHours <= 50)
                            return FinishCallType.OpenAtRisk;
                        return FinishCallType.Open;
                    }
                
                else
                {
                    if ((call.MaxCloseTime - DateTime.Now).TotalHours <= 50)
                        return FinishCallType.InProcessAtRisk;
                    return FinishCallType.InProcess;
                }
            }
        }
    }
    public static TimeSpan? RestTimeForTreatment(DO.Call call)
    {
        var assignmentsProcessedList = s_dal.Assignment.Read(a => a.CallId == call.Id&&a.FinishType==DO.FinishType.Processed);
        if (assignmentsProcessedList != null)
            return (TimeSpan?)(assignmentsProcessedList.FinishTime - DateTime.Now);


        return null;

        
    }

public static int GetAmountOfAssignments(DO.Call call)
    {
        var assignmentsList = s_dal.Assignment.ReadAll(a => a.CallId == call.Id);
        return assignmentsList.Count();
    }
}
