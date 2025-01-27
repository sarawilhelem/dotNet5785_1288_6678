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
        public Finish_Type? FinishType { get; set; }
    }
}
