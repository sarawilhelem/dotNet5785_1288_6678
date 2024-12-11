
using DalApi;
namespace Dal;

internal class ConfigImplementation : IConfig
{
    //Realization of config's actions
    public DateTime Clock   //Get and set config's clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }

    public TimeSpan RiskRange   //Get and set config's riskRange
    {
        get => Config.RiskRange;
        set => Config.RiskRange = value;
    }
    public void Reset() //Reset config's fields
    {
        Config.Reset();
    }

}
