
using DalApi;
namespace Dal;

/// <summary>
/// Realization of config's actions
/// </summary>
internal class ConfigImplementation : IConfig
{
    /// <summary>
    /// Get and set config's clock
    /// </summary>
    public DateTime Clock   
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }

    /// <summary>
    /// Get and set config's riskRange
    /// </summary>
    public TimeSpan RiskRange 
    {
        get => Config.RiskRange;
        set => Config.RiskRange = value;
    }

    /// <summary>
    /// Reset config's fields
    /// </summary>
    public void Reset() 
    {
        Config.Reset();
    }

}
