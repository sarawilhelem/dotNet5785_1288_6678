namespace Dal;

/// <summary>
/// Configuration Entity provide 4 static fields to other records in the volunteers organization
/// </summary>
/// <param name="nextCallId">keep the next id for calls</param>
/// <param name="nextAssignment">keep the next id for assignment</param>
/// <param name="CourseNumber">Course official number</param>
/// <param name="clock">manage the program's clock</param>
/// <param name="riskRange">keep the range time before the finishTime- it is in risk</param>

static internal class Config
{
    internal const int startCallId = 1000;
    private static int nextCallId = startCallId;
    internal static int NextCallId { get => nextCallId++; }

    internal const int startAssigmentId = 2000;
    private static int nextAssignmentId = startAssigmentId;
    internal static int NextAssignmentId { get => nextAssignmentId++; }

    internal static DateTime Clock { get; set; } = new DateTime(2024, 1, 1, 0, 0, 0);
    public static TimeSpan RiskRange { get; set; }

    internal static void Reset()
    {
        nextCallId = startCallId;
        nextAssignmentId = startAssigmentId;
        Clock = new DateTime(2024, 1, 1);
        RiskRange = new TimeSpan(24, 0, 0);
    }
}

