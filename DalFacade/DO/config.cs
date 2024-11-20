

namespace DO;

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
