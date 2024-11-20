namespace DO;

/// <summary>
/// Student Entity represents a student with all its props
/// </summary>
/// <param name="callId">Personal unique ID of the student (as in national id card)</param>
/// <param name="Name">Private Name of the student</param>
/// <param name="RegistrationDate">Registration date of the student into the graduation program</param>
/// <param name="Alias">student’s alias name (default empty)</param>
/// <param name="IsActive">whether the student is active in studies (default true)</param>
/// <param name="BirthDate">student’s birthday (default empty)</param>
public record Assignment
    (
    int callId,
    int volunteerId,
    
    DateTime? finishTime=null,
    Finish_Type? finishType=null
    )
{
    internal int Id = Config.NextAssignment;

    private DateTime insersiontTime;

    public int InsersiontTime { get; }
    


}

