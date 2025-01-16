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
        const int MIN_ID = 200000000;
        const int MAX_ID = 400000000;
        string[] vNames =
            ["Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof", "Sarah Cohen", "Jacob Levi", "Leah Goldstein", "David Rosenberg", "Rachel Schwartz", "Isaac Cohen", "Rebecca Levy", "Aaron Friedman", "Miriam Cohen", "Benjamin Levy", "Esther Cohen", "Samuel Goldberg", "Hannah Levy", "Solomon Cohen"];
        string[] vStartsPhones = ["05041", "05271", "05341", "05484", "05567", "05832"];
        string[] vAddresses =
            ["הרצל 10 כיכר המדינה ישראל",
            "דיזנגוף 48 תל אביב ישראל",
            "הירקון 7 נמל תל אביב ישראל",
            "רוטשילד 22 תל אביב ישראל",
            "בן יהודה 15 נמל חיפה ישראל",
            "המרכז 5 מרכז העיר חיפה ישראל",
            "הגבעה 3 מושב חורפיש ישראל",
            "הצופים 12 הר הצופים ישראל",
            "הנחל 30 מושב כפר סבא ישראל",
            "האלון 4 גבעתיים ישראל",
            "הצבי 9 נחלת בנימין ישראל",
            "היער 17 שכונת יערות ישראל",
            "הכרמל 6 כפר חסידים ישראל",
            "האדמור 11 שכונת אדמורים ישראל",
            "אלנבי 25 שינקין ישראל",
            "שפע חיים 14 ירושלים ישראל"];
        double[] vLatitude = [34.770410, 34.775382, 34.778712, 34.774574, 34.983579, 34.987190, 34.988004, 34.742663, 34.912945, 34.794960, 34.912181, 34.799589, 34.801429, 34.790317, 34.773367, 34.325698];
        double[] vLongitude = [32.093035, 32.075237, 32.074741, 32.069773, 32.820103, 32.820918, 32.820536, 32.977347, 32.175679, 32.072830, 32.175537, 32.070820, 32.070131, 32.074038, 32.072976, 32.845631];

        for (int i = 0; i < COUNT_VOLUNTEERS; i++)
        {
            int vId;
            do
                vId = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dal!.Volunteer!.Read(vId) != null);

            string vPhone = string.Concat(vStartsPhones[s_rand.Next(0, 6)], s_rand.Next(10000, 100000).ToString());
            string vEmail = string.Concat(vNames[i].Split(' ')[0], "@gmail.com");
            int maxDistance = s_rand.Next(10, 1000);
            Role role = i == managerIndex ? Role.Manager : Role.Volunteer;

            Volunteer newV = new (vId, vNames[i], vPhone, vEmail, vAddresses[i], vLatitude[i], vLongitude[i], maxDistance, role);
            try
            {
                s_dal!.Volunteer.Create(newV);
            }
            catch
            {
                i--;            //This iteration in the for didn't succeed, so we do it again
            }
        }
    }

    /// <summary>
    /// create 50 calls
    /// </summary>
    private static void CreateCalls() 
    {
        const int COUNT_CALLS = 50;
        string[] cAddresses =
            ["Har Safra St 1, Jerusalem", "Mount Scopus St 4, Jerusalem", "Keren Hayesod St 30, Jerusalem",
            "Neve Yaakov St 17, Jerusalem", "Shmuel HaNavi St 12, Jerusalem", "Yechiel St 3, Jerusalem",
            "Rav Kook St 4, Jerusalem", "Talmud Torah St 8, Jerusalem", "Sanhedria St 18, Jerusalem",
            "Kiryat Moshe St 6, Jerusalem", "Achad Ha'am St 2, Jerusalem", "Bar Ilan St 7, Jerusalem",
            "City Center St 14, Jerusalem", "Rechov Yechiel 3, Jerusalem", "Giv'at Shaul St 7, Jerusalem",
            "Nachlaot St 7, Jerusalem", "Rav Kook St 5, Jerusalem", "Har Nof St 18, Jerusalem",
            "Ramat Shlomo St 15, Jerusalem", "Sderot Yitzhak Rabin St 5, Jerusalem", "Har Hatzofim St 8, Jerusalem",
            "Giv'at HaMivtar St 6, Jerusalem", "Tefilat Yisrael St 14, Jerusalem", "Malkhei Yisrael St 10, Jerusalem",
            "Kiryat Tzahal St 6, Jerusalem", "Nachal Noach St 17, Jerusalem", "Maalot Dafna St 6, Jerusalem",
            "Har HaMor St 3, Jerusalem", "Ramat HaSharon St 2, Jerusalem", "Yakar St 3, Jerusalem",
            "Rav Haim Ozer St 9, Jerusalem", "Yehoshua Ben-Nun St 5, Jerusalem", "Meir Schauer St 12, Jerusalem",
            "Meah Shearim St 10, Jerusalem", "Chazon Ish St 6, Jerusalem", "Ramat Eshkol St 11, Jerusalem",
            "Menachem Begin St 11, Jerusalem", "Yisrael Yaakov St 13, Jerusalem", "Ben Yehuda St 6, Jerusalem" ,
            "Sulam Yaakov St 10, Jerusalem", "Rambam St 29 Bne-Brack", " Chazon Ish St 15, Bne-Brack", 
            "Ktzot Hashen St 3, Kiryat-Sefer", "Netivot Hamishpat St 15, Kiryat-Sefer", "HaRimon St 3, Telz-Stown",
            "Nachal Sorek St 18, Bet-Shemesh", "Shefa Chaim St 18, Netanya", "Maymon St 30, Bne-Brack", 
            "Nachl Shimshon St 6, Bet-Shemesh", "Bet Shamay St 20, Kiryat-Sefer"];
        double[] cLatitudes =
       [
            31.785228, 31.786335, 31.769799, 31.773315, 31.786812,
            31.776216, 31.773144, 31.764577, 31.767558, 31.774280,
            31.782129, 31.784256, 31.779211, 31.783858, 31.783022,
            31.774607, 31.773122, 31.782645, 31.783712, 31.773770,
            31.779614, 31.767658, 31.785070, 31.778488, 31.766734,
            31.780314, 31.783537, 31.775809, 31.773657, 31.781039,
            31.779433, 31.771505, 31.770824, 31.774722, 31.776229,
            31.773940, 31.777524, 31.774912, 31.770963, 31.777611,
            31.776545, 31.771675, 31.767727, 31.771267, 31.768520,
            31.776597, 31.785040, 31.772628, 31.776763, 31.780179
        ];
        double[] cLongitudes =
       [
            35.224211, 35.219538, 35.224968, 35.226063, 35.219375,
            35.213736, 35.217712, 35.229053, 35.217509, 35.220429,
            35.222809, 35.222797, 35.226436, 35.221255, 35.220655,
            35.229191, 35.222992, 35.227074, 35.221162, 35.227591,
            35.225712, 35.220829, 35.223016, 35.219865, 35.230012,
            35.220076, 35.221336, 35.228300, 35.221133, 35.224713,
            35.227271, 35.219754, 35.226358, 35.225099, 35.228086,
            35.228418, 35.222438, 35.221694, 35.223145, 35.221228,
            35.225721, 35.217133, 35.229169, 35.230535, 35.225939,
            35.222590, 35.222579, 35.222869, 35.226072, 35.221711
       ];
        string[] cDescriptions = ["7 years old boy", "Disabled boy", "Miserable and cute girl", "Have a brother's wedding", "Need phisyothraphy 5 times a week"];
        DateTime startOpen = s_dal!.Config.Clock.AddYears(-1);
        int range = (int)(s_dal!.Config.Clock - startOpen).TotalDays;

        for (int i = 0; i < COUNT_CALLS; i++)
        {
            Call_Type type = (Call_Type)s_rand.Next(0, 3);
            DateTime open = startOpen.AddDays(s_rand.Next(0,range));
            DateTime endClose = i > 45
                ? open.AddDays(3)
                : open.AddYears(1);   //in order that part of the calls (45-50) probably will be expired
            DateTime close = i>45
                ?endClose.AddDays(-s_rand.Next(0,3))
                :endClose.AddDays(-s_rand.Next(0,365));

            Call newC = new (type, cAddresses[i], cLatitudes[i], cLongitudes[i], open, close, cDescriptions[i%5]);


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
            
            Finish_Type? fType = calls[i].MaxCloseTime < s_dal!.Config.Clock ? Finish_Type.Expired :
                fTime != null ? (Finish_Type)s_rand.Next(0, 3) : null;

            Assignment newA = new(cId, vId, insersion, fTime, fType);
            s_dal!.Assignment.Create(newA);
        }

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