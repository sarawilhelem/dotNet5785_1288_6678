
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

/// <summary>
/// Realization of assignment in xml CRUD using XmlSerializer
/// </summary>
internal class AssignmenImplementaion : IAssignment
{
    /// <summary>
    /// create new assignment
    /// </summary>
    /// <param name="item">an item to add to the list</param>
    public void Create(Assignment item)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        int id = Config.NextAssignmentId;
        Assignment newAssignment = item with { Id = id };
        Assignments.Add(newAssignment);
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }

    /// <summary>
    /// delete assignment according to id
    /// </summary>
    /// <param name="id">id of an assignment which it has to be deleted</param>
    /// <exception cref="DalDeleteImpossible">exception when delete was failed because assignment with tha id was not found</exception>
    public void Delete(int id)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        Assignment? thisAssignment = Assignments.Find(a => a.Id == id);
        if (Assignments.RemoveAll(it => it.Id == id) == 0)
            throw new DalDeleteImpossible($"Assignment with id {id} is not exist");
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }

    /// <summary>
    /// delete all assignments
    /// </summary>
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Assignment>(), Config.s_assignments_xml);
    }

    /// <summary>
    /// read assignment according to id
    /// </summary>
    /// <param name="id">id of an assignment to read</param>
    /// <returns>the assignment with that id</returns>
    public Assignment? Read(int id)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        return Assignments.FirstOrDefault(a => a.Id == id); //stage2

    }

    /// <summary>
    /// read first assignment in datasource.assignments which return true to filter function
    /// </summary>
    /// <param name="filter">a function who accepts an assignment and returns to it true or false</param>
    /// <returns></returns>
    public Assignment? Read(Func<Assignment, bool> filter)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        return Assignments.FirstOrDefault(a => filter(a));
    }

    /// <summary>
    /// read all assignments who returns true to filter
    /// </summary>
    /// <param name="filter">a function who accepts an assignment and returns to it true or false if any function wasn't sended filter=null</param>
    /// <returns>all the assignments who returns true to filter, or all the assignments if filter=null</returns>
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null) //stage 2
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        return filter == null
            ? Assignments.Select(item => item)
            : Assignments.Where(filter);
    }

    /// <summary>
    /// update assignment by item id
    /// </summary>
    /// <param name="item">an assignment to update the assignment with the same id like it</param>
    /// <exception cref="DalDoesNotExistException">an exception if there is not any assignment with that id</exception>
    public void Update(Assignment item)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        if (Assignments.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Assignment with id {item.Id} is not exist");
        Assignments.Add(item);
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }
}
