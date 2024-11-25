/// <summary>
/// Volunteer Entity represents a student with all its props 
/// </summary> 
/// <param name=" Id "> Personal unique ID of the volunteer  </param> 
/// <param name=" Name "> Private Name of the volunteer </param> 
/// <param name=" Email "> email address of the volunteer </param> 
/// <param name=" Password "> password of privte volunteer </param>
/// <param name=" Address "> volunteer's address to know him area</param>
/// <param name=" Latitude "> distance from the equator to north or south</param>
/// <param name=" Longitude "> distance from the equator to east or west</param>
/// <param name=" Role "> volunteer's role</param>
/// <param name=" IsActive ">if volunteer is active he can procces</param>
/// <param name=" MaxDistanceCall ">volunteer will get procceses according to the max distance he chose</param>
/// <param name=" Distance_Type ">type of distance</param>

namespace DO;

public record Volunteer
(
    int Id,
    string Name,
    string Phone,
    string Email,
    double? MaxDistanceCall = null,
    Role Role = Role.Volunteer,
    Distance_Type Distance_Type = Distance_Type.Air,
    string? Password = null,
    string? Address = null,
    double? Latitude = null,
    double? Longitude = null,
    bool? IsActive = null
    )
{
    public Volunteer() : this(0, "no name", "000-000-0000", "email@gmail.com")
    { }
}
