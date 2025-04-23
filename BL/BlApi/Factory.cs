namespace BlApi;

public static class Factory
{
    /// <summary>
    /// static field to access blImlentation
    /// </summary>
    /// <returns>blImplentation</returns>
    public static IBl Get() => new BlImplementation.Bl();
}
