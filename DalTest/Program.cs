using Dal;

using DalApi;
using DO;

namespace DalTest
{
    internal enum Main_Menu { Exit, Volunteer_Menu, Call_Menu, Assignment_Menu, Config_Menu, Init_All, Display_All, Reset }
    internal enum Entity_Menu { Exit, Create, Read, Read_All, Update, Delete, Delete_All }
    internal enum Config_Menu { Exit, Add_Minute, Add_Hour, Add_Day, Add_Month, Display_Clock, Update_Risk_Range, Display_Risk_Range, Reset }
    internal class Program
    {
        private static IDal? s_dal = new DallList();
        static void Main(string[] args)
        {
            try
            {
                MainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private static void MainMenu()
        {
            foreach (Main_Menu menu in Enum.GetValues(typeof(Main_Menu)))
            {
                Console.WriteLine($"{(int)menu}: {menu}");
            }
            Main_Menu choice;
            Enum.TryParse(Console.ReadLine(), out choice);
            while (choice != Main_Menu.Exit)
            {
                switch (choice)
                {
                    case Main_Menu.Volunteer_Menu:
                        EntityMenu("volunteer");
                        break;
                    case Main_Menu.Call_Menu:
                        EntityMenu("call");
                        break;
                    case Main_Menu.Assignment_Menu:
                        EntityMenu("assignment");
                        break;
                    case Main_Menu.Config_Menu:
                        ConfigMenu();
                        break;
                    case Main_Menu.Init_All:
                        Initialization.Do(s_dal);
                        break;
                    case Main_Menu.Display_All:
                        Console.WriteLine(s_dal!.Volunteer.ReadAll());
                        Console.WriteLine(s_dal!.Call.ReadAll());
                        Console.WriteLine(s_dal!.Assignment.ReadAll());
                        break;
                    case Main_Menu.Reset:
                        s_dal!.Volunteer.DeleteAll();
                        s_dal!.Call.DeleteAll();
                        s_dal!.Assignment.DeleteAll();
                        s_dal!.Config.Reset();
                        break;
                }
                foreach (Main_Menu menu in Enum.GetValues(typeof(Main_Menu)))
                {
                    Console.WriteLine($"{(int)menu}: {menu}");
                }
                Enum.TryParse(Console.ReadLine(), out choice);
            }
        }
        private static void EntityMenu(string type)
        {
            foreach (Entity_Menu menu in Enum.GetValues(typeof(Entity_Menu)))
            {
                Console.WriteLine($"{(int)menu}: {menu}");
            }
            Entity_Menu choice;
            Enum.TryParse(Console.ReadLine(), out choice);
            while (choice != Entity_Menu.Exit)
            {
                switch (choice)
                {
                    case Entity_Menu.Create:
                        createEntity(type);
                        break;
                    case Entity_Menu.Read:
                        read(type);
                        break;
                    case Entity_Menu.Read_All:
                        readAll(type);
                        break;
                    case Entity_Menu.Update:
                        updateEntity(type);
                        break;
                    case Entity_Menu.Delete:
                        try
                        {
                            delete(type);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    case Entity_Menu.Delete_All:
                        deleteAll(type);
                        break;

                }
                foreach (Entity_Menu menu in Enum.GetValues(typeof(Entity_Menu)))
                {
                    Console.WriteLine($"{(int)menu}: {menu}");
                }
                Enum.TryParse(Console.ReadLine(), out choice);
            }
        }
        private static void createEntity(string type)
        {
            switch (type)
            {
                case "volunteer":
                    Volunteer volunteer = inputVolunteer();

                    try
                    {
                        if (volunteer == null)
                            throw new Exception("you didn't input details");
                        s_dal!.Volunteer.Create(volunteer);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    break;
                case "call":

                    Call call = inputCall();
                    if (call == null)
                    {
                        Console.WriteLine("you didn't enter details");
                        break;
                    }
                    s_dal!.Call.Create(call);
                    break;
                case "assignment":

                    Assignment assingment = inputAssignment();
                    if (assingment == null)
                    {
                        Console.WriteLine("you didn't enter details");
                        break;
                    }
                    s_dal!.Assignment.Create(assingment);
                    break;
            }
        }
        private static void updateEntity(string type)
        {
            Console.WriteLine("Please enter ID:");
            int id = int.Parse(Console.ReadLine());
            switch (type)
            {
                case "volunteer":
                    Console.WriteLine($"Volunteer details: {s_dal!.Volunteer.Read(id)}");
                    Console.WriteLine("If you want to update, enter details. else click 'ENTER'");
                    Volunteer? volunteer = inputVolunteer();

                    if (volunteer != null)
                    {
                        try
                        {
                            if (volunteer.Id != id)
                            {
                                Console.WriteLine("different id");
                                break;
                            }
                            s_dal!.Volunteer.Update(volunteer);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    break;
                case "call":
                    Console.WriteLine($"Call details: {s_dal!.Call.Read(id)}");
                    Console.WriteLine("If you want to update, enter updated details. else click 'ENTER' now");
                    Call? call = inputCall() with { Id = id };
                    if (call != null)
                        s_dal!.Call.Update(call);
                    break;
                case "assignment":
                    Console.WriteLine($"Assignment details: {s_dal!.Assignment.Read(id)}");
                    Console.WriteLine("If you want to update, enter updatede details. else click 'ENTER'");
                    Assignment? assingment = inputAssignment() with { Id = id };
                    if (assingment != null)
                        s_dal!.Assignment.Update(assingment);
                    break;
            }
        }
        private static Volunteer? inputVolunteer()
        {
            Console.Write("enter id");
            string idStr = Console.ReadLine();
            if (idStr == "")
                return null;
            if (!int.TryParse(idStr, out int id))
                throw new FormatException("id is invalid!");
            Console.Write("Enter name");
            string name = Console.ReadLine();
            Console.Write("Enter phone");
            string phone = Console.ReadLine();
            Console.Write("Enter email");
            string email = Console.ReadLine();
            Console.Write("Enter address");
            string address = Console.ReadLine();
            Console.Write("enter latitude");
            string latitudeStr = Console.ReadLine();
            if (!double.TryParse(latitudeStr, out double latitude) && latitudeStr != "")
                throw new FormatException("latitude is invalid!");
            Console.Write("enter longitude");
            string longitudeStr = Console.ReadLine();
            if (!double.TryParse(longitudeStr, out double longitude) && longitudeStr != "")
                throw new FormatException("longitude is invalid!");
            Console.Write("Enter max distance call");
            string maxDistanceStr = Console.ReadLine();
            if (!double.TryParse(maxDistanceStr, out double maxDistance) && maxDistanceStr != "")
                throw new FormatException("max distance is invalid!");
            Console.Write("Enter role 0/1");
            string roleStr = Console.ReadLine();
            if (!Role.TryParse(roleStr, out Role role) && roleStr != "")
                throw new FormatException("role is invalid!");
            if (roleStr == "")
                role = Role.Volunteer;
            Console.Write("Enter distance type");
            string distanceTypeStr = Console.ReadLine();
            if (!Distance_Type.TryParse(distanceTypeStr, out Distance_Type distanceType) && distanceTypeStr != "")
                throw new FormatException("role is invalid!");
            if (distanceTypeStr == "")
                distanceType = Distance_Type.Air;
            Console.Write("Enter password");
            string password = Console.ReadLine();
            Volunteer newV = new Volunteer(id, name, phone, email, address, latitude, longitude, maxDistance, role, distanceType, password);
            return newV;


        }
        private static Call? inputCall()
        {
            Console.Write("enter call type");
            string typeStr = Console.ReadLine();
            if (typeStr == "")
                return null;
            if (!Call_Type.TryParse(typeStr, out Call_Type type))
                throw new FormatException("call type is invalid!");
            Console.Write("Enter address");
            string address = Console.ReadLine();

            Console.Write("enter latitude");

            if (!double.TryParse(Console.ReadLine(), out double latitude))
                throw new FormatException("latitude is invalid!");
            Console.Write("enter longitude");
            if (!double.TryParse(Console.ReadLine(), out double longitude))
                throw new FormatException("longitude is invalid!");
            Console.Write("enter open time");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime open))
                throw new FormatException("open time is invalid!");
            Console.Write("enter max close time");
            string maxCloseStr = Console.ReadLine();
            if (!DateTime.TryParse(maxCloseStr, out DateTime maxClose) && maxCloseStr != "")
                throw new FormatException("latitude is invalid!");
            Console.Write("enter description");
            string description = Console.ReadLine();
            Call newC = new Call(type, address, latitude, longitude, open, maxClose, description);
            return newC;
        }
        private static Assignment? inputAssignment()
        {
            Console.Write("enter call id");
            string idStr = Console.ReadLine();
            if (idStr == "")
                return null;
            if (!int.TryParse(idStr, out int cId))
                throw new FormatException("id is invalid!");
            Console.Write("enter volunteer id");
            if (!int.TryParse(Console.ReadLine(), out int vId))
                throw new FormatException("id is invalid!");
            Console.Write("enter insersion time");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime insersion))
                throw new FormatException("insersion time is invalid!");
            Console.Write("Enter finish time");
            string finishTimeStr = Console.ReadLine();
            if (!DateTime.TryParse(finishTimeStr, out DateTime finishTime) && finishTimeStr != "")
                throw new FormatException("finish time is invalid!");
            Console.Write("Enter finish type");
            string finishTypeStr = Console.ReadLine();
            if (!Finish_Type.TryParse(finishTypeStr, out Finish_Type finishType) && finishTypeStr != "")
                throw new FormatException("finish type is invalid!");
            Assignment newA = new Assignment(cId, vId, insersion, finishTime, finishType);
            return newA;
        }
        private static void read(string type)
        {
            Console.WriteLine("Enter ID");
            int ID = int.Parse(Console.ReadLine());
            switch (type)
            {
                case "volunteer":

                    Console.WriteLine(s_dal!.Volunteer.Read(ID));
                    break;
                case "call":

                    Console.WriteLine(s_dal!.Call.Read(ID));

                    break;
                case "assignment":

                    Console.WriteLine(s_dal!.Assignment.Read(ID));

                    break;
            }

        }
        private static void delete(string type)
        {
            Console.WriteLine("Enter ID");
            int ID = int.Parse(Console.ReadLine());
            switch (type)
            {
                case "volunteer":

                    s_dal!.Volunteer.Delete(ID);
                    break;
                case "call":

                    s_dal!.Call.Delete(ID);
                    break;
                case "assignment":

                    s_dal!.Assignment.Delete(ID);
                    break;
            }

        }
        private static void readAll(string type)
        {

            switch (type)
            {
                case "volunteer":

                        foreach (Volunteer v in s_dal!.Volunteer.ReadAll())
                            Console.WriteLine(v);
                    break;
                case "call":

                        foreach (Call c in s_dal!.Call.ReadAll())
                            Console.WriteLine(c);

                    break;
                case "assignment":

                        foreach (Assignment ass in s_dal!.Assignment.ReadAll())
                            Console.WriteLine(ass);

                    break;
            }

        }
        private static void deleteAll(string type)
        {

            switch (type)
            {
                case "volunteer":

                        s_dal!.Volunteer.DeleteAll();
                    break;
                case "call":

                        s_dal!.Call.DeleteAll();
                    break;
                case "assignment":

                        s_dal!.Assignment.DeleteAll();
                    break;
            }

        }
        private static void ConfigMenu()
        {
            foreach (Config_Menu menu in Enum.GetValues(typeof(Config_Menu)))
            {
                Console.WriteLine($"{(int)menu}: {menu}");
            }
            Config_Menu choice;
            Enum.TryParse(Console.ReadLine(), out choice);
            while (choice != Config_Menu.Exit)
            {
                switch (choice)
                {
                    case Config_Menu.Add_Minute:
                        s_dal!.Config.Clock.AddMinutes(1);
                        break;
                    case Config_Menu.Add_Hour:
                        s_dal!.Config.Clock.AddHours(1);
                        break;
                    case Config_Menu.Add_Day:
                        s_dal!.Config.Clock.AddDays(1);
                        break;
                    case Config_Menu.Add_Month:
                        s_dal!.Config.Clock.AddMonths(1);
                        break;
                    case Config_Menu.Display_Clock:
                        Console.WriteLine(s_dal!.Config.Clock);
                        break;
                    case Config_Menu.Update_Risk_Range:
                        Console.WriteLine("Enter risk range: ");
                        TimeSpan range = (TimeSpan)Enum.Parse(typeof(TimeSpan), Console.ReadLine());
                        s_dal!.Config.RiskRange = range;
                        break;
                    case Config_Menu.Display_Risk_Range:
                        Console.WriteLine(s_dal!.Config.RiskRange);
                        break;
                    case Config_Menu.Reset:
                        s_dal!.Config.Reset();
                        break;

                }
                foreach (Config_Menu menu in Enum.GetValues(typeof(Config_Menu)))
                {
                    Console.WriteLine($"{(int)menu}: {menu}");
                }
                Enum.TryParse(Console.ReadLine(), out choice);
            }
        }
    }

}

internal enum Config_Menu { Exit, Add_Minute, Add_Hour, Add_Day, Add_Month, Display_Clock, Update_Time_Span, Display_Time_Span, Reset }

