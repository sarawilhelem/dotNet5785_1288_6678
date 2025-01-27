using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using DalApi;
using System.Runtime.InteropServices;

namespace Helpers;

internal static class VolunteerManager
{
    private static IDal s_dal = Factory.Get;
    static public bool CheckValidation(BO.Volunteer v)
    {
        var trimmedEmail = v.Email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(v.Email);
            if (addr.Address != trimmedEmail)
                return false;
        }
        catch
        {
            return false;
        }

        if (!decimal.TryParse(v.Phone, out decimal phoneInt) || v.Phone[0] != '0' || v.Phone.Length != 10)
            return false;
        if (!IsValidIdNumber(v.Id))
            return false;
        if (!IsStrongPassword(v.Password))
            return false;
        return true;
    }
    private static bool IsValidIdNumber(int id)
    {
        string idStr = id.ToString();

        if (idStr.Length != 9)
            return false;

        int sum = 0;
        for (int i = 0; i < idStr.Length - 1; i++)
        {
            int digit = int.Parse(idStr[i].ToString());
            if (i % 2 == 1)
            {
                digit *= 2;
                if (digit > 9) digit -= 9;
            }
            sum += digit;
        }

        int lastDigit = int.Parse(idStr[idStr.Length - 1].ToString());
        return (sum + lastDigit) % 10 == 0;
    }

    private static bool IsStrongPassword(string password)
    {
        if (password.Length < 8)
            return false;

        bool hasUpperCase = false;
        bool hasLowerCase = false;
        bool hasDigit = false;
        bool hasSpecialChar = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c)) hasUpperCase = true;
            if (char.IsLower(c)) hasLowerCase = true;
            if (char.IsDigit(c)) hasDigit = true;
            if (!char.IsLetterOrDigit(c)) hasSpecialChar = true;
        }
        return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
    }


    internal static async Task<(double? latitude, double? longitude)> GetCoordinatesAsync(string address)
    {
        string apiKey = "API_KEY";
        string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={apiKey}";

        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync(url);
            var json = JObject.Parse(response);

            if (json["status"].ToString() == "OK")
            {
                var location = json["results"][0]["geometry"]["location"];
                double latitude = (double)location["lat"];
                double longitude = (double)location["lng"];
                return (latitude, longitude);
            }
        }

        return (null, null);
    }

    internal  static void Create(DO.Volunteer volunteer)
    {
        s_dal.Volunteer.Create(volunteer);
    }

}
