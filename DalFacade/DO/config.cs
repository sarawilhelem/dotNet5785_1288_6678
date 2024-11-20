
namespace DO;

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
	private static int nextCallId = 1;
	public static int NextCallId
	{
		get { return nextCallId++; }
	}


    private static int nextAssignment = 1;

    public static int NextAssignment
    {
        get { return nextAssignment++; }

    }
    static DateTime Clock;
    static TimeSpan RiskRange;
}
