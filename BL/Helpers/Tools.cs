
namespace Helpers;

static internal class Tools
{
    internal static bool IsWithinRiskRange(DateTime maxClose)
    {
        DateTime now = ClockManager.Now;
        TimeSpan range = AdminImplentation.RiskRange;
        DateTime rangeStart = maxClose.Add(-range);
        return now >= rangeStart && now <= maxClose;
    }
}
