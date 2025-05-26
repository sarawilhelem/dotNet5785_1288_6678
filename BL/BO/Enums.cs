namespace BO;
public enum Role
{
    Manager,
    Volunteer,
    All
}


public enum DistanceType
{
    Air,
    Walk,
    Drive,
    All
}

public enum CallStatus
{
    InProcess,
    InProcessInRiskRange,
    All
}

public enum CallType
{
    TakeCareAtHome,
    TakeCareOut,
    Physiotherapy,
    All
}
public enum FinishType
{
    Processed,
    SelfCancel,
    ManagerCancel,
    Expired,
    All
}
public enum FinishCallType
{
    Open,
    InProcess,
    Close, 
    OpenInRisk,
    InProcessInRisk,
    Expired,
    All
}

public enum VolunteerInListFields
{
    Id,
    Name,
    IsActive,
    NumCallsHandle,
    NumCallsCancel,
    NumCallsNotValid,
    CallId,
    CallType,
    None
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
    AmountOfAssignments,
    None
}

public enum ClosedCallInListFields
{
    Id,
    CallType,
    Addres,
    OpenCallTime,
    StartCallTime,
    FinishCallTime,
    FinishType,
    None
}
public enum OpenCallInListFields
{
    Id,
    CallType,
    Address,
    OpenTime,
    MaxCloseTime,
    Distance,
    None
}
public enum TimeUnit
{
    Minute,
    Hour,
    Day,
    Month,
    Year
}