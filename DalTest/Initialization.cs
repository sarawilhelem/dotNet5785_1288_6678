

namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{

    private static IAssignment? s_dalAssignment;
    private static ICall? s_dalCall;
    private static IVolunteer? s_dalVolunteer;
    private static IConfig? s_dalConfig;

    private static readonly Random s_rand = new();

    private static void createVolunteers()
    {
        const int COUNT_VOLUNTEERS = 20;
        int managerIndex = s_rand.Next(0, 19);
        const int MIN_ID = 200000000;
        const int MAX_ID = 399999999;
        string[] vNames =
            { "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof", "Sarah Cohen", "Jacob Levi", "Leah Goldstein", "David Rosenberg", "Rachel Schwartz", "Isaac Cohen", "Rebecca Levy", "Aaron Friedman", "Miriam Cohen", "Benjamin Levy", "Esther Cohen", "Samuel Goldberg", "Hannah Levy", "Solomon Cohen" };
        string[] vStartsPhones = { "05041", "05271", "05341", "05484", "05567", "05832" };
        string[] vAddresses = {
            "Sunset Blvd, 123, Los Angeles, USA",
            "Broadway, 456, New York City, USA",
           "Oxford Street, 789, London, UK",
           "Champs-Élysées, 101, Paris, France",
           "Alexanderplatz, 222, Berlin, Germany",
           "Gran Vía, 333, Madrid, Spain",
           "Shibuya Crossing, 444, Tokyo, Japan",
            "Sydney Opera House, 555, Sydney, Australia",
            "Piazza San Marco, 666, Venice, Italy",
            "Red Square, 777, Moscow, Russia",
           "Copacabana Beach, 888, Rio de Janeiro, Brazil",
            "Cape Town Waterfront, 999, Cape Town, South Africa",
            "Petronas Towers, 111, Kuala Lumpur, Malaysia",
           "Burj Khalifa, 222, Dubai, UAE",
            "Taj Mahal, 333, Agra, India",
            "Machu Picchu, 444, Cusco, Peru",
            "Great Wall of China, 555, Beijing, China",
            "Colosseum, 666, Rome, Italy",
           "Christ the Redeemer, 777, Rio de Janeiro, Brazil",
           "Golden Gate Bridge, 888, San Francisco, USA"
        };
        for (int i = 0; i < COUNT_VOLUNTEERS; i++)
        {
            int vId;
            do
                vId = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalVolunteer!.Read(vId) != null);

            string vPhone = string.Concat(vStartsPhones[s_rand.Next(0, 5)], s_rand.Next(0, 99999).ToString());
            string vEmail = string.Concat(vNames[i].Split(' ')[0], "@gmail.com");
            int maxDistance = s_rand.Next(10, 1000);
            Role role = i == managerIndex ? Role.Manager : Role.Volunteer;
            Volunteer newV = new Volunteer(vId, vNames[i], vPhone, vEmail, maxDistance, role);
            try
            {
                s_dalVolunteer.Create(newV);
            }
            catch
            {
                i--;            //This iteration in the for didn't succeed, so we do it again
            }
        }
    }
    private static void createCalls()
    {
        const int COUNT_CALLS = 50;
        string[] cAddresses = {
            "Sunset Blvd, 123, Los Angeles, USA",
            "Broadway, 456, New York City, USA",
            "Oxford Street, 789, London, UK",
            "Champs-Élysées, 101, Paris, France",
            "Alexanderplatz, 222, Berlin, Germany",
            "Gran Vía, 333, Madrid, Spain",
            "Shibuya Crossing, 444, Tokyo, Japan",
            "Sydney Opera House, 555, Sydney, Australia",
            "Piazza San Marco, 666, Venice, Italy",
            "Red Square, 777, Moscow, Russia",
            "Copacabana Beach, 888, Rio de Janeiro, Brazil",
            "Cape Town Waterfront, 999, Cape Town, South Africa",
            "Petronas Towers, 111, Kuala Lumpur, Malaysia",
            "Burj Khalifa, 222, Dubai, UAE",
            "Taj Mahal, 333, Agra, India",
            "Machu Picchu, 444, Cusco, Peru",
            "Great Wall of China, 555, Beijing, China",
            "Colosseum, 666, Rome, Italy",
            "Christ the Redeemer, 777, Rio de Janeiro, Brazil",
            "Golden Gate Bridge, 888, San Francisco, USA",
            "Table Mountain, 999, Cape Town, South Africa",
            "Grand Canyon, 111, Arizona, USA",
            "Eiffel Tower, 222, Paris, France",
            "Statue of Liberty, 333, New York City, USA",
            "Acropolis of Athens, 444, Athens, Greece",
            "Angkor Wat, 555, Siem Reap, Cambodia",
            "Alhambra, 666, Granada, Spain",
            "Hollywood Sign, 777, Los Angeles, USA",
            "Neuschwanstein Castle, 888, Bavaria, Germany",
            "Sagrada Família, 999, Barcelona, Spain",
            "Buckingham Palace, 111, London, UK",
            "Forbidden City, 222, Beijing, China",
            "Saint Basil's Cathedral, 333, Moscow, Russia",
            "Sydney Harbour Bridge, 444, Sydney, Australia",
            "Zhangjiajie National Forest Park, 555, Zhangjiajie, China",
            "Petra, 666, Ma'an, Jordan",
            "Mesa Verde, 777, Colorado, USA",
            "Dubrovnik Old Town, 888, Dubrovnik, Croatia",
            "Mont Saint-Michel, 999, Normandy, France",
            "Palace of Versailles, 111, Versailles, France",
            "Burano Island, 222, Venice, Italy",
            "Santorini, 333, Cyclades, Greece",
            "Tulum, 444, Quintana Roo, Mexico",
            "Meteora, 555, Thessaly, Greece",
            "Yosemite National Park, 666, California, USA",
            "Victoria Falls, 777, Livingstone, Zambia",
            "Antelope Canyon, 888, Arizona, USA",
            "Bora Bora, 999, French Polynesia"
        };
        string[] cDescriptions = {"7 years old boy", "Disabled boy", "Miserable and cute girl", "Have a brother's wedding", "Need phisyothraphy 5 times a week"};
        for (int i = 0; i< COUNT_CALLS;i++)
        {
            Call_Type type = (Call_Type)s_rand.Next(0, 2);

        }
    }
}
