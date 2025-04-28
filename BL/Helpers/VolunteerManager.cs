using DalApi;
using System.Text;
using System.Security.Cryptography;

namespace Helpers;

internal static class VolunteerManager
{
    /// <summary>
    /// A static field to approach the entities crud
    /// </summary>
    private static IDal s_dal = Factory.Get;

    internal static ObserverManager Observers = new();
    /// <summary>
    /// Checks the validation of the volunteer details
    /// </summary>
    /// <param name="v">a volunteer to check its details</param>
    /// <returns>return true if all details valid, else- false</returns>
    static internal bool CheckValidation(BO.Volunteer v)
    {
        var trimmedEmail = v.Email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false;
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
        if (v.Password is not null && !IsStrongPassword(v.Password))
            return false;
        return true;
    }

    /// <summary>
    /// Checks an Israeli id number validation
    /// </summary>
    /// <param name="id">an id to check if valid</param>
    /// <returns>is the id valid</returns>
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

    /// <summary>
    /// Checks if a password is strong - has at least 8 chars and contains: upper and little leters, numbers and special chars
    /// </summary>
    /// <param name="password">A password to check if strong</param>
    /// <returns>Is the password strong</returns>
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

    /// <summary>
    /// Hash a password
    /// </summary>
    /// <param name="password">The password to encryp</param>
    /// <returns>the cipherText</returns>
    public static string? HashPassword(string? password)
    {
        if (password == null)
            return null;
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower(); // Convert byte array to hex string
        }
    }

    

    /// <summary>
    /// Calculate distance betweent 2 addresses
    /// </summary>
    /// <param name="lat1">The latitude of the first address</param>
    /// <param name="lon1">The longitude of the first address</param>
    /// <param name="lat2">The latitude of the second address</param>
    /// <param name="lon2">The longitude of the second address</param>
    /// <returns>The distance between the addresses</returns>
    internal static double CalculateDistance(double? lat1, double? lon1, double? lat2, double? lon2)
    {
        const double EarthRadius = 6371.0;

        if (lat1 is null || lon1 is null || lat2 is null || lon2 is null)
            return 0;

        double lat1Rad = lat1.Value * Math.PI / 180.0;
        double lon1Rad = lon1.Value * Math.PI / 180.0;
        double lat2Rad = lat2.Value * Math.PI / 180.0;
        double lon2Rad = lon2.Value * Math.PI / 180.0;

        double dlat = lat2Rad - lat1Rad;
        double dlon = lon2Rad - lon1Rad;

        double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                   Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                   Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadius * c;
    }

    /// <summary>
    /// calculate if now is already in risk range to the given maxClose
    /// </summary>
    /// <param name="maxClose">the max close to check if now is in risk range to it</param>
    /// <returns>true if now is in risk range and false if not</returns>
    internal static bool IsWithinRiskRange(DateTime maxClose)
    {
        BlApi.IBl bl = BlApi.Factory.Get();
        DateTime now = AdminManager.Now;
        TimeSpan range = bl.Admin.GetRiskRange();
        DateTime rangeStart = maxClose.Add(-range);
        return now >= rangeStart && now <= maxClose;
    }
}
