namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

/// <summary>
/// Realization of volunteer in xml CRUD using xElement class
/// </summary>
internal class VolunteerImplementation : IVolunteer
{
    /// <summary>
    /// Converting xElement which saves volunteer details to a volunteer object
    /// </summary>
    /// <param name="v">a volunteer xmlelement</param>
    /// <returns>a Volunteer Object</returns>
    /// <exception cref="FormatException">whent the converting the Id of the volunteer was failed</exception>
    private static Volunteer GetVolunteer(XElement v)
    {
        return new DO.Volunteer()
        {
            Id = v.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            Name = (string?)v.Element("Name") ?? "",
            Phone = (string?)v.Element("Phone") ?? "",
            Email = (string?)v.Element("Email") ?? "",
            Address = (string?)v.Element("Address") ?? null,
            Latitude = v.ToDoubleNullable("Langitude") ?? null,
            Longitude = v.ToDoubleNullable("Longitude") ?? null,
            MaxDistanceCall = v.ToDoubleNullable("MaxDistanceCall") ?? null,
            Role = v.ToEnumNullable<Role>("Role") ?? DO.Role.Volunteer,
            Distance_Type = v.ToEnumNullable<Distance_Type>("DistanceType") ?? Distance_Type.Air,
            Password = (string?)v.Element("Password") ?? null,
            IsActive = (bool?)v.Element("IsActive") ?? false
        };
    }

    /// <summary>
    /// Converting Volunteer object to XElemnt
    /// </summary>
    /// <param name="v">a volunteer object</param>
    /// <returns>a volunteer xmlelement</returns>
    private static XElement CreateVolunteerElement(Volunteer v)
    {
        return new XElement("Volunteer",
            new XElement("Id", v.Id),
            new XElement("Name", v.Name),
            new XElement("Phone", v.Phone),
            new XElement("Email", v.Email),
            new XElement("Address", v.Address),
            new XElement("Longitude", v.Longitude),
            new XElement("Latitude", v.Latitude),
            new XElement("MaxDistanceCall", v.MaxDistanceCall),
            new XElement("Role", v.Role),
            new XElement("DistanceType", v.Distance_Type),
            new XElement("Password", v.Password),
            new XElement("IsActive", v.IsActive));
    }

    /// <summary>
    /// create new volunteer
    /// </summary>
    /// <param name="item">a volunteer to add</param>
    /// <exception cref="DalAlreadyExistsException">when trying add an existing volunteer</exception>
    public void Create(Volunteer item)
    {
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);
        if (volunteersRootElem.Elements().Any(v => (int?)v.Element("Id") == item.Id))
            throw new DalAlreadyExistsException($"Volunteer with id {item.Id} is yet exist");
        volunteersRootElem.Add(new XElement(CreateVolunteerElement(item)));
        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);
    }

    /// <summary>
    /// delete volunteer from the list by id
    /// </summary>
    /// <param name="id">a volunteer's id to delete</param>
    /// <exception cref="DO.DalDoesNotExistException">if deleting was failed because there is not volunteer with that id</exception>
    public void Delete(int id)
    {
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        (volunteersRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == id)
        ?? throw new DO.DalDoesNotExistException($"Volunteer with ID={id} does Not exist"))
                .Remove();
        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);
    }

    /// <summary>
    /// delete volunteers list
    /// </summary>
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLElement(new XElement("ArrayOfVolunteers"), Config.s_volunteers_xml);
    }

    /// <summary>
    /// reed volunteer by id
    /// </summary>
    /// <param name="id">id of a volunteer to reed</param>
    /// <returns>a volunteer with that id</returns>
    public Volunteer? Read(int id)
    {
        XElement? volunteerElement =
            XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml)
            .Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return volunteerElement is null ? null : GetVolunteer(volunteerElement);
    }

    /// <summary>
    /// reed first volunteer in datasource.volunteers which return true to filter function
    /// </summary>
    /// <param name="filter">a function who accept a volunteer and returns true of false</param>
    /// <returns>the first volunteer who returns true to filter, or null if there is not any such volunteer</returns>
    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml)
            .Elements().Select(s => GetVolunteer(s)).FirstOrDefault(filter);
    }

    /// <summary>
    /// read all volunteers who return true to filter
    /// </summary>
    /// <param name="filter">a function who accept a volunteer and returns true of false or null if any function was not sended</param>
    /// <returns>all volunteers who returns true to filter, or all volunteers if filter=null</returns>
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null) //stage 2
    {
        IEnumerable<XElement> volunteersElement = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements();
        List<Volunteer> volunteersObjects = [];
        foreach (XElement element in volunteersElement)
            volunteersObjects.Add(GetVolunteer(element));
        return filter == null
            ? volunteersObjects.Select(item => item)
            : volunteersObjects.Where(filter);

    }

    /// <summary>
    /// Updating volunteer details by id
    /// </summary>
    /// <param name="item">a volunteer to update the volunteer with the same id to be like it</param>
    /// <exception cref="DO.DalDoesNotExistException">if there is not any volunteer with that id</exception>
    public void Update(Volunteer item)
    {
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        (volunteersRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == item.Id)
        ?? throw new DO.DalDoesNotExistException($"Volunteer with ID={item.Id} does Not exist"))
                .Remove();

        volunteersRootElem.Add(new XElement(CreateVolunteerElement(item)));

        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);
    }

}

