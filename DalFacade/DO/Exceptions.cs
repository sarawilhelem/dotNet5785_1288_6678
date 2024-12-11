

namespace DO;

[Serializable]
public class DalDoesNotExistException(string? message) : Exception(message)
{
    //exception for when trying use not exist entity
}

[Serializable]
public class DalAlreadyExistsException(string? message) : Exception(message)
{
    //exception for when trying to add exist entity
}

[Serializable]
public class DalDeleteImpossible(string? message) : Exception(message)
{ //exception for when deleting is impossible
}

[Serializable]
public class DalXMLFileLoadCreateException(string? message) : Exception(message)
//exception for when load create xml file is failed
{
}

