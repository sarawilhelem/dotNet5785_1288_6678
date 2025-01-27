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
        public string name {  get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
        public string? Address {  get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Role Role { get; set; }
        public bool IsActive {  get; set; }
        public double? MaxDistance {  get; set; }
        public Distance_Type Distance_Type { get; set; }
        public int NumCallsHanle { get; set; }
        public int NumCallsCancele { get; set ; }
        public int NumCallsNotValid { get; set; }
        public BO.CallInProgress? Call {  get; set; }

    }
}
