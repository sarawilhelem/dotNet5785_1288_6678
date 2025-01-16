

namespace DO;

/// <summary>
/// exception for when trying use not exist entity
/// </summary>
/// <param name="message">the string was throwed in the try</param>

[Serializable]
public class DalDoesNotExistException(string? message) : Exception(message)
{
}

/// <summary>
/// exception for when trying to add exist entity
/// </summary>
/// <param name="message">the string was throwed in the try</param>[Serializable]
public class DalAlreadyExistsException(string? message) : Exception(message)
{
}

/// <summary>
/// exception for when deleting is impossible
/// </summary>
/// <param name="message">the string was throwed in the try</param>[Serializable]
[Serializable]
public class DalDeleteImpossible(string? message) : Exception(message)
{
}
[Serializable]
public class DalConfigException : Exception
{
    public DalConfigException(string msg) : base(msg) { }
    public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
}


/// <summary>
/// exception for when load create xml file is failed
/// </summary>
/// <param name="message">the string was throwed in the try</param>[Serializable]
[Serializable]
public class DalXMLFileLoadCreateException(string? message) : Exception(message)
{ 
}

[Serializable]
public class DalConfigException : Exception
{
    public DalConfigException(string msg) : base(msg) { }
    public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
}