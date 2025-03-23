
using Microsoft.VisualBasic;

namespace BO;

public enum Role
{
    Manager,
    Volunteer
}


public enum Distance_Type
{
    Air,
    Walk,
    Drive
}

public enum CallStatus
{
    InProcess,
    InProcessInRiskRange
}

public enum Call_Type
{
    Take_Care_At_Home,
    Take_Care_Out,
    Physiotherapy
}
public enum FinishType
{
    Processed,
    SelfCancel,
    ManagerCancel,
    Expired
}
public enum FinishCallType
{
    Open,
    InProcess,
    Close,
    OpenAtRisk,
    InProcessAtRisk,
    Expired
}

public enum Volunteer_In_List_Fields
{
    Id,
    Name,
    IsActive,
    NumCallsHanle,
    NumCallsCancele,
    NumCallsNotValid,
    CallId,
    Call_Type
}
public enum Call_In_List_Fields
{
    Id,
    CallId,
    CallType,
    OpenTime,
    MaxCloseTime,
    LastVolunteerName,
    TotalProcessingTime,
    Status,
    AmountOfAssignments
}

public enum Closed_Call_In_List_Fields
{
    Id,
    CallType,
    Addres,
    OpenCallTime,
    StartCallTime,
    FinishCallTime,
    Finish_Type
}
public enum OpenCallInListFields
{
     Id,
    CallType ,
    Address,
    OpenTime ,
    MaxCloseTime,
    Distance
}
public enum Time_Unit
{
    Minute,
    Hour,
    Day,
    Month,
    Year
}