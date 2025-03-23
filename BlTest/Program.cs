using DalApi;
using DalTest;
using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Transactions;

namespace BlTest
{
    internal class Program
    {
        internal enum Main_Menu { Exit, Volunteer_Menu, Call_Menu, Admin_menu }
        internal enum Volunteer_Menu { Exit, EnterSystem, Create, Read, ReadAll, Update, Delete }
        internal enum Admin_Menu { Exit, GetClock, AdvanceClock, GetRiskRange, SetRiskRange, Reset, Initialization}
        internal enum Call_Menu
        {
            Exit, Create, Read, ReadAll, Update, Delete,
            GetCountsGroupByStatus, ReadAllVolunteerClosedCalls, ReadAllVolunteerOpenCalls,
            FinishProcess, CanceleProcess, ChooseCall
        }

         summary
         A static field to aproach the bl entities' functions
         summary
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

         summary
         The main function calls to the main menu with try and catch
         summary
        static void Main()
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

         summary
         The main menu which offers entities menu and public actions
         summary
        private static void MainMenu()
        {

            Console.WriteLine(Main menu);
            foreach (Main_Menu menu in Enum.GetValues(typeof(Main_Menu)))
            {
                Print the menu
                Console.WriteLine(${(int)menu} {menu});
            }
            Main_Menu choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice)  !Enum.IsDefined(typeof(Main_Menu), choice))
                Console.WriteLine(Wrong choice, try again);
            while (choice != Main_Menu.Exit)
            {
                switch (choice)
                {
                    case Main_Menu.Volunteer_Menu
                        VolunteerMenu();
                        break;
                    case Main_Menu.Call_Menu
                        CallMenu();
                        break;
                    case Main_Menu.Admin_menu
                        AdminMenu();
                        break;
                }

                Console.WriteLine(Main menu);
                foreach (Main_Menu menu in Enum.GetValues(typeof(Main_Menu)))
                {
                    Printhin the menu
                    Console.WriteLine(${(int)menu} {menu});
                }
                while (!Enum.TryParse(Console.ReadLine(), out choice)  !Enum.IsDefined(typeof(Main_Menu), choice))
                    Console.WriteLine(Wrong choice, try again);
            }
        }

         summary
         menue to each data entity (accept etity type as string)
         summary
        private static void VolunteerMenu()
        {
            Console.WriteLine(Volunteer menu);
            foreach (Volunteer_Menu menu in Enum.GetValues(typeof(Volunteer_Menu)))
            {
                Printing the menu
                Console.WriteLine(${(int)menu} {menu});
            }
            Volunteer_Menu choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice)  !Enum.IsDefined(typeof(Volunteer_Menu), choice))
                Console.WriteLine(Wrong choice, enter again);
            while (choice != Volunteer_Menu.Exit)
            {
                try
                {
                    switch (choice)
                    {
                        case Volunteer_Menu.EnterSystem
                            Console.Write(Enter name );
                            string name = Console.ReadLine()!;
                            Console.Write(Enter password );
                            string pass = Console.ReadLine()!;
                            Console.WriteLine($Role {s_bl.Volunteer.EnterSystem(name, pass)});
                            break;
                        case Volunteer_Menu.Create
                            BO.Volunteer newVolunteer = InputVolunteer();
                            if (newVolunteer != null)
                                s_bl.Volunteer.Create(newVolunteer);
                            break;
                        case Volunteer_Menu.Read
                            Console.Write(Enter id );
                            int id;
                            while (!int.TryParse(Console.ReadLine(), out id))
                                Console.WriteLine(Id is illegal! enter again!);
                            Console.WriteLine(s_bl.Volunteer.Read(id));
                            break;
                        case Volunteer_Menu.ReadAll
                            ReadAllVolunteers();
                            break;
                        case Volunteer_Menu.Update
                            UpdateVolunteer();
                            break;
                        case Volunteer_Menu.Delete
                            Console.Write(Enter id );
                            int vId;
                            while (!int.TryParse(Console.ReadLine(), out vId))
                                Console.WriteLine(Id is illegal! enter again!);
                            s_bl.Volunteer.Delete(vId);
                            break;
                        default
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Console.WriteLine(Volunteer menu);
                foreach (Volunteer_Menu menu in Enum.GetValues(typeof(Volunteer_Menu)))
                {
                    Printing the menu
                    Console.WriteLine(${(int)menu} {menu});
                }
                while (!Enum.TryParse(Console.ReadLine(), out choice)  !Enum.IsDefined(typeof(Volunteer_Menu), choice))
                    Console.WriteLine(Wrong choice, enter again);
            }
        }

         summary
         Inputs volunteer's details. 
         summary
         returnsa volunteer with that details which inputed, or null if userId=''returns
        private static BO.Volunteer InputVolunteer()
        {
            Console.Write(enter id );
            string idStr = Console.ReadLine()!;
            if (idStr == )
                return null;
            int id;
            while (!int.TryParse(idStr, out id))
            {
                Console.WriteLine(id is illegal! enter id again!);
                idStr = Console.ReadLine()!;
                if (idStr == )
                    return null;
            }
            Console.Write(Enter name );
            string name = Console.ReadLine()!;
            Console.Write(Enter phone );
            string phone = Console.ReadLine()!;
            Console.Write(Enter email );
            string email = Console.ReadLine()!;
            Console.Write(Enter address );
            string address = Console.ReadLine()!;
            Console.Write(Enter max distance call );
            string maxDistanceStr = Console.ReadLine()!;
            double maxDistance;
            while (!double.TryParse(maxDistanceStr, out maxDistance) && maxDistanceStr != )
            {
                Console.WriteLine(max distance is illegal! enter again!);
                maxDistanceStr = Console.ReadLine()!;
            }
            Console.Write(Enter role 01 );
            string roleStr = Console.ReadLine()!;
            BO.Role role;
            while (!(Enum.TryParse(roleStr, out role) && Enum.IsDefined(typeof(BO.Role), role)) && roleStr != )
            {
                Console.WriteLine(role is illegal! enter again);
                roleStr = Console.ReadLine()!;
            }
            if (roleStr == )
                role = BO.Role.Volunteer;
            Console.Write(Enter distance type 0-2 );
            string distanceTypeStr = Console.ReadLine()!;
            BO.Distance_Type distanceType;
            while (!(Enum.TryParse(distanceTypeStr, out distanceType) && Enum.IsDefined(typeof(BO.Distance_Type), distanceType)) && distanceTypeStr != )
            {
                Console.WriteLine(role is illegal! enter again);
                distanceTypeStr = Console.ReadLine()!;
            }
            if (distanceTypeStr == )
                distanceType = BO.Distance_Type.Air;
            Console.Write(Enter password );
            string password = Console.ReadLine()!;
            Console.Write(enter is active ('true' or 'false') );
            string isActiveStr = Console.ReadLine()!;
            bool isActive;
            while (!bool.TryParse(isActiveStr, out isActive) && isActiveStr != )
            {
                Console.WriteLine(isActive is illegal! enter again!);
                isActiveStr = Console.ReadLine()!;
            }
            if (isActiveStr == )
                isActive = true;
            BO.Volunteer newV = new(id, name, phone, email, password, address, null, null, role, isActive, maxDistance, distanceType);
            return newV;

        }

         summary
         read all volunteers, just active or not active or all volunteers, sorted by the chosen field
         summary
        private static void ReadAllVolunteers()
        {
            Console.Write(Read just active (true) or not active (false) );
            string isActiveStr = Console.ReadLine()!;
            bool isActive;
            while (!bool.TryParse(isActiveStr, out isActive) && isActiveStr != )
            {
                Console.WriteLine(isActive is illegal! enter again!);
                isActiveStr = Console.ReadLine()!;
            }

            Console.WriteLine(choose the field you want to sort by it );
            foreach (BO.Volunteer_In_List_Fields field in Enum.GetValues(typeof(Main_Menu)))
            {
                Print the menu
                Console.WriteLine(${(int)field} {field});
            }
            BO.Volunteer_In_List_Fields sortBy;
            string soryByStr = Console.ReadLine()  ;
            while (!Enum.TryParse(soryByStr, out sortBy)  !Enum.IsDefined(typeof(BO.Volunteer_In_List_Fields), sortBy))
            {
                Console.WriteLine(Wrong input, try again);
                soryByStr = Console.ReadLine()  ;
            }
            foreach (BO.VolunteerInList v in s_bl!.Volunteer.ReadAll(isActiveStr ==   null  isActive, soryByStr ==   null  sortBy))
                Console.WriteLine(v);
        }

         summary
         inputs volunteer new details and update them
         summary
        private static void UpdateVolunteer()
        {
            Console.Write(Enter your id );
            int userId;
            while (!int.TryParse(Console.ReadLine(), out userId))
                Console.WriteLine(Wrong id, try again);
            Console.WriteLine(Please enter ID of volunteer to update );
            int vId;
            while (!int.TryParse(Console.ReadLine(), out vId))
                Console.WriteLine(Wrong id, try again);

            Console.WriteLine($Volunteer details {s_bl!.Volunteer.Read(vId)});
            Console.WriteLine(If you want to update, enter details. else click 'ENTER');
            BO.Volunteer volunteer = InputVolunteer();

            if (volunteer != null)
            {
                try
                {
                    if (volunteer.Id != vId)
                        Console.WriteLine(different id);
                    else
                        s_bl!.Volunteer.Update(userId, volunteer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

         summary
         Offer the call menu and perform the choice
         summary
        private static void CallMenu()
        {
            Console.WriteLine(Call menu);
            foreach (Call_Menu menu in Enum.GetValues(typeof(Call_Menu)))
            {
                Printing the menu
                Console.WriteLine(${(int)menu} {menu});
            }
            Call_Menu choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice)  !Enum.IsDefined(typeof(Call_Menu), choice))
                Console.WriteLine(Wrong choice, enter again);
            try
            {
                switch (choice)
                {
                    case Call_Menu.Create
                        BO.Call newCall = InputCall();
                        if (newCall != null)
                            s_bl.Call.Create(newCall);
                        break;
                    case Call_Menu.Read
                        Console.Write(Enter id );
                        int id;
                        while (!int.TryParse(Console.ReadLine(), out id))
                            Console.WriteLine(Id is illegal! enter again!);
                        Console.WriteLine(s_bl.Call.Read(id));
                        break;
                    case Call_Menu.ReadAll
                        ReadAllCalls();
                        break;
                    case Call_Menu.Update
                        UpdateCall();
                        break;
                    case Call_Menu.Delete
                        Console.Write(Enter id );
                        int cId;
                        while (!int.TryParse(Console.ReadLine(), out cId))
                            Console.WriteLine(Id is illegal! enter again!);
                        s_bl.Volunteer.Delete(cId);
                        break;
                    case Call_Menu.GetCountsGroupByStatus
                        int[] countArr = s_bl.Call.GetCountsGroupByStatus();
                        foreach (BO.CallStatus status in Enum.GetValues(typeof(BO.CallStatus)))
                            Console.WriteLine(${countArr[(int)status]} calls with {status} status);
                        break;
                    case Call_Menu.ReadAllVolunteerClosedCalls
                        ReadAllVolunteerClosedCalls();
                        break;
                    case Call_Menu.ReadAllVolunteerOpenCalls
                        ReadAllVolunteerOpenedCalls();
                        break;
                    case Call_Menu.FinishProcess
                        Console.WriteLine(Enter your id);
                        int vId;
                        while (!int.TryParse(Console.ReadLine(), out vId))
                            Console.WriteLine(Id is illegal! enter again!);
                        Console.WriteLine(Enter assignment id);
                        int aId;
                        while (!int.TryParse(Console.ReadLine(), out aId))
                            Console.WriteLine(Id is illegal! enter again!);
                        s_bl.Call.FinishProcess(vId, aId);
                        break;
                    case Call_Menu.CanceleProcess
                        Console.WriteLine(Enter your id);
                        int volId;
                        while (!int.TryParse(Console.ReadLine(), out volId))
                            Console.WriteLine(Id is illegal! enter again!);
                        Console.WriteLine(Enter assignment id);
                        int assId;
                        while (!int.TryParse(Console.ReadLine(), out assId))
                            Console.WriteLine(Id is illegal! enter again!);
                        s_bl.Call.CanceleProcess(volId, assId);
                        break;
                    case Call_Menu.ChooseCall
                        Console.WriteLine(Enter your id);
                        int volunteerId;
                        while (!int.TryParse(Console.ReadLine(), out volunteerId))
                            Console.WriteLine(Id is illegal! enter again!);
                        Console.WriteLine(Enter call id);
                        int callId;
                        while (!int.TryParse(Console.ReadLine(), out callId))
                            Console.WriteLine(Id is illegal! enter again!);
                        s_bl.Call.ChooseCall(volunteerId, callId);
                        break;
                    default
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            Console.WriteLine(Call menu);
            foreach (Volunteer_Menu menu in Enum.GetValues(typeof(Call_Menu)))
            {
                Printing the menu
                Console.WriteLine(${(int)menu} {menu});
            }
            while (!Enum.TryParse(Console.ReadLine(), out choice)  !Enum.IsDefined(typeof(Call_Menu), choice))
                Console.WriteLine(Wrong choice, enter again);
        }

         summary
         Inputs Call's details.
         summary
         returnsA call with the inputs details, or null if user entered type=''returns
        private static BO.Call InputCall(int cId = -1)
        {
            Console.Write(enter call type 0-2 );
            string typeStr = Console.ReadLine()!;
            if (typeStr == )
                return null;
            BO.Call_Type type;
            while (!(Enum.TryParse(typeStr, out type) && Enum.IsDefined(typeof(BO.Call_Type), type)))
            {
                Console.WriteLine(call type is illegal! enter again);
                typeStr = Console.ReadLine()!;
            }
            Console.Write(Enter address );
            string address = Console.ReadLine()!;
            DateTime open;
            while (!DateTime.TryParse(Console.ReadLine(), out open))
                Console.WriteLine(open time is illegal! enter again);
            Console.Write(enter max close time );
            string maxCloseStr = Console.ReadLine()!;
            DateTime maxClose;
            while (!DateTime.TryParse(maxCloseStr, out maxClose) && maxCloseStr != )
            {
                Console.WriteLine(max close is illegal! enter again);
                maxCloseStr = Console.ReadLine()!;
            }
            Console.Write(enter description );
            string description = Console.ReadLine()!;

            BO.Call newC = cId == -1  new(type, address, open, maxClose, description)
                 new(type, address, open, maxClose, description) { Id = cId };
            return newC;
        }

         summary
         read all calls filtered by the chosen field and value, and sorted by the chosen other field
         summary
        private static void ReadAllCalls()
        {
            Console.WriteLine(By which field do you want to filter);
            foreach (BO.Call_In_List_Fields field in Enum.GetValues(typeof(BO.Call_In_List_Fields)))
                Console.WriteLine(${(int)field} {field});

            string filterFieldStr = Console.ReadLine()!;
            BO.Call_In_List_Fields filterField = BO.Call_In_List_Fields.Id;
            while (filterFieldStr !=  && (!Enum.TryParse(filterFieldStr, out filterField)  !Enum.IsDefined(typeof(BO.Call_In_List_Fields), filterField)))
            {
                Console.WriteLine(Wrong field, enter again);
                filterFieldStr = Console.ReadLine()!;
            }

            Console.WriteLine(By which valud do you want to filter);
            Object filterVal = Console.ReadLine()!;

            Console.WriteLine(By which field do you want to sort);
            foreach (BO.Call_In_List_Fields f in Enum.GetValues(typeof(BO.Call_In_List_Fields)))
                Console.WriteLine(${(int)f} {f});

            string sortFieldStr = Console.ReadLine()!;
            BO.Call_In_List_Fields sortField = BO.Call_In_List_Fields.Id;
            while (sortFieldStr !=  && (!Enum.TryParse(sortFieldStr, out sortField)  !Enum.IsDefined(typeof(BO.Call_In_List_Fields), sortField)))
            {
                Console.WriteLine(Wrong field, enter again);
                sortFieldStr = Console.ReadLine()!;
            }

            foreach (BO.CallInList v in s_bl!.Call.
                ReadAll(filterFieldStr ==   null  filterField, filterVal, sortFieldStr ==   null  sortField))
                Console.WriteLine(v);
        }

         summary
         inputs exist call new details and update the call details to the new details
         summary
        private static void UpdateCall()
        {
            Console.WriteLine(Please enter ID of call to update );
            int cId;
            while (!int.TryParse(Console.ReadLine(), out cId))
                Console.WriteLine(Wrong id, try again);

            Console.WriteLine($Call details {s_bl!.Volunteer.Read(cId)});
            Console.WriteLine(If you want to update, enter details. else click ENTER );
            BO.Call call = InputCall(cId);

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

         summary
         inputs a volunteer id and read all his closed calls
         summary
        private static void ReadAllVolunteerClosedCalls()
        {
            Console.Write(Enter volunteer id );
            int vId;
            while (!int.TryParse(Console.ReadLine(), out vId))
                Console.WriteLine(Id is illegal! enter again!);

            Console.Write(Enter call type to filter );
            string typeStr = Console.ReadLine()!;
            BO.Call_Type type;
            while (!(Enum.TryParse(typeStr, out type) && Enum.IsDefined(typeof(BO.Call_Type), type)) && typeStr != )
            {
                Console.WriteLine(Type is illegal! enter again);
                typeStr = Console.ReadLine()!;
            }

            Console.Write(Enter field to sort );
            string fStr = Console.ReadLine()!;
            BO.Closed_Call_In_List_Fields f;
            while (!(Enum.TryParse(fStr, out f) && Enum.IsDefined(typeof(BO.Closed_Call_In_List_Fields), f)) && fStr != )
            {
                Console.WriteLine(Field is illegal! enter again);
                fStr = Console.ReadLine()!;
            }

            IEnumerableBO.ClosedCallInList callsArr = s_bl.Call.ReadAllVolunteerClosedCalls(vId, typeStr ==   null  type, fStr ==   null  f);
            foreach (BO.ClosedCallInList c in callsArr)
                Console.WriteLine(c);
        }

         summary
         inputs a volunteer id and read all his opened calls
         summary
        private static void ReadAllVolunteerOpenedCalls()
        {
            Console.Write(Enter volunteer id );
            int vId;
            while (!int.TryParse(Console.ReadLine(), out vId))
                Console.WriteLine(Id is illegal! enter again!);

            Console.Write(Enter call type to filter );
            string typeStr = Console.ReadLine()!;
            BO.Call_Type type;
            while (!(Enum.TryParse(typeStr, out type) && Enum.IsDefined(typeof(BO.Call_Type), type)) && typeStr != )
            {
                Console.WriteLine(Type is illegal! enter again);
                typeStr = Console.ReadLine()!;
            }

            Console.Write(Enter field to sort );
            string fStr = Console.ReadLine()!;
            BO.Open_Call_In_List_Fields f;
            while (!(Enum.TryParse(fStr, out f) && Enum.IsDefined(typeof(BO.Open_Call_In_List_Fields), f)) && fStr != )
            {
                Console.WriteLine(Field is illegal! enter again);
                fStr = Console.ReadLine()!;
            }

            IEnumerableBO.OpenCallInList callsArr = s_bl.Call.ReadAllVolunteerOpenCalls(vId, typeStr ==   null  type, fStr ==   null  f);
            foreach (BO.OpenCallInList c in callsArr)
                Console.WriteLine(c);
        }

         summary
         Offers the admin menu
         summary
        private static void AdminMenu()
        {
            Console.WriteLine(Admin menu);
            foreach (Admin_Menu menu in Enum.GetValues(typeof(Admin_Menu)))
            {
                Printing the menu
                Console.WriteLine(${(int)menu} {menu});
            }
            Admin_Menu choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice)  !Enum.IsDefined(typeof(Admin_Menu), choice))
                Console.WriteLine(Wrong choice, enter again);
            while (choice != Admin_Menu.Exit)
            {
                try
                {
                    switch (choice)
                    {
                        case Admin_Menu.GetClock
                            Console.WriteLine(s_bl.Admin.GetClock());
                            break;
                        case Admin_Menu.AdvanceClock
                            Console.Write(Enter time unit type to filter );
                            BO.Time_Unit unitTime;
                            while (!(Enum.TryParse(Console.ReadLine(), out unitTime) && Enum.IsDefined(typeof(BO.Time_Unit), unitTime)))
                                Console.WriteLine(Unit time is illegal! enter again);
                            s_bl.Admin.AdvanceClock(unitTime);
                            break;
                        case Admin_Menu.GetRiskRange
                            Console.WriteLine(s_bl.Admin.GetRiskRange());
                            break;
                        case Admin_Menu.SetRiskRange
                            Console.WriteLine(Enter risk range );
                            TimeSpan range;
                            while (!Enum.TryParse(Console.ReadLine(), out range))
                                Console.WriteLine(Risk range illegal, enter again);
                            s_bl.Admin.SetRiskRange(range);
                            break;
                        case Admin_Menu.Reset
                            s_bl.Admin.Reset();
                            break;
                        case Admin_Menu.Initialization
                            s_bl.Admin.Initialization();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Console.WriteLine(Admin menu);
                foreach (Admin_Menu menu in Enum.GetValues(typeof(Volunteer_Menu)))
                {
                    Printing the menu
                    Console.WriteLine(${(int)menu} {menu});
                }
                while (!Enum.TryParse(Console.ReadLine(), out choice)  !Enum.IsDefined(typeof(Admin_Menu), choice))
                    Console.WriteLine(Wrong choice, enter again);
            }
        }
    }
}
