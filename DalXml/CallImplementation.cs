namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

/// <summary>
/// Realization of Call in xml CRUD using XmlSerializer 
/// </summary>
internal class CallImplementation : ICall
{
    /// <summary>
    /// create new Call
    /// </summary>
    /// <param name="item">a call to calls.xml</param>
    public void Create(Call item)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        int id = Config.NextCallId;
        Call newCall = item with { Id = id };
        Calls.Add(newCall);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }

    /// <summary>
    /// delete Call from the list by id
    /// </summary>
    /// <param name="id">id of the call who has to be deleted</param>
    /// <exception cref="DalDeleteImpossible">an exception when the deleting failed because any call with that id isn't found</exception>
    public void Delete(int id)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.RemoveAll(it => it.Id == id) == 0)
            throw new DalDeleteImpossible($"Call with id {id} is not exist");
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }

    /// <summary>
    /// delete Calls list
    /// </summary>
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Call>(), Config.s_calls_xml);
    }

    /// <summary>
    /// reed Call by id
    /// </summary>
    /// <param name="id">an id of a call</param>
    /// <returns>the call with that id</returns>
    public Call? Read(int id)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return Calls.FirstOrDefault(v => v.Id == id);
    }

    /// <summary>
    /// read first Call in datasource.Calls which return true to filter function
    /// </summary>
    /// <param name="filter">a function who accepts a call and returns true of false</param>
    /// <returns>the first call which returns true to filter or null if there is not any call which returns true to filter</returns>
    public Call? Read(Func<Call, bool> filter)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return Calls.FirstOrDefault(v => filter(v));
    }

    /// <summary>
    /// read all Calls who return true to filter
    /// </summary>
    /// <param name="filter">a function who accepts a call and returns true of false the default value is null</param>
    /// <returns>all calls which returns true to filter, or all the calls if filter=null</returns>
    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null) //stage 2
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return filter == null
            ? Calls.Select(item => item)
            : Calls.Where(filter);

    }

    /// <summary>
    /// update Call by id
    /// </summary>
    /// <param name="item">a call to update the call with the same id to be like it</param>
    /// <exception cref="DalDoesNotExistException">lif there is not any call with that id</exception>
    public void Update(Call item)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Call with id {item.Id} is not exist");
        Calls.Add(item);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }
}

