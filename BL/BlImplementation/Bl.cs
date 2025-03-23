using BlApi;

namespace BlImplementation;

internal class Bl : IBl
{
    public ICall Call { get; } = new CallImplentation();

    public IVolunteer Volunteer { get; } = new VolunteerImplentation();

    public IAdmin Admin { get; } = new AdminImplentaition();
}
