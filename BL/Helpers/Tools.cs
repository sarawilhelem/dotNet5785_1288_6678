using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;
using System.Text.Json;

namespace Helpers;

static internal class Tools
{

    /// <summary>
    /// calculate the address' longitude and latiude
    /// </summary>
    /// <param name="address">An address to casculate latitude and longitude</param>
    /// <returns>The latitude and longitude</returns>
    /// <exception cref="BO.BlCoordinatesException">If there is a problem when calculate coorcodination</exception>
    public static async Task<(double?, double?)> GetCoordinatesAsync(string address)
    {
        const string apiKey = "PK.83B935C225DF7E2F9B1ee90A6B46AD86";

        using var client = new HttpClient();
        string url = $"https://us1.locationiq.com/v1/search.php?key={apiKey}&q={Uri.EscapeDataString(address)}&format=json";

        var response = await client.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            await Task.Delay(1000);
            response = await client.GetAsync(url);
        }

        if (!response.IsSuccessStatusCode)
            throw new BO.BlCoordinatesException("Invalid address or API error.");

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0)
            throw new BO.BlCoordinatesException("Address not found.");

        var root = doc.RootElement[0];

        if (!root.TryGetProperty("lat", out var latProperty) ||
            !root.TryGetProperty("lon", out var lonProperty))
        {
            throw new BO.BlCoordinatesException("Missing latitude or longitude in response.");
        }

        if (!double.TryParse(latProperty.GetString(), out double latitude) ||
            !double.TryParse(lonProperty.GetString(), out double longitude))
        {
            throw new BO.BlCoordinatesException("Invalid latitude or longitude format.");
        }

        return (latitude, longitude);
    }



    /// <summary>
    /// converting an object to string by all his properties
    /// </summary>
    /// <param name="obj">the object to convert</param>
    /// <returns>the string of all proporties</returns>
    public static string ToStringProperty(object obj)
    {
        if (obj == null)
            return "Object is null";

        Type objectType = obj.GetType();
        PropertyInfo[] properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        if (properties.Length == 0)
            return "No public properties found";

        string result = "";

        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            if (value is IList list)
            {
                result += $"{property.Name}:\n";
                foreach (var item in list)
                {
                    result += $"  - {item}\n";
                }
            }
            else
            {
                result += $"{property.Name}: {value}\n";
            }
        }

        return result;
    }
}