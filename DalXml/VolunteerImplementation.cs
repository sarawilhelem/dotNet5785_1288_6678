namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class VolunteerImplementation : IVolunteer
{
    //Realization of volunteer in xml CRUD using xElement class


    private static Volunteer GetVolunteer(XElement s)
    {
        //Convert xElement which saves volunteer details to a volunteer object
        return new DO.Volunteer()
        {
            Id = s.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            Name = (string?)s.Element("Name") ?? "",
            Phone = (string?)s.Element("Phone") ?? "",
            Email = (string?)s.Element("Email") ?? "",
            Address = (string?)s.Element("Address") ?? null,
            Latitude = s.ToDoubleNullable("Langitude") ?? null,
            Longitude = s.ToDoubleNullable("Longitude") ?? null,
            MaxDistanceCall = s.ToDoubleNullable("MaxDistanceCall") ?? null,
            Role = s.ToEnumNullable<Role>("Role") ?? DO.Role.Volunteer,
            Distance_Type = s.ToEnumNullable<Distance_Type>("DistanceType") ?? Distance_Type.Air,
            Password = (string?)s.Element("Password") ?? null,
            IsActive = (bool?)s.Element("IsActive") ?? false,

        };
    }

    private static XElement CreateVolunteerElement(Volunteer v)
    {
        //Converting Volunteer object to XElemnt

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

    public void Create(Volunteer item)
    {
        //create new volunteer
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);
        if (volunteersRootElem.Elements().Any(v => (int?)v.Element("Id") == item.Id))
            throw new DalAlreadyExistsException($"Volunteer with id {item.Id} is yet exist");
        volunteersRootElem.Add(new XElement(CreateVolunteerElement(item)));
        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);
    }

    public void Delete(int id)
    {
        //delete volunteer from the list by id
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (Volunteers.RemoveAll(it => it.Id == id) == 0)
            throw new DalDeleteImpossible($"Volunteer with id {id} is not exist");
        XMLTools.SaveListToXMLSerializer(Volunteers, Config.s_volunteers_xml);

        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        (volunteersRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == id)
        ?? throw new DO.DalDoesNotExistException($"Volunteer with ID={id} does Not exist"))
                .Remove();
    }

    public void DeleteAll()
    {
        //delete volunteers list
        XMLTools.SaveListToXMLElement(new XElement("ArrayOfVolunteers"), Config.s_volunteers_xml);

    }

    public Volunteer? Read(int id)
    {
        //reed volunteer by id
        XElement? volunteerElement =
            XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml)
            .Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return volunteerElement is null ? null : GetVolunteer(volunteerElement);

    }

    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        // return first volunteer in datasource.volunteers which return true to filter function
        return XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml)
            .Elements().Select(s => GetVolunteer(s)).FirstOrDefault(filter);
    }

    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null) //stage 2
    {
        //read all volunteers who return true to filter
        IEnumerable<XElement> volunteersElement = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements();
        List<Volunteer> volunteersObjects = [];
        foreach (XElement element in volunteersElement)
            volunteersObjects.Add(GetVolunteer(element));
        return filter == null
            ? volunteersObjects.Select(item => item)
            : volunteersObjects.Where(filter);

    }

    public void Update(Volunteer item)
    {
        //Updating volunteer details by id
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        (volunteersRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == item.Id)
        ?? throw new DO.DalDoesNotExistException($"Volunteer with ID={item.Id} does Not exist"))
                .Remove();

        volunteersRootElem.Add(new XElement(CreateVolunteerElement(item)));

        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);
    }

}

