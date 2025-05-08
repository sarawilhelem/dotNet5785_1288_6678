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
        return AdminManager.Now;
    }

    /// <summary>
    /// Initialization db and clock
    /// </summary>
    public void InitializationDB()
    {
        DalTest.Initialization.Do();
        AdminManager.UpdateClock(AdminManager.Now);

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
                AdminManager.UpdateClock(AdminManager.Now.AddMinutes(1));
                break;
            case BO.TimeUnit.Hour:
                AdminManager.UpdateClock(AdminManager.Now.AddHours(1));
                break;
            case BO.TimeUnit.Day:
                AdminManager.UpdateClock(AdminManager.Now.AddDays(1));
                break;
            case BO.TimeUnit.Month:
                AdminManager.UpdateClock(AdminManager.Now.AddMonths(1));
                break;
            case BO.TimeUnit.Year:
                AdminManager.UpdateClock(AdminManager.Now.AddYears(1));
                break;
        }
    }

    /// <summary>
    /// get the risk range
    /// </summary>
    /// <returns>risk range</returns>
    public TimeSpan GetRiskRange()
    {
        return AdminManager.RiskRange;
    }

    /// <summary>
    /// Reset db and clock
    /// </summary>
    public void ResetDB()
    {
        _dal.ResetDB();
        AdminManager.UpdateClock(AdminManager.Now);
    }

    /// <summary>
    /// set the risk range
    /// </summary>
    /// <param name="riskRange">the new risk range</param>
    public void SetRiskRange(TimeSpan riskRange)
    {
        AdminManager.RiskRange = riskRange;
    }

    public void AddClockObserver(Action clockObserver) =>
AdminManager.ClockUpdatedObservers += clockObserver;
    public void RemoveClockObserver(Action clockObserver) =>
    AdminManager.ClockUpdatedObservers -= clockObserver;
    public void AddConfigObserver(Action configObserver) =>
   AdminManager.ConfigUpdatedObservers += configObserver;
    public void RemoveConfigObserver(Action configObserver) =>
    AdminManager.ConfigUpdatedObservers -= configObserver;
}
