

namespace DalApi;


public interface IConfig
{
    //IConfig interface. 
    DateTime Clock { get; set; }    //get and set the config's clock
    TimeSpan RiskRange { get; set; }    //get and set the config's risgRange
    void Reset();   //reset the config
}
