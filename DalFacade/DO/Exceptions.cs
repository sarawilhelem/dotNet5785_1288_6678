

namespace DO;

[Serializable]
public class DalDoesNotExistException : Exception
{
    //exception for when trying use not exist entity
    public DalDoesNotExistException(string? message) : base(message)
    { 

    }
}

[Serializable]
public class DalAlreadyExistsException : Exception
{
    //exception for when trying to add exist entity
    public DalAlreadyExistsException(string? message) : base(message)
    {

    }
}

[Serializable]
public class DalDeleteImpossible : Exception
{
    //exception for when deleting is impossible
    public DalDeleteImpossible(string? message) : base(message)
    {

    }
}