using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class VolunteerInList
    {
        public int Id {  get; set; }
        required public string Name { get; set; }
        public bool IsActive { get; set; }
        public int NumCallsHanle {  get; set; }
        public int NumCallsCancele {  get; set; }
        public int NumCallsNotValid {  get; set; }
        public int? CallId { get; set; }
        public Call_Type Call_Type { get; set; }
    }
}
