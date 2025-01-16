

using DO;

namespace DalApi;

/// <summary>
/// Genery interface of CRUD to the data entities
/// </summary>
/// <typeparam name="T">T is an entities</typeparam>
public interface ICrud<T> where T : class
{
    /// <summary>
    /// Creates new entity object in DAL
    /// </summary>
    /// <param name="item">an item by that entity type</param>
    void Create(T item);

    /// <summary>
    /// Reads entity object by its ID
    /// </summary>
    /// <param name="id">id of an entity</param>
    /// <returns>the entity with that id or null if not exists</returns>
    T? Read(int id);

    /// <summary>
    /// read the first object which return true to the filter
    /// </summary>
    /// <param name="filter">a function which accept an item with that entity type and returns true or false</param>
    /// <returns>the first item which the function returns true to it</returns>
    T? Read(Func<T, bool> filter);

    /// <summary>
    ///accept all entity objects which returns true to filter
    /// </summary>
    /// <param name="filter">null or a function which accept an item with that entity type and returns true or false</param>
    /// <returns>all the item which returns true to the filter, or all the entities if filter is null</returns>
    IEnumerable<T> ReadAll(Func<T, bool>? filter = null);

    /// <summary>
    /// Updates the entity with the same id to be like the parameter
    /// </summary>
    /// <param name="item">an item with the entity type</param>
    void Update(T item);

    /// <summary>
    /// Delete an object by its Id
    /// </summary>
    /// <param name="id">id of an item</param>
    void Delete(int id);

    /// <summary>
    /// Delete all entity's objects
    /// </summary>
    void DeleteAll();
}
