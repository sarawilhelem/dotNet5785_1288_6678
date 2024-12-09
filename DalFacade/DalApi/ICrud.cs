

using DO;

namespace DalApi;

public interface ICrud<T> where T : class
{
    //Genery interface of CRUD to the data entities
    void Create(T item); //Creates new entity object in DAL
    T? Read(int id); //Reads entity object by its ID 
    T? Read(Func<T, bool> filter); // stage 2   read the first object which return true to the filter
    IEnumerable<T> ReadAll(Func<T, bool>? filter = null); // stage 2  accept all entity objects
    void Update(T item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
    void DeleteAll(); //Delete all entity objects
}
