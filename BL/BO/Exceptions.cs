

namespace BO;

/// <summary>
/// exception for when trying use not exist entity
/// </summary>
/// <param name="message">the string was throwed in the try</param>

[Serializable]
public class BlDoesNotExistException(string? message) : Exception(message)
{
}

/// <summary>
/// exception for when trying to add exist entity
/// </summary>
/// <param name="message">the string was throwed in the try</param>[Serializable]
public class BlAlreadyExistsException(string? message) : Exception(message)
{
}

/// <summary>
/// exception for when deleting is impossible
/// </summary>
/// <param name="message">the string was throwed in the try</param>[Serializable]
[Serializable]
public class BlDeleteImpossible(string? message) : Exception(message)
{
}

/// <summary>
/// exception for when load create xml file is failed
/// </summary>
/// <param name="message">the string was throwed in the try</param>[Serializable]
[Serializable]
public class BlXMLFileLoadCreateException(string? message) : Exception(message)
{ 
}

[Serializable]
public class BlConfigException : Exception
{
    public BlConfigException(string msg) : base(msg) { }
    public BlConfigException(string msg, Exception ex) : base(msg, ex) { }
}