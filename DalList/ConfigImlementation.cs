
using DalApi;
using DAL;
namespace Dal;

public class ConfigImlementation:IConfig
{

	public DateTime Clock
	{
		get => Config.Clock;
		set => Config.Clock = value;
	}

    public TimeSpan RiskRange
    {
        get => Config.RiskRange;
        set => Config.RiskRange = value;
    }
    public void Reset()
    {
        Config.reset();
    }

}
