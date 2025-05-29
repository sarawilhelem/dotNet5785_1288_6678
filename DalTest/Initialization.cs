namespace DalTest;
using DalApi;
using DO;


public static class Initialization
{
    /// <summary>
    /// static field to approach the cruds
    /// </summary>
    private static IDal? s_dal;

    /// <summary>
    /// static field which used to rand
    /// </summary>
    private static readonly Random s_rand = new();

    /// <summary>
    /// create 16 volunteers
    /// </summary>
    private static void CreateVolunteers()
    {
        const int COUNT_VOLUNTEERS = 16;
        int managerIndex = s_rand.Next(0, 20);
        const int MIN_ID = 20000000;
        const int MAX_ID = 40000000;
        string[] vNames =
            ["Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof", "Sarah Cohen", "Jacob Levi", "Leah Goldstein", "David Rosenberg", "Rachel Schwartz", "Isaac Cohen", "Rebecca Levy", "Aaron Friedman", "Miriam Cohen", "Benjamin Levy", "Esther Cohen", "Samuel Goldberg", "Hannah Levy", "Solomon Cohen"];
        string[] vStartsPhones = ["05041", "05271", "05341", "05484", "05567", "05832"];
        string[] vAddresses = [
        "Etsel 13, Jerusalem",
        "King David Street 7, Jerusalem",
        "Ben Yehuda Street 45, Jerusalem",
        "Jaffa Street 56, Jerusalem",
        "Agrippas Street 22, Jerusalem",
        "Shmuel Hanavi Street 5, Jerusalem",
        "Yehuda Halevi Street 3, Jerusalem",
        "Hillel Street 19, Jerusalem",
        "Ramban Street 9, Jerusalem",
        "Strauss Street 12, Jerusalem",
        "Yafo Road 34, Jerusalem",
        "Kehilat Yaakov Street 8, Jerusalem",
        "Mordechai Ben Hillel Street 11, Jerusalem",
        "Keren Hayesod Street 16, Jerusalem",
        "Shazar Boulevard 21, Jerusalem",
        "25 Shlomzion Hamalka Street, Jerusalem:",
        "Ha'arava 4, Eilat, Israel",
        "King George 15, Tel Aviv, Israel",
        "HaNevi'im 3, Jerusalem, Israel",
        "Sokolov 20, Holon, Israel" ];



        double[] vLatitude = [
          31.7884, 31.7767, 31.7810, 31.7835, 31.7822, 31.7963,
            31.7761, 31.7795, 31.7809, 31.7852, 31.7819, 31.7890,
            31.7830, 31.7745, 31.7897, 31.7773,29.5581, 32.0809, 31.7765, 32.0515 ];

        double[] vLongitude = [
            35.2132, 35.2285, 35.2206, 35.2150, 35.2158, 35.2201, 35.2234,
            35.2202, 35.2209, 35.2193, 35.2180, 35.2245, 35.2200, 35.2228,
            35.2034, 35.2256, 34.9482, 34.7792, 35.2344, 34.7734 ];



        for (int i = 0; i < COUNT_VOLUNTEERS; i++)
        {
            int vId;
            do
                vId = FullIdentityNumber(s_rand.Next(MIN_ID, MAX_ID));
            while (s_dal!.Volunteer!.Read(vId) != null);

            string vPhone = string.Concat(vStartsPhones[s_rand.Next(0, 6)], s_rand.Next(10000, 100000).ToString());
            string vEmail = string.Concat(vNames[i].Split(' ')[0], "@gmail.com");
            int maxDistance = s_rand.Next(10, 1000);
            Role role = i==managerIndex? Role.Manager : Role.Volunteer;
            Volunteer newV = new(vId, vNames[i], vPhone, vEmail, vAddresses[i], vLatitude[i], vLongitude[i], maxDistance, role);
            try
            {
                s_dal!.Volunteer.Create(newV);
            }
            catch
            {
                i--; 
            }
        }
    }

    /// <summary>
    /// create 50 calls
    /// </summary>
    
        public static void CreateCalls()
        {
            const int COUNT_CALLS = 50;
            string[] cAddresses =
            {
                "Meah Shearim St 10, Jerusalem", "Chazon Ish St 6, Jerusalem", 
                "Ramat Eshkol St 11, Jerusalem", "Har Safra St 1, Jerusalem", 
                "Mount Scopus St 4, Jerusalem", "Keren Hayesod St 30, Jerusalem",
                "Neve Yaakov St 17, Jerusalem", "Shmuel HaNavi St 12, Jerusalem", 
                "Yechiel St 3, Jerusalem", "Rav Kook St 4, Jerusalem", 
                "Talmud Torah St 8, Jerusalem", "Sanhedria St 18, Jerusalem",
                "Kiryat Moshe St 6, Jerusalem", "Achad Ha'am St 2, Jerusalem", 
                "Bar Ilan St 7, Jerusalem", "City Center St 14, Jerusalem", 
                "Rechov Yechiel 3, Jerusalem", "Giv'at Shaul St 7, Jerusalem", 
                "Nachlaot St 7, Jerusalem", "Rav Kook St 5, Jerusalem", 
                "Har Nof St 18, Jerusalem", "Ramat Shlomo St 15, Jerusalem", 
                "Sderot Yitzhak Rabin St 5, Jerusalem", "Har Hatzofim St 8, Jerusalem", 
                "Giv'at HaMivtar St 6, Jerusalem", "Tefilat Yisrael St 14, Jerusalem", 
                "Malkhei Yisrael St 10, Jerusalem", "Kiryat Tzahal St 6, Jerusalem", 
                "Nachal Noach St 17, Jerusalem", "Maalot Dafna St 6, Jerusalem", 
                "Har HaMor St 3, Jerusalem", "Ramat HaSharon St 2, Jerusalem", 
                "Yakar St 3, Jerusalem", "Rav Haim Ozer St 9, Jerusalem", 
                "Yehoshua Ben-Nun St 5, Jerusalem", "Meir Schauer St 12, Jerusalem", 
                "Menachem Begin St 11, Jerusalem", "Yisrael Yaakov St 13, Jerusalem", 
                "Ben Yehuda St 6, Jerusalem", "Hadar St 3, Jerusalem", 
                "Maharal St 8, Jerusalem", "Yosef Schwartz St 4, Jerusalem", 
                "Jabotinsky St 7, Jerusalem", "Shazar St 5, Jerusalem", 
                "Gonenim St 12, Jerusalem", "Talpiot St 14, Jerusalem", 
                "Bilu St 9, Jerusalem", "Yovel St 2, Jerusalem", 
                "Herzl St 3, Jerusalem", "Hashmonai St 6, Jerusalem", 
                "Ramot St 17, Jerusalem", "Shalom Aleichem St 10, Jerusalem", 
                "Eli Cohen St 4, Jerusalem", "Shlomo HaMelech St 7, Jerusalem"
            };

            double[] cLatitudes =
            {
                31.7857, 31.7924, 31.7970, 31.7780, 31.7947, 31.7743,
                31.8300, 31.7965, 31.7762, 31.7818, 31.7800, 31.7995,
                31.7875, 31.7765, 31.7910, 31.7805, 31.7762, 31.7975,
                31.7810, 31.7819, 31.7730, 31.8100, 31.7685, 31.7940,
                31.7978, 31.7833, 31.7850, 31.7802, 31.7935, 31.7945,
                31.7650, 31.7930, 31.7760, 31.7855, 31.7768, 31.7785,
                31.7860, 31.7800, 31.7790, 31.7788, 31.7765, 31.7740,
                31.7710, 31.7725, 31.7735, 31.7745, 31.7750, 31.7780,
                31.7820, 31.7840, 31.7865, 31.7783, 31.7760
            };

            double[] cLongitudes =
            {
                35.2250, 35.2200, 35.2195, 35.2130, 35.2430, 35.2225,
                35.2190, 35.2205, 35.2255, 35.2180, 35.2200, 35.2190,
                35.2020, 35.2205, 35.2195, 35.2150, 35.2255, 35.1900,
                35.2155, 35.2185, 35.1700, 35.2300, 35.2100, 35.2435,
                35.2190, 35.2200, 35.2205, 35.2250, 35.2200, 35.2200,
                35.2300, 35.2200, 35.2250, 35.2200, 35.2250, 35.2130,
                35.2200, 35.2200, 35.2200, 35.2200, 35.2200, 35.2200,
                35.2200, 35.2200, 35.2200, 35.2200, 35.2200, 35.2200,
                35.2200, 35.2200, 35.2200, 35.2200, 35.2200
            };

            string[] cDescriptions = 
            {
                "7 years old boy", "Disabled boy", "Miserable and cute girl", 
                "Have a brother's wedding", "Need physiotherapy 5 times a week"
            };

            DateTime startOpen = DateTime.Now.AddYears(-1);
            int range = (int)(DateTime.Now - startOpen).TotalDays;

            for (int i = 0; i < COUNT_CALLS; i++)
            {
                CallType type = (CallType)s_rand.Next(0, 3);
                DateTime open = startOpen.AddDays(s_rand.Next(0, range));

                DateTime close = open.AddDays(s_rand.Next(1, 365)); 

                Call newC = new Call(type, cAddresses[i], cLatitudes[i], cLongitudes[i], open, close, cDescriptions[i % cDescriptions.Length]);

                s_dal!.Call.Create(newC);
            }
        }

    /// <summary>
    /// create 50 assignments according to the rules
    /// </summary>
    private static void CreateAssignments()
    {
        const int COUNT_ASSIGNMENTS = 50;
        List<Call> calls = (List<Call>)s_dal!.Call.ReadAll().ToList();
        List<Volunteer> volunteers = (List<Volunteer>)s_dal!.Volunteer.ReadAll().ToList();

        for (int i = 0; i < COUNT_ASSIGNMENTS; i++)
        {
            int cId = calls[i].Id;
            int vId = i < 10
                ? volunteers[s_rand.Next(0, 4)].Id
                : volunteers[s_rand.Next(0, 14)].Id;   //in order that part of the volunteers (0-3) proccessed did many calls, part of them (4-13) some calls, and part of them (14-15) no calls
            DateTime insersion = calls[i].OpenTime.AddDays(s_rand.Next(0, (int)(calls[i].MaxCloseTime - calls[i].OpenTime).TotalDays));
            DateTime? fTime;
            try
            {
                //To cope the case that config.clock is before insersion (It can be because the user can set the clock
                fTime = calls[i].MaxCloseTime < s_dal!.Config.Clock
                ? calls[i].MaxCloseTime : i > 10
                    ? insersion.AddDays(s_rand.Next(0, (int)(s_dal!.Config.Clock - insersion).TotalDays))
                    : null;
            }
            catch
            {
                fTime = null;
            }

            FinishType? fType = calls[i].MaxCloseTime < s_dal!.Config.Clock ? FinishType.Expired :
                fTime != null ? (FinishType)s_rand.Next(0, 3) : null;

            Assignment newA = new(cId, vId, insersion, fTime, fType);
            s_dal!.Assignment.Create(newA);
        }

    }

    /// <summary>
    /// function to calculate the check digit in the identity number
    /// </summary>
    /// <param name="identityNumber">8 first digits in id</param>
    /// <returns>all 9 digits of id</returns>
    /// <exception cref="ArgumentException">if the given number is not 8 digits</exception>
    public static int FullIdentityNumber(int identityNumber)
    {
        string identityString = identityNumber.ToString();

        if (identityString.Length != 8)
        {
            throw new ArgumentException("Identity number must be 8 digits long.");
        }

        int sum = 0;
        for (int i = 0; i < identityString.Length; i++)
        {
            int digit = int.Parse(identityString[i].ToString());
            if (i % 2 == 1)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }
            sum += digit;
        }

        int checkDigit = (10 - (sum % 10)) % 10;

        return int.Parse(identityString + checkDigit.ToString());
    }




    /// <summary>
    /// static function to call the function in order to init
    /// </summary>
    public static void Do()
    {
        s_dal = DalApi.Factory.Get;

        s_dal.ResetDB();

        CreateVolunteers();
        CreateCalls();
        CreateAssignments();
    }
}