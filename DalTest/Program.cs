using Dal;
using DalApi;
using DO;

namespace DalTest
{
    internal enum MainMenu { Exit, VolunteerMenu, CallMenu, AssignmentMenu, ConfigMenu, InitAll, DisplayAll, Reset }
    internal enum EntityMenu { Exit, Create, Read, ReadAll, Update, Delete, DeleteAll }
    internal enum ConfigMenu { Exit, AddMinute, AddHour, AddDay, AddMonth, DisplayClock, UpdateRiskRange, DisplayRiskRange, Reset }
    internal class Program
    {
        //static readonly IDal? S_dal = new DalList(); //Static field of DalList type which through it we approach to the implentations
        //static readonly IDal? S_dal = new DalXml(); //Static field of DalXml type which through it we approach to the implentations
        /// <summary>
        /// Static field of factory which through it we approach to the implentations
        /// </summary>
        static readonly IDal? S_dal =Factory.Get;

        /// <summary>
        /// The main function calls to the main menu with try and catch
        /// </summary>
        static void Main()
        {
            try
            {
                DisplayMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// The main menu which offers entities menu and public actions
        /// </summary>
        private static void DisplayMainMenu()
        {
            Console.WriteLine("Main menu:");
            foreach (MainMenu menu in Enum.GetValues(typeof(MainMenu)))
            {
                //Print the menu
                Console.WriteLine($"{(int)menu}: {menu}");
            }
            MainMenu choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(MainMenu), choice))
                Console.WriteLine("Wrong choice, try again");
            while (choice != MainMenu.Exit)
            {
                switch (choice)
                {
                    case MainMenu.VolunteerMenu:
                        DisplayEntityMenu("volunteer");
                        break;
                    case MainMenu.CallMenu:
                        DisplayEntityMenu("call");
                        break;
                    case MainMenu.AssignmentMenu:
                        DisplayEntityMenu("assignment");
                        break;
                    case MainMenu.ConfigMenu:
                        DisplayConfigMenu();
                        break;
                    case MainMenu.InitAll:
                        Initialization.Do();
                        break;
                    case MainMenu.DisplayAll:
                        DisplayAll();
                        break;
                    case MainMenu.Reset:
                        S_dal!.Volunteer.DeleteAll();
                        S_dal!.Call.DeleteAll();
                        S_dal!.Assignment.DeleteAll();
                        S_dal!.Config.Reset();
                        break;
                }
                
                Console.WriteLine("Main menu:");
                foreach (MainMenu menu in Enum.GetValues(typeof(MainMenu)))
                {
                    //Printhin the menu
                    Console.WriteLine($"{(int)menu}: {menu}");
                }
                while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(MainMenu), choice))
                    Console.WriteLine("Wrong choice, try again");
            }
        }
        private static void DisplayEntityMenu(string type)
        {
            //menue to each data entity (accept etity type as string
            Console.WriteLine($"{type} menu");
            foreach (EntityMenu menu in Enum.GetValues(typeof(EntityMenu)))
            {
                //Printing the menu
                Console.WriteLine($"{(int)menu}: {menu}");
            }
            EntityMenu choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(EntityMenu), choice))
                Console.WriteLine("Wrong choice, enter again");
            while (choice != EntityMenu.Exit)
            {
                switch (choice)
                {
                    case EntityMenu.Create:
                        CreateEntity(type);
                        break;
                    case EntityMenu.Read:
                        Read(type);
                        break;
                    case EntityMenu.ReadAll:
                        ReadAll(type);
                        break;
                    case EntityMenu.Update:
                        UpdateEntity(type);
                        break;
                    case EntityMenu.Delete:
                        try
                        {
                            Delete(type);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    case EntityMenu.DeleteAll:
                        DeleteAll(type);
                        break;

                }
                Console.WriteLine($"{type} menu");
                foreach (EntityMenu menu in Enum.GetValues(typeof(EntityMenu)))
                {
                    //Printing the menu
                    Console.WriteLine($"{(int)menu}: {menu}");
                }
                while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(EntityMenu), choice))
                    Console.WriteLine("Wrong choice, enter again");
            }
        }

        /// <summary>
        /// Create an entity 
        /// </summary>
        /// <param name="type">the entity type in string</param>
        private static void CreateEntity(string type)
        {
            switch (type)
            {
                case "volunteer":
                    Volunteer? volunteer = InputVolunteer();

                    try
                    {
                        if (volunteer == null)
                            throw new Exception("you didn't input details");
                        S_dal!.Volunteer.Create(volunteer);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    break;
                case "call":

                    Call? call = InputCall();
                    if (call == null)
                    {
                        Console.WriteLine("you didn't enter details");
                        break;
                    }
                    S_dal!.Call.Create(call);
                    break;
                case "assignment":

                    Assignment? assingment = InputAssignment();
                    if (assingment == null)
                    {
                        Console.WriteLine("you didn't enter details");
                        break;
                    }
                    S_dal!.Assignment.Create(assingment);
                    break;
            }
        }

        /// <summary>
        /// Update an entity. Inputs an id and print the entity's details, and offers to update it 
        /// </summary>
        /// <param name="type">the entity type in string</param>
        private static void UpdateEntity(string type)
        {
            Console.WriteLine("Please enter ID:");
            int id = int.Parse(Console.ReadLine()!);
            switch (type)
            {
                case "volunteer":
                    Console.WriteLine($"Volunteer details: {S_dal!.Volunteer.Read(id)}");
                    Console.WriteLine("If you want to update, enter details. else click 'ENTER'");
                    Volunteer? volunteer = InputVolunteer();

                    if (volunteer != null)
                    {
                        try
                        {
                            if (volunteer.Id != id)
                            {
                                Console.WriteLine("different id");
                                break;
                            }
                            S_dal!.Volunteer.Update(volunteer);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    break;
                case "call":
                    Console.WriteLine($"Call details: {S_dal!.Call.Read(id)}");
                    Console.WriteLine("If you want to update, enter updated details. else click 'ENTER' now");
                    Call? call = InputCall()! with { Id = id };
                    if (call != null)
                        S_dal!.Call.Update(call);
                    break;
                case "assignment":
                    Console.WriteLine($"Assignment details: {S_dal!.Assignment.Read(id)}");
                    Console.WriteLine("If you want to update, enter updatede details. else click 'ENTER'");
                    Assignment? assingment = InputAssignment()! with { Id = id };
                    if (assingment != null)
                        S_dal!.Assignment.Update(assingment);
                    break;
            }
        }

        /// <summary>
        /// Inputs volunteer's details. 
        /// </summary>
        /// <returns>a volunteer with that details which inputed, or null if id=''</returns>
        private static Volunteer? InputVolunteer()
        {
            Console.Write("enter id: ");
            string idStr = Console.ReadLine()!;
            if (idStr == "")
                return null;
            int id;
            while (!int.TryParse(idStr, out id))
            {
                Console.WriteLine("id is illegal! enter id again!");
                idStr = Console.ReadLine()!;
                if (idStr == "")
                    return null;
            }
            Console.Write("Enter name: ");
            string name = Console.ReadLine()!;
            Console.Write("Enter phone: ");
            string phone = Console.ReadLine()!;
            Console.Write("Enter email: ");
            string email = Console.ReadLine()!;
            Console.Write("Enter address: ");
            string address = Console.ReadLine()!;
            Console.Write("enter latitude:");
            string latitudeStr = Console.ReadLine()!;
            double latitude;
            while (!double.TryParse(latitudeStr, out latitude) && latitudeStr != "")
            {
                Console.WriteLine("latitude is illegal! enter again!");
                latitudeStr = Console.ReadLine()!;
            }
            Console.Write("enter longitude: ");
            string longitudeStr = Console.ReadLine()!;
            double longitude;
            while (!double.TryParse(longitudeStr, out longitude) && longitudeStr != "")
            {
                Console.WriteLine("longitude is illegal! enter again!");
                longitudeStr = Console.ReadLine()!;
            }
            Console.Write("Enter max distance call: ");
            string maxDistanceStr = Console.ReadLine()!;
            double maxDistance;
            while (!double.TryParse(maxDistanceStr, out maxDistance) && maxDistanceStr != "")
            {
                Console.WriteLine("max distance is illegal! enter again!");
                maxDistanceStr = Console.ReadLine()!;
            }
            Console.Write("Enter role 0/1: ");
            string roleStr = Console.ReadLine()!;
            Role role;
            while (!(Enum.TryParse(roleStr, out role) && Enum.IsDefined(typeof(Role), role)) && roleStr != "")
            {
                Console.WriteLine("role is illegal! enter again");
                roleStr = Console.ReadLine()!;
            }
            if (roleStr == "")
                role = Role.Volunteer;
            Console.Write("Enter distance type 0-2: ");
            string distanceTypeStr = Console.ReadLine()!;
            DistanceType distanceType;
            while (!(Enum.TryParse(distanceTypeStr, out distanceType) && Enum.IsDefined(typeof(DistanceType), distanceType)) && distanceTypeStr != "")
            {
                Console.WriteLine("role is illegal! enter again");
                distanceTypeStr = Console.ReadLine()!;
            }
            if (distanceTypeStr == "")
                distanceType = DistanceType.Air;
            Console.Write("Enter password: ");
            string password = Console.ReadLine()!;
            Console.Write("enter is active ('true' or 'false') :");
            string isActiveStr = Console.ReadLine()!;
            bool isActive;
            while (!bool.TryParse(isActiveStr, out isActive) && isActiveStr != "")
            {
                Console.WriteLine("isActive is illegal! enter again!");
                isActiveStr = Console.ReadLine()!;
            }
            if (isActiveStr == "")
                isActive = true;
            Volunteer newV = new(id, name, phone, email, address, latitude, longitude, maxDistance, role, distanceType, password, isActive);
            return newV;

        }

        /// <summary>
        /// Inputs Call's details.
        /// </summary>
        /// <returns>a call with the inputs details, or null if user entered type=''</returns>
        private static Call? InputCall()
        {
            Console.Write("enter call type 0-2: ");
            string typeStr = Console.ReadLine()!;
            if (typeStr == "")
                return null;
            CallType type;
            while (!(Enum.TryParse(typeStr, out type) && Enum.IsDefined(typeof(CallType), type)))
            {
                Console.WriteLine("call type is illegal! enter again");
                typeStr = Console.ReadLine()!;
            }
            Console.Write("Enter address: ");
            string address = Console.ReadLine()!;
            Console.Write("enter latitude: ");
            double latitude;
            while (!double.TryParse(Console.ReadLine(), out latitude))
                Console.WriteLine("latitude is illegal! enter again");
            Console.Write("enter longitude: ");
            double longitude;
            while (!double.TryParse(Console.ReadLine(), out longitude))
                Console.WriteLine("longitude is illegal! enter again");
            Console.Write("enter open time :");
            DateTime open;
            while (!DateTime.TryParse(Console.ReadLine(), out open))
                Console.WriteLine("open time is illegal! enter again");
            Console.Write("enter max close time :");
            string maxCloseStr = Console.ReadLine()!;
            DateTime maxClose;
            while (!DateTime.TryParse(maxCloseStr, out maxClose) && maxCloseStr != "" )
            {
                Console.WriteLine("max close is illegal! enter again");
                maxCloseStr = Console.ReadLine()!;
            }
            Console.Write("enter description: ");
            string description = Console.ReadLine()!;
            Call newC = new(type, address,  open, maxClose, latitude, longitude, description);
            return newC;
        }

        /// <summary>
        /// Inputs Assignment's details.
        /// </summary>
        /// <returns>an assignment with the inputs details. or null if user inputs id=''</returns>
        private static Assignment? InputAssignment()
        {
            Console.Write("enter call id: ");
            string cIdStr = Console.ReadLine()!;
            if (cIdStr == "")
                return null;
            int cId;
            while (!int.TryParse(cIdStr, out cId))
            {
                Console.WriteLine("id is illegal! try again");
                cIdStr = Console.ReadLine()!;
            }
            Console.Write("enter volunteer id");
            int vId;
            while (!int.TryParse(Console.ReadLine(), out vId))
                Console.WriteLine("id is illegal! try again");
            Console.Write("enter insersion time: ");
            DateTime insersion;    
            while (!DateTime.TryParse(Console.ReadLine(), out insersion))
                Console.WriteLine("insersion time is illegal! try agagin");
            Console.Write("Enter finish time: ");
            string finishTimeStr = Console.ReadLine()!;
            DateTime finishTime;
            while (!DateTime.TryParse(finishTimeStr, out finishTime) && finishTimeStr != "")
            {
                Console.WriteLine("finish time is illegal! try again");
                finishTimeStr = Console.ReadLine()!;
            }
            Console.Write("Enter finish type: ");
            string finishTypeStr = Console.ReadLine()!;
            FinishType finishType;
            while (!(Enum.TryParse(finishTypeStr, out finishType) && Enum.IsDefined(typeof(FinishType), finishType)) && finishTypeStr != "")
            {
                Console.WriteLine("finish type is illegal!");
                finishTypeStr = Console.ReadLine()!;
            }
            Assignment newA = new(cId, vId, insersion, finishTime, finishType);
            return newA;
        }

        /// <summary>
        /// Inputs id and Read the entity with that id
        /// </summary>
        /// <param name="type">the entity type as string</param>
        private static void Read(string type)
        {
            Console.WriteLine("Enter ID");
            int ID = int.Parse(Console.ReadLine()!);
            switch (type)
            {
                case "volunteer":

                    Console.WriteLine(S_dal!.Volunteer.Read(ID));
                    break;
                case "call":

                    Console.WriteLine(S_dal!.Call.Read(ID));

                    break;
                case "assignment":

                    Console.WriteLine(S_dal!.Assignment.Read(ID));

                    break;
            }

        }

        /// <summary>
        /// Inputs id and Delete the entity with that id.
        /// </summary>
        /// <param name="type">the entity type as string</param>
        private static void Delete(string type)
        {
            Console.WriteLine("Enter ID");
            int ID = int.Parse(Console.ReadLine()!);
            switch (type)
            {
                case "volunteer":

                    S_dal!.Volunteer.Delete(ID);
                    break;
                case "call":

                    S_dal!.Call.Delete(ID);
                    break;
                case "assignment":

                    S_dal!.Assignment.Delete(ID);
                    break;
            }

        }

        /// <summary>
        /// Read all entities of this type
        /// </summary>
        /// <param name="type">the entity type as string</param>
        private static void ReadAll(string type)
        {
            switch (type)
            {
                case "volunteer":

                        foreach (Volunteer v in S_dal!.Volunteer.ReadAll())
                            Console.WriteLine(v);
                    break;
                case "call":

                        foreach (Call c in S_dal!.Call.ReadAll())
                            Console.WriteLine(c);

                    break;
                case "assignment":

                        foreach (Assignment ass in S_dal!.Assignment.ReadAll())
                            Console.WriteLine(ass);

                    break;
            }

        }

        /// <summary>
        /// Deletes all entities of this type
        /// </summary>
        /// <param name="type">the entity type as string</param>
        private static void DeleteAll(string type)
        {
            switch (type)
            {
                case "volunteer":

                        S_dal!.Volunteer.DeleteAll();
                    break;
                case "call":

                        S_dal!.Call.DeleteAll();
                    break;
                case "assignment":

                        S_dal!.Assignment.DeleteAll();
                    break;
            }

        }

        /// <summary>
        /// Offering and performing config's menue
        /// </summary>
        private static void DisplayConfigMenu()
        {
            Console.WriteLine("Config menu");
            //Offers config menu
            foreach (ConfigMenu menu in Enum.GetValues(typeof(ConfigMenu)))
            {
                //Printing the menu
                Console.WriteLine($"{(int)menu}: {menu}");
            }
            ConfigMenu choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(ConfigMenu), choice))
                Console.WriteLine("Wrong choice, enter again");
            while (choice != ConfigMenu.Exit)
            {
                switch (choice)
                {
                    case ConfigMenu.AddMinute:
                        S_dal!.Config.Clock = S_dal!.Config.Clock.AddMinutes(1);
                        break;
                    case ConfigMenu.AddHour:
                        S_dal!.Config.Clock = S_dal!.Config.Clock.AddHours(1);
                        break;
                    case ConfigMenu.AddDay:
                        S_dal!.Config.Clock = S_dal!.Config.Clock.AddDays(1);
                        break;
                    case ConfigMenu.AddMonth:
                        S_dal!.Config.Clock = S_dal!.Config.Clock.AddMonths(1);
                        break;
                    case ConfigMenu.DisplayClock:
                        Console.WriteLine(S_dal!.Config.Clock);
                        break;
                    case ConfigMenu.UpdateRiskRange:
                        Console.WriteLine("Enter risk range: ");
                        TimeSpan range;
                        while (!Enum.TryParse(Console.ReadLine(),out range))
                            Console.WriteLine("Risk range illegal, enter again");
                        S_dal!.Config.RiskRange = range;
                        break;
                    case ConfigMenu.DisplayRiskRange:
                        Console.WriteLine(S_dal!.Config.RiskRange);
                        break;
                    case ConfigMenu.Reset:
                        S_dal!.Config.Reset();
                        break;

                }

                Console.WriteLine("Config menu");
                foreach (ConfigMenu menu in Enum.GetValues(typeof(ConfigMenu)))
                {
                    //Printing the menu
                    Console.WriteLine($"{(int)menu}: {menu}");
                }
                while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(ConfigMenu), choice))
                    Console.WriteLine("Wrong choice, enter again");
            }
        }


        /// <summary>
        /// Displaying all volunteers, calls and assignments
        /// </summary>
        private static void DisplayAll()
        {
            Console.WriteLine("Volunteers:");
            foreach (Volunteer v in S_dal!.Volunteer.ReadAll())
                Console.WriteLine(v);
            Console.WriteLine("Calls:");
            foreach (Call c in S_dal!.Call.ReadAll())
                Console.WriteLine(c);
            Console.WriteLine("Assignments: ");
            foreach (Assignment a in S_dal!.Assignment.ReadAll())
                Console.WriteLine(a);
        }
    }

}