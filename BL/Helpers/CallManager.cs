

using BlApi;
using BO;
using DalApi;
using System.Formats.Tar;
using System.Reflection.Metadata.Ecma335;

namespace Helpers;

internal class CallManager
{
    private static IDal s_dal = DalApi.Factory.Get;
    public static IEnumerable<DO.Assignment> AssignmentsListForCall(int id)
    {
        var assignmentsList = s_dal.Assignment.ReadAll(a => a.CallId ==id);
        return assignmentsList;
    }
    public static IEnumerable<DO.Assignment> AssignmentsListForVolunteer(int id)
    {
        var assignmentsList = s_dal.Assignment.ReadAll(a => a.VolunteerId == id);
        return assignmentsList;
    }
    public static DO.Volunteer GetVolunteer(int id)
    {
        return s_dal.Volunteer.Read(v => v.Id == id) ??
            throw new BO.BlDoesNotExistException($"Volunteer with id {id} does not exists");
    }
   public static string? GetLastVolunteerName(DO.Call call)
    {
        var assignmentsList= AssignmentsListForCall(call.Id).OrderByDescending(a => a.OpenTime);
        var lastAssignment= assignmentsList.FirstOrDefault();
        if (lastAssignment is null)
            return null;
        var volunteer = GetVolunteer(lastAssignment.VolunteerId);
        return volunteer.Name;
    }
    public static TimeSpan? RestTimeForCall(DO.Call call)
    {
        return DateTime.Now > call.MaxCloseTime ? (TimeSpan?)(call.MaxCloseTime - DateTime.Now) : null;
    }
    public static FinishCallType GetCallStatus(int id)
    {
        var call= s_dal.Call.Read(a => a.Id == id);
        var assignmentsList = s_dal.Assignment.ReadAll(a => a.CallId == id);
        var Processed = assignmentsList.Select(a => a.FinishType == DO.FinishType.Processed);
        if (Processed!=null)
        {
            return FinishCallType.InProcess;
        }
        else
        {
            if (call is null || RestTimeForCall(call) == null)
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
    public static double DistanceBetweenVolunteerAndCall(int volunteerId,int callId)
    {
        var volunteer = s_dal.Volunteer.Read(v => v.Id == volunteerId);
        var call = s_dal.Call.Read(c => c.Id == callId);
        var distance =Math.Sqrt (Math.Pow(((double)(volunteer!.Latitude!).Value - call!.Latitude),2)+ Math.Pow(((double)(volunteer!.Longitude!).Value - call.Longitude),2));
        return distance;
    }

public static int GetAmountOfAssignments(DO.Call call)
    {
        var assignmentsList = s_dal.Assignment.ReadAll(a => a.CallId == call.Id);
        return assignmentsList.Count();
    }
    public static void UpdateCall(DO.Call call)
    {
            s_dal.Call.Update(call);
    }
    public static void DeleteCall(int id)
    {
        s_dal.Call.Delete(id);
    }
    public static void CreateCall(DO.Call call)
    {
        s_dal.Call.Create(call);
    }
}
