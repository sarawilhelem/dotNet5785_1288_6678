using System;
using System.Collections;
using System.Collections.Generic;
using BO;

namespace PL;

internal class VolunteerInListFieldsCollection : IEnumerable
{
    private static readonly IEnumerable<VolunteerInListFields> s_enums = (Enum.GetValues(typeof(VolunteerInListFields)) as IEnumerable<VolunteerInListFields>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class CallInListFieldsCollection : IEnumerable
{
    private static readonly IEnumerable<CallInListFields> s_enums = (Enum.GetValues(typeof(CallInListFields)) as IEnumerable<CallInListFields>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class OpenCallInListFieldsCollection : IEnumerable
{
    private static readonly IEnumerable<OpenCallInListFields> s_enums = (Enum.GetValues(typeof(OpenCallInListFields)) as IEnumerable<OpenCallInListFields>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class ClosedCallInListFieldsCollection : IEnumerable
{
    private static readonly IEnumerable<ClosedCallInListFields> s_enums = (Enum.GetValues(typeof(ClosedCallInListFields)) as IEnumerable<ClosedCallInListFields>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class FinishCallTypeCollection : IEnumerable
{
    private static readonly IEnumerable<FinishCallType> s_enums = (Enum.GetValues(typeof(FinishCallType)) as IEnumerable<FinishCallType>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class FinishTypeCollection : IEnumerable
{
    private static readonly IEnumerable<FinishType> s_enums = (Enum.GetValues(typeof(FinishType)) as IEnumerable<FinishType>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class CallTypeCollection : IEnumerable
{
    private static readonly IEnumerable<CallType> s_enums = (Enum.GetValues(typeof(CallType)) as IEnumerable<CallType>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class CallStatusCollection : IEnumerable
{
    private static readonly IEnumerable<CallStatus> s_enums = (Enum.GetValues(typeof(CallStatus)) as IEnumerable<CallStatus>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class DistanceTypeCollection : IEnumerable
{
    private static readonly IEnumerable<DistanceType> s_enums = (Enum.GetValues(typeof(DistanceType)) as IEnumerable<DistanceType>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class RoleCollection : IEnumerable
{
    private static readonly IEnumerable<Role> s_enums = (Enum.GetValues(typeof(Role)) as IEnumerable<Role>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}