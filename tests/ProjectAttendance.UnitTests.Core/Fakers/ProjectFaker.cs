using Bogus;
using ProjectAttendance.Domain.Projects.Entities;
using ProjectAttendance.Domain.Users.Entities;

namespace ProjectAttendance.UnitTests.Core.Fakers
{
    public class ProjectFaker : Faker<Project>
    {
        public ProjectFaker()
        {
            CustomInstantiator((f) =>
            {
                var user = new UserFaker().Generate();
                return new Project(f.Company.CompanyName(), f.Company.CatchPhrase(), new[] { user });
            });
        }
    }
}