

namespace DO;

public record Call
(
 Call_Type Call_Type,
 string address,
 double Latitude,
 double Longitude,
 DateTime Open_Time,
  string? Description = null,
 DateTime? Close_Time = null
)
{
    public static int Id;
}


