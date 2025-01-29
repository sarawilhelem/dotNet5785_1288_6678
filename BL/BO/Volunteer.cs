using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
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
        public Distance_Type Distance_Type { get; set; } = Distance_Type.Air;
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
            Distance_Type distanceType = Distance_Type.Air,
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
            Distance_Type = distanceType;
            NumCallsHandle = numCallsHandle;
            NumCallsCancel = numCallsCancel;
            NumCallsNotValid = numCallsNotValid;
            Call = call;
        }
    }

}
