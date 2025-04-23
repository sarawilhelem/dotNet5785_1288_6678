using BlApi;
using DalApi;
using Helpers;

namespace BlImplementation;

internal class AdminImplentaition : IAdmin
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// get the system clock
    /// </summary>
    /// <returns>system's current clock</returns>
    public DateTime GetClock()
    {
        return ClockManager.Now;
    }

    /// <summary>
    /// Initialization db and clock
    /// </summary>
    public void Initialization()
    {
        DalTest.Initialization.Do();
        ClockManager.UpdateClock(ClockManager.Now);

    }

    /// <summary>
    /// advance the clock by a unit time
    /// </summary>
    /// <param name="timeUnit">the unit time to advance clock: second/ minute/ hour/ day/ mont / year</param>

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

    /// <summary>
    /// get the risk range
    /// </summary>
    /// <returns>risk range</returns>
    public TimeSpan GetRiskRange()
    {
        return _dal.Config.RiskRange;
    }

    /// <summary>
    /// Reset db and clock
    /// </summary>
    public void Reset()
    {
        _dal.ResetDB();
        ClockManager.UpdateClock(ClockManager.Now);
    }

    /// <summary>
    /// set the risk range
    /// </summary>
    /// <param name="riskRange">the new risk range</param>
    public void SetRiskRange(TimeSpan riskRange)
    {
        _dal.Config.RiskRange = riskRange;
    }
}
