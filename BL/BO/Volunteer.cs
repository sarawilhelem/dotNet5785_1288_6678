using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
/// <summary>
/// Volunteer Entity represents a volunteer with all its props 
/// </summary> 
/// <param name=" Id "> Personal unique ID of the volunteer  </param> 
/// <param name=" Name "> Private Name of the volunteer </param> 
/// <param name=" Phone "> phone of the volunteer </param> 
/// <param name=" Email "> email address of the volunteer </param> 
/// <param name=" Password "> password of privte volunteer </param>
/// <param name=" Address "> volunteer's address to know him area</param>
/// <param name=" Latitude "> distance from the equator to north or south</param>
/// <param name=" Longitude "> distance from the equator to east or west</param>
/// <param name=" Role "> volunteer's role</param>
/// <param name=" IsActive ">if volunteer is active he can procces</param>
/// <param name=" MaxDistanceCall ">volunteer will get procceses according to the max distance he chose</param>
/// <param name=" DistanceType ">type of distance</param>
/// <param name=" NumCallsHandle ">how many calls this volunteer handled</param>
/// <param name=" NumCallsCancel ">how many calls this volunteer canceled</param>
/// <param name=" call ">a call the volunteer process now</param>


public class Volunteer
{

    public int Id { get; init; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Role Role { get; set; } = Role.Volunteer;
    public bool IsActive { get; set; }
    public double? MaxDistance { get; set; }
    public DistanceType DistanceType { get; set; } = DistanceType.Air;
    public int NumCallsHandle { get; set; } = 0;
    public int NumCallsCancel { get; set; } = 0;
    public int NumCallsNotValid { get; set; } = 0;
    public BO.CallInProgress? Call { get; set; }

    public Volunteer(
        int id,
        string name,
        string phone,
        string email,
        string? password = null,
        string? address = null,
        double? latitude = null,
        double? longitude = null,
        Role role = Role.Volunteer,
        bool isActive = true,
        double? maxDistance = null,
        DistanceType distanceType = DistanceType.Air,
        int numCallsHandle = 0,
        int numCallsCancel = 0,
        int numCallsNotValid = 0,
        BO.CallInProgress? call = null)
    {
        Id = id;
        Name = name;
        Phone = phone;
        Email = email;
        Password = password;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
        Role = role;
        IsActive = isActive;
        MaxDistance = maxDistance;
        DistanceType = distanceType;
        NumCallsHandle = numCallsHandle;
        NumCallsCancel = numCallsCancel;
        NumCallsNotValid = numCallsNotValid;
        Call = call;
    }
    public override string ToString() => Helpers.Tools.ToStringProperty(this);

}
