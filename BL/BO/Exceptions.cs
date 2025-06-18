

namespace BO;

/// <summary>
///  exception for when trying use not exist entity
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// exception for when trying to add exist entity
/// </summary>
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// exception for when deleting is impossible
/// </summary>

[Serializable]
public class BlDeleteImpossible : Exception
{
    public BlDeleteImpossible(string? message) : base(message) { }
    public BlDeleteImpossible(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// exception for when load create xml file is failed
/// </summary>
[Serializable] 
public class BlXMLFileLoadCreateException : Exception
{
    public BlXMLFileLoadCreateException(string? message) : base(message) { }
    public BlXMLFileLoadCreateException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlConfigException : Exception
{
    public BlConfigException(string msg) : base(msg) { }
    public BlConfigException(string msg, Exception ex) : base(msg, ex) { }
}

/// <summary>
/// Exception for when try using null object
/// </summary>
/// <param name="message">exception message</param>
[Serializable]
public class BlNullPropertyException(string? message) : Exception(message)
{
}

/// <summary>
/// Exception for when order between dates is illegal
/// </summary>
/// <param name="message">exception message</param>
[Serializable]
public class BlIllegalDatesOrder(string? message) : Exception(message)
{
}

/// <summary>
/// Exception for when fields values are illegal
/// </summary>
/// <param name="message">exception message</param>
[Serializable]
public class BlIllegalValues(string? message) : Exception(message)
{
}

/// <summary>
/// Exception for when there is an exception when calculate latitude and longitude
/// </summary>
/// <param name="message">exception message</param>
[Serializable]
public class BlCoordinatesException(string? message) : Exception(message)
{
}

/// <summary>
/// Exception for when Finish assignment process is illegal
/// </summary>
/// <param name="message">exceotion mesage</param>
[Serializable]
public class BlFinishProcessIllegalException(string? message) : Exception(message)
{
}

/// <summary>
/// Exception for when Cancel assignment process is illegal
/// </summary>
/// <param name="message">exception message</param>
[Serializable]
public class BlCancelProcessIllegalException(string? message) : Exception(message)
{
}

/// <summary>
/// Exception for when updating is illegal
/// </summary>
/// <param name="message">exception message</param>
[Serializable]
public class BlUpdateImpossibleException(string? message) : Exception(message)
{
}

/// <summary>
/// exception to when trying to chose a call when it is imposible
/// </summary>
/// <param name="message">exception message</param>
[Serializable]
public class BlIllegalChoseCallException(string? message) : Exception(message)
{
}