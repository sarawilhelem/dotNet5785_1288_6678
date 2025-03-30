using BlApi;
using DalApi;
using Helpers;

namespace BlImplementation;

internal class AdminImplentaition : IAdmin
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public DateTime GetClock()
    {
        return ClockManager.Now;
    }

    public void Initialization()
    {
        DalTest.Initialization.Do();
        ClockManager.UpdateClock(ClockManager.Now);

    }

    public void AdvanceClock(BO.TimeUnit timeUnit)
    {
        switch (timeUnit)
        {
            case BO.TimeUnit.Minute:
                ClockManager.UpdateClock(ClockManager.Now.AddMinutes(1));
                break;
            case BO.TimeUnit.Hour:
                ClockManager.UpdateClock(ClockManager.Now.AddHours(1));
                break;
            case BO.TimeUnit.Day:
                ClockManager.UpdateClock(ClockManager.Now.AddDays(1));
                break;
            case BO.TimeUnit.Month:
                ClockManager.UpdateClock(ClockManager.Now.AddMonths(1));
                break;
            case BO.TimeUnit.Year:
                ClockManager.UpdateClock(ClockManager.Now.AddYears(1));
                break;
        }
    }

    public TimeSpan GetRiskRange()
    {
        return _dal.Config.RiskRange;
    }

    public void Reset()
    {
        _dal.ResetDB();
        ClockManager.UpdateClock(ClockManager.Now);
    }

    public void SetRiskRange(TimeSpan riskRange)
    {
        _dal.Config.RiskRange = riskRange;
    }
}
