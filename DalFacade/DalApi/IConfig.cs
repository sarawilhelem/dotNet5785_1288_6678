

namespace DalApi;

/// <summary>
/// IConfig interface. 
/// </summary>
public interface IConfig
{
    /// <summary>
    /// get and set the config's clock
    /// </summary>
    DateTime Clock { get; set; }

    /// <summary>
    /// get and set the config's risgRange
    /// </summary>
    TimeSpan RiskRange { get; set; }

    /// <summary>
    /// reset the config
    /// </summary>
    void Reset();
}
