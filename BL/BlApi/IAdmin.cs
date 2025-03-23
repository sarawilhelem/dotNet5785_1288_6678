

namespace BlApi;

public interface IAdmin
{
    public DateTime GetClock();
    public void AdvanceClock(BO.Time_Unit timeUnit);
    public TimeSpan GetRiskRange();
    public void SetRiskRange(TimeSpan riskRange);
    public void Reset();
    public void Initialization();
}
