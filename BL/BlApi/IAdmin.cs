

namespace BlApi;

public interface IAdmin
{
    public DateTime GetClock();
    public void PromoteClock(BO.Time_Unit timeUnit);
    public TimeSpan ReadRiskRange();
    public void SetRiskRange(TimeSpan riskRange);
    public void Reset();
    public void Initialization();
}
