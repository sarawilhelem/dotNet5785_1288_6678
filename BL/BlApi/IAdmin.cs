

namespace BlApi;

public interface IAdmin
{
    /// <summary>
    /// get the system clock
    /// </summary>
    /// <returns>system's current clock</returns>
    public DateTime GetClock();
    
    /// <summary>
    /// advance the clock by a unit time
    /// </summary>
    /// <param name="timeUnit">the unit time to advance clock: second/ minute/ hour/ day/ mont / year</param>
    public void AdvanceClock(BO.TimeUnit timeUnit);
    
    /// <summary>
    /// get the risk range
    /// </summary>
    /// <returns>risk range</returns>
    public TimeSpan GetRiskRange();

    /// <summary>
    /// set the risk range
    /// </summary>
    /// <param name="riskRange">the new risk range</param>
    public void SetRiskRange(TimeSpan riskRange);

    /// <summary>
    /// Reset db and clock
    /// </summary>
    public void ResetDB();

    /// <summary>
    /// Initialization db and clock
    /// </summary>
    public void InitializationDB();
    void StartSimulator(int interval); //stage 7
    Task StopSimulator(); //stage 7


    void AddConfigObserver(Action configObserver);
    void RemoveConfigObserver(Action configObserver);
    void AddClockObserver(Action clockObserver);
    void RemoveClockObserver(Action clockObserver);
}
