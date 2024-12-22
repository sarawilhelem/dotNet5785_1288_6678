namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class CallImplementation : ICall
{
    //Realization of Call in xml CRUD using XmlSerializer 
    public void Create(Call item)
    {
        //create new Call
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        int id = Config.NextCallId;
        Call newCall = item with { Id = id };
        Calls.Add(newCall);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }

    public void Delete(int id)
    {
        //delete Call from the list by id
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.RemoveAll(it => it.Id == id) == 0)
            throw new DalDeleteImpossible($"Call with id {id} is not exist");
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }

    public void DeleteAll()
    {
        //delete Calls list
        XMLTools.SaveListToXMLSerializer(new List<Call>(), Config.s_calls_xml);

    }

    public Call? Read(int id)
    {
        //reed Call by id
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return Calls.FirstOrDefault(v => v.Id == id);

    }

    public Call? Read(Func<Call, bool> filter)
    {
        // return first Call in datasource.Calls which return true to filter function
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return Calls.FirstOrDefault(v => filter(v));
    }

    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null) //stage 2
    {
        //read all Calls who return true to filter
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return filter == null
            ? Calls.Select(item => item)
            : Calls.Where(filter);

    }

    public void Update(Call item)
    {
        //update Call by id
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Call with id {item.Id} is not exist");
        Calls.Add(item);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }
}

