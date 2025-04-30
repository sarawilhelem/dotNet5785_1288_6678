using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    internal class VolunteersCollection : IEnumerable
    {
        static readonly IEnumerable<BO.VolunteerInList> s_enums =
    (Enum.GetValues(typeof(BO.VolunteerInList)) as IEnumerable<BO.VolunteerInList>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class CallsCollection : IEnumerable
    {
        static readonly IEnumerable<BO.CallInList> s_enums =
    (Enum.GetValues(typeof(BO.CallInList)) as IEnumerable<BO.CallInList>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
}
