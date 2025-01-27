

using DalApi;

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

        if(!decimal.TryParse(v.Phone, out decimal phoneInt) || v.Phone[0]!=0 || v.Phone.Length!=10)
            return false;

    }


}
