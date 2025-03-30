using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CallAssignInList
    {
        public int? VolunteerId {  get; set; }
        public string? Name { get; set; }
        public DateTime Insersion {  get; set; }
        public DateTime? FinishTime {  get; set; }
        public FinishType? FinishType { get; set; }

        public CallAssignInList(int? volunteerId, string? name, DateTime insersion, DateTime? finishTime, FinishType? finishType)
        {
            VolunteerId = volunteerId;
            Name = name;
            Insersion = insersion;
            FinishTime = finishTime;
            FinishType = finishType;
        }
        public override string ToString() => Helpers.Tools.ToStringProperty(this);

    }
}
