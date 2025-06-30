
using DalApi;
using System.Runtime.CompilerServices;
namespace Dal;

/// <summary>
/// Realization of config's actions
/// </summary>
internal class ConfigImplementation:IConfig
{

    /// <summary>
    /// Get and set config's clock
    /// </summary>
	public DateTime Clock   
	{
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.Clock;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.Clock = value;
	}

    /// <summary>
    /// Get and set config's riskRange
    /// </summary>
    public TimeSpan RiskRange  
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.RiskRange;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.RiskRange = value;
    }

    /// <summary>
    /// Reset config's fields
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Reset()
    {
        Config.Reset();
    }

}
