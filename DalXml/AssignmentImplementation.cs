
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class AssignmenImplementaion : IAssignment
{
    //  Realization of assignment in xml CRUD using XmlSerializer 

    public void Create(Assignment item)
    {
        //create new assignment
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        int id = Config.NextAssignmentId;
        Assignment newAssignment = item with { Id = id };
        Assignments.Add(newAssignment);
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }

    public void Delete(int id)
    {
        //delete assignment according to id
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        Assignment? thisAssignment = Assignments.Find(a => a.Id == id);
        if (Assignments.RemoveAll(it => it.Id == id) == 0)
            throw new DalDeleteImpossible($"Assignment with id {id} is not exist");
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }

    public void DeleteAll()
    {
        //delete all assignments
        XMLTools.SaveListToXMLSerializer(new List<Assignment>(), Config.s_assignments_xml);
    }

    public Assignment? Read(int id)
    {
        //read assignment according to id
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        return Assignments.FirstOrDefault(a => a.Id == id); //stage2

    }

    public Assignment? Read(Func<Assignment, bool> filter)
    {
        // return first assignment in datasource.assignments which return true to filter function
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        return Assignments.FirstOrDefault(a => filter(a));
    }


    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null) //stage 2
    {
        //read all assignments
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        return filter == null
            ? Assignments.Select(item => item)
            : Assignments.Where(filter);

    }


    public void Update(Assignment item)
    {
        //update assignment by item id
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        if (Assignments.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Assignment with id {item.Id} is not exist");
        Assignments.Add(item);
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }
}
