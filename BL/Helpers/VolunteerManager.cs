using DalApi;
using System.Text;
using System.Security.Cryptography;
using System;
using System.IO;

namespace Helpers;

internal static class VolunteerManager
{
    /// <summary>
    /// A static field to approach the entities crud
    /// </summary>
    private static IDal s_dal = Factory.Get;

    private static readonly byte[] Key = Encoding.UTF8.GetBytes("f9tkb05nv8f4n6mb"); // Must be 16 bytes for AES-128
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("ngi6kg9fn5mbo98g"); // Must be 16 bytes


    internal static ObserverManager Observers = new();

    /// <summary>
    /// Checks the validation of the volunteer details
    /// </summary>
    /// <param name="v">a volunteer to check its details</param>
    /// <exception cref="BO.BlIllegalValues">throwen where there is a illegal detail</exception>
    static internal void CheckValidation(BO.Volunteer v)
    {
        var trimmedEmail = v.Email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            throw new BO.BlIllegalValues("Email cannot end with a period.");
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(v.Email);
            if (addr.Address != trimmedEmail)
                throw new BO.BlIllegalValues("Invalid email format.");
        }
        catch
        {
            throw new BO.BlIllegalValues("Invalid email address.");
        }

        if (!decimal.TryParse(v.Phone, out decimal phoneInt) || v.Phone[0] != '0' || v.Phone.Length != 10)
            throw new BO.BlIllegalValues("Phone number must start with '0' and be exactly 10 digits long.");

        if (!IsValidIdNumber(v.Id))
            throw new BO.BlIllegalValues("Invalid ID number.");

        if (v.Password is not null && v.Password != "" && !IsStrongPassword(v.Password))
            throw new BO.BlIllegalValues("Password must be at least 8 characters long and contain upper and lower case letters, numbers, and special characters.");
    }

    /// <summary>
    /// Checks an Israeli id number validation
    /// </summary>
    /// <param name="id">an id to check if valid</param>
    /// <returns>is the id valid</returns>
    private static bool IsValidIdNumber(int id)
    {
        if (!int.TryParse(id.ToString(), out int validId))
            throw new BO.BlIllegalValues("ID number must be a valid integer.");

        string idStr = validId.ToString().PadLeft(9, '0');

        if (idStr.Length != 9)
            throw new BO.BlIllegalValues("ID number must be 9 digits long.");

        int sum = 0;
        for (int i = 0; i < idStr.Length - 1; i++)
        {
            int digit = int.Parse(idStr[i].ToString());
            if (i % 2 == 1)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
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
            throw new BO.BlIllegalValues("Password must be at least 8 characters long.");

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

        if (!hasUpperCase || !hasLowerCase || !hasDigit || !hasSpecialChar)
            throw new BO.BlIllegalValues("Password must contain upper and lower case letters, numbers, and special characters.");

        return true;
    }

    /// <summary>
    /// Encrypt a password
    /// </summary>
    /// <param name="plainText">The text to encryp</param>
    /// <returns>the cipherText</returns>
    public static string? Encrypt(string? plainText)
    {
        if (plainText == null)
            return null;
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
        using (StreamWriter sw = new(cs))
        {
            sw.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    /// Decrypt a password
    /// </summary>
    /// <param name="cipherText">the text to decrypt</param>
    /// <returns>the plainText</returns>
    public static string? Decrypt(string? cipherText)
    {
        if (cipherText == null)
            return null;
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using MemoryStream ms = new (Convert.FromBase64String(cipherText));
        using CryptoStream cs = new (ms, decryptor, CryptoStreamMode.Read);
        using StreamReader sr = new (cs);
        return sr.ReadToEnd();
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

    internal static void Create(DO.Volunteer volunteer)
    {
        lock (AdminManager.BlMutex)
            s_dal.Volunteer.Create(volunteer);
    }
    internal static void Update(DO.Volunteer volunteer)
    {
        lock (AdminManager.BlMutex)
            s_dal.Volunteer.Update(volunteer);
    }
    internal static void Delete(int volunteerId)
    {
        lock (AdminManager.BlMutex)
            s_dal.Volunteer.Delete(volunteerId);
    }
}
