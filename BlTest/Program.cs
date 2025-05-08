using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Transactions;

namespace BlTest
{
    internal class Program
    {
        internal enum MainMenu { Exit, VolunteerMenu, CallMenu, AdminMenu }
        internal enum VolunteerMenu { Exit, EnterSystem, Create, Read, ReadAll, Update, Delete }
        internal enum CallMenu
        {
            Exit, Create, Read, ReadAll, Update, Delete,
            GetCountsGroupByStatus, ReadAllVolunteerClosedCalls, ReadAllVolunteerOpenCalls,
            FinishProcess, CanceleProcess, ChooseCall
        }

        internal enum AdminMenu { Exit, GetClock, AdvanceClock, GetRiskRange, SetRiskRange, Reset, Initialization }
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

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
            Console.WriteLine("Pay attention!!! when you enter enums values, sometimes you have to enter the number and sometimes you have to enter the value:");
            Console.WriteLine("Main menu:");
            foreach (MainMenu menu in Enum.GetValues(typeof(MainMenu)))
            {
                // Print the menu
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
                        DisplayVolunteerMenu();
                        break;
                    case MainMenu.CallMenu:
                        DisplayCallMenu();
                        break;
                    case MainMenu.AdminMenu:
                        DisplayAdminMenu();
                        break;
                }

                Console.WriteLine("Main menu:");
                foreach (MainMenu menu in Enum.GetValues(typeof(MainMenu)))
                {
                    // Printhin the menu
                    Console.WriteLine($"{(int)menu}: {menu}");
                }
                while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(MainMenu), choice))
                    Console.WriteLine("Wrong choice, try again");
            }
        }

        /// <summary>
        /// Menu to each data entity
        /// </summary>
        private static void DisplayVolunteerMenu()
        {
            Console.WriteLine("Pay attention!!! when you enter enums values, sometimes you have to enter the number and sometimes you have to enter the value:");
            Console.WriteLine("Volunteer menu");
            foreach (VolunteerMenu menu in Enum.GetValues(typeof(VolunteerMenu)))
            {
                // Printing the menu
                Console.WriteLine($"{(int)menu}: {menu}");
            }
            VolunteerMenu choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(VolunteerMenu), choice))
                Console.WriteLine("Wrong choice, enter again");
            while (choice != VolunteerMenu.Exit)
            {
                try
                {
                    switch (choice)
                    {
                        case VolunteerMenu.EnterSystem:
                            Console.Write("Enter name: ");
                            string name = Console.ReadLine()!;
                            Console.Write("Enter password: ");
                            string pass = Console.ReadLine()!;
                            Console.WriteLine($"Role: {s_bl.Volunteer.EnterSystem(name, pass)}");
                            break;
                        case VolunteerMenu.Create:
                            BO.Volunteer? newVolunteer = InputVolunteer();
                            if (newVolunteer != null)
                                s_bl.Volunteer.Create(newVolunteer);
                            break;
                        case VolunteerMenu.Read:
                            Console.Write("Enter id: ");
                            int id;
                            while (!int.TryParse(Console.ReadLine(), out id))
                                Console.WriteLine("Id is illegal! enter again!");
                            Console.WriteLine(s_bl.Volunteer.Read(id));
                            break;
                        case VolunteerMenu.ReadAll:
                            ReadAllVolunteers();
                            break;
                        case VolunteerMenu.Update:
                            UpdateVolunteer();
                            break;
                        case VolunteerMenu.Delete:
                            Console.Write("Enter id: ");
                            int vId;
                            while (!int.TryParse(Console.ReadLine(), out vId))
                                Console.WriteLine("Id is illegal! enter again!");
                            s_bl.Volunteer.Delete(vId);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Console.WriteLine("Volunteer menu");
                foreach (VolunteerMenu menu in Enum.GetValues(typeof(VolunteerMenu)))
                {
                    // Printing the menu
                    Console.WriteLine($"{(int)menu}: {menu}");
                }
                while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(VolunteerMenu), choice))
                    Console.WriteLine("Wrong choice, enter again");
            }
        }

        /// <summary>
        /// Inputs volunteer's details. 
        /// </summary>
        /// <returns>A volunteer with that details which inputed, or null if userId=''</returns>
        private static BO.Volunteer? InputVolunteer()
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
            string? address = Console.ReadLine();
            if (address == "")
                address = null;
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
            BO.Role role;
            while (!(Enum.TryParse(roleStr, out role) && Enum.IsDefined(typeof(BO.Role), role)) && roleStr != "")
            {
                Console.WriteLine("role is illegal! enter again");
                roleStr = Console.ReadLine()!;
            }
            if (roleStr == "")
                role = BO.Role.Volunteer;
            Console.Write("Enter distance type 0-2: ");
            string distanceTypeStr = Console.ReadLine()!;
            BO.DistanceType distanceType;
            while (!(Enum.TryParse(distanceTypeStr, out distanceType) && Enum.IsDefined(typeof(BO.DistanceType), distanceType)) && distanceTypeStr != "")
            {
                Console.WriteLine("role is illegal! enter again");
                distanceTypeStr = Console.ReadLine()!;
            }
            if (distanceTypeStr == "")
                distanceType = BO.DistanceType.Air;
            Console.Write("Enter password: ");
            string? password = Console.ReadLine();
            if (password == "")
                password = null;
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
            BO.Volunteer newV = new(id, name, phone, email, password, address, null, null, role, isActive, maxDistance, distanceType);
            return newV;
        }

        /// <summary>
        /// Reads all volunteers, optionally filtering by active state and sorting.
        /// </summary>
        /// <returns>void</returns>
        private static void ReadAllVolunteers()
        {
            Console.Write("Read just active (true) or not active (false)? ");
            string isActiveStr = Console.ReadLine()!;
            bool isActive;
            while (!bool.TryParse(isActiveStr, out isActive) && isActiveStr != "")
            {
                Console.WriteLine("isActive is illegal! enter again!");
                isActiveStr = Console.ReadLine()!;
            }

            Console.WriteLine("choose the field you want to sort by it: ");
            foreach (BO.VolunteerInListFields field in Enum.GetValues(typeof(MainMenu)))
            {
                // Print the menu
                Console.WriteLine($"{(int)field}: {field}");
            }
            BO.VolunteerInListFields sortBy;
            string soryByStr = Console.ReadLine() ?? "";
            while (!Enum.TryParse(soryByStr, out sortBy) || !Enum.IsDefined(typeof(BO.VolunteerInListFields), sortBy))
            {
                Console.WriteLine("Wrong input, try again");
                soryByStr = Console.ReadLine() ?? "";
            }
            var volunteers = s_bl!.Volunteer.ReadAll(isActiveStr == "" ? null : isActive, soryByStr == "" ? null : sortBy);
            foreach (BO.VolunteerInList v in volunteers)
                Console.WriteLine(v);
        }

        /// <summary>
        /// Updates a volunteer's details.
        /// </summary>
        /// <returns>void</returns>
        private static void UpdateVolunteer()
        {
            Console.Write("Enter your id: ");
            int userId;
            while (!int.TryParse(Console.ReadLine(), out userId))
                Console.WriteLine("Wrong id, try again");
            Console.WriteLine("Please enter ID of volunteer to update: ");
            int vId;
            while (!int.TryParse(Console.ReadLine(), out vId))
                Console.WriteLine("Wrong id, try again");

            Console.WriteLine($"Volunteer details: {s_bl!.Volunteer.Read(vId)}");
            Console.WriteLine("If you want to update, enter details. else click 'ENTER'");
            BO.Volunteer? volunteer = InputVolunteer();

            if (volunteer != null)
            {
                try
                {
                    if (volunteer.Id != vId)
                        Console.WriteLine("different id");
                    else
                        s_bl!.Volunteer.Update(userId, volunteer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Displays the call menu and processes user choices.
        /// </summary>
        /// <returns>void</returns>
        private static void DisplayCallMenu()
        {
            Console.WriteLine("Pay attention!!! when you enter enums values, sometimes you have to enter the number and sometimes you have to enter the value:");
            Console.WriteLine("Call menu");
            foreach (CallMenu menu in Enum.GetValues(typeof(CallMenu)))
            {
                // Printing the menu
                Console.WriteLine($"{(int)menu}: {menu}");
            }
            CallMenu choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(CallMenu), choice))
                Console.WriteLine("Wrong choice, enter again");
            while (choice != CallMenu.Exit)
            {
                try
                {
                    switch (choice)
                    {
                        case CallMenu.Create:
                            BO.Call? newCall = InputCall();
                            if (newCall is not null)
                                s_bl.Call.Create(newCall);
                            break;
                        case CallMenu.Read:
                            Console.Write("Enter id: ");
                            int id;
                            while (!int.TryParse(Console.ReadLine(), out id))
                                Console.WriteLine("Id is illegal! enter again!");
                            var call = s_bl.Call.Read(id);
                            if (call is not null)
                                Console.WriteLine(call);
                            else
                                throw new BO.BlDoesNotExistException($"call with id {id} does not exists");
                            break;
                        case CallMenu.ReadAll:
                            ReadAllCalls();
                            break;
                        case CallMenu.Update:
                            UpdateCall();
                            break;
                        case CallMenu.Delete:
                            Console.Write("Enter id: ");
                            int cId;
                            while (!int.TryParse(Console.ReadLine(), out cId))
                                Console.WriteLine("Id is illegal! enter again!");
                            s_bl.Call.Delete(cId);
                            break;
                        case CallMenu.GetCountsGroupByStatus:
                            int[] countArr = s_bl.Call.GetCountsGroupByStatus();
                            foreach (BO.FinishCallType status in Enum.GetValues(typeof(BO.FinishCallType)))
                                Console.WriteLine($"{countArr[(int)status]} calls with {status} status");
                            break;
                        case CallMenu.ReadAllVolunteerClosedCalls:
                            ReadAllVolunteerClosedCalls();
                            break;
                        case CallMenu.ReadAllVolunteerOpenCalls:
                            ReadAllVolunteerOpenedCalls();
                            break;
                        case CallMenu.FinishProcess:
                            Console.WriteLine("Enter your id:");
                            int vId;
                            while (!int.TryParse(Console.ReadLine(), out vId))
                                Console.WriteLine("Id is illegal! enter again!");
                            Console.WriteLine("Enter assignment id:");
                            int aId;
                            while (!int.TryParse(Console.ReadLine(), out aId))
                                Console.WriteLine("Id is illegal! enter again!");
                            s_bl.Call.FinishProcess(vId, aId);
                            break;
                        case CallMenu.CanceleProcess:
                            Console.WriteLine("Enter your id:");
                            int volId;
                            while (!int.TryParse(Console.ReadLine(), out volId))
                                Console.WriteLine("Id is illegal! enter again!");
                            Console.WriteLine("Enter assignment id:");
                            int assId;
                            while (!int.TryParse(Console.ReadLine(), out assId))
                                Console.WriteLine("Id is illegal! enter again!");
                            s_bl.Call.CanceleProcess(volId, assId);
                            break;
                        case CallMenu.ChooseCall:
                            Console.WriteLine("Enter your id:");
                            int volunteerId;
                            while (!int.TryParse(Console.ReadLine(), out volunteerId))
                                Console.WriteLine("Id is illegal! enter again!");
                            Console.WriteLine("Enter call id:");
                            int callId;
                            while (!int.TryParse(Console.ReadLine(), out callId))
                                Console.WriteLine("Id is illegal! enter again!");
                            s_bl.Call.ChooseCall(volunteerId, callId);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                Console.WriteLine("Call menu");
                foreach (CallMenu menu in Enum.GetValues(typeof(CallMenu)))
                {
                    // Printing the menu
                    Console.WriteLine($"{(int)menu}: {menu}");
                }
                while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(CallMenu), choice))
                    Console.WriteLine("Wrong choice, enter again");
            }
        }

        /// <summary>
        /// Inputs Call's details.
        /// </summary>
        /// <returns>A call with the inputs details, or null if user entered type=''</returns>
        private static BO.Call? InputCall(int cId = -1)
        {
            Console.Write("enter call type 0-2: ");
            string typeStr = Console.ReadLine()!;
            if (typeStr == "")
                return null;
            BO.CallType type;
            while (!(Enum.TryParse(typeStr, out type) && Enum.IsDefined(typeof(BO.CallType), type)))
            {
                Console.WriteLine("call type is illegal! enter again");
                typeStr = Console.ReadLine()!;
            }
            Console.Write("Enter address: ");
            string address = Console.ReadLine()!;
            Console.Write("Enter open time");
            DateTime open;
            while (!DateTime.TryParse(Console.ReadLine(), out open))
                Console.WriteLine("open time is illegal! enter again");
            Console.Write("enter max close time :");
            string maxCloseStr = Console.ReadLine()!;
            DateTime maxClose;
            while (!DateTime.TryParse(maxCloseStr, out maxClose) && maxCloseStr != "")
            {
                Console.WriteLine("max close is illegal! enter again");
                maxCloseStr = Console.ReadLine()!;
            }
            Console.Write("enter description: ");
            string description = Console.ReadLine()!;

            BO.Call newC = cId == -1 ? new(type, address, open, maxClose, description)
                : new(type, address, open, maxClose, description) { Id = cId };
            return newC;
        }

        /// <summary>
        /// Reads all calls based on filter and sort criteria.
        /// </summary>
        /// <returns>void</returns>
        private static void ReadAllCalls()
        {
            Console.WriteLine("By which field do you want to filter?");
            foreach (BO.CallInListFields field in Enum.GetValues(typeof(BO.CallInListFields)))
                Console.WriteLine($"{(int)field}: {field}");

            string filterFieldStr = Console.ReadLine()!;
            BO.CallInListFields filterField = BO.CallInListFields.Id;
            while (filterFieldStr != "" && (!Enum.TryParse(filterFieldStr, out filterField) || !Enum.IsDefined(typeof(BO.CallInListFields), filterField)))
            {
                Console.WriteLine("Wrong field, enter again");
                filterFieldStr = Console.ReadLine()!;
            }

            Console.WriteLine("By which valud do you want to filter?");
            string? filterVal = Console.ReadLine()!;

            Console.WriteLine("By which field do you want to sort?");
            foreach (BO.CallInListFields f in Enum.GetValues(typeof(BO.CallInListFields)))
                Console.WriteLine($"{(int)f}: {f}");

            string sortFieldStr = Console.ReadLine()!;
            BO.CallInListFields sortField = BO.CallInListFields.Id;
            while (sortFieldStr != "" && (!Enum.TryParse(sortFieldStr, out sortField) || !Enum.IsDefined(typeof(BO.CallInListFields), sortField)))
            {
                Console.WriteLine("Wrong field, enter again");
                sortFieldStr = Console.ReadLine()!;
            }
            var calls = s_bl!.Call.ReadAll(filterFieldStr == "" ? null : filterField, filterVal, sortFieldStr == "" ? null : sortField).ToList();
            foreach (BO.CallInList v in calls)
                Console.WriteLine(v);
        }

        /// <summary>
        /// Updates a call's details.
        /// </summary>
        /// <returns>void</returns>
        private static void UpdateCall()
        {
            Console.WriteLine("Please enter ID of call to update: ");
            int cId;
            while (!int.TryParse(Console.ReadLine(), out cId))
                Console.WriteLine("Wrong id, try again");

            Console.WriteLine($"Call details: {s_bl!.Call.Read(cId)}");
            Console.WriteLine("If you want to update, enter details. else click ENTER ");
            BO.Call? call = InputCall(cId);

            if (call != null)
            {
                try
                {
                    s_bl!.Call.Update(call);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Reads all volunteer closed calls for the specified volunteer ID.
        /// </summary>
        /// <returns>void</returns>
        private static void ReadAllVolunteerClosedCalls()
        {
            Console.Write("Enter volunteer id: ");
            int vId;
            while (!int.TryParse(Console.ReadLine(), out vId))
                Console.WriteLine("Id is illegal! enter again!");

            Console.Write("Enter call type to filter: ");
            string typeStr = Console.ReadLine()!;
            BO.CallType type;
            while (!(Enum.TryParse(typeStr, out type) && Enum.IsDefined(typeof(BO.CallType), type)) && typeStr != "")
            {
                Console.WriteLine("Type is illegal! enter again");
                typeStr = Console.ReadLine()!;
            }

            Console.Write("Enter field to sort: ");
            string fStr = Console.ReadLine()!;
            BO.ClosedCallInListFields f;
            while (!(Enum.TryParse(fStr, out f) && Enum.IsDefined(typeof(BO.ClosedCallInListFields), f)) && fStr != "")
            {
                Console.WriteLine("Field is illegal! enter again");
                fStr = Console.ReadLine()!;
            }

            IEnumerable<BO.ClosedCallInList> callsArr = s_bl.Call.ReadAllVolunteerClosedCalls(vId, typeStr == "" ? null : type, fStr == "" ? null : f);
            foreach (BO.ClosedCallInList c in callsArr)
                Console.WriteLine(c);
        }

        /// <summary>
        /// Reads all volunteer open calls for the specified volunteer ID.
        /// </summary>
        /// <returns>void</returns>
        private static void ReadAllVolunteerOpenedCalls()
        {
            Console.Write("Enter volunteer id: ");
            int vId;
            while (!int.TryParse(Console.ReadLine(), out vId))
                Console.WriteLine("Id is illegal! enter again!");

            Console.Write("Enter call type to filter: ");
            string typeStr = Console.ReadLine()!;
            BO.CallType type;
            while (!(Enum.TryParse(typeStr, out type) && Enum.IsDefined(typeof(BO.CallType), type)) && typeStr != "")
            {
                Console.WriteLine("Type is illegal! enter again");
                typeStr = Console.ReadLine()!;
            }

            Console.Write("Enter field to sort: ");
            string fStr = Console.ReadLine()!;
            BO.OpenCallInListFields f;
            while (!(Enum.TryParse(fStr, out f) && Enum.IsDefined(typeof(BO.OpenCallInListFields), f)) && fStr != "")
            {
                Console.WriteLine("Field is illegal! enter again");
                fStr = Console.ReadLine()!;
            }

            IEnumerable<BO.OpenCallInList> callsArr = s_bl.Call.ReadAllVolunteerOpenCalls(vId, typeStr == "" ? null : type, fStr == "" ? null : f);
            foreach (BO.OpenCallInList c in callsArr)
                Console.WriteLine(c);
        }

        /// <summary>
        /// Displays the admin menu and handles admin functionalities.
        /// </summary>
        /// <returns>void</returns>
        private static void DisplayAdminMenu()
        {
            Console.WriteLine("Pay attention!!! when you enter enums values, sometimes you have to enter the number and sometimes you have to enter the value:");
            Console.WriteLine("Admin menu");
            foreach (AdminMenu menu in Enum.GetValues(typeof(AdminMenu)))
            {
                // Printing the menu
                Console.WriteLine($"{(int)menu}: {menu}");
            }
            AdminMenu choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(AdminMenu), choice))
                Console.WriteLine("Wrong choice, enter again");
            while (choice != AdminMenu.Exit)
            {
                try
                {
                    switch (choice)
                    {
                        case AdminMenu.GetClock:
                            Console.WriteLine(s_bl.Admin.GetClock());
                            break;
                        case AdminMenu.AdvanceClock:
                            Console.Write("Enter time unit type to advance to clock: ");
                            BO.TimeUnit unitTime;
                            while (!(Enum.TryParse(Console.ReadLine(), out unitTime) && Enum.IsDefined(typeof(BO.TimeUnit), unitTime)))
                                Console.WriteLine("Unit time is illegal! enter again");
                            s_bl.Admin.AdvanceClock(unitTime);
                            break;
                        case AdminMenu.GetRiskRange:
                            Console.WriteLine(s_bl.Admin.GetRiskRange());
                            break;
                        case AdminMenu.SetRiskRange:
                            Console.WriteLine("Enter risk range: ");
                            TimeSpan range;
                            while (!TimeSpan.TryParse(Console.ReadLine(), out range))
                                Console.WriteLine("Risk range illegal, enter again");
                            s_bl.Admin.SetRiskRange(range);
                            break;
                        case AdminMenu.Reset:
                            s_bl.Admin.ResetDB();
                            break;
                        case AdminMenu.Initialization:
                            s_bl.Admin.InitializationDB();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Console.WriteLine("Admin menu");
                foreach (AdminMenu menu in Enum.GetValues(typeof(VolunteerMenu)))
                {
                    // Printing the menu
                    Console.WriteLine($"{(int)menu}: {menu}");
                }
                while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(AdminMenu), choice))
                    Console.WriteLine("Wrong choice, enter again");
            }
        }
    }
}