namespace Dal;
internal static class Config
{
    //Config class to store and load to xml files

    internal const string s_data_config_xml = "data-config.xml";    //Keeping config xml file's name
    internal const string s_volunteers_xml = "volunteers.xml";  //Keeping volunteers xml file's name
    internal const string s_calls_xml = "calls.xml";    //Keeping calls xml file's name
    internal const string s_assignments_xml = "assignments.xml";    //Keeping assignments xml file's name


    internal static int NextCallId  //store(and increase) and load nextCallId in data-config.xml
    {
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextCallId");
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextCallId", value);
    }

    internal static int NextAssignmentId    //store(and increase) and load nextAssignmentId in data-config.xml
    {
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextAssignmentId");
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextAssignmentId", value);
    }
	

    internal static DateTime Clock
    {
        get => XMLTools.GetConfigDateVal(s_data_config_xml, "Clock");
        set => XMLTools.SetConfigDateVal(s_data_config_xml, "Clock", value);
    }

    internal static TimeSpan RiskRange
    {
        get => XMLTools.GetConfigTimeSpanVal(s_data_config_xml, "RiskRange");
        set => XMLTools.SetConfigTimeSpanVal(s_data_config_xml, "RiskRange", value);
    }

    internal static void Reset()
    {
        //Reseting the config fields (will set in the xml files)

        NextCallId = 1000;
        NextAssignmentId = 2000;
        Clock = new DateTime(2024, 1, 1, 0, 0, 0);
        RiskRange = new TimeSpan(24, 0, 0);
    }
}
