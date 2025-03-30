
using Microsoft.VisualBasic;

namespace BO;

public enum Role
{
    Manager,
    Volunteer
}


public enum DistanceType
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

public enum CallType
{
    TakeCareAtHome,
    TakeCareOut,
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

public enum VolunteerInListFields
{
    Id,
    Name,
    IsActive,
    NumCallsHanle,
    NumCallsCancele,
    NumCallsNotValid,
    CallId,
    CallType
}
public enum CallInListFields
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

public enum ClosedCallInListFields
{
    Id,
    CallType,
    Addres,
    OpenCallTime,
    StartCallTime,
    FinishCallTime,
    FinishType
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
public enum TimeUnit
{
    Minute,
    Hour,
    Day,
    Month,
    Year
}