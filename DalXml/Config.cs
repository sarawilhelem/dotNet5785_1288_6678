using System.Runtime.CompilerServices;

namespace Dal;
/// <summary>
/// Config class to store and load to xml files
/// </summary>
internal static class Config
{
    /// <summary>
    /// Keeping config xml file's name
    /// </summary>
    internal const string s_data_config_xml = "data-config.xml";

    /// <summary>
    /// Keeping volunteers xml file's name
    /// </summary>
    internal const string s_volunteers_xml = "volunteers.xml";

    /// <summary>
    /// Keeping calls xml file's name
    /// </summary>
    internal const string s_calls_xml = "calls.xml";

    /// <summary>
    /// Keeping assignments xml file's name
    /// </summary>
    internal const string s_assignments_xml = "assignments.xml";

    /// <summary>
    /// store(and increase) and load nextCallId in data-config.xml
    /// </summary>
    internal static int NextCallId  
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextCallId");
        [MethodImpl(MethodImplOptions.Synchronized)]
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextCallId", value);
    }

    /// <summary>
    /// store(and increase) and load nextAssignmentId in data-config.xml
    /// </summary>
    internal static int NextAssignmentId
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextAssignmentId");
        [MethodImpl(MethodImplOptions.Synchronized)]
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextAssignmentId", value);
    }
	
    /// <summary>
    /// refer to xmlTools function to set and get the clock
    /// </summary>
    internal static DateTime Clock
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => XMLTools.GetConfigDateVal(s_data_config_xml, "Clock");
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => XMLTools.SetConfigDateVal(s_data_config_xml, "Clock", value);
    }

    /// <summary>
    /// refer to xmlTools function to set and get the riskRange
    /// </summary>
    internal static TimeSpan RiskRange
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => XMLTools.GetConfigTimeSpanVal(s_data_config_xml, "RiskRange");
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => XMLTools.SetConfigTimeSpanVal(s_data_config_xml, "RiskRange", value);
    }

    /// <summary>
    /// Reseting the config fields (will set in the xml files)
    /// </summary>
    internal static void Reset()
    {
        NextCallId = 1000;
        NextAssignmentId = 2000;
        Clock = new DateTime(2024, 1, 1, 0, 0, 0);
        RiskRange = new TimeSpan(24, 0, 0);
    }
}
